using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInformationSystem
{
    public void ShowInformation(BuildingElementView _productionElementView, ProductionModel _productionModel);

    public void ShowInformation(UnitModel _model);

    public void ShowInformation(ProductionModel _model);

    public void HideInformation();

    public event Action<BarrackModel, UnitModel, Vector2Int> OnUnitCreated;
}
