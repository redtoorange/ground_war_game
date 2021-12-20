shader_type spatial;

float red() {
	return 10.0*abs(sin(TIME));
}

float green() {
	return 5.0*abs(sin(TIME));
}

float blue() {
	return 3.0*abs(sin(TIME));
}

vec4 getColor() {
	return vec4(red(), green(), blue(), 1);
}

void fragment() {
	vec2 st = FRAGCOORD.xy / 
	vec4 color = getColor();
	
	ALBEDO = color.rgb;
	ALPHA = color.a;
}
