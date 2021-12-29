using System.Collections.Generic;
using Godot;

namespace GroundWar.game.soldiers
{
    /**
     * The UnitManager is just a Container that holds all the Units for a given player and allows for easy retrieval.
     */
    public class UnitManager : Node
    {
        [Export] private NodePath navigation2DPath;
        private Navigation2D navigation2D;

        private List<BaseSoldier> allUnits;
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

        public List<BaseSoldier> GetAll() => allUnits;

        public bool AddUnit(BaseSoldier newSoldier)
        {
            bool success = false;
            if (!allUnits.Contains(newSoldier))
            {
                allUnits.Add(newSoldier);
                success = true;
            }

            return success;
        }

        public bool RemoveUnit(BaseSoldier soldier)
        {
            bool success = false;
            if (allUnits.Contains(soldier))
            {
                allUnits.Remove(soldier);
                success = true;
            }

            return success;
        }

        private void GatherAll()
        {
            allUnits = new List<BaseSoldier>();

            for (int i = 0; i < GetChildCount(); i++)
            {
                BaseSoldier soldier = GetChildOrNull<BaseSoldier>(i);
                if (soldier != null)
                {
                    allUnits.Add(soldier);
                }
            }
        }
    }
}