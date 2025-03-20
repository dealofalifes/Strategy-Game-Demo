using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildingSystem
{
    bool IsBuildingModeActive { get; }

    public void ChecktoPlaceBuilding(Vector2Int _pos);

    public void PlaceBuilding(Vector2Int _pos, GridElementView _gridElementView);

    public void OnCancelBuildingInformation();

    public void OnBuildingModeCancelled();

    public bool IsOnBuildingElement();

    public void ActivateBuildingMode(BuildingModel _buildingModel);

    public event Action<IDamagable> OnTriggerAttack;
}
