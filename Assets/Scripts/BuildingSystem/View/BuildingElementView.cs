using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingElementView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IDamagable
{
    [Header("Data")]
    [SerializeField] private BuildingModel _Data;

    [Header("Building Icon")]
    [Tooltip("The image that displays icon of building")]
    [SerializeField] private Image _BuildingHealthFiller;
    [SerializeField] private Image _BuildingIcon;
    [SerializeField] private Image _BuildingFrame;

    [Header("Active States & DEBUG")]
    [SerializeField] private List<BuildingElementState> _ElementStates;

    private bool _Targetted;
    private bool _Dead;

    private Coroutine _HealthCoroutine;

    public event Action<BuildingElementView> OnHover;
    public event Action<BuildingElementView> OnClickedLeft;
    public event Action<BuildingElementView> OnClickedRight;
    public event Action<BuildingElementView> OnDead;

    public void SetData(BuildingModel _data)
    {
        _BuildingHealthFiller.fillAmount = 1;
        _Targetted = false;
        _Dead = false;

        _Data = _data;

        _BuildingHealthFiller.fillAmount = 1f;
        _BuildingIcon.sprite = Resources.Load<Sprite>(Constants.ProductionResourcePath + _Data.GetProductionID());
        transform.name = "P_" + _Data.GetProductionID();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_Targetted || _Dead)
            return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnClickedLeft?.Invoke(this);
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnClickedRight?.Invoke(this);
        }
    }

    private void FixedUpdate()
    {
        
    }

    public BuildingModel GetData()
    {
        return _Data;
    }

    public void SetNewState(BuildingElementState _newState)
    {
        if (!_ElementStates.Contains(_newState))
        {
            _ElementStates.Add(_newState);
            UpdateVisual();
        }
    }

    public void EndState(BuildingElementState _oldState)
    {
        if (_ElementStates.Contains(_oldState))
        {
            _ElementStates.Remove(_oldState);
            UpdateVisual();
        }
    }

    private void UpdateVisual()
    {
        _BuildingFrame.color = ColorPalettes.Instance.GetBuildingColorByState(BuildingElementState.Free);

        int statesCount = _ElementStates.Count;
        if (statesCount > 0)
        {
            //Update to last state (Last state is always on the other ones)
            _BuildingFrame.color = ColorPalettes.Instance.GetBuildingColorByState(_ElementStates[statesCount - 1]);
        }
    }

    public void TakeDamage(float _damage)
    {
        var _hpStatBefore = _Data.GetHealthRatio();

        bool isDead = _Data.TakeDamage(_damage);
        if (isDead)
        {
            Dead();
            return;
        }

        var _hpStatAfter = _Data.GetHealthRatio();

        if (_HealthCoroutine != null)
            StopCoroutine(_HealthCoroutine);

        _HealthCoroutine = StartCoroutine(UpdateHealthFiller(_hpStatBefore._ratio, _hpStatAfter._ratio));

        _Targetted = false;
    }

    public void Dead()
    {
        _Dead = true;
        _BuildingHealthFiller.fillAmount = 0;

        gameObject.SetActive(false); //Pool object, we do not destroy it.

        OnDead.Invoke(this);
    }

    public void PrepareDamage()
    {
        _Targetted = true;
    }

    IEnumerator UpdateHealthFiller(float _currentRatio, float _targetRatio)
    {
        float elapsedTime = 0f;
        float duration = 0.5f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            _BuildingHealthFiller.fillAmount = Mathf.Lerp(_currentRatio, _targetRatio, t);
            yield return null;
        }

        _BuildingHealthFiller.fillAmount = _targetRatio;
    }

    public Vector2Int GetPosition()
    {
        return _Data.GetPosition();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_ElementStates.Count < 1)
        {
            OnHover.Invoke(this);
            SetNewState(BuildingElementState.Hovered);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnHover.Invoke(null);
        EndState(BuildingElementState.Hovered);
    }

    public Vector2Int GetSize()
    {
        return _Data.GetSize();
    }
}
