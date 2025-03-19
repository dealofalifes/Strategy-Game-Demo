using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProductionModel
{
    public string _Name;
    //Normally, since I use details like name and description in translation,
    //I would get it as 'Language Database.Instance.GetText("P_ItemID")',
    //but since we are not going into that much detail here, I am doing it like this.

    public int _ProductionID;
    public float _MaxHealth;

    public Vector2Int _ProductionSize;

    public ProductionModel()
    {

    }

    public ProductionModel(ProductionModel _model)
    {
        _Name = _model._Name;
        _ProductionID = _model._ProductionID;
        _MaxHealth = _model._MaxHealth;
        _ProductionSize = _model._ProductionSize;
    }
}
