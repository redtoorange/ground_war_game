using Godot;

namespace GroundWar.game.orders
{
    public class MoveOrder : Order
    {
        public Vector2 destination;

        public MoveOrder(Vector2 destination)
        {
            this.destination = destination;
        }
    }
}