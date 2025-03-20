using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGridSystem
{
    public bool CanPlaceBuilding(BuildingModel _building, ProductionModel _productionModel, Vector2Int _position);

    public bool PlaceBuilding(BuildingModel _building, ProductionModel _productionModel, Vector2Int _position);

    public Vector2Int GetCellSize();

    public event Action<GridElementView> OnClickedRight;

    public bool IsOnGridElement();

    public GridElementView GetGridElementViewByPosition(Vector2Int _pos);

    public void OnRechecktoPlaceBuilding();

    public void OnBuildingModeCancelled();

    public void SetGridsFree(List<Vector2Int> _gridPositions);

    public GridElementView IsValidGrid(Vector2Int _pos);
}
