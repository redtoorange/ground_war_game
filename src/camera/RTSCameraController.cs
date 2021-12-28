using Godot;

namespace GroundWar.camera
{
    public class RTSCameraController : Node2D
    {
        [Export] private float cameraSpeed = 5.0f;
        [Export] private float cameraFastMultiplier = 2.0f;

        private Camera2D camera;

        public override void _Ready()
        {
            camera = GetNode<Camera2D>("Camera2D");
        }

        public override void _Process(float delta)
        {
            Vector2 inputDelta = Vector2.Zero;
            if (Input.IsActionPressed("camera_up"))
            {
                inputDelta.y -= 1;
            }

            if (Input.IsActionPressed("camera_down"))
            {
                inputDelta.y += 1;
            }

            if (Input.IsActionPressed("camera_left"))
            {
                inputDelta.x -= 1;
            }

            if (Input.IsActionPressed("camera_right"))
            {
                inputDelta.x += 1;
            }

            if (inputDelta.LengthSquared() > 0)
            {
                float speed = cameraSpeed;
                if (Input.IsActionPressed("camera_fast"))
                {
                    speed *= cameraFastMultiplier;
                }

                Position += inputDelta.Normalized() * speed * delta;
            }
        }
    }
}