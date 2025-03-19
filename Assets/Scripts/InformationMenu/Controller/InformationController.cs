using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationController : MonoBehaviour
{
    [Tooltip("Information View Component for this Controller")]
    [Header("View Class")]
    [SerializeField] private InformationView _View;

    [Header("DEBUG")]
    [SerializeField] private ProductionModel _CurrentSelectedProductionModel;

    private void Awake()
    {
        ServiceLocator.Instance.Register(this);
    }

    public void ShowInformation(ProductionModel _model)
    {
        _CurrentSelectedProductionModel = _model;

        _View.ShowInformation(_CurrentSelectedProductionModel);
    }

    public void HideInformation()
    {
        _CurrentSelectedProductionModel = null;

        _View.HideInformation();
    }
}
