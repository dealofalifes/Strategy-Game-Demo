using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationGridModel
{
    public GridElementView Position;
    public NavigationGridModel Parent;
    public float G;
    public float H;
    public float F => G + H;

    public NavigationGridModel(GridElementView _position, NavigationGridModel _parent, float _g, float _h)
    {
        Position = _position;
        Parent = _parent;
        G = _g;
        H = _h;
    }
}
