using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using System.IO;

public class UnitElementView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IDamagable
{
    [SerializeField] private UnitModel _UnitModel; //Core Data
    [SerializeField] private UnitElementModel _UnitElementModel; //Save Data

    [Header("Building Icon")]
    [Tooltip("The image that displays icon of building")]
    [SerializeField] private Image _UnitHealthFiller;
    [SerializeField] private Image _UnitIcon;
    [SerializeField] private Image _UnitFrame;


    [Header("Active States & DEBUG")]
    [SerializeField] private List<UnitElementState> _ElementStates;
    [SerializeField] private List<GridElementView> _CurrentPath;
    [SerializeField] private GridElementView _Target;

    private bool _HasTarget;
    private bool _Targetted;
    private bool _Dead;
    private Coroutine _MoveCoroutine;
    private Coroutine _HealthCoroutine;

    public event Action<UnitElementView> OnHover;
    public event Action<UnitElementView> OnClickedLeft;
    public event Action<UnitElementView> OnClickedRight;
    public event Action<UnitElementView> OnDead;
    public event Action<List<GridElementView>, UnitElementView, GridElementView> OnStepUpdate;

    private void Start()
    {
        _CurrentPath = new();
    }

    public void SetData(UnitModel _unitModel, UnitElementModel _unitElementModel)
    {
        _UnitHealthFiller.fillAmount = 1;
        _HasTarget = false;
        _Targetted = false;
        _Dead = false;

        _UnitModel = _unitModel;
        _UnitElementModel = _unitElementModel;

        _UnitIcon.sprite = Resources.Load<Sprite>(Constants.UnitResourcePath + _UnitModel.UnitID);
        transform.name = "Unit_" + _UnitModel.UnitID;
    }

    public void SetNewState(UnitElementState _newState)
    {
        if (!_ElementStates.Contains(_newState))
        {
            _ElementStates.Add(_newState);
            UpdateVisual();
        }
    }

    public void EndState(UnitElementState _oldState)
    {
        if (_ElementStates.Contains(_oldState))
        {
            _ElementStates.Remove(_oldState);
            UpdateVisual();
        }
    }

    private void UpdateVisual()
    {
        _UnitFrame.color = ColorPalettes.Instance.GetUnitColorByState(UnitElementState.Free);

        int statesCount = _ElementStates.Count;
        if (statesCount > 0)
        {
            //Update to last state (Last state is always on the other ones)
            _UnitFrame.color = ColorPalettes.Instance.GetUnitColorByState(_ElementStates[statesCount - 1]);
        }
    }

    public UnitModel GetUnitModelData()
    {
        return _UnitModel;
    }
    
    public UnitElementModel GetUnitElementModelData()
    {
        return _UnitElementModel;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_Targetted || _Dead)
            return;

        if (_CurrentPath.Count == 0)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnClickedLeft?.Invoke(this);
            }
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                OnClickedRight?.Invoke(this);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_ElementStates.Count < 1)
        {
            if (_CurrentPath.Count == 0)
            {
                OnHover.Invoke(this);
                SetNewState(UnitElementState.Hovered);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnHover.Invoke(null);
        EndState(UnitElementState.Hovered);
    }

    public void OnGridUpdate(List<UnitElementView> _myUnits)
    {
        //int length = _CurrentPath.Count;
        //int removeIndex = 999999;
        //for (int i = 0; i < length; i++)
        //{
        //    Vector2Int currentPoint = new(_CurrentPath[i].GetData().Get_X(), _CurrentPath[i].GetData().Get_Y());
        //    foreach (var item in _myUnits)
        //    {
        //        if (item == this)
        //            continue;

        //        if (item.GetUnitElementModelData().GetCurrentPosition() == currentPoint)
        //        {
        //            removeIndex = i;
        //            break;
        //        }
        //    }

        //    if (_CurrentPath[i].IsOccupied())
        //    {
        //        removeIndex = i;
        //        break;
        //    }
        //}

        //for (int i = length - 1; i >= removeIndex; i--)
        //{
        //    _CurrentPath[i].EndState(GridElementState.OnNavigation);
        //    _CurrentPath.RemoveAt(i);
        //}
    }

    public void StartMove(List<GridElementView> _path)
    {
        EndState(UnitElementState.Selected);

        _CurrentPath = _path;

        if (_MoveCoroutine != null)
            StopCoroutine(_MoveCoroutine);

        _MoveCoroutine = StartCoroutine(Move());
    }

    IEnumerator Move(IDamagable _target = null)
    {
        _HasTarget = _target != null;
        if(_CurrentPath.Count > 0)
            _Target = _CurrentPath[_CurrentPath.Count - 1];

        float moveSpeed = 300f;

        while (_CurrentPath.Count > 0)
        {
            _UnitElementModel.SetCurrentPosition(new(_CurrentPath[0].GetData().Get_X(), _CurrentPath[0].GetData().Get_Y()));

            Vector3 worldTargetPoint = _CurrentPath[0].transform.position;
            Vector3 localTargetPoint = transform.parent.transform.InverseTransformPoint(worldTargetPoint);

            OnStepBegins(); //The soldier entered in e new block, let's refresh if still canPlace the building

            if (_CurrentPath.Count == 0)
            {
                Debug.Log("Stopped", transform);
                transform.position = worldTargetPoint;
                _target = null;
                break;
            }

            while (Vector2.Distance(transform.localPosition, localTargetPoint) > 0.01f)
            {
                transform.localPosition = Vector2.MoveTowards(transform.localPosition, localTargetPoint, moveSpeed * Time.deltaTime);
                yield return null;
            }

            transform.localPosition = localTargetPoint;

            _CurrentPath[0].EndState(GridElementState.OnNavigation);
            _CurrentPath.RemoveAt(0);

            Debug.Log("_CurrentPath count" + _CurrentPath.Count, transform);
            yield return new WaitForEndOfFrame();
        }

        if (_target != null)
        {
            _target.TakeDamage(_UnitModel.Damage);
        }

        _HasTarget = false;
    }

    public void SetTarget(List<GridElementView> _path, IDamagable _target)
    {
        _target.PrepareDamage();

        EndState(UnitElementState.Selected);

        _CurrentPath = _path;

        if (_MoveCoroutine != null)
            StopCoroutine(_MoveCoroutine);

        _MoveCoroutine = StartCoroutine(Move(_target));
    }

    void OnStepBegins()
    {
        foreach (var item in _CurrentPath)
            item.SetNewState(GridElementState.OnNavigation);

        OnStepUpdate.Invoke(_CurrentPath, this, _Target);
    }

    public void TakeDamage(float _damage)
    {
        var _hpStatBefore = _UnitElementModel.GetHealthRatio();

        bool isDead = _UnitElementModel.TakeDamage(_damage);
        if (isDead)
        {
            Dead();
            return;
        }

        var _hpStatAfter = _UnitElementModel.GetHealthRatio();

        if (_HealthCoroutine != null)
            StopCoroutine(_HealthCoroutine);

        _HealthCoroutine = StartCoroutine(UpdateHealthFiller(_hpStatBefore._ratio, _hpStatAfter._ratio));

        _Targetted = false;
    }

    public void Dead()
    {
        _Dead = true;
        _UnitHealthFiller.fillAmount = 0;
        gameObject.SetActive(false); //Pool Object, we do not destroy.

        OnDead.Invoke(this);
    }

    public void PrepareDamage()
    {
        _Targetted = true;
    }

    public void SetNewPath(List<GridElementView> _path)
    {
        _CurrentPath = _path;
    }

    IEnumerator UpdateHealthFiller(float _currentRatio, float _targetRatio)
    {
        float elapsedTime = 0f;
        float duration = 0.5f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            _UnitHealthFiller.fillAmount = Mathf.Lerp(_currentRatio, _targetRatio, t);
            yield return null;
        }

        _UnitHealthFiller.fillAmount = _targetRatio;
    }

    public bool UnitHasTarget()
    {
        return _HasTarget;
    }

    public Vector2Int GetPosition()
    {
        return _UnitElementModel.GetCurrentPosition();
    }

    public Vector2Int GetSize()
    {
        return Vector2Int.one;
    }
}
