using System.Collections.Generic;
using Godot;
using Godot.Collections;
using GroundWar.game.buildings;
using GroundWar.game.units;

namespace GroundWar.managers
{
    enum SelectionMode
    {
        NORMAL,
        ADD,
        REMOVE
    }

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
        private SelectionMode selectionMode = SelectionMode.NORMAL;

        public override void _Ready()
        {
            selectionSquareController = GetNode<SelectionSquareController>("SelectionSquareController");
            unitManager = GetNode<UnitManager>(unitManagerPath);
            buildingManager = GetNode<BuildingManager>(buildingManagerPath);


            selectionSquareController.Connect("FinishedDragging", this, nameof(OnFinishedDragging));

            rng.Randomize();
        }

        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventKey keyEvent)
            {
                if (keyEvent.IsActionPressed("selection_add"))
                {
                    selectionMode = SelectionMode.ADD;
                }
                else if (keyEvent.IsActionReleased("selection_add"))
                {
                    selectionMode = SelectionMode.NORMAL;
                }
                else if (keyEvent.IsActionPressed("selection_remove"))
                {
                    selectionMode = SelectionMode.REMOVE;
                }
                else if (keyEvent.IsActionReleased("selection_remove"))
                {
                    selectionMode = SelectionMode.NORMAL;
                }
            }
        }


        private void OnFinishedDragging(Rect2 selectionRect, Area2D selectionArea, SelectionType selectionType)
        {
            if (selectionMode == SelectionMode.NORMAL)
            {
                ClearUnits();
                ClearBuildings();
            }

            List<BaseUnit> ownedUnits = unitManager.GetAll();
            List<BaseBuilding> ownedBuildings = buildingManager.GetAll();
            
            Array overlappingAreas = selectionArea.GetOverlappingAreas();
            List<BaseUnit> unitsInArea = new List<BaseUnit>();
            List<BaseBuilding> buildingsInArea = new List<BaseBuilding>();

            for (int i = 0; i < overlappingAreas.Count; i++)
            {
                Area2D n = overlappingAreas[i] as Area2D;
                if (n.GetParent() is BaseUnit unit && ownedUnits.Contains(unit))
                {
                    unitsInArea.Add(unit);
                }
                else if (n.GetParent() is BaseBuilding building && ownedBuildings.Contains(building))
                {
                    buildingsInArea.Add(building);
                }
            }

            if (unitsInArea.Count > 0)
            {
                if (selectionType == SelectionType.SINGLE)
                {
                    int index = rng.RandiRange(0, unitsInArea.Count - 1);

                    if (selectionMode == SelectionMode.ADD || selectionMode == SelectionMode.NORMAL)
                    {
                        if (!selectedUnits.Contains(unitsInArea[index]))
                        {
                            selectedUnits.Add(unitsInArea[index]);
                            unitsInArea[index].SetSelected(true);
                        }
                    }
                    else if (selectionMode == SelectionMode.REMOVE)
                    {
                        selectedUnits.Remove(unitsInArea[index]);
                        unitsInArea[index].SetSelected(false);
                    }
                }
                else if (selectionType == SelectionType.GROUP)
                {
                    for (int u = 0; u < unitsInArea.Count; u++)
                    {
                        BaseUnit unit = unitsInArea[u];
                        if (unit != null)
                        {
                            if (selectionMode == SelectionMode.ADD || selectionMode == SelectionMode.NORMAL)
                            {
                                if (!selectedUnits.Contains(unit))
                                {
                                    selectedUnits.Add(unit);
                                    unit.SetSelected(true);
                                }
                            }
                            else if (selectionMode == SelectionMode.REMOVE)
                            {
                                selectedUnits.Remove(unit);
                                unit.SetSelected(false);
                            }
                        }
                    }
                }
            }


            // Select Buildings
            if (selectedUnits.Count == 0 && buildingsInArea.Count > 0)
            {
                if (selectionType == SelectionType.SINGLE)
                {
                    int index = rng.RandiRange(0, buildingsInArea.Count - 1);
                    if (selectionMode == SelectionMode.ADD || selectionMode == SelectionMode.NORMAL)
                    {
                        if (!selectedBuildings.Contains(buildingsInArea[index]))
                        {
                            selectedBuildings.Add(buildingsInArea[index]);
                            buildingsInArea[index].SetSelected(true);
                        }
                    }
                    else if (selectionMode == SelectionMode.REMOVE)
                    {
                        selectedBuildings.Remove(buildingsInArea[index]);
                        buildingsInArea[index].SetSelected(false);
                    }
                }
                else if (selectionType == SelectionType.GROUP)
                {
                    for (int u = 0; u < buildingsInArea.Count; u++)
                    {
                        BaseBuilding building = buildingsInArea[u];
                        if (building != null)
                        {
                            if (selectionMode == SelectionMode.ADD || selectionMode == SelectionMode.NORMAL)
                            {
                                if (!selectedBuildings.Contains(building))
                                {
                                    selectedBuildings.Add(building);
                                    building.SetSelected(true);
                                }
                            }
                            else if (selectionMode == SelectionMode.REMOVE)
                            {
                                selectedBuildings.Remove(building);
                                building.SetSelected(false);
                            }
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