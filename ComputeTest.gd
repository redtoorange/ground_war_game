extends Node2D

func _ready():
	var compute = ComputeTest.new()
	compute.loadComputeShader()
