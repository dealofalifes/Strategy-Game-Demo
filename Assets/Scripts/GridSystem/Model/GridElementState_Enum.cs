public enum GridElementState
{
    Free,              // The grid is empty and available
    Hovered,           // The mouse is currently over this grid element
    Buildable,         // The grid is available for building
    NotBuildable,      // The grid is not available for building
    BarrackDoor,       // Area to create soldiers in front of the Barrack
    OnNavigation,      // While a soldier moves on this road
    OccupiedBySoldier, // A soldier is currently positioned on this grid
    Length,            // to count easily
}
