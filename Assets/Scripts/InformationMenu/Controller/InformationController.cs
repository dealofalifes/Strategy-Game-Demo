using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationController : MonoBehaviour, IInformationSystem
{
    [Tooltip("Information View Component for this Controller")]
    [Header("View Class")]
    [SerializeField] private InformationView _View;

    public event Action<BarrackModel, UnitModel, Vector2Int> OnUnitCreated;

    //-----
    private IProductionSystem _ProductionSystem;
    private IUnitSystem _UnitSystem;
    private void Awake()
    {
        _View.OnProduceButtonClicked += OnProduceButtonClicked;
    }

    public void Initialize(IProductionSystem _productionSystem, IUnitSystem _unitSystem)
    {
        _ProductionSystem = _productionSystem;
        _UnitSystem = _unitSystem;
    }

    public void OnProduceButtonClicked()
    {
        BuildingElementView _currentBarrack = _View.GetCurrentSelectedBarrack();
        if (_currentBarrack != null)
        {
            BarrackModel barrackModel = _ProductionSystem.GetProductionModelByID(_currentBarrack.GetData().GetProductionID()) as BarrackModel;
            UnitModel newUnit = _UnitSystem.GetUnitModelByID(barrackModel._SoldierID);

            Vector2Int position = _currentBarrack.GetData().GetPosition();
            Vector2Int doorPos = Helper.FindDoorPosForBarrack(position, barrackModel);

            if (_UnitSystem.CanCreateUnit(doorPos))
            {
                OnUnitCreated.Invoke(barrackModel, newUnit, doorPos);
            }
        }
    }

    public void ShowInformation(ProductionModel _model)
    {
        _View.ShowInformation(_model);
    }

    public void HideInformation()
    {
        _View.HideInformation();
    }

    public void ShowInformation(BuildingElementView _productionElementView, ProductionModel _productionModel)
    {
        _View.ShowInformation(_productionElementView, _productionModel);
    }

    public void ShowInformation(UnitModel _model)
    {
        _View.ShowInformation(_model);
    }

    public void OnCreatedUnit(BarrackModel _barrackModel, UnitModel _unitModel, Vector2Int _doorPos)
    {
        OnUnitCreated.Invoke(_barrackModel, _unitModel, _doorPos);
    }

    private void OnDestroy()
    {
        _View.OnProduceButtonClicked -= OnProduceButtonClicked;
    }
}
