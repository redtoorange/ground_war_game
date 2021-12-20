extends Control

@onready var slider: HSlider = get_node("HSlider")
@onready var value_label: Label = get_node("Value")

func _ready():
	value_label.text = str(slider.value)


func _on_slider_value_changed(value):
	value_label.text = str(value)
