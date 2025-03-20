using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UnitSystemView : MonoBehaviour
{
    [SerializeField] private Transform _UnitElementsContent;

    [Tooltip("To get units from the pool")]
    [Header("Pool System")]
    [SerializeField] private List<GameObject> _UnitsPool;
    [SerializeField] private UnitElementView _CurrentSelectedUnitElement;

    public event Action OnCancelled;
    private void LateUpdate()
    {
        if (Input.GetMouseButtonDown(1))
            OnCancelled.Invoke();
    }

    public void OnGridChanged(List<UnitElementView> _myUnits)
    {
        foreach (var item in _myUnits)
            item.OnGridUpdate(_myUnits);
    }

    public void MoveUnit(UnitElementView _moveUnit, List<GridElementView> _path)
    {
        _CurrentSelectedUnitElement = null;
        _moveUnit.StartMove(_path);
    }
    
    public void SetTargetUnit(UnitElementView _moveUnit, List<GridElementView> _path, IDamagable _target)
    {
        _CurrentSelectedUnitElement = null;
        _moveUnit.SetTarget(_path, _target);
    }

    public void OnSelectedUnit(UnitElementView _unitElementView)
    {
        foreach (Transform item in _UnitElementsContent)
        {
            UnitElementView i = item.GetComponent<UnitElementView>();
            i.EndState(UnitElementState.Selected);
        }

        if(_unitElementView == null)
        {
            if(_CurrentSelectedUnitElement != null)
            {
                _CurrentSelectedUnitElement.EndState(UnitElementState.Selected);
            }
        }
        else
        {
            _unitElementView.SetNewState(UnitElementState.Selected);
        }
           
        _CurrentSelectedUnitElement = _unitElementView;
    }

    public UnitElementView OnCreateUnit(GridElementView _gridElementView, UnitModel _unitModel, Vector2Int _doorPos, Vector2Int _cellSize)
    {
        GameObject newUnitElement = GetUnitFromPool();
        newUnitElement.transform.SetParent(_UnitElementsContent);

        RectTransform newUnitElementRect = newUnitElement.GetComponent<RectTransform>();
        newUnitElementRect.position = _gridElementView.GetComponent<RectTransform>().position;
        newUnitElementRect.sizeDelta = new Vector2Int(_cellSize.x, _cellSize.y);

        UnitElementView unitElementView = newUnitElement.GetComponent<UnitElementView>();
        Vector2Int pos = new(_doorPos.x, _doorPos.y); //Spawn and Current positions are the same at the beginning;
        Stats _unitStats = new Stats(_unitModel.MaxHealth);
        UnitElementModel currentUnitElementModel = new UnitElementModel(_unitModel.UnitID, pos, pos, _unitStats);

        unitElementView.SetData(_unitModel, currentUnitElementModel);

        return unitElementView;
    }

    public Transform GetUnitElementsContent()
    {
        return _UnitElementsContent;
    }

    public GameObject GetUnitFromPool()
    {
        foreach (var item in _UnitsPool)
        {
            if (!item.activeSelf)
            {
                item.SetActive(true);
                return item;
            }
        }

        GameObject newBuildingElementPrefab = Resources.Load<GameObject>(Constants.UnitElementResourcePath);
        GameObject newBuildingElement = Instantiate(newBuildingElementPrefab);
        _UnitsPool.Add(newBuildingElement);
        return newBuildingElement;
    }

    public UnitElementView GetCurrentSelectedUnitElement()
    {
        return _CurrentSelectedUnitElement;
    }
}
