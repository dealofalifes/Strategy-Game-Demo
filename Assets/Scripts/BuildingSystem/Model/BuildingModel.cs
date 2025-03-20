using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildingModel : IStat
{
    private int _ProductionID;
    private Vector2Int _Size;
    private Vector2Int _Pos;
    private Stats _Stats;

    public BuildingModel(int _productionID, Vector2Int _size, Vector2Int _pos, Stats _stats)
    {
        _ProductionID = _productionID;
        _Size = _size;
        _Pos = _pos;
        _Stats = _stats;
    }

    public int GetProductionID()
    {
        return _ProductionID;
    }

    public Vector2Int GetPosition()
    {
        return _Pos;
    }

    public Vector2Int GetSize()
    {
        return _Size;
    }

    public bool TakeDamage(float _amount)
    {
        return _Stats.TakeDamage(_amount);
    }

    public (float _maxHealth, float _currentHealth, float _ratio) GetHealthRatio()
    {
        return (_Stats.GetMaxHealth(), _Stats.GetCurrentHealth(), (_Stats.GetCurrentHealth() / _Stats.GetMaxHealth()));
    }
}
