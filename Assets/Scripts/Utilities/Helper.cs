using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class Helper
{
    public static Vector2Int FindDoorPosForBarrack(Vector2Int _position, ProductionModel _barrack)
    {
        int offset = Mathf.CeilToInt((float)_barrack._ProductionSize.x / 2) - 1;
        return new Vector2Int(_position.x + Mathf.Max(0, offset), _position.y + _barrack._ProductionSize.y);
    }
}
