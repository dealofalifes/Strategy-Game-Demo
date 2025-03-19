using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingElementView : MonoBehaviour, IPointerClickHandler
{
    [Header("Data")]
    [SerializeField] private BuildingModel _Data;

    [Header("Building Icon")]
    [Tooltip("The image that displays icon of building")]
    [SerializeField] private Image _BuildingIcon;

    public event Action<BuildingElementView> OnClicked;
    public void SetData(BuildingModel _data)
    {
        _Data = _data;

        _BuildingIcon.sprite = Resources.Load<Sprite>(Constants.ProductionResourcePath + _Data._ProductionID);
        transform.name = "P_" + _Data._ProductionID;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnClicked?.Invoke(this);
        }
    }

    public BuildingModel GetData()
    {
        return _Data;
    }
}
