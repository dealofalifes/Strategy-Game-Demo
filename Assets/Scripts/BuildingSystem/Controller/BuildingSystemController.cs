using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSystemController : MonoBehaviour
{
    [Tooltip("Building View Component for this Controller")]
    [Header("View Class")]
    [SerializeField] private BuildingSystemView _View;

    [Header("DEBUG")]
    [SerializeField] private BuildingElementView _SelectedBuildingElement;
    private void Awake()
    {
        ServiceLocator.Instance.Register(this);
    }

    private void Start()
    {
        OnBuildingModeCancelled();

        _View.OnCancelled += OnCancelBuildingInformation;
    }

    public void ChecktoPlaceBuilding(Vector2Int _position)
    {
        bool canPlace = ServiceLocator.Instance.Get<GridSystemController>().
            CanPlaceBuilding(_View._SelectedBuilding, _View._SelectedProduction, _position);
    }

    public void ActivateBuildingMode(BuildingModel _buildingModel)
    {
        ProductionModel productionModel = ServiceLocator.Instance.Get<ProductionController>().GetProductionModelByID(_buildingModel._ProductionID);
        _View.ActivateBuildingMode(_buildingModel, productionModel);
    }

    public void PlaceBuilding(Vector2Int _position, GridElementView _gridElementView)
    {
        bool placed = ServiceLocator.Instance.Get<GridSystemController>().PlaceBuilding(_View._SelectedBuilding, _View._SelectedProduction, _position);

        if (placed)
        {
            Vector2Int cellSize = ServiceLocator.Instance.Get<GridSystemController>().GetCellSize();
            BuildingElementView buildingElementView = _View.CreateBuilding(_position, _gridElementView, cellSize);
            buildingElementView.OnClicked += OnClickedBuilding;
            ServiceLocator.Instance.Get<ProductionController>().CancelSelection();
            Debug.Log("Building is successfully Placed");
        }
    }

    public void OnClickedBuilding(BuildingElementView _buildingElementView)
    {
        if (!ServiceLocator.Instance.Get<BuildingSystemController>().IsBuildingModeActive())
        {
            _SelectedBuildingElement = _buildingElementView;

            ProductionModel productionModel = ServiceLocator.Instance.Get<ProductionController>().GetProductionModelByID(_SelectedBuildingElement.GetData()._ProductionID);
            ServiceLocator.Instance.Get<InformationController>().ShowInformation(productionModel);
        }
    }

    public void OnCancelBuildingInformation()
    {
        if (_SelectedBuildingElement != null)
        {
            _SelectedBuildingElement = null;

            ServiceLocator.Instance.Get<InformationController>().HideInformation();
        }
    }

    public void OnBuildingModeCancelled()
    {
        _View.CancelBuildingMode();
    }

    public bool IsBuildingModeActive()
    {
        return (_View._SelectedBuilding != null && _View._SelectedProduction != null);
    }
}
