#[compute]

#version 450

layout(local_size_x = 8, local_size_y = 1, local_size_z = 1) in;

layout(set = 0, binding = 0, std430) buffer ColorBuffer {
	float data[];
}
color_buffer;

void main() {
	color_buffer.data[gl_GlobalInvocationID.x] = gl_GlobalInvocationID.x;
}