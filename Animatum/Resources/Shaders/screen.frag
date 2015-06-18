varying vec2 Texcoord;
uniform sampler2D texFramebuffer;

void main() {
	// Get the color from the framebuffer texture
	vec4 color = texture2D(texFramebuffer, Texcoord.st);

	gl_FragColor = color;
}