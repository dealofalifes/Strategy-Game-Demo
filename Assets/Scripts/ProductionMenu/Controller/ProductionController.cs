using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionController : MonoBehaviour
{
    [Tooltip("Production View Component for this Controller")]
    [Header("View Class")]
    [SerializeField] private ProductionView _View;

    [Header("Productions & DEBUG")]
    [SerializeField] private List<ProductionModel> _Productions;

    [SerializeField] private ProductionElementView _SelectedProductionElementView = null;

    private void Awake()
    {
        ServiceLocator.Instance.Register(this);
    }

    void Start()
    {
        CreateProductions();

        _View.OnElementCreated += OnEndCreateProduction;
        _View.OnCancelled += CancelSelection;

        _View.CreateProductions(_Productions);
    }

    private void OnEndCreateProduction(List<ProductionElementView> _elements)
    {
        foreach (var item in _elements)
            item.OnClicked += OnItemClicked;
    }

    public void CreateProductions()
    {
        _Productions = new();

        #region Example Productions

        #region PowerPlants
        PowerPlantModel powerPlant1 = new PowerPlantModel()
        {
            _Name = "Common Powerplant",
            _ProductionID = 1001,
            _MaxHealth = 50,
            _ProductionSize = new(2, 2),
            _EnergyPerSecond = 3,
        };

        _Productions.Add(powerPlant1);

        PowerPlantModel powerPlant2 = new PowerPlantModel()
        {
            _Name = "Uncommon Powerplant",
            _ProductionID = 1002,
            _MaxHealth = 50,
            _ProductionSize = new(3, 2),
            _EnergyPerSecond = 5,
        };

        _Productions.Add(powerPlant2);

        PowerPlantModel powerPlant3 = new PowerPlantModel()
        {
            _Name = "Rare Powerplant",
            _ProductionID = 1003,
            _MaxHealth = 50,
            _ProductionSize = new(3, 3),
            _EnergyPerSecond = 8,
        };

        _Productions.Add(powerPlant3);

        PowerPlantModel powerPlant4 = new PowerPlantModel()
        {
            _Name = "Unique Powerplant",
            _ProductionID = 1004,
            _MaxHealth = 50,
            _ProductionSize = new(4, 3),
            _EnergyPerSecond = 12,
        };

        _Productions.Add(powerPlant4);

        PowerPlantModel powerPlant5 = new PowerPlantModel()
        {
            _Name = "Legendary Powerplant",
            _ProductionID = 1005,
            _MaxHealth = 50,
            _ProductionSize = new(5, 3),
            _EnergyPerSecond = 18,
        };

        _Productions.Add(powerPlant5);

        #endregion

        #region Barracks
        BarrackModel barrack1 = new BarrackModel()
        {
            _Name = "Common Barrack",
            _ProductionID = 2001,
            _MaxHealth = 100,
            _ProductionSize = new(1, 1),
            _SoldierLevel = 1,
        };

        _Productions.Add(barrack1);

        BarrackModel barrack2 = new BarrackModel()
        {
            _Name = "Rare Barrack",
            _ProductionID = 2002,
            _MaxHealth = 100,
            _ProductionSize = new(2, 2),
            _SoldierLevel = 2,
        };

        _Productions.Add(barrack2);

        BarrackModel barrack3 = new BarrackModel()
        {
            _Name = "Legendary Barrack",
            _ProductionID = 2003,
            _MaxHealth = 100,
            _ProductionSize = new(3, 4),
            _SoldierLevel = 3,
        };

        _Productions.Add(barrack3);
        #endregion

        #region Watchtowers
        WatchtowerModel watchtower1 = new WatchtowerModel()
        {
            _Name = "Common Watchtower",
            _ProductionID = 3001,
            _MaxHealth = 150,
            _ProductionSize = new(1, 1),
            _Range = 1,
        };

        _Productions.Add(watchtower1);

        WatchtowerModel watchtower2 = new WatchtowerModel()
        {
            _Name = "Rare Watchtower",
            _ProductionID = 3002,
            _MaxHealth = 150,
            _ProductionSize = new(1, 1),
            _Range = 2,
        };

        _Productions.Add(watchtower2);

        WatchtowerModel watchtower3 = new WatchtowerModel()
        {
            _Name = "Legendary Watchtower",
            _ProductionID = 3003,
            _MaxHealth = 100,
            _ProductionSize = new(1, 1),
            _Range = 3,
        };

        _Productions.Add(watchtower3);
        #endregion

        #region FarmAreas
        FarmAreaModel farm1 = new FarmAreaModel()
        {
            _Name = "Common Farm Area",
            _ProductionID = 4001,
            _MaxHealth = 25,
            _ProductionSize = new(4, 4),
            _HarvestTime = 60,
            _HarvestQuantity = 150,
        };

        _Productions.Add(farm1);

        FarmAreaModel farm2 = new FarmAreaModel()
        {
            _Name = "Rare Farm Area",
            _ProductionID = 4002,
            _MaxHealth = 35,
            _ProductionSize = new(6, 6),
            _HarvestTime = 50,
            _HarvestQuantity = 250,
        };

        _Productions.Add(farm2);

        FarmAreaModel farm3 = new FarmAreaModel()
        {
            _Name = "Legendary Farm Area",
            _ProductionID = 4003,
            _MaxHealth = 50,
            _ProductionSize = new(9, 9),
            _HarvestTime = 40,
            _HarvestQuantity = 400,
        };

        _Productions.Add(farm3);
        #endregion

        #endregion
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

        ServiceLocator.Instance.Get<InformationController>().ShowInformation(item.GetData());
        ServiceLocator.Instance.Get<BuildingSystemController>().ActivateBuildingMode(new() { _ProductionID = item.GetProductionID(), _StartGrid = Vector2Int.zero });
    }

    public void CancelSelection()
    {
        if (_SelectedProductionElementView != null)
        {
            _SelectedProductionElementView.EndState(ProductionElementState.Selected);
            _SelectedProductionElementView = null;

            ServiceLocator.Instance.Get<InformationController>().HideInformation();
            ServiceLocator.Instance.Get<BuildingSystemController>().OnBuildingModeCancelled();
            ServiceLocator.Instance.Get<GridSystemController>().OnBuildingModeCancelled();
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
