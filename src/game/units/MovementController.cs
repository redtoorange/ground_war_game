using System.Collections.Generic;
using Godot;

namespace GroundWar.game.units
{
    public class MovementController : Node2D
    {
        [Export] private float acceptanceRange = 5.0f;
        [Export] private float speed = 50.0f;
        
        private Line2D movementRenderer;

        private BaseUnit owningUnit;
        private Navigation2D navigation2D;
        private Node2D navigationPathContainer;
        
        private Vector2[] navigationPath;
        private int pathIndex;
        private Vector2 movementDirection;

        private bool drawPath = false;
        
        public void Initialize(BaseUnit owningUnit, Navigation2D navigation2D, Node2D navigationPathContainer)
        {
            this.owningUnit = owningUnit;
            this.navigation2D = navigation2D;
            this.navigationPathContainer = navigationPathContainer;

            movementRenderer = new Line2D();
            movementRenderer.DefaultColor = Colors.Red;
            movementRenderer.Width = 2.5f;
            movementRenderer.Name = this.owningUnit.Name + "_PathRenderer";
            this.navigationPathContainer.AddChild(movementRenderer);

            owningUnit.OnDeselected += () =>
            {
                drawPath = false;
                movementRenderer.Visible = false;
            };
            owningUnit.OnSelected += () =>
            {
                drawPath = true;
                movementRenderer.Visible = true;
                UpdateRenderedPath();
            };
        }
        
        public override void _Ready()
        {
        }
        

        public override void _Process(float delta)
        {
            if (navigationPath != null && navigationPath.Length > 0)
            {
                owningUnit.Position += movementDirection * speed * delta;
                float distance = owningUnit.Position.DistanceTo(navigationPath[pathIndex]);
                if (distance < acceptanceRange)
                {
                    pathIndex += 1;
                    if (pathIndex <= navigationPath.Length - 1)
                    {
                        movementDirection = (navigationPath[pathIndex] - owningUnit.Position).Normalized();
                        owningUnit.RotateSprite(movementDirection.Angle());
                    }
                    else
                    {
                        navigationPath = null;
                    }

                    if (drawPath)
                    {
                        UpdateRenderedPath();
                    }
                }
                else
                {
                    if (drawPath)
                    {
                        Vector2[] points = movementRenderer.Points;
                        points[0] = GlobalPosition;
                        movementRenderer.Points = points;
                    }
                }
            }
        }

        public void MoveToLocation(Vector2 destination)
        {
            Vector2[] path = navigation2D.GetSimplePath(owningUnit.Position, destination);
           

            navigationPath = path;
            pathIndex = 1;

            movementDirection = (navigationPath[pathIndex] - owningUnit.Position).Normalized();
            owningUnit.RotateSprite(movementDirection.Angle());
            UpdateRenderedPath();
        }

        public void CancelMovement()
        {
            
        }

        private void UpdateRenderedPath()
        {
            List<Vector2> remainingPoints = new List<Vector2>();
            if (navigationPath != null)
            {
                for (int i = pathIndex - 1; i < navigationPath.Length; i++)
                {
                    remainingPoints.Add(navigationPath[i]);
                }
            }
            movementRenderer.Points = remainingPoints.ToArray();
        }
    }
}