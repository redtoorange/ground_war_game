using System.Collections.Generic;
using Godot;

namespace GroundWar.game.units
{
    /**
     * The UnitManager is just a Container that holds all the Units for a given player and allows for easy retrieval.
     */
    public class UnitManager : Node
    {
        [Export] private NodePath navigation2DPath;
        private Navigation2D navigation2D;

        private List<BaseUnit> allUnits;
        private Node2D unitNavigationContainer;

        public override void _Ready()
        {
            navigation2D = GetNode<Navigation2D>(navigation2DPath);
            unitNavigationContainer = GetNode<Node2D>("UnitNavigationContainer");
            
            GatherAll();

            for (int i = 0; i < allUnits.Count; i++)
            {
                allUnits[i].Initialize(navigation2D, unitNavigationContainer);
            }
        }

        public List<BaseUnit> GetAll() => allUnits;

        public bool AddUnit(BaseUnit newUnit)
        {
            bool success = false;
            if (!allUnits.Contains(newUnit))
            {
                allUnits.Add(newUnit);
                success = true;
            }

            return success;
        }

        public bool RemoveUnit(BaseUnit unit)
        {
            bool success = false;
            if (allUnits.Contains(unit))
            {
                allUnits.Remove(unit);
                success = true;
            }

            return success;
        }

        private void GatherAll()
        {
            allUnits = new List<BaseUnit>();

            for (int i = 0; i < GetChildCount(); i++)
            {
                BaseUnit unit = GetChildOrNull<BaseUnit>(i);
                if (unit != null)
                {
                    allUnits.Add(unit);
                }
            }
        }
    }
}