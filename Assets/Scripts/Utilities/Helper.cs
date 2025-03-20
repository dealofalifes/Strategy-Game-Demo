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

    public static List<Vector2Int> FindClosestEdgePoint(Vector2Int start, Vector2Int targetPosition, Vector2Int targetSize)
    {
        List<Vector2Int> edgePoints = new List<Vector2Int>();

        int xMin = targetPosition.x - 1;
        int xMax = targetPosition.x + targetSize.x;
        int yMin = targetPosition.y - 1;
        int yMax = targetPosition.y + targetSize.y;

        // Add all edge points (including corners)
        for (int x = xMin; x <= xMax; x++)
        {
            edgePoints.Add(new Vector2Int(x, yMin));  // Top edge
            edgePoints.Add(new Vector2Int(x, yMax));  // Bottom edge
        }

        for (int y = yMin; y <= yMax; y++)
        {
            edgePoints.Add(new Vector2Int(xMin, y));  // Left edge
            edgePoints.Add(new Vector2Int(xMax, y));  // Right edge
        }

        // Return the edge point closest to the start position
        return edgePoints.OrderBy(point => Vector2Int.Distance(start, point)).ToList();
    }
}
