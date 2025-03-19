using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BarrackModel : ProductionModel
{
    public int _SoldierLevel; //Level of soldier to produce

    public BarrackModel()
    {

    }

    public BarrackModel(BarrackModel _model) : base(_model)
    {
        _SoldierLevel = _model._SoldierLevel;
    }
}
