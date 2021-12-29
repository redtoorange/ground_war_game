using System;
using Godot;
using GroundWar.game.orders;

namespace GroundWar.game.soldiers
{
    public class BaseSoldier : Node2D
    {
        public Action OnSelected;
        public Action OnDeselected;

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

        private Sprite soldierSprite;
        private Sprite selectionSprite;
        private Sprite hoveredSprite;
        private Area2D area2D;

        private Node2D navigationPathParent;
        private UnitOrderHandler soldierOrderHandler;
        private MovementController movementController;

        private SelectionState currentSelectionState = SelectionState.UNSELECTED;
        private HoverState currentHoverState = HoverState.UNHOVERED;

        public override void _Ready()
        {
            soldierSprite = GetNode<Sprite>("SoldierSprite");
            selectionSprite = GetNode<Sprite>("SelectionSprite");
            hoveredSprite = GetNode<Sprite>("HoveredSprite");
            area2D = GetNode<Area2D>("Area2D");
            soldierOrderHandler = GetNode<UnitOrderHandler>("SoldierOrderHandler");
            movementController = GetNode<MovementController>("MovementController");

            selectionSprite.Visible = false;
            hoveredSprite.Visible = false;

            // area2D.Connect("input_event", this, nameof(OnInputEvent));
            area2D.Connect("mouse_entered", this, nameof(OnMouseEntered));
            area2D.Connect("mouse_exited", this, nameof(OnMouseExited));
        }

        public void Initialize(Navigation2D navigation2D, Node2D navigationPathParent)
        {
            soldierOrderHandler.Initialize(this, movementController);
            movementController.Initialize(this, navigation2D, navigationPathParent);
        }

        public void SetSelected(bool selected)
        {
            if (currentSelectionState != SelectionState.SELECTED && selected)
            {
                currentSelectionState = SelectionState.SELECTED;
                OnSelected?.Invoke();
            }
            else if (currentSelectionState != SelectionState.UNSELECTED && !selected)
            {
                currentSelectionState = SelectionState.UNSELECTED;
                OnDeselected?.Invoke();
            }

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

        public void AddOrder(Order newOrder)
        {
            soldierOrderHandler.AddOrder(newOrder);
        }

        public void RotateSprite(float angle)
        {
            soldierSprite.Rotation = angle + Mathf.Deg2Rad(90.0f);
        }

        public Area2D GetArea()
        {
            return area2D;
        }
    }
}