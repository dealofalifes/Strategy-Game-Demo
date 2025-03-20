using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Building System")]
    [SerializeField] private BuildingSystemController _BuildingSystem;

    [Header("Grid System")]
    [SerializeField] private GridSystemController _GridSystem;

    [Header("Information System")]
    [SerializeField] private InformationController _InformationSystem;

    [Header("Navigation System")]
    [SerializeField] private NavigationController _NavigationSystem;

    [Header("Production System")]
    [SerializeField] private ProductionController _ProductionSystem;

    [Header("Unit System")]
    [SerializeField] private UnitSystemController _UnitSystem;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        Initialize();
    }

    private void Initialize()
    {
        CheckNullReferences();

        _BuildingSystem.Initialize(_GridSystem, _InformationSystem, _ProductionSystem, _UnitSystem);
        _GridSystem.Initialize(_BuildingSystem, _UnitSystem);
        _InformationSystem.Initialize(_ProductionSystem, _UnitSystem);
        _NavigationSystem.Initialize(_GridSystem, _UnitSystem);
        _ProductionSystem.Initialize(_BuildingSystem, _GridSystem, _InformationSystem, _UnitSystem);
        _UnitSystem.Initialize(_BuildingSystem, _GridSystem, _InformationSystem, _NavigationSystem);
    }

    private void CheckNullReferences()
    {
        if (_BuildingSystem == null)
            Debug.LogError("BuildingSystemController is null in the GameManager");

        if (_GridSystem == null)
            Debug.LogError("GridSystemController is null in the GameManager");

        if (_InformationSystem == null)
            Debug.LogError("InformationController is null in the GameManager");

        if (_NavigationSystem == null)
            Debug.LogError("NavigationController is null in the GameManager");

        if (_ProductionSystem == null)
            Debug.LogError("ProductionController is null in the GameManager");

        if (_UnitSystem == null)
            Debug.LogError("UnitSystemController is null in the GameManager");
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_BuildingSystem == null)
            _BuildingSystem = FindFirstObjectByType<BuildingSystemController>();

        if (_GridSystem == null)
            _GridSystem = FindFirstObjectByType<GridSystemController>();

        if (_InformationSystem == null)
            _InformationSystem = FindFirstObjectByType<InformationController>();

        if (_NavigationSystem == null)
            _NavigationSystem = FindFirstObjectByType<NavigationController>();

        if (_ProductionSystem == null)
            _ProductionSystem = FindFirstObjectByType<ProductionController>();

        if (_UnitSystem == null)
            _UnitSystem = FindFirstObjectByType<UnitSystemController>();

        EditorUtility.SetDirty(this);
    }
#endif
}
