using System.Collections.Generic;
using Godot;

namespace GroundWar.game.buildings
{
    public class BuildingManager : Node
    {
        private List<BaseBuilding> buildings;

        public override void _Ready()
        {
            GatherAll();
        }

        public List<BaseBuilding> GetAll() => buildings;

        public bool AddBuilding(BaseBuilding newBuilding)
        {
            bool success = false;

            if (!buildings.Contains(newBuilding))
            {
                buildings.Add(newBuilding);
                success = true;
            }

            return success;
        }

        public bool RemoveBuilding(BaseBuilding building)
        {
            bool success = false;
            if (buildings.Contains(building))
            {
                buildings.Remove(building);
                success = true;
            }

            return success;
        }

        private void GatherAll()
        {
            buildings = new List<BaseBuilding>();

            for (int i = 0; i < GetChildCount(); i++)
            {
                BaseBuilding unit = GetChildOrNull<BaseBuilding>(i);
                if (unit != null)
                {
                    buildings.Add(unit);
                }
            }
        }
    }
}