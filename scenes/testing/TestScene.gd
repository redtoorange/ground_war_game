extends Node3D
class_name TestScene

@export var distance: float = 1000.0
@export_node_path(Camera3D) var camera_path: NodePath
var camera: Camera3D

@export var path_point_prefab: PackedScene
var path_point: Node3D

var selected_units = []

var clicked_spot: Vector3

func _ready():
	camera = get_node(camera_path)


func add_unit(unit: MainUnit):
	selected_units.append(unit)
	print(selected_units.size())


func _on_clicked_ground(position: Vector3):
	for u in selected_units:
		u.move_to(position)
		if path_point == null:
			path_point = path_point_prefab.instantiate()
			add_child(path_point)
		path_point.position = position
