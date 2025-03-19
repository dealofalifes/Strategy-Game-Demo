using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProductionElementView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Data")]
    [SerializeField] private ProductionModel _Data;

    [Header("Production Icon")]
    [Tooltip("The image that displays icon of production")]
    [SerializeField] private Image _ProductionIcon;

    [Header("Production Frame")]
    [Tooltip("The image that displays frame of production to change color on states")]
    [SerializeField] private Image _ProductionFrame;

    [Header("Active States & DEBUG")]
    [SerializeField] private List<ProductionElementState> _ElementStates;

    public event Action<ProductionElementView> OnClicked;

    public void SetData(ProductionModel _data)
    {
        _ElementStates = new();
        _Data = _data;

        _ProductionIcon.sprite = Resources.Load<Sprite>(Constants.ProductionResourcePath + _Data._ProductionID);
        transform.name = "P_" + _Data._ProductionID;
    }

    public void SetNewState(ProductionElementState _newState)
    {
        if (!_ElementStates.Contains(_newState))
        {
            _ElementStates.Add(_newState);
            UpdateVisual();
        }
    }

    public void EndState(ProductionElementState _oldState)
    {
        if (_ElementStates.Contains(_oldState))
        {
            _ElementStates.Remove(_oldState);
            UpdateVisual();
        }
    }

    private void UpdateVisual()
    {
        _ProductionFrame.color = ColorPalettes.Instance.GetProductionColorByState(ProductionElementState.Free);
        
        int statesCount = _ElementStates.Count;
        if (statesCount > 0)
        {
            //Update to last state (Last state is always on the other ones)
            _ProductionFrame.color = ColorPalettes.Instance.GetProductionColorByState(_ElementStates[statesCount - 1]);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetNewState(ProductionElementState.Hovered);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EndState(ProductionElementState.Hovered);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnClicked?.Invoke(this);
        }
    }

    public int GetProductionID()
    {
        return _Data._ProductionID;
    }

    public ProductionModel GetData()
    {
        return _Data;
    }
}
