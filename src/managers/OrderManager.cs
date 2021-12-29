using System.Collections.Generic;
using Godot;
using GroundWar.game.orders;
using GroundWar.game.soldiers;

namespace GroundWar.managers
{
    public class OrderManager : Node2D
    {
        [Export] private bool debugging = true;
    
        [Export] private NodePath selectionManagerPath;
        [Export] private float unitSpacing = 50.0f;

        private SelectionManager selectionManager;

    
        private bool drawPoint = false;
        private Vector2 unitOrderAveragePosition;
        private Vector2 mouseTargetLocation;

        private Vector2 direction;
        private Vector2 perpDirection;
    
    
    
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
        private void GenerateUnitOrders(InputEventMouseButton inputEvent, List<BaseSoldier> units)
        {
            Vector2 mouseTarget = GetGlobalMousePosition();
        
            Vector2 frontLineDirection = CalculateFrontLine(units);
            float frontLineLength = (units.Count - 1) * unitSpacing;
            Vector2 frontStartPoint = mouseTarget + (frontLineDirection * -(frontLineLength / 2.0f));
        

            for (int i = 0; i < units.Count; i++)
            {
                units[i].AddOrder(
                    new MoveOrder(frontStartPoint + (frontLineDirection * unitSpacing * i))
                );
            }
        }

        private Vector2 CalculateFrontLine(List<BaseSoldier> units)
        {
            Vector2 averagePosition = Vector2.Zero;
            for (int i = 0; i < units.Count; i++)
            {
                averagePosition += units[i].Position;
            }

            averagePosition /= units.Count;
            unitOrderAveragePosition = averagePosition;
            mouseTargetLocation = GetGlobalMousePosition();

            direction = (mouseTargetLocation - unitOrderAveragePosition).Normalized();
            perpDirection = new Vector2(-direction.y, direction.x);

            if (debugging)
            {
                drawPoint = true;
                Update();
            }

            return perpDirection;
        }

        // What will this do?
        private void GenerateBuildingOrders()
        {
        }

        public override void _Draw()
        {
            if (drawPoint && debugging)
            {
                DrawCircle(unitOrderAveragePosition, 7.5f, Colors.Orange);
                DrawCircle(mouseTargetLocation, 7.5f, Colors.Blue);
                DrawLine(unitOrderAveragePosition, mouseTargetLocation, Colors.Black);
            
                DrawLine(mouseTargetLocation, mouseTargetLocation + (perpDirection * 100.0f), Colors.Brown);
                DrawLine(mouseTargetLocation, mouseTargetLocation + (perpDirection * -100.0f), Colors.Tan);
            }
        }
    }
}