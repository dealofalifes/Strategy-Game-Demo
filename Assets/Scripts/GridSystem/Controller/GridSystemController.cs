using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GridSystemController : MonoBehaviour, IGridSystem
{
    [Tooltip("Grid System View Component for this Controller")]
    [Header("View Class")]
    [SerializeField] private GridSystemView _View;

    [Header("DEBUG")]
    [HideInInspector] [SerializeField] private bool _GridCreated;
    [HideInInspector] [SerializeField] private Vector2Int _GridSize;
    [HideInInspector] [SerializeField] private Vector2Int _CellSize;
    [SerializeField] private GridElementView[,] _GridElements;

    [HideInInspector][SerializeField] private GridElementView _OnGridElement;
    [HideInInspector][SerializeField] private Vector2Int _LastPosition;

    public event Action<GridElementView> OnClickedRight;

    //---
    private IBuildingSystem _BuildingSystem;
    private IUnitSystem _UnitSystem;
    private void Awake()
    {
        if (!_GridCreated)
        {
            Debug.LogWarning("There is no gridsize data entered. So map will be created with default size 25x25.");
            CreateGridArea(new(25, 25), new(64, 64));
            return;
        }

        //This is necessary. GridElementView[,] can not be saved in editor so we simply need to resync it on Awake.
        if (_GridElements == null || _GridElements.GetLength(0) != _GridSize.x || _GridElements.GetLength(1) != _GridSize.y)
        {
            _GridCreated = _GridSize != Vector2Int.zero;

            _GridElements = new GridElementView[_GridSize.x, _GridSize.y];
            _View.CreateGridMap(_GridElements, _CellSize);

            foreach (var gridElement in _GridElements)
            {
                gridElement.OnHover += OnHoverGridElement;
                gridElement.OnEnter += OnEnteredAGrid;
                gridElement.OnClickedLeft += OnClickedLeftAGrid;
                gridElement.OnClickedRight += OnClickedRightAGrid;
            }
        }
    }

    public void Initialize(IBuildingSystem _buildingSystem, IUnitSystem _unitSystem)
    {
        _BuildingSystem = _buildingSystem;
        _UnitSystem = _unitSystem;
    }

    public void CreateGridArea(Vector2Int _gridSize, Vector2Int _cellSize)
    {
        _GridSize = _gridSize;
        _CellSize = _cellSize;

        _GridCreated = _GridSize != Vector2Int.zero;

        _GridElements = new GridElementView[_GridSize.x, _GridSize.y];
        _View.CreateGridMap(_GridElements, _CellSize);
    }

    public bool CanPlaceBuilding(BuildingModel _building, ProductionModel _productionModel, Vector2Int _position)
    {
        _View.ResetBuildingStates();

        bool isBarrack = _productionModel is BarrackModel;
        bool canPlace = true;

        //Door position for Barrack to be able to create Soldier
        Vector2Int doorPos = Helper.FindDoorPosForBarrack(_position, _productionModel);

        if (!_UnitSystem.IsValidGrid(new(doorPos.x, doorPos.y)))
        {
            canPlace = false;
        }

        if (canPlace)
        {
            for (int x = 0; x < _productionModel._ProductionSize.x; x++)
            {
                for (int y = 0; y < _productionModel._ProductionSize.y; y++)
                {
                    Vector2Int gridPos = new Vector2Int(_position.x + x, _position.y + y);

                    if (gridPos.x >= _GridSize.x || gridPos.y >= _GridSize.y || gridPos.x < 0 || gridPos.y < 0)
                    {
                        //Set NotBuildable if it extends the limits.
                        canPlace = false;
                    }
                    else
                    {
                        //If it is occupied also set as NotBuildable
                        if (_GridElements[gridPos.x, gridPos.y].IsOccupied())
                        {
                            canPlace = false;
                        }
                    }

                    if (!_UnitSystem.IsValidGrid(new(gridPos.x, gridPos.y)))
                    {
                        canPlace = false;
                    }
                }
            }
        }

        if (isBarrack)
        {
            if (doorPos.x >= _GridSize.x || doorPos.y >= _GridSize.y || doorPos.x < 0 || doorPos.y < 0 ||
                _GridElements[doorPos.x, doorPos.y].IsOccupied())
            {
                canPlace = false;
            }
        }

        if (canPlace)
        {
            for (int x = 0; x < _productionModel._ProductionSize.x; x++)
            {
                for (int y = 0; y < _productionModel._ProductionSize.y; y++)
                {
                    Vector2Int gridPos = new Vector2Int(_position.x + x, _position.y + y);
                    _View.SetGridElementViewState(_GridElements[gridPos.x, gridPos.y], GridElementState.Buildable);
                }
            }

            if (isBarrack)
            {
                _View.SetGridElementViewState(_GridElements[doorPos.x, doorPos.y], GridElementState.BarrackDoor);
            }
        }
        else
        {
            for (int x = 0; x < _productionModel._ProductionSize.x; x++)
            {
                for (int y = 0; y < _productionModel._ProductionSize.y; y++)
                {
                    Vector2Int gridPos = new Vector2Int(_position.x + x, _position.y + y);
                    if (gridPos.x >= 0 && gridPos.x < _GridSize.x && gridPos.y >= 0 && gridPos.y < _GridSize.y)
                    {
                        _View.SetGridElementViewState(_GridElements[gridPos.x, gridPos.y], GridElementState.NotBuildable);
                    }
                }
            }
            
        }

        return canPlace;
    }

    private void OnHoverGridElement(GridElementView _gridElementView)
    {
        _OnGridElement = _gridElementView;
        if(_OnGridElement != null)
            _LastPosition = new(_OnGridElement.GetData().Get_X(), _OnGridElement.GetData().Get_Y());
    }

    private void OnEnteredAGrid(Vector2Int _position)
    {
        if (_BuildingSystem.IsBuildingModeActive)
        {
            _LastPosition = _position;
            _BuildingSystem.ChecktoPlaceBuilding(new(_position.x, _position.y));
        }
    }

    public void OnRechecktoPlaceBuilding()
    {
        if (_BuildingSystem.IsBuildingModeActive)
            _BuildingSystem.ChecktoPlaceBuilding(_LastPosition);
    }

    private void OnClickedLeftAGrid(Vector2Int _position, GridElementView _gridElement)
    {
        if (_BuildingSystem.IsBuildingModeActive)
            _BuildingSystem.PlaceBuilding(_position, _gridElement);
    }

    private void OnClickedRightAGrid(GridElementView _gridElement)
    {
        if (!_BuildingSystem.IsBuildingModeActive)
            OnClickedRight.Invoke(_gridElement);
    }

    public bool PlaceBuilding(BuildingModel _building, ProductionModel _productionModel, Vector2Int _position)
    {
        if (!CanPlaceBuilding(_building, _productionModel, _position)) return false;

        for (int x = 0; x < _productionModel._ProductionSize.x; x++)
        {
            for (int y = 0; y < _productionModel._ProductionSize.y; y++)
            {
                _GridElements[_position.x + x, _position.y + y].SetOccupied(_productionModel._ProductionID);
            }
        }

        bool isBarrack = _productionModel is BarrackModel;

        //Door position for Barrack to be able to create Soldier
        Vector2Int doorPos = Helper.FindDoorPosForBarrack(_position, _productionModel);

        if (isBarrack)
            _GridElements[doorPos.x, doorPos.y].SetOccupied(_productionModel._ProductionID);

        return true;
    }

    public void OnBuildingModeCancelled()
    {
        _View.ResetBuildingStates();
    }

    public GridElementView IsValidGrid(Vector2Int _pos)
    {
        if (_pos.x < 0 || _pos.y < 0 || _pos.x >= _GridSize.x || _pos.y >= _GridSize.y)
            return null;

        if (_GridElements[_pos.x, _pos.y].IsOccupied())
            return null;

        return _GridElements[_pos.x, _pos.y];
    }

    public Vector2Int GetGridSize()
    {
        return _GridSize;
    }

    public Vector2Int GetCellSize()
    {
        return _CellSize;
    }

    public void SetGridsFree(List<Vector2Int> _gridPositions)
    {
        foreach (var item in _gridPositions)
        {
            GridElementView currentGrid = GetGridElementViewByPosition(item);
            currentGrid.SetOccupied(0);
        }
    }

    public GridElementView GetGridElementViewByPosition(Vector2Int _pos)
    {
        foreach (var item in _GridElements)
        {
            if (item.GetData().Get_X() == _pos.x && item.GetData().Get_Y() == _pos.y)
            {
                return item;
            }
        }

        return null;
    }

    public bool IsOnGridElement()
    {
        return _OnGridElement != null;
    }
}
