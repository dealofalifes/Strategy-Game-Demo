using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WatchtowerModel : ProductionModel
{
    public int _Range; //Just an example.

    public WatchtowerModel()
    {

    }

    public WatchtowerModel(WatchtowerModel _model) : base (_model)
    {
        _Range = _model._Range;
    }
}
