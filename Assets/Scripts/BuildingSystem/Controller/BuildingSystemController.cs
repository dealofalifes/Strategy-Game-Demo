using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class BuildingSystemController : MonoBehaviour, IBuildingSystem
{
    [Tooltip("Building View Component for this Controller")]
    [Header("View Class")]
    [SerializeField] private BuildingSystemView _View;

    [Header("DEBUG")]
    [SerializeField] private BuildingElementView _SelectedBuildingElement;
    [HideInInspector][SerializeField] private BuildingElementView _OnBuildingElement;

    public event Action<IDamagable> OnTriggerAttack;

    public bool IsBuildingModeActive { get; private set; }

    //---
    private IGridSystem _GridSystem;
    private IInformationSystem _InformationSystem;
    private IProductionSystem _ProductionSystem;
    private IUnitSystem _UnitSystem;

    private void Start()
    {
        OnBuildingModeCancelled();

        _View.OnCancelled += OnCancelBuildingInformation;
    }

    public void Initialize(IGridSystem _gridSystem, IInformationSystem _informationSystem, IProductionSystem _productionSystem, IUnitSystem _unitSystem)
    {
        _GridSystem = _gridSystem;
        _InformationSystem = _informationSystem;
        _ProductionSystem = _productionSystem;
        _UnitSystem = _unitSystem;
    }

    public void ChecktoPlaceBuilding(Vector2Int _position)
    {
        bool canPlace = _GridSystem.CanPlaceBuilding(_View._SelectedBuilding, _View._SelectedProduction, _position);
    }

    public void ActivateBuildingMode(BuildingModel _buildingModel)
    {
        ProductionModel productionModel = _ProductionSystem.GetProductionModelByID(_buildingModel.GetProductionID());
        _View.ActivateBuildingMode(_buildingModel, productionModel);
        OnBuildingModeStarted();
    }

    public void PlaceBuilding(Vector2Int _position, GridElementView _gridElementView)
    {
        bool placed = _GridSystem.PlaceBuilding(_View._SelectedBuilding, _View._SelectedProduction, _position);

        if (placed)
        {
            Vector2Int cellSize = _GridSystem.GetCellSize();
            BuildingElementView buildingElementView = _View.CreateBuilding(_position, _gridElementView, cellSize);
            buildingElementView.OnClickedLeft += OnClickedBuilding;
            buildingElementView.OnClickedRight += OnTargettedBuilding;
            buildingElementView.OnHover += OnHoverBuildingElement;
            buildingElementView.OnDead += OnBuildingBeDestroyed;
            _ProductionSystem.CancelSelection();
            _UnitSystem.OnGridChanged();
            Debug.Log("Building is successfully Placed");
        }
    }

    public void OnBuildingBeDestroyed(BuildingElementView _buildingElementView)
    {
        _buildingElementView.OnClickedLeft -= OnClickedBuilding;
        _buildingElementView.OnClickedRight -= OnTargettedBuilding;
        _buildingElementView.OnHover -= OnHoverBuildingElement;
        _buildingElementView.OnDead -= OnBuildingBeDestroyed;

        ProductionModel productionModel = _ProductionSystem.GetProductionModelByID(_buildingElementView.GetData().GetProductionID());

        bool hasDoor = productionModel is BarrackModel;

        List<Vector2Int> buildingPoints = new();

        Vector2Int corner = _buildingElementView.GetPosition();
        Vector2Int size = _buildingElementView.GetSize();

        for (int x = corner.x; x < corner.x + size.x; x++)
            for (int y = corner.y; y < corner.y + size.y; y++)
                buildingPoints.Add(new Vector2Int(x, y));

        if (hasDoor)
        {
            Vector2Int doorPoss = Helper.FindDoorPosForBarrack(corner, productionModel);
            _GridSystem.GetGridElementViewByPosition(doorPoss).EndState(GridElementState.BarrackDoor);
            buildingPoints.Add(doorPoss);
        }

        _GridSystem.SetGridsFree(buildingPoints);
    }

    public void OnTargettedBuilding(IDamagable _buildingTarget)
    {
        if (!IsBuildingModeActive)
        {
            OnTriggerAttack.Invoke(_buildingTarget);
        }
    }

    public void OnClickedBuilding(BuildingElementView _buildingElementView)
    {
        if (!IsBuildingModeActive)
        {
            if (_SelectedBuildingElement != null)
                _SelectedBuildingElement.EndState(BuildingElementState.Selected);

            _SelectedBuildingElement = _buildingElementView;

            ProductionModel productionModel = _ProductionSystem.GetProductionModelByID(_SelectedBuildingElement.GetData().GetProductionID());

            _SelectedBuildingElement.SetNewState(BuildingElementState.Selected);
            if (productionModel is BarrackModel barrackModel)
            {
                //If it is barrack, I send buildings data to create soldier in front of Barrack.
                _InformationSystem.ShowInformation(_buildingElementView, productionModel);
            }
            else
            {
                _InformationSystem.ShowInformation(productionModel);
            }

            _UnitSystem.ClearSelected();
        }
    }

    public void OnCancelBuildingInformation()
    {
        if (_SelectedBuildingElement != null)
        {
            _SelectedBuildingElement.EndState(BuildingElementState.Selected);
            _SelectedBuildingElement = null;

            _InformationSystem.HideInformation();
        }
    }

    public void OnBuildingModeStarted()
    {
        IsBuildingModeActive = true;
    }

    public void OnBuildingModeCancelled()
    {
        IsBuildingModeActive = false;
        _View.CancelBuildingMode();
    }

    private void OnHoverBuildingElement(BuildingElementView _gridElementView)
    {
        _OnBuildingElement = _gridElementView;
    }

    public bool IsOnBuildingElement() 
    { 
        return _OnBuildingElement != null;
    }

    private void OnDestroy()
    {
        _View.OnCancelled -= OnCancelBuildingInformation;
    }
}
