extends Node2D
class_name RTSCameraController

@export var movement_speed: float = 50.0
@export var fast_multiplier: float = 2.0

func _ready():
	pass

# Handle normal kayboard input
func _process(delta):
	var movement_delta: Vector2 = Vector2.ZERO
	var fast_camera = Input.is_action_pressed("camera_fast")
	
	if Input.is_action_pressed("camera_up"):
		movement_delta.y -= 1
	if Input.is_action_pressed("camera_down"):
		movement_delta.y += 1
	if Input.is_action_pressed("camera_left"):
		movement_delta.x -= 1
	if Input.is_action_pressed("camera_right"):
		movement_delta.x += 1
	
	var speed = movement_speed * (fast_multiplier if fast_camera else 1)
	position += movement_delta.normalized() * delta * speed

# Handle the mouse input for selection and dragging
func _unhandled_input(event):
	pass
