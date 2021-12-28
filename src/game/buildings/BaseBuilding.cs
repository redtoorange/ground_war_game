using Godot;

namespace GroundWar.game.buildings
{
    public class BaseBuilding : Node2D
    {
         enum SelectionState
        {
            UNSELECTED,
            SELECTED
        }

        enum HoverState
        {
            UNHOVERED,
            HOVERED
        }

        private Sprite selectionSprite;
        private Sprite hoveredSprite;
        private Area2D area2D;

        private SelectionState currentSelectionState = SelectionState.UNSELECTED;
        private HoverState currentHoverState = HoverState.UNHOVERED;

        public override void _Ready()
        {
            selectionSprite = GetNode<Sprite>("SelectionSprite");
            hoveredSprite = GetNode<Sprite>("HoveredSprite");
            area2D = GetNode<Area2D>("Area2D");

            selectionSprite.Visible = false;
            hoveredSprite.Visible = false;

            // area2D.Connect("input_event", this, nameof(OnInputEvent));
            area2D.Connect("mouse_entered", this, nameof(OnMouseEntered));
            area2D.Connect("mouse_exited", this, nameof(OnMouseExited));
        }

        public void SetSelected(bool selected)
        {
            currentSelectionState = selected ? SelectionState.SELECTED : SelectionState.UNSELECTED;
            UpdateOverlays();
        }

        private void OnInputEvent(Node viewport, InputEvent inputEvent, int shapeIndex)
        {
            GD.Print("Some Input event on " + Name);
        }

        private void OnMouseEntered()
        {
            currentHoverState = HoverState.HOVERED;
            UpdateOverlays();
        }

        private void OnMouseExited()
        {
            currentHoverState = HoverState.UNHOVERED;
            UpdateOverlays();
        }

        private void UpdateOverlays()
        {
            if (currentSelectionState == SelectionState.UNSELECTED)
            {
                selectionSprite.Visible = false;
                hoveredSprite.Visible = currentHoverState == HoverState.HOVERED;
            }
            else
            {
                selectionSprite.Visible = currentSelectionState == SelectionState.SELECTED;
                hoveredSprite.Visible = false;
            }
        }
    }
}