using System.Collections.Generic;
using Godot;
using GroundWar.game.buildings;
using GroundWar.game.units;

namespace GroundWar.managers
{
    public class SelectionManager : Node
    {
        [Export] private NodePath unitManagerPath;
        [Export] private NodePath buildingManagerPath;

        private UnitManager unitManager;
        private BuildingManager buildingManager;

        private SelectionSquareController selectionSquareController;

        private List<BaseUnit> selectedUnits = new List<BaseUnit>();
        private List<BaseBuilding> selectedBuildings = new List<BaseBuilding>();

        public override void _Ready()
        {
            selectionSquareController = GetNode<SelectionSquareController>("SelectionSquareController");
            unitManager = GetNode<UnitManager>(unitManagerPath);
            buildingManager = GetNode<BuildingManager>(buildingManagerPath);

            selectionSquareController.Connect("FinishedDragging", this, nameof(OnFinishedDragging));
        }

        private void OnFinishedDragging(Rect2 bounds)
        {
            ClearUnits();
            ClearBuildings();

            List<BaseUnit> allUnits = unitManager.GetAll();
            for (int u = 0; u < allUnits.Count; u++)
            {
                BaseUnit unit = allUnits[u];
                if (unit != null && bounds.HasPoint(unit.Position))
                {
                    selectedUnits.Add(unit);
                    unit.SetSelected(true);
                }
            }

            // Select Buildings
            if (selectedUnits.Count == 0)
            {
                List<BaseBuilding> allBuildings = buildingManager.GetAll();
                for (int b = 0; b < allBuildings.Count; b++)
                {
                    BaseBuilding building = allBuildings[b];
                    if (building != null && bounds.HasPoint(building.Position))
                    {
                        selectedBuildings.Add(building);
                        building.SetSelected(true);
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
    }
}