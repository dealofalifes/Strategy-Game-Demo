using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class BuildingFactory : IBuildingFactory
{
    public ProductionModel CreateProduction(int _id)
    {
        switch (_id)
        {
            case 1001:
                return new PowerPlantModel()
                {
                    _Name = "Common Powerplant",
                    _ProductionID = 1001,
                    _MaxHealth = 50,
                    _ProductionSize = new(2, 2),
                    _EnergyPerSecond = 3,
                };
            case 1002:
                return new PowerPlantModel()
                {
                    _Name = "Uncommon Powerplant",
                    _ProductionID = 1002,
                    _MaxHealth = 50,
                    _ProductionSize = new(3, 2),
                    _EnergyPerSecond = 5,
                };
            case 1003:
                return new PowerPlantModel()
                {
                    _Name = "Rare Powerplant",
                    _ProductionID = 1003,
                    _MaxHealth = 50,
                    _ProductionSize = new(3, 3),
                    _EnergyPerSecond = 8,
                };
            case 1004:
                return new PowerPlantModel()
                {
                    _Name = "Unique Powerplant",
                    _ProductionID = 1004,
                    _MaxHealth = 50,
                    _ProductionSize = new(4, 3),
                    _EnergyPerSecond = 12,
                };
            case 1005:
                return new PowerPlantModel()
                {
                    _Name = "Legendary Powerplant",
                    _ProductionID = 1005,
                    _MaxHealth = 50,
                    _ProductionSize = new(5, 3),
                    _EnergyPerSecond = 18,
                };

            case 2001:
                return new BarrackModel()
                {
                    _Name = "Common Barrack",
                    _ProductionID = 2001,
                    _MaxHealth = 100,
                    _ProductionSize = new(1, 1),
                    _SoldierID = 1001,
                };
            case 2002:
                return new BarrackModel()
                {
                    _Name = "Rare Barrack",
                    _ProductionID = 2002,
                    _MaxHealth = 100,
                    _ProductionSize = new(2, 2),
                    _SoldierID = 2001,
                };
            case 2003:
                return new BarrackModel()
                {
                    _Name = "Legendary Barrack",
                    _ProductionID = 2003,
                    _MaxHealth = 100,
                    _ProductionSize = new(3, 4),
                    _SoldierID = 3001,
                };

            case 3001:
                return new WatchtowerModel()
                {
                    _Name = "Common Watchtower",
                    _ProductionID = 3001,
                    _MaxHealth = 150,
                    _ProductionSize = new(1, 1),
                    _Range = 1,
                };
            case 3002:
                return new WatchtowerModel()
                {
                    _Name = "Rare Watchtower",
                    _ProductionID = 3002,
                    _MaxHealth = 150,
                    _ProductionSize = new(1, 1),
                    _Range = 2,
                };
            case 3003:
                return new WatchtowerModel()
                {
                    _Name = "Legendary Watchtower",
                    _ProductionID = 3003,
                    _MaxHealth = 100,
                    _ProductionSize = new(1, 1),
                    _Range = 3,
                };

            case 4001:
                return new FarmAreaModel()
                {
                    _Name = "Common Farm Area",
                    _ProductionID = 4001,
                    _MaxHealth = 25,
                    _ProductionSize = new(4, 4),
                    _HarvestTime = 60,
                    _HarvestQuantity = 150,
                };
            case 4002:
                return new FarmAreaModel()
                {
                    _Name = "Rare Farm Area",
                    _ProductionID = 4002,
                    _MaxHealth = 35,
                    _ProductionSize = new(6, 6),
                    _HarvestTime = 50,
                    _HarvestQuantity = 250,
                };
            case 4003:
                return new FarmAreaModel()
                {
                    _Name = "Legendary Farm Area",
                    _ProductionID = 4003,
                    _MaxHealth = 50,
                    _ProductionSize = new(9, 9),
                    _HarvestTime = 40,
                    _HarvestQuantity = 400,
                };
            default:
                throw new ArgumentException("Bilinmeyen bina tipi");
        }
    }
}

public interface IBuildingFactory
{
    public ProductionModel CreateProduction(int _id);
}
