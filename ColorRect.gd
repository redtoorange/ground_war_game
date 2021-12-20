extends ColorRect

var shader_material: ShaderMaterial

func _ready() -> void:
	shader_material = material as ShaderMaterial

func _process(delta):
	shader_material.set_shader_param("u_mouse", get_global_mouse_position())
	shader_material.set_shader_param("u_resolution", get_viewport_rect().size)
