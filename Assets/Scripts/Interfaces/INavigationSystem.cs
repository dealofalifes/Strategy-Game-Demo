using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INavigationSystem
{
    public List<GridElementView> FindPath(GridElementView start, GridElementView goal);
}
