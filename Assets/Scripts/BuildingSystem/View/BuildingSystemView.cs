using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSystemView : MonoBehaviour
{
    [Header("Container Elements")]
    [SerializeField] private Transform _PowerplantsContainer;
    [SerializeField] private Transform _BarracksContainer;
    [SerializeField] private Transform _WatchtowersContainer;
    [SerializeField] private Transform _FarmAreasContainer;
    [SerializeField] private Transform _SoldiersContainer;

    [Header("DEBUG")]
    [SerializeField] public BuildingModel _SelectedBuilding;
    [SerializeField] public ProductionModel _SelectedProduction;

    [Tooltip("To get buildings from the pool")]
    [Header("Pool System")]
    [SerializeField] private List<GameObject> _BuildingsPool;

    public event Action OnCancelled;
    private void Start()
    {
        _BuildingsPool = new();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
            OnCancelled.Invoke();
    }

    public void ActivateBuildingMode(BuildingModel _buildingModel, ProductionModel _productionModel)
    {
        _SelectedBuilding = _buildingModel;
        _SelectedProduction = _productionModel;
    }

    public void CancelBuildingMode()
    {
        _SelectedBuilding = null;
        _SelectedProduction = null;
    }

    public BuildingElementView CreateBuilding(Vector2Int _position, GridElementView _gridElementView, Vector2Int _cellSize)
    {
        GameObject newBuildingElement = GetBuildingFromPool();

        switch (_SelectedProduction) //If you get error be mind that for this C# 7.0 is required.
        {
            case PowerPlantModel:
                newBuildingElement.transform.SetParent(_PowerplantsContainer);
                break;
            case BarrackModel:
                newBuildingElement.transform.SetParent(_BarracksContainer);
                break;
            case WatchtowerModel:
                newBuildingElement.transform.SetParent(_WatchtowersContainer);
                break;
            case FarmAreaModel:
                newBuildingElement.transform.SetParent(_FarmAreasContainer);
                break;
        }

        //if (_SelectedProduction is PowerPlantModel)
        //    newBuildingElement.transform.SetParent(_PowerplantsContainer);

        //if (_SelectedProduction is BarrackModel)
        //    newBuildingElement.transform.SetParent(_BarracksContainer);

        //if (_SelectedProduction is WatchtowerModel)
        //    newBuildingElement.transform.SetParent(_WatchtowersContainer);

        //if (_SelectedProduction is FarmAreaModel)
        //    newBuildingElement.transform.SetParent(_FarmAreasContainer);

        Vector2Int productionSize = _SelectedProduction._ProductionSize;
        Vector2Int gridSpace = Constants._GridSpace;

        RectTransform buildingElementRect = newBuildingElement.GetComponent<RectTransform>();
        buildingElementRect.position = _gridElementView.GetComponent<RectTransform>().position;
        buildingElementRect.sizeDelta = new Vector2Int(
            (_cellSize.x * productionSize.x) + ((productionSize.x - 1) * gridSpace.x),
            (_cellSize.y * productionSize.y) + ((productionSize.y - 1) * gridSpace.y));

        BuildingElementView buildingElementView = newBuildingElement.GetComponent<BuildingElementView>();
        BuildingModel currentBuildingModel = new BuildingModel()
        {
            _ProductionID = _SelectedProduction._ProductionID,
            _StartGrid = _position,
        };
        buildingElementView.SetData(currentBuildingModel);

        CancelBuildingMode();

        return buildingElementView;
    }

    public GameObject GetBuildingFromPool()
    {
        foreach (var item in _BuildingsPool)
        {
            if (!item.activeSelf)
            {
                return item;
            }
        }

        GameObject newBuildingElementPrefab = Resources.Load<GameObject>(Constants.BuildingResourcePath);
        GameObject newBuildingElement = Instantiate(newBuildingElementPrefab);
        _BuildingsPool.Add(newBuildingElement);
        return newBuildingElement;
    }
}
