shader_type canvas_item;

uniform vec2 u_resolution;
uniform vec2 u_mouse;

float plotLine(vec2 st) {
	return smoothstep(0.02, 0.0, abs(st.x - st.y));
}

void fragment() {
	vec2 st = FRAGCOORD.xy / u_resolution;
	
	float y = st.x;
	vec3 color = vec3(y);
	
	// Plot Line
	float pct = plotLine(st);
	color = (1.0 - pct) * color ;
	
	COLOR = vec4(color, 1.0);
}
