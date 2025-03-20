using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitSystem
{
    public void ClearSelected();

    public void OnGridChanged();

    public bool CanCreateUnit(Vector2Int _doorPos);

    public bool IsValidGrid(Vector2Int _pos);

    public UnitModel GetUnitModelByID(int _unitID);
}
