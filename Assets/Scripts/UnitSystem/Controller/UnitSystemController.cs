using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UnitSystemController : MonoBehaviour, IUnitSystem
{
    [Tooltip("Unit System View Component for this Controller")]
    [Header("View Class")]
    [SerializeField] private UnitSystemView _View;

    [Header("DEBUG")]
    [SerializeField] private List<UnitModel> _Units;
    [SerializeField] private List<UnitElementView> _MySoldiers;

    [HideInInspector][SerializeField] private UnitElementView _OnUnitElement;
    //---
    private IBuildingSystem _BuildingSystem;
    private IGridSystem _GridSystem;
    private IInformationSystem _InformationSystem;
    private INavigationSystem _NavigationSystem;

    private void Awake()
    {
        CreateSoldierModels();

        _View.OnCancelled += OnCancelled;
    }

    private void Start()
    {
        _BuildingSystem.OnTriggerAttack += OnTargettedDamagable;
        _GridSystem.OnClickedRight += OnUnitStartMove;
        _InformationSystem.OnUnitCreated += OnCreatedUnit;
    }

    public void Initialize(IBuildingSystem _buildingSystem, IGridSystem _gridSystem, IInformationSystem _informationSystem, INavigationSystem _navigationSystem)
    {
        _BuildingSystem = _buildingSystem;
        _GridSystem = _gridSystem;
        _InformationSystem = _informationSystem;
        _NavigationSystem = _navigationSystem;
    }

    private void CreateSoldierModels()
    {
        _Units = new();

        UnitModel soldier1 = new UnitModel()
        {
            UnitName = "Barbarian",
            UnitID = 1001,
            MaxHealth = 10,
            Damage = 2,
        };
        _Units.Add(soldier1);

        UnitModel soldier2 = new UnitModel()
        {
            UnitName = "Archer",
            UnitID = 2001,
            MaxHealth = 10,
            Damage = 5,
        };
        _Units.Add(soldier2);

        UnitModel soldier3 = new UnitModel()
        {
            UnitName = "Gunner",
            UnitID = 3001,
            MaxHealth = 10,
            Damage = 10,
        };
        _Units.Add(soldier3);
    }

    public UnitModel GetUnitModelByID(int _unitID)
    {
        foreach (var item in _Units)
        {
            if (item.UnitID == _unitID)
            {
                return item;
            }
        }

        return null;
    }

    public void OnCancelled()
    {
        if(_View.GetCurrentSelectedUnitElement() != null)
        {
            _InformationSystem.HideInformation();
            if(!_GridSystem.IsOnGridElement() && !_BuildingSystem.IsOnBuildingElement() && !IsOnUnitElement())
                _View.OnSelectedUnit(null);
        }
    }

    public bool CanCreateUnit(Vector2Int _doorPos)
    {
        bool canCreate = true;
        foreach (Transform item in _View.GetUnitElementsContent())
        {
            if (item.gameObject.activeSelf)
            {
                Vector2Int currentPos = item.GetComponent<UnitElementView>().GetUnitElementModelData().GetCurrentPosition();
                if (_doorPos == currentPos)
                {
                    canCreate = false;
                }
            }
        }

        return canCreate;
    }

    public void OnCreatedUnit(BarrackModel _barrackModel, UnitModel _unitModel, Vector2Int _doorPos)
    {
        Vector2Int cellSize = _GridSystem.GetCellSize();
        GridElementView gridElementView = _GridSystem.GetGridElementViewByPosition(_doorPos);

        UnitElementView unitElementView = _View.OnCreateUnit(gridElementView, _unitModel, _doorPos, cellSize);
        unitElementView.OnHover += OnHoverUnitElement;
        unitElementView.OnClickedLeft += OnClickedUnit;
        unitElementView.OnClickedRight += OnTargettedUnit;
        unitElementView.OnStepUpdate += OnStepStart;
        unitElementView.OnDead += OnAnUnitBeDestroyed;
        _MySoldiers.Add(unitElementView);
        _InformationSystem.HideInformation();
        ClearSelected();
    }

    public void ClearSelected()
    {
        _View.OnSelectedUnit(null);
    }

    public void OnClickedUnit(UnitElementView _unitElementView)
    {
        if (!_BuildingSystem.IsBuildingModeActive)
        {
            _BuildingSystem.OnCancelBuildingInformation();

            _View.OnSelectedUnit(_unitElementView);
            _InformationSystem.ShowInformation(_unitElementView.GetUnitModelData());
        }
    }

    public void OnTargettedUnit(UnitElementView _unitElementView)
    {
        
        if (!_BuildingSystem.IsBuildingModeActive)
        {
            UnitElementView currentUnit = _View.GetCurrentSelectedUnitElement();
            if (currentUnit != null)
            {
                _BuildingSystem.OnCancelBuildingInformation();

                Vector2Int start = currentUnit.GetUnitElementModelData().GetCurrentPosition();
                Vector2Int end = _unitElementView.GetUnitElementModelData().GetCurrentPosition();

                GridElementView startGridElement = _GridSystem.GetGridElementViewByPosition(start);
                GridElementView targetGridElement = _GridSystem.GetGridElementViewByPosition(end);
                List<GridElementView> path = _NavigationSystem.FindPath(startGridElement, targetGridElement);

                foreach (var item in path)
                    item.SetNewState(GridElementState.OnNavigation);

                _View.SetTargetUnit(currentUnit, path, _unitElementView);
            }
        }
    }

    public bool IsValidGrid(Vector2Int _pos)
    {
        return CanCreateUnit(_pos);
    }

    public void OnGridChanged()
    {
        _View.OnGridChanged(_MySoldiers);
    }

    public void OnStepStart()
    {
        _GridSystem.OnRechecktoPlaceBuilding();
    }

    public void OnUnitStartMove(GridElementView _targetGridElement)
    {
        UnitElementView currentUnit = _View.GetCurrentSelectedUnitElement();
        if (currentUnit != null)
        {
            Vector2Int start = currentUnit.GetUnitElementModelData().GetCurrentPosition();
            GridElementView startGridElement = _GridSystem.GetGridElementViewByPosition(start);
            List<GridElementView> path = _NavigationSystem.FindPath(startGridElement, _targetGridElement);

            foreach (var item in path)
                item.SetNewState(GridElementState.OnNavigation);

            _View.MoveUnit(currentUnit, path);
        }
    }

    public void OnTargettedDamagable(IDamagable _target)
    {
        UnitElementView currentUnit = _View.GetCurrentSelectedUnitElement();
        if (currentUnit != null)
        {
            Vector2Int start = currentUnit.GetUnitElementModelData().GetCurrentPosition();
            Vector2Int end = Vector2Int.zero;
            List<Vector2Int> endPoints = Helper.FindClosestEdgePoint(start, _target.GetPosition(), _target.GetSize());

            foreach (var item in endPoints)
            {
                Debug.Log(item);
            }
            foreach (var item in endPoints)
            {
                if (_GridSystem.GetGridElementViewByPosition(item).IsOccupied())
                    continue;

                if (!IsValidGrid(item))
                    continue;

                end = item;
                break;
            }

            if (end == Vector2Int.zero)
            {
                OnUnitStartMove(_GridSystem.GetGridElementViewByPosition(endPoints[0]));
                return;
            }
            GridElementView startGridElement = _GridSystem.GetGridElementViewByPosition(start);
            GridElementView endGridElement = _GridSystem.GetGridElementViewByPosition(end);

            List<GridElementView> path = _NavigationSystem.FindPath(startGridElement, endGridElement);

            foreach (var item in path)
                item.SetNewState(GridElementState.OnNavigation);

            _View.SetTargetUnit(currentUnit, path, _target);
        }
    }

    private void OnHoverUnitElement(UnitElementView _unitElementView)
    {
        _OnUnitElement = _unitElementView;
    }

    public bool IsOnUnitElement()
    {
        return _OnUnitElement != null;
    }

    public void OnAnUnitBeDestroyed(UnitElementView _unitElementView)
    {
        if (_MySoldiers.Contains(_unitElementView))
        {
            _unitElementView.OnHover -= OnHoverUnitElement;
            _unitElementView.OnClickedLeft -= OnClickedUnit;
            _unitElementView.OnStepUpdate -= OnStepStart;
            _unitElementView.OnDead -= OnAnUnitBeDestroyed;
            _MySoldiers.Remove(_unitElementView);
        }
    }

    private void OnDestroy()
    {
        _InformationSystem.OnUnitCreated -= OnCreatedUnit;
        _GridSystem.OnClickedRight -= OnUnitStartMove;
        _BuildingSystem.OnTriggerAttack -= OnTargettedDamagable;
        _View.OnCancelled -= OnCancelled;
    }
}
