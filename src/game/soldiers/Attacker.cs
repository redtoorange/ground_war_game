using System.Collections.Generic;
using Godot;
using GroundWar.game.soldiers.ai;

namespace GroundWar.game.soldiers
{
    public class Attacker : Node2D
    {
        private SensingController sensingController;
        private BaseSoldier currentTarget = null;

        private BaseSoldier owningSoldier;

        public override void _Ready()
        {
            owningSoldier = GetParent<BaseSoldier>();
            sensingController = GetNode<SensingController>("SensingController");
            sensingController.Connect("ThreatEnteredRange", this, nameof(HandleThreatEnteredRange));
            sensingController.Connect("ThreatExitedRange", this, nameof(HandleThreatExitedRange));
        }

        public override void _Process(float delta)
        {
            if (currentTarget != null)
            {
                owningSoldier.RotateSprite((currentTarget.Position - owningSoldier.Position).Angle());
            }
        }

        private void HandleThreatEnteredRange(BaseSoldier soldier)
        {
            GD.Print("Threat Entered Range");
            if (currentTarget == null)
            {
                currentTarget = soldier;
            }
        }

        private void HandleThreatExitedRange(BaseSoldier soldier)
        {
            if (soldier == currentTarget)
            {
                currentTarget = null;
            
                List<BaseSoldier> threats = sensingController.GetAllThreats();
                if (threats.Count > 0)
                {
                    currentTarget = threats[0];
                }
            }

            GD.Print("Threat Exited Range");
        }
    }
}