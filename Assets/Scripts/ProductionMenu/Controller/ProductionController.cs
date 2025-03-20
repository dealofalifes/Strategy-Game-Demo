using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionController : MonoBehaviour, IProductionSystem
{
    [Tooltip("Production View Component for this Controller")]
    [Header("View Class")]
    [SerializeField] private ProductionView _View;

    [Header("Productions & DEBUG")]
    [SerializeField] private ProductionElementView _SelectedProductionElementView = null;


    //---
    private IBuildingSystem _BuildingSystem;
    private IGridSystem _GridSystem;
    private IInformationSystem _InformationSystem;
    private IUnitSystem _UnitSystem;
    private IBuildingFactory _BuildingFactory;

    void Start()
    {
        _BuildingFactory = new BuildingFactory();

        _View.OnElementCreated += OnEndCreateProduction;
        _View.OnCancelled += CancelSelection;

        _View.CreateProductions(CreateProductions());
    }

    public void Initialize(IBuildingSystem _buildingSystem, IGridSystem _gridSystem, IInformationSystem _informationSystem, IUnitSystem _unitSystem)
    {
        _BuildingSystem = _buildingSystem;
        _GridSystem = _gridSystem;
        _InformationSystem = _informationSystem;
        _UnitSystem = _unitSystem;
    }

    private void OnEndCreateProduction(List<ProductionElementView> _elements)
    {
        foreach (var item in _elements)
            item.OnClicked += OnItemClicked;
    }

    public List<ProductionModel> CreateProductions()
    {
        List<ProductionModel> productions = new();

        #region PowerPlants

            productions.Add(_BuildingFactory.CreateProduction(1001));
            productions.Add(_BuildingFactory.CreateProduction(1002));
            productions.Add(_BuildingFactory.CreateProduction(1003));
            productions.Add(_BuildingFactory.CreateProduction(1004));
            productions.Add(_BuildingFactory.CreateProduction(1005));

        #endregion

        #region Barracks

            productions.Add(_BuildingFactory.CreateProduction(2001));
            productions.Add(_BuildingFactory.CreateProduction(2002));
            productions.Add(_BuildingFactory.CreateProduction(2003));

        #endregion

        #region Watchtowers

            productions.Add(_BuildingFactory.CreateProduction(3001));
            productions.Add(_BuildingFactory.CreateProduction(3002));
            productions.Add(_BuildingFactory.CreateProduction(3003));

        #endregion

        #region FarmAreas

            productions.Add(_BuildingFactory.CreateProduction(4001));
            productions.Add(_BuildingFactory.CreateProduction(4002));
            productions.Add(_BuildingFactory.CreateProduction(4003));

        #endregion

        return productions;
    }


    private void OnDestroy()
    {
        _View.OnElementCreated -= OnEndCreateProduction;
        _View.OnCancelled -= CancelSelection;
    }

    public void OnItemClicked(ProductionElementView item)
    {
        CancelSelection();

        _SelectedProductionElementView = item;
        _SelectedProductionElementView.SetNewState(ProductionElementState.Selected);

        _InformationSystem.ShowInformation(item.GetData());
        _BuildingSystem.ActivateBuildingMode(new(item.GetProductionID(), item.GetData()._ProductionSize, Vector2Int.zero, null));

        _UnitSystem.ClearSelected();
        _BuildingSystem.OnCancelBuildingInformation();
    }

    public void CancelSelection()
    {
        if (_SelectedProductionElementView != null)
        {
            _SelectedProductionElementView.EndState(ProductionElementState.Selected);
            _SelectedProductionElementView = null;

            _InformationSystem.HideInformation();
            _BuildingSystem.OnBuildingModeCancelled();
            _GridSystem.OnBuildingModeCancelled();
        }
    }

    public ProductionElementView GetProductionElementViewByProductionID(int _productionID)
    {
        return _View.GetProductionElementViewByProductionID(_productionID);
    }

    public ProductionElementView GetProductionElementViewByIndex(int _index)
    {
        return _View.GetProductionElementViewByIndex(_index);
    }

    public ProductionModel GetProductionModelByID(int _productionID)
    {
        return _View.GetProductionModelByID(_productionID);
    }
}
