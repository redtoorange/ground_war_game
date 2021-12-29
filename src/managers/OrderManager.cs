using System.Collections.Generic;
using Godot;
using GroundWar.game.orders;
using GroundWar.game.units;
using GroundWar.managers;

public class OrderManager : Node2D
{
    [Export] private NodePath selectionManagerPath;

    private SelectionManager selectionManager;

    public override void _Ready()
    {
        selectionManager = GetNode<SelectionManager>(selectionManagerPath);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton inputEvent)
        {
            if (inputEvent.ButtonIndex == (int)ButtonList.Right && inputEvent.Pressed)
            {
                if (selectionManager.HasUnitsSelected())
                {
                    GenerateUnitOrders(inputEvent, selectionManager.GetSelectedUnits());
                }
                else if (selectionManager.HasBuildingsSelected())
                {
                    GenerateBuildingOrders();
                }
            }
        }
    }

    // Move, Attack, Capture... Depends on what the user clicked on...
    private void GenerateUnitOrders(InputEventMouseButton inputEvent, List<BaseUnit> units)
    {
        for (int i = 0; i < units.Count; i++)
        {
            units[i].AddOrder(
                new MoveOrder(GetGlobalMousePosition())
            );
        }
    }

    // What will this do?
    private void GenerateBuildingOrders()
    {
    }
}