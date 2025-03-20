using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;
using System;

[System.Serializable]
public class GridElementView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Data")]
    [SerializeField] private GridElementModel _Data;

    [Header("Border Images")]
    [Tooltip("Be sure the order of these items! It should be set same order as enum 'Direction'!")]
    [SerializeField] private List<Image> _Borders;

    [Header("Active States & DEBUG")]
    [SerializeField] private List<GridElementState> _ElementStates;

    public event Action<GridElementView> OnHover;
    public event Action<Vector2Int> OnEnter;
    public event Action<Vector2Int, GridElementView> OnClickedLeft;
    public event Action<GridElementView> OnClickedRight;
    public void SetData(GridElementModel _data)
    {
        _ElementStates = new();
        _Data = _data;

        transform.name = _Data.Get_X() + "_" + _Data.Get_Y();
    }

    public void SetNewState(GridElementState _newState)
    {
        if (!_ElementStates.Contains(_newState))
        {
            _ElementStates.Add(_newState);
            UpdateVisual();
        }
    }

    public void EndState(GridElementState _oldState)
    {
        if (_ElementStates.Contains(_oldState))
        {
            _ElementStates.Remove(_oldState);
            UpdateVisual();
        }
    }

    private void UpdateVisual()
    {
        foreach (var item in _Borders)
        {
            item.gameObject.SetActive(true);
            item.color = ColorPalettes.Instance.GetGridColorByState(GridElementState.Free);
        }

        int statesCount = _ElementStates.Count;
        if (statesCount > 0)
        {
            //Update to last state (Last state is always on the other ones)
            foreach (var item in _Borders)
            {
                item.color = ColorPalettes.Instance.GetGridColorByState(_ElementStates[statesCount - 1]);
            }
        }
    }

    public void SetOccupied(int _productionID)
    {
        _Data.SetProduction(_productionID);
    }

    public bool IsOccupied()
    {
        return _Data.GetProductionID() > 0;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnEnter.Invoke(new(_Data.Get_X(), _Data.Get_Y()));
        if (_ElementStates.Count < 1)
        {
            OnHover.Invoke(this);
            SetNewState(GridElementState.Hovered);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnHover.Invoke(null);
        EndState(GridElementState.Hovered);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnClickedLeft.Invoke(new(_Data.Get_X(), _Data.Get_Y()), this);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnClickedRight.Invoke(this);
        }
    }

    public GridElementModel GetData()
    {
        return _Data;
    }
}
