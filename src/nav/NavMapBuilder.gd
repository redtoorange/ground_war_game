extends Node2D

@export var start_pos: Vector2
@export var extents: Vector2
@export_flags_2d_physics var collision_mask = 0
@export var bad_spot: PackedScene
@export var good_spot: PackedScene
@export_node_path(TileMap) var terrain_path: NodePath
var terrain: TileMap

func _ready():
	terrain = get_node(terrain_path)
#	_scan_for_static()
	_scan_tile_map()

func _scan_tile_map() -> void:
	var tile_set: TileSet = terrain.tile_set
	print(tile_set.get_custom_data("cost"))
#	for x in range(start_pos.x, start_pos.x + extents.x, 32):
#		for y in range(start_pos.y, start_pos.y + extents.y, 32):
#			var pos = Vector2(x, y)
#			var map_pos = terrain.world_to_map(pos)
#			var id = terrain.get_cell_source_id(0, map_pos, false)

func _scan_for_static() -> void:
	var space_state = get_world_2d().direct_space_state
	var query = _build_query()
	
	for x in range(start_pos.x, start_pos.x + extents.x, 32):
		for y in range(start_pos.y, start_pos.y + extents.y, 32):
			query.position = Vector2(x, y)
			var result = space_state.intersect_point(query)
			if result.size() > 0:
				var s = bad_spot.instantiate()
				s.position = query.position
				add_child(s)
			else:
				var s = good_spot.instantiate()
				s.position = query.position
				add_child(s)

func _build_query() -> PhysicsPointQueryParameters2D:
	var query: PhysicsPointQueryParameters2D = PhysicsPointQueryParameters2D.new()
	query.collision_mask = collision_mask
	query.collide_with_areas = true
	query.collide_with_bodies = true
	return query

func _draw():
	for x in range(start_pos.x, start_pos.x + extents.x, 32):
		for y in range(start_pos.y, start_pos.y + extents.y, 32):
			draw_circle(Vector2(x, y), 2.5, Color.RED)
