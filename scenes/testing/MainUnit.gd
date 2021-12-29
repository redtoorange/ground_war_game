extends CharacterBody3D
class_name MainUnit

@export var movement_speed: float = 10.0
@export var test_scene_path: NodePath
var test_scene: TestScene

@export var path_point_prefab: PackedScene
var path_points = []

var nav_node: NavigationAgent3D
var selected = false
var path
var next_point: Vector3

var velocity: Vector3

func _ready():
	nav_node = get_node("NavigationAgent3D")
	test_scene = get_node(test_scene_path)


func _on_clicked(camera, event, position, normal, shape_idx):
	if event is InputEventMouseButton && event.pressed:
		selected = true
		test_scene.add_unit(self)

func _physics_process(delta):
	if path != null:
		if !nav_node.is_navigation_finished():
			var temp_point = nav_node.get_next_location()
			if temp_point != next_point:
				next_point = temp_point
				var direction = next_point - position
				var velocity = direction.normalized() * movement_speed
				nav_node.set_velocity(velocity)
	
	position += velocity

func move_to(position: Vector3):
	nav_node.set_target_location(position)
	if nav_node.is_target_reachable():
		print("Target it reachable")
		path = nav_node.get_nav_path()
		if path_points.size() > 0:
			for point in path_points:
				point.queue_free()
			path_points.clear()
		for i in path.size() - 1:
			var p = path[i]
			var point = path_point_prefab.instantiate() as Node3D
			point.position = p
			path_points.append(point)
			get_parent().add_child(point)
	else:
		print("Target it not reachable")



func _on_velocity_computed(safe_velocity):
	print("Safe Velocity: ", safe_velocity)
	velocity = Vector3(safe_velocity.x, 0, safe_velocity.z)


func _on_NavigationAgent3D_navigation_finished():
	velocity = Vector3.ZERO
	path = null
	next_point = position


func _on_NavigationAgent3D_path_changed():
	velocity = Vector3.ZERO
	path = null
	next_point = position
