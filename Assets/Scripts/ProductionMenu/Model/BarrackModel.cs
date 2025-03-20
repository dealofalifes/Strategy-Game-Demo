using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BarrackModel : ProductionModel
{
    public int _SoldierID; //ID of soldier to produce

    public BarrackModel()
    {

    }

    public BarrackModel(BarrackModel _model) : base(_model)
    {
        _SoldierID = _model._SoldierID;
    }
}
