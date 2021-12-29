using System.Collections.Generic;
using Godot;
using GroundWar.game.orders;

namespace GroundWar.game.units
{
    /**
     * UnitOrderHandler is a way of abstractly handling orders given to units, this might be replaced with a more simple
     * system when soldiers are merged into units.
     */
    public class UnitOrderHandler : Node
    {
        private Order currentOrder = null;
        private Queue<Order> currentOrders = new Queue<Order>();

        private BaseUnit owningUnit;
        private MovementController movementController;

        public void Initialize(BaseUnit owningUnit, MovementController movementController)
        {
            this.owningUnit = owningUnit;
            this.movementController = movementController;
        }

        public override void _Ready()
        {
        }

        public void AddOrder(Order newOrder)
        {
            currentOrders.Enqueue(newOrder);
        }

        public override void _Process(float delta)
        {
            if (currentOrder == null && currentOrders.Count > 0)
            {
                currentOrder = currentOrders.Dequeue();
            }

            if (currentOrder != null)
            {
                ProcessCurrentOrder();
            }
        }

        private void ProcessCurrentOrder()
        {
            if (currentOrder is MoveOrder moveOrder)
            {
                HandleMoveOrder(moveOrder);
                currentOrder = null;
            }
            else if (currentOrder is AttackOrder attackOrder)
            {
            }
            else if (currentOrder is BuildOrder buildOrder)
            {
            }
            else if (currentOrder is CaptureOrder captureOrder)
            {
            }
        }

        private void HandleMoveOrder(MoveOrder moveOrder)
        {
            movementController.MoveToLocation(moveOrder.destination);
        }
    }
}