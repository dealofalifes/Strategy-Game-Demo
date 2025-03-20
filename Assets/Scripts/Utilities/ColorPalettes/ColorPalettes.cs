using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPalettes : MonoBehaviour
{
    public static ColorPalettes Instance { get; private set; }

    [Header("Grid Border Colors")]
    [Tooltip("Place to init grid border colors depends on its state")]
    [SerializeField] private Color GridFreeBorder;
    [SerializeField] private Color GridHoveredBorder;
    [SerializeField] private Color GridBuildableBorder;
    [SerializeField] private Color GridNotBuildableBorder;
    [SerializeField] private Color GridBuiltBorder;
    [SerializeField] private Color GridOnNavigationBorder;
    [SerializeField] private Color GridOccupiedBySoldierBorder;

    private List<Color> GridBorderColors;

    [Header("Production Border Colors")]
    [Tooltip("Place to init productions border colors depends on its state")]
    [SerializeField] private Color ProductionFreeBorder;
    [SerializeField] private Color ProductionHoveredBorder;
    [SerializeField] private Color ProductionSelectedBorder;

    private List<Color> ProductionBorderColors;

    [Header("Building Border Colors")]
    [Tooltip("Place to init Buildings border colors depends on its state")]
    [SerializeField] private Color BuildingsFreeBorder;
    [SerializeField] private Color BuildingsHoveredBorder;
    [SerializeField] private Color BuildingsSelectedBorder;

    private List<Color> BuildingsBorderColors;

    [Header("Units Border Colors")]
    [Tooltip("Place to init Units border colors depends on its state")]
    [SerializeField] private Color UnitsFreeBorder;
    [SerializeField] private Color UnitsHoveredBorder;
    [SerializeField] private Color UnitsSelectedBorder;

    private List<Color> UnitsBorderColors;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        InitColors();
    }

    void InitColors()
    {
        //Grid Color Palettes
        GridBorderColors = new()
        {
            GridFreeBorder,
            GridHoveredBorder,
            GridBuildableBorder,
            GridNotBuildableBorder,
            GridBuiltBorder,
            GridOnNavigationBorder,
            GridOccupiedBySoldierBorder
        };

        int neededGridBorderColors = (int)GridElementState.Length;
        if (GridBorderColors.Count != neededGridBorderColors)
        {
            Debug.LogError("Looks like you forgot to Add/Remove color(s) for some states!");
            Debug.LogError("For details compare the 'GridElementState' Enum & 'Grid Border Colors' !");
        }

        //Production Color Palettes
        ProductionBorderColors = new()
        {
           ProductionFreeBorder,
           ProductionHoveredBorder,
           ProductionSelectedBorder,
        };

        int neededProductionBorderColors = (int)ProductionElementState.Length;
        if (ProductionBorderColors.Count != neededProductionBorderColors)
        {
            Debug.LogError("Looks like you forgot to Add/Remove color(s) for some states!");
            Debug.LogError("For details compare the 'ProductionElementState' Enum & 'Production Border Colors' !");
        }

        //Buildings Color Palettes
        BuildingsBorderColors = new()
        {
           BuildingsFreeBorder,
           BuildingsHoveredBorder,
           BuildingsSelectedBorder,
        };

        int neededBuildingBorderColors = (int)BuildingElementState.Length;
        if (BuildingsBorderColors.Count != neededBuildingBorderColors)
        {
            Debug.LogError("Looks like you forgot to Add/Remove color(s) for some states!");
            Debug.LogError("For details compare the 'BuildingElementState' Enum & 'Production Border Colors' !");
        }

        //Units Color Palettes
        UnitsBorderColors = new()
        {
           UnitsFreeBorder,
           UnitsHoveredBorder,
           UnitsSelectedBorder,
        };

        int neededUnitBorderColors = (int)UnitElementState.Length;
        if (UnitsBorderColors.Count != neededUnitBorderColors)
        {
            Debug.LogError("Looks like you forgot to Add/Remove color(s) for some states!");
            Debug.LogError("For details compare the 'BuildingElementState' Enum & 'Production Border Colors' !");
        }
    }

    public Color GetGridColorByState(GridElementState _state)
    {
        return GridBorderColors[(int)_state];
    }

    public Color GetProductionColorByState(ProductionElementState _state)
    {
        return ProductionBorderColors[(int)_state];
    }

    public Color GetBuildingColorByState(BuildingElementState _state)
    {
        return BuildingsBorderColors[(int)_state];
    }

    public Color GetUnitColorByState(UnitElementState _state)
    {
        return UnitsBorderColors[(int)_state];
    }
}
