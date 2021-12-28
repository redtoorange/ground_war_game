using Godot;

namespace GroundWar
{
    public class TestMap : Node2D
    {
        public override void _Process(float delta)
        {
            if (Input.IsKeyPressed((int)KeyList.Escape))
            {
                GetTree().Quit();
            }
        }
    }
}