using Godot;

namespace GroundWar.managers
{
    public enum SelectionType
    {
        SINGLE,
        GROUP
    }

    public class SelectionSquareController : Node2D
    {
        [Signal] public delegate void FinishedDragging(Rect2 selectionRect, Area2D selectionArea,
            SelectionType selectionType);

        [Export] private float selectionShowThreshold = 5.0f;

        private NinePatchRect selectionSquare;
        private Vector2 rectPosition;
        private Vector2 rectSize;
        private Vector2 startMousePosition;
        private Vector2 currentMousePosition;
        private bool selecting = false;
        private SelectionType currentSelectionType = SelectionType.SINGLE;

        private Area2D area2D;
        private RectangleShape2D collisionShape;

        public override void _Ready()
        {
            selectionSquare = GetNode<NinePatchRect>("SelectionSquare");
            area2D = GetNode<Area2D>("SelectionSquare/Area2D");

            CollisionShape2D coll = GetNode<CollisionShape2D>("SelectionSquare/Area2D/CollisionShape2D");
            collisionShape = coll.Shape as RectangleShape2D;
        }

        public override void _Process(float delta)
        {
            if (selecting)
            {
                currentMousePosition = GetGlobalMousePosition();
                UpdateRectExtents();
                if (!selectionSquare.Visible && rectSize.Length() >= selectionShowThreshold)
                {
                    currentSelectionType = SelectionType.GROUP;
                    ShowSquare();
                }

                UpdateCollisionArea();
            }
        }

        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventMouseButton iem)
            {
                if (iem.ButtonIndex == (int)ButtonList.Left)
                {
                    HandleLeftMouse(iem);
                }
            }
        }

        private void HandleLeftMouse(InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.Pressed)
            {
                currentSelectionType = SelectionType.SINGLE;
                startMousePosition = GetGlobalMousePosition();
                currentMousePosition = startMousePosition;
                UpdateRectExtents();
                selecting = true;
            }
            else
            {
                EmitSignal(nameof(FinishedDragging), new Rect2(rectPosition, rectSize), area2D, currentSelectionType);

                selectionSquare.Visible = false;
                selecting = false;
            }
        }

        private void ShowSquare()
        {
            selectionSquare.Visible = true;
        }

        private void UpdateRectExtents()
        {
            // Handle X
            if (currentMousePosition.x > startMousePosition.x)
            {
                rectPosition.x = startMousePosition.x;
                rectSize.x = currentMousePosition.x - startMousePosition.x;
            }
            else
            {
                rectPosition.x = currentMousePosition.x;
                rectSize.x = startMousePosition.x - currentMousePosition.x;
            }

            // Handle Y
            if (currentMousePosition.y > startMousePosition.y)
            {
                rectPosition.y = startMousePosition.y;
                rectSize.y = currentMousePosition.y - startMousePosition.y;
            }
            else
            {
                rectPosition.y = currentMousePosition.y;
                rectSize.y = startMousePosition.y - currentMousePosition.y;
            }

            selectionSquare.RectPosition = rectPosition;
            selectionSquare.RectSize = rectSize;
        }

        private void UpdateCollisionArea()
        {
            Vector2 halfSize = rectSize / 2.0f;

            area2D.GlobalPosition = rectPosition + halfSize;
            collisionShape.Extents = halfSize;
        }
    }
}