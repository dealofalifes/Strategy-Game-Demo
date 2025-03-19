using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerPlantModel : ProductionModel
{
    public float _EnergyPerSecond; //Just an example data.

    public PowerPlantModel()
    {

    }

    public PowerPlantModel(PowerPlantModel _model) : base(_model)
    {
        _EnergyPerSecond = _model._EnergyPerSecond;
    }
}
