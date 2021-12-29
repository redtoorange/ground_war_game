using System.Collections.Generic;
using Godot;
using GroundWar.game.units;

public class SensingController : Node2D
{
    [Signal]
    public delegate void ThreatEnteredRange(BaseUnit threat);

    [Signal]
    public delegate void ThreatExitedRange(BaseUnit threat);

    [Export] private string enemyGroup;

    private Area2D sensingArea;
    private List<BaseUnit> currentThreats = new List<BaseUnit>();

    public override void _Ready()
    {
        sensingArea = GetNode<Area2D>("SensingRange");

        sensingArea.Connect("area_entered", this, nameof(OnAreaEntered));
        sensingArea.Connect("area_exited", this, nameof(OnAreaExited));
    }

    public List<BaseUnit> GetAllThreats() => currentThreats;

    private void OnAreaEntered(Area2D other)
    {
        if (other.GetParent() is BaseUnit unit && unit.IsInGroup(enemyGroup))
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
        if (other.GetParent() is BaseUnit unit && unit.IsInGroup(enemyGroup))
        {
            if (currentThreats.Contains(unit))
            {
                currentThreats.Remove(unit);
                EmitSignal("ThreatExitedRange", unit);
            }
        }
    }
}