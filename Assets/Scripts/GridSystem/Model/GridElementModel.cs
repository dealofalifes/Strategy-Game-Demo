using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridElementModel
{
    private Vector2Int _Pos;
    private int _ProductionID;

    public GridElementModel(GridElementModel _model)
    {
        _Pos = _model._Pos;
    }

    public GridElementModel(Vector2Int _pos)
    {
        _Pos = _pos;
    }

    public int Get_X()
    {
        return _Pos.x;
    }

    public int Get_Y()
    {
        return _Pos.y;
    }

    public int GetProductionID()
    {
        return _ProductionID;
    }

    public void SetProduction(int _productionID)
    {
        _ProductionID = _productionID;
    }

    public void RemoveProduction()
    {
        _ProductionID = 0;
    }
}
