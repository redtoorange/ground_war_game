@tool
extends Planet

@export var speed: float = 1.0

func _process(delta):
	rotate(Vector3(0, 1, 0), speed * delta)
