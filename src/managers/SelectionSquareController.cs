using Godot;

namespace GroundWar.managers
{
    public class SelectionSquareController : Node2D
    {
        [Signal] public delegate void FinishedDragging(Rect2 rect);

        private NinePatchRect selectionSquare;
        private Vector2 rectPosition;
        private Vector2 rectSize;
        private Vector2 startMousePosition;
        private Vector2 currentMousePosition;
        [Export] private bool selecting = false;

        public override void _Ready()
        {
            selectionSquare = GetNode<NinePatchRect>("SelectionSquare");
        }

        public override void _Process(float delta)
        {
            if (selecting)
            {
                currentMousePosition = GetGlobalMousePosition();
                UpdateRectExtents();
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
                startMousePosition = GetGlobalMousePosition();
                currentMousePosition = startMousePosition;
                UpdateRectExtents();
                selecting = true;
                selectionSquare.Visible = true;
            }
            else
            {
                selectionSquare.Visible = false;
                selecting = false;
                EmitSignal(nameof(FinishedDragging), new Rect2(rectPosition, rectSize));
            }
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
    }
}