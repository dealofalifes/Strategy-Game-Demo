using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitElementModel : IStat //Save data for Units
{
    private int _UnitModelID;
    private Vector2Int _SpawnPoint;
    private Vector2Int _CurrentPosition;
    private Stats _Stats;

    public UnitElementModel(int _unitModelID, Vector2Int _spawnPoint, Vector2Int _currentPosition, Stats _stats)
    {
        _UnitModelID = _unitModelID;
        _SpawnPoint = _spawnPoint;
        _CurrentPosition = _currentPosition;
        _Stats = _stats;
    }

    public UnitElementModel(UnitElementModel _unit, float _maxHP)
    {
        _UnitModelID = _unit._UnitModelID;
        _SpawnPoint = _unit._SpawnPoint;
        _CurrentPosition = _unit._CurrentPosition;
        _Stats = new(_maxHP);
    }

    public Vector2Int GetCurrentPosition()
    {
        return _CurrentPosition;
    }

    public void SetCurrentPosition(Vector2Int _currentPos)
    {
        _CurrentPosition = _currentPos;
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
