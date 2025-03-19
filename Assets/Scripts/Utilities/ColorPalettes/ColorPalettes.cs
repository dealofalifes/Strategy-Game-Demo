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
    [SerializeField] private Color GridOccupiedBySoldierBorder;

    private List<Color> GridBorderColors;

    [Header("Production Border Colors")]
    [Tooltip("Place to init grid border colors depends on its state")]
    [SerializeField] private Color ProductionFreeBorder;
    [SerializeField] private Color ProductionHoveredBorder;
    [SerializeField] private Color ProductionSelectedBorder;

    private List<Color> ProductionBorderColors;

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
        GridBorderColors = new()
        {
            GridFreeBorder,
            GridHoveredBorder,
            GridBuildableBorder,
            GridNotBuildableBorder,
            GridBuiltBorder,
            GridOccupiedBySoldierBorder
        };

        int neededGridBorderColors = (int)GridElementState.Length;
        if (GridBorderColors.Count != neededGridBorderColors)
        {
            Debug.LogError("Looks like you forgot to Add/Remove color(s) for some states!");
            Debug.LogError("For details compare the 'GridElementState' Enum & 'Grid Border Colors' !");
        }


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
    }

    public Color GetGridColorByState(GridElementState _state)
    {
        return GridBorderColors[(int)_state];
    }

    public Color GetProductionColorByState(ProductionElementState _state)
    {
        return ProductionBorderColors[(int)_state];
    }
}
