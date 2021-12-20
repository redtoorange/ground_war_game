extends Node3D

@export var mouse_sensitivity = 0.05
@export var movement_speed = 0.1

var dragging = false
var mouse_pos = Vector2.ZERO

func _unhandled_input(event):
	if event is InputEventMouseButton:
		if event.button_index == MOUSE_BUTTON_LEFT:
			if event.pressed:
				dragging = true
				mouse_pos = event.position
				Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
			else:
				dragging = false
				Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
				Input.warp_mouse_position(mouse_pos)
	
	if dragging && event is InputEventMouseMotion:
		var rot = rotation
		rot.y += -mouse_sensitivity * event.relative.x / 10
		rot.x += -mouse_sensitivity * event.relative.y / 10
		rot.x = clampf(rot.x, -90, 90)
		rotation = rot

func _process(delta):
	var movement_delta = Vector3.ZERO
	if Input.is_action_pressed("move_forward"):
		movement_delta.z -= 1
	if Input.is_action_pressed("move_backward"):
		movement_delta.z += 1
	if Input.is_action_pressed("move_left"):
		movement_delta.x -= 1
	if Input.is_action_pressed("move_right"):
		movement_delta.x += 1
	if Input.is_action_pressed("move_up"):
		movement_delta.y += 1
	if Input.is_action_pressed("move_down"):
		movement_delta.y -= 1
	
	if movement_delta.length_squared() > 0:
		position += movement_delta.normalized().rotated(Vector3.UP, rotation.y) * movement_speed
