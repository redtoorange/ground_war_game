using System.Collections.Generic;
using Godot;
using Godot.Collections;
using GroundWar.game.buildings;
using GroundWar.game.units;

namespace GroundWar.managers
{
    public class SelectionManager : Node
    {
        [Export] private float selectionAreaThreshold = 5.0f;

        [Export] private NodePath unitManagerPath;
        [Export] private NodePath buildingManagerPath;

        private UnitManager unitManager;
        private BuildingManager buildingManager;

        private SelectionSquareController selectionSquareController;

        private List<BaseUnit> selectedUnits = new List<BaseUnit>();
        private List<BaseBuilding> selectedBuildings = new List<BaseBuilding>();
        private RandomNumberGenerator rng = new RandomNumberGenerator();

        public override void _Ready()
        {
            selectionSquareController = GetNode<SelectionSquareController>("SelectionSquareController");
            unitManager = GetNode<UnitManager>(unitManagerPath);
            buildingManager = GetNode<BuildingManager>(buildingManagerPath);


            selectionSquareController.Connect("FinishedDragging", this, nameof(OnFinishedDragging));

            rng.Randomize();
        }


        private void OnFinishedDragging(Rect2 selectionRect, Area2D selectionArea, SelectionType selectionType)
        {
            ClearUnits();
            ClearBuildings();

            Array overlappingAreas = selectionArea.GetOverlappingAreas();
            List<BaseUnit> units = new List<BaseUnit>();
            List<BaseBuilding> buildings = new List<BaseBuilding>();
            for (int i = 0; i < overlappingAreas.Count; i++)
            {
                Area2D n = overlappingAreas[i] as Area2D;
                if (n.GetParent() is BaseUnit unit)
                {
                    units.Add(unit);
                }
                else if (n.GetParent() is BaseBuilding building)
                {
                    buildings.Add(building);
                }
            }

            if (units.Count > 0)
            {
                if (selectionType == SelectionType.SINGLE)
                {
                    int index = rng.RandiRange(0, units.Count - 1);
                    selectedUnits.Add(units[index]);
                    units[index].SetSelected(true);
                }
                else if (selectionType == SelectionType.GROUP)
                {
                    for (int u = 0; u < units.Count; u++)
                    {
                        BaseUnit unit = units[u];
                        if (unit != null)
                        {
                            selectedUnits.Add(unit);
                            unit.SetSelected(true);
                        }
                    }
                }
            }


            // Select Buildings
            if (selectedUnits.Count == 0 && buildings.Count > 0)
            {
                if (selectionType == SelectionType.SINGLE)
                {
                    int index = rng.RandiRange(0, buildings.Count - 1);
                    selectedBuildings.Add(buildings[index]);
                    buildings[index].SetSelected(true);
                }
                else if (selectionType == SelectionType.GROUP)
                {
                    for (int u = 0; u < buildings.Count; u++)
                    {
                        BaseBuilding building = buildings[u];
                        if (building != null)
                        {
                            selectedBuildings.Add(building);
                            building.SetSelected(true);
                        }
                    }
                }
            }
        }

        private void ClearUnits()
        {
            for (int i = 0; i < selectedUnits.Count; i++)
            {
                selectedUnits[i].SetSelected(false);
            }

            selectedUnits.Clear();
        }

        private void ClearBuildings()
        {
            for (int i = 0; i < selectedBuildings.Count; i++)
            {
                selectedBuildings[i].SetSelected(false);
            }

            selectedBuildings.Clear();
        }

        public bool HasUnitsSelected() => selectedUnits.Count > 0;

        public List<BaseUnit> GetSelectedUnits() => selectedUnits;

        public bool HasBuildingsSelected() => selectedBuildings.Count > 0;

        public List<BaseBuilding> GetSelectedBuildings() => selectedBuildings;
    }
}