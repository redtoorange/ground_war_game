using System.Collections.Generic;
using Godot;

namespace GroundWar.game.units
{
    public class UnitManager : Node
    {
        private List<BaseUnit> allUnits;

        public override void _Ready()
        {
            GatherAll();
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