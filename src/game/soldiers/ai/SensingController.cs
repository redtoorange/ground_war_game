using System.Collections.Generic;
using Godot;

namespace GroundWar.game.soldiers.ai
{
    public class SensingController : Node2D
    {
        [Signal]
        public delegate void ThreatEnteredRange(BaseSoldier threat);

        [Signal]
        public delegate void ThreatExitedRange(BaseSoldier threat);

        [Export] private string enemyGroup;

        private Area2D sensingArea;
        private List<BaseSoldier> currentThreats = new List<BaseSoldier>();

        public override void _Ready()
        {
            sensingArea = GetNode<Area2D>("SensingRange");

            sensingArea.Connect("area_entered", this, nameof(OnAreaEntered));
            sensingArea.Connect("area_exited", this, nameof(OnAreaExited));
        }

        public List<BaseSoldier> GetAllThreats() => currentThreats;

        private void OnAreaEntered(Area2D other)
        {
            if (other.GetParent() is BaseSoldier unit && unit.IsInGroup(enemyGroup))
            {
                if (!currentThreats.Contains(unit))
                {
                    currentThreats.Add(unit);
                    EmitSignal("ThreatEnteredRange", unit);
                }
            }
        }

        private void OnAreaExited(Area2D other)
        {
            if (other.GetParent() is BaseSoldier unit && unit.IsInGroup(enemyGroup))
            {
                if (currentThreats.Contains(unit))
                {
                    currentThreats.Remove(unit);
                    EmitSignal("ThreatExitedRange", unit);
                }
            }
        }
    }
}