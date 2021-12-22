extends Node2D

@onready var nav_agent: NavigationAgent2D = get_node("NavigationAgent2D")
@onready var nav_line: Line2D = get_node("NavigationLine")

#func _ready():
#	nav_agent.set_navigable_layers(1)

func _physics_process(delta):
	if Input.is_mouse_button_pressed(MOUSE_BUTTON_LEFT):
		var m_pos = get_global_mouse_position()
		nav_agent.set_target_location(m_pos)
		if nav_agent.is_target_reachable():
			var points = nav_agent.get_nav_path()
			nav_line.clear_points()
			for p in points:
				nav_line.add_point(to_local(p))


func _on_NavigationAgent2D_velocity_computed(safe_velocity):
	print(safe_velocity)
