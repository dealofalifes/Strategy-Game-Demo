using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public static class Helper
{
    public static Vector2Int FindDoorPosForBarrack(Vector2Int _position, ProductionModel _barrack)
    {
        int offset = Mathf.CeilToInt((float)_barrack._ProductionSize.x / 2) - 1;
        return new Vector2Int(_position.x + Mathf.Max(0, offset), _position.y + _barrack._ProductionSize.y);
    }

    public static Vector2Int FindClosestEdgePoint(Vector2Int start, Vector2Int targetPosition, Vector2Int targetSize)
    {
        List<Vector2Int> edgePoints = new List<Vector2Int>();

        int xMin = targetPosition.x;
        int xMax = targetPosition.x + targetSize.x - 1;
        int yMin = targetPosition.y;
        int yMax = targetPosition.y + targetSize.y - 1;

        for (int x = xMin; x <= xMax; x++)
        {
            edgePoints.Add(new Vector2Int(x, yMin));
            edgePoints.Add(new Vector2Int(x, yMax));
        }
        for (int y = yMin; y <= yMax; y++)
        {
            edgePoints.Add(new Vector2Int(xMin, y));
            edgePoints.Add(new Vector2Int(xMax, y));
        }

        return edgePoints.OrderBy(point => Vector2.Distance(start, point)).First();
    }
}
