extends MeshInstance3D

var start_y: float
var max_y: float
var min_y: float
var going_up: bool = true

@export var speed: float = 5.0
@export var amount: float = 10.0

func _ready():
	start_y = position.y
	max_y = start_y + amount
	min_y = start_y - amount

func _process(delta):
	var pos = position
	
	if going_up:
		if pos.y > max_y:
			going_up = false
			pos.y = max_y
		else:
			pos.y += speed * delta
	else:
		if pos.y < min_y:
			going_up = true
			pos.y = min_y
		else:
			pos.y -= speed * delta
	
	position = pos




