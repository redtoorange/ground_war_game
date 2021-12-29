using System.Collections.Generic;
using Godot;
using GroundWar.game.units;

public class Attacker : Node2D
{
    private SensingController sensingController;
    private BaseUnit currentTarget = null;

    private BaseUnit owningUnit;

    public override void _Ready()
    {
        owningUnit = GetParent<BaseUnit>();
        sensingController = GetNode<SensingController>("SensingController");
        sensingController.Connect("ThreatEnteredRange", this, nameof(HandleThreatEnteredRange));
        sensingController.Connect("ThreatExitedRange", this, nameof(HandleThreatExitedRange));
    }

    public override void _Process(float delta)
    {
        if (currentTarget != null)
        {
            owningUnit.RotateSprite((currentTarget.Position - owningUnit.Position).Angle());
        }
    }

    private void HandleThreatEnteredRange(BaseUnit unit)
    {
        GD.Print("Threat Entered Range");
        if (currentTarget == null)
        {
            currentTarget = unit;
        }
    }

    private void HandleThreatExitedRange(BaseUnit unit)
    {
        if (unit == currentTarget)
        {
            currentTarget = null;
            
            List<BaseUnit> threats = sensingController.GetAllThreats();
            if (threats.Count > 0)
            {
                currentTarget = threats[0];
            }
        }

        GD.Print("Threat Exited Range");
    }
}