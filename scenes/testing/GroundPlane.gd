extends MeshInstance3D

signal clicked_ground(position: Vector3)

func _on_click_event(camera, event, position, normal, shape_idx):
	if event is InputEventMouseButton && event.pressed:
		emit_signal("clicked_ground", position)
