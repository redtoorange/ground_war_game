using Godot;

namespace GroundWar.camera
{
    public class RTSCameraController : Node2D
    {
        [Export] private float cameraSpeed = 5.0f;
        [Export] private float cameraFastMultiplier = 2.0f;
        [Export] private float minZoom = 0.5f;
        [Export] private float maxZoom = 10.0f;
        [Export] private float zoomSpeed = 5.0f;

        private float zoomLevel;
        
        private Camera2D camera;
        private Tween zoomTweener;

        public override void _Ready()
        {
            camera = GetNode<Camera2D>("Camera2D");
            zoomLevel = camera.Zoom.x;

            zoomTweener = new Tween();
            zoomTweener.Name = "ZoomTweener";
            AddChild(zoomTweener);
        }

        public override void _Process(float delta)
        {
            HandleMovement(delta);
            HandleZoom(delta);
        }

        private void HandleMovement(float delta)
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

                Position += inputDelta.Normalized() * speed * delta * zoomLevel;
            }
        }

        private void HandleZoom(float delta)
        {
            float zoomDelta = 0.0f;

            if (Input.IsActionJustReleased("camera_zoom_in"))
            {
                zoomDelta -= 1;
            }
            else if (Input.IsActionJustReleased("camera_zoom_out"))
            {
                zoomDelta += 1;
            }

            if (zoomDelta != 0.0f)
            {
                zoomDelta *= zoomSpeed * delta;
                Vector2 targetZoom = camera.Zoom;
                zoomLevel = Mathf.Clamp(targetZoom.x + zoomDelta, minZoom, maxZoom);

                targetZoom.x = zoomLevel;
                targetZoom.y = zoomLevel;

                TweenZoom(targetZoom);
            }
        }

        private void TweenZoom(Vector2 targetZoom)
        {
            zoomTweener.StopAll();
            zoomTweener.InterpolateProperty(
                camera,
                "zoom",
                camera.Zoom,
                targetZoom,
                0.1f,
                Tween.TransitionType.Quad,
                Tween.EaseType.Out
            );
            zoomTweener.Start();
        }
    }
}