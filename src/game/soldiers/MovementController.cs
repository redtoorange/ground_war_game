using System.Collections.Generic;
using Godot;

namespace GroundWar.game.soldiers
{
    /**
     * MovementController is attached at the Unit Level, and handles the internal logic for the units to move.
     */
    public class MovementController : Node2D
    {
        [Export] private float acceptanceRange = 5.0f;
        [Export] private float speed = 50.0f;
        
        private Line2D movementRenderer;

        private BaseSoldier owningSoldier;
        private Navigation2D navigation2D;
        private Node2D navigationPathContainer;
        
        private Vector2[] navigationPath;
        private int pathIndex;
        private Vector2 movementDirection;

        private bool drawPath = false;
        
        public void Initialize(BaseSoldier owningSoldier, Navigation2D navigation2D, Node2D navigationPathContainer)
        {
            this.owningSoldier = owningSoldier;
            this.navigation2D = navigation2D;
            this.navigationPathContainer = navigationPathContainer;

            movementRenderer = new Line2D();
            movementRenderer.DefaultColor = Colors.Red;
            movementRenderer.Width = 2.5f;
            movementRenderer.Name = this.owningSoldier.Name + "_PathRenderer";
            this.navigationPathContainer.AddChild(movementRenderer);

            owningSoldier.OnDeselected += () =>
            {
                drawPath = false;
                movementRenderer.Visible = false;
            };
            owningSoldier.OnSelected += () =>
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
                owningSoldier.Position += movementDirection * speed * delta;
                float distance = owningSoldier.Position.DistanceTo(navigationPath[pathIndex]);
                if (distance < acceptanceRange)
                {
                    pathIndex += 1;
                    if (pathIndex <= navigationPath.Length - 1)
                    {
                        movementDirection = (navigationPath[pathIndex] - owningSoldier.Position).Normalized();
                        owningSoldier.RotateSprite(movementDirection.Angle());
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
            Vector2[] path = navigation2D.GetSimplePath(owningSoldier.Position, destination);
           

            navigationPath = path;
            pathIndex = 1;

            movementDirection = (navigationPath[pathIndex] - owningSoldier.Position).Normalized();
            owningSoldier.RotateSprite(movementDirection.Angle());
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