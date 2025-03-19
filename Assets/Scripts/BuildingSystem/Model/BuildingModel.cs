using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildingModel
{
    public int _ProductionID;
    public Vector2Int _StartGrid;

    public BuildingModel()
    {

    }

    public BuildingModel(BuildingModel _model)
    {
        _ProductionID = _model._ProductionID;
        _StartGrid = _model._StartGrid;
    }
}
