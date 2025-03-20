using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NavigationController : MonoBehaviour, INavigationSystem
{
    //------
    private IGridSystem _GridSystem;
    private IUnitSystem _UnitSystem;

    public void Initialize(IGridSystem _gridSystem, IUnitSystem _unitSystem)
    {
        _GridSystem = _gridSystem;
        _UnitSystem = _unitSystem;
    }

    public List<GridElementView> FindPath(GridElementView start, GridElementView goal)
    {
        List<NavigationGridModel> openSet = new List<NavigationGridModel>();
        HashSet<GridElementView> closedSet = new HashSet<GridElementView>();

        NavigationGridModel startNode = new NavigationGridModel(start, null, 0, GetHeuristic(start, goal));
        openSet.Add(startNode);

        NavigationGridModel closestNode = startNode;

        while (openSet.Count > 0)
        {
            NavigationGridModel currentNode = openSet.OrderBy(n => n.F).First();
       
            if (currentNode.Position == goal)
            {
                return ReconstructPath(currentNode);
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode.Position);

            foreach (GridElementView neighbor in GetValidNeighbors(currentNode.Position))
            {
                if (closedSet.Contains(neighbor))
                    continue;

                float tentativeG = currentNode.G + 1; //In our grid system every step costs 1;

                NavigationGridModel existingNode = openSet.FirstOrDefault(n => n.Position == neighbor);
                if (existingNode == null)
                {
                    //new point to set
                    NavigationGridModel newNode = new NavigationGridModel(neighbor, currentNode, tentativeG, GetHeuristic(neighbor, goal));
                    openSet.Add(newNode);

                    if (GetHeuristic(neighbor, goal) < GetHeuristic(closestNode.Position, goal))
                    {
                        closestNode = newNode;
                    }
                }
                else if (tentativeG < existingNode.G)
                {
                    //Better point to set
                    existingNode.G = tentativeG;
                    existingNode.Parent = currentNode;
                }
            }
        }

        return ReconstructPath(closestNode); //Can not be accessed, Max possible path.
    }

    private float GetHeuristic(GridElementView _pos, GridElementView _goal)
    {
        return Mathf.Abs(_pos.GetData().Get_X() - _goal.GetData().Get_X()) 
            + Mathf.Abs(_pos.GetData().Get_Y() - _goal.GetData().Get_Y()); // Manhattan Distance
    }

    private List<GridElementView> GetValidNeighbors(GridElementView _position)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>
        {
            new Vector2Int(_position.GetData().Get_X() + 1, _position.GetData().Get_Y()),
            new Vector2Int(_position.GetData().Get_X() - 1, _position.GetData().Get_Y()),
            new Vector2Int(_position.GetData().Get_X(), _position.GetData().Get_Y() + 1),
            new Vector2Int(_position.GetData().Get_X(), _position.GetData().Get_Y() - 1)
        };

        List<GridElementView> neighborsFinal = new();
        foreach (Vector2Int item in neighbors)
        {
            GridElementView g = IsValidGrid(item);
            if (g != null)
                neighborsFinal.Add(g);
        }

        return neighborsFinal;
    }

    private GridElementView IsValidGrid(Vector2Int _pos)
    {
        if (!_UnitSystem.IsValidGrid(_pos))
            return null;

        GridElementView g = _GridSystem.IsValidGrid(_pos);
        return g;
    }

    private List<GridElementView> ReconstructPath(NavigationGridModel _currentNode)
    {
        List<GridElementView> path = new List<GridElementView>();

        while (_currentNode != null)
        {
            path.Add(_currentNode.Position);
            _currentNode = _currentNode.Parent;
        }

        path.Reverse();
        return path;
    }
}
