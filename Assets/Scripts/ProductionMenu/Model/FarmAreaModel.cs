using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmAreaModel : ProductionModel
{
    public float _HarvestTime;
    public int _HarvestQuantity;

    public FarmAreaModel()
    {

    }

    public FarmAreaModel(FarmAreaModel _model) : base(_model)
    {
        _HarvestTime = _model._HarvestTime;
        _HarvestQuantity = _model._HarvestQuantity;
    }
}
