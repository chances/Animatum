precision mediump float;

//make sure to have a resolution uniform set to the screen size
uniform int resolution_x;
uniform int resolution_y;
vec2 resolution = vec2(resolution_x, resolution_y);

varying vec2 Texcoord;
uniform sampler2D texFramebuffer;

// https://github.com/demoscenepassivist/SocialCoding/blob/master/code_demos_jogamp/shaders/fxaa.fs
// https://www.npmjs.com/package/glsl-fxaa

float FXAA_REDUCE_MIN = (1.0 / 128.0);
float FXAA_REDUCE_MUL = 1.0 / 8.0;
float FXAA_SPAN_MAX = 8.0;

//FXAA_REDUCE_MIN = (1.0 / 8.0);
//FXAA_REDUCE_MUL = 1.0 / 8.0;
//FXAA_SPAN_MAX = 2.0;

void main() {
    //can also use gl_FragCoord.xy
    //vec2 fragCoord = Texcoord * resolution;
	//vec2 fragCoord = gl_FragCoord.xy * resolution;

    //vec4 color = apply(texFramebuffer, fragCoord, resolution);

    //gl_FragColor = color;

	//gl_FragColor = texture2D(texFramebuffer, Texcoord.xy);

	vec2 inverseVP = 1.0 / resolution.xy;
	vec2 v_rgbNW = (gl_FragCoord.xy + vec2(-1.0, -1.0)) * inverseVP;
	vec2 v_rgbNE = (gl_FragCoord.xy + vec2(1.0, -1.0)) * inverseVP;
	vec2 v_rgbSW = (gl_FragCoord.xy + vec2(-1.0, 1.0)) * inverseVP;
	vec2 v_rgbSE = (gl_FragCoord.xy + vec2(1.0, 1.0)) * inverseVP;

	vec3 rgbNW = texture2D( texFramebuffer, v_rgbNW ).xyz;
    vec3 rgbNE = texture2D( texFramebuffer, v_rgbNE ).xyz;
    vec3 rgbSW = texture2D( texFramebuffer, v_rgbSW ).xyz;
    vec3 rgbSE = texture2D( texFramebuffer, v_rgbSE ).xyz;

    vec4 rgbaM  = texture2D( texFramebuffer,  gl_FragCoord.xy  * resolution );
    vec3 rgbM  = rgbaM.xyz;
    float opacity  = rgbaM.w;
	opacity = 1.0;

    vec3 luma = vec3( 0.299, 0.587, 0.114 );
    float lumaNW = dot( rgbNW, luma );
    float lumaNE = dot( rgbNE, luma );
    float lumaSW = dot( rgbSW, luma );
    float lumaSE = dot( rgbSE, luma );
    float lumaM  = dot( rgbM,  luma );
    float lumaMin = min( lumaM, min( min( lumaNW, lumaNE ), min( lumaSW, lumaSE ) ) );
    float lumaMax = max( lumaM, max( max( lumaNW, lumaNE) , max( lumaSW, lumaSE ) ) );

    vec2 dir;
    dir.x = -((lumaNW + lumaNE) - (lumaSW + lumaSE));
    dir.y =  ((lumaNW + lumaSW) - (lumaNE + lumaSE));
    float dirReduce = max( ( lumaNW + lumaNE + lumaSW + lumaSE ) * ( 0.25 * FXAA_REDUCE_MUL ), FXAA_REDUCE_MIN );
    float rcpDirMin = 1.0 / ( min( abs( dir.x ), abs( dir.y ) ) + dirReduce );
    dir = min( vec2( FXAA_SPAN_MAX,  FXAA_SPAN_MAX),max( vec2(-FXAA_SPAN_MAX, -FXAA_SPAN_MAX),dir * rcpDirMin)) * resolution;

    vec3 rgbA = 0.5 * (
        texture2D( texFramebuffer, gl_FragCoord.xy * resolution + dir * ( 1.0 / 3.0 - 0.5 ) ).xyz +
        texture2D( texFramebuffer, gl_FragCoord.xy * resolution + dir * ( 2.0 / 3.0 - 0.5 ) ).xyz );

    vec3 rgbB = rgbA * 0.5 + 0.25 * (
        texture2D( texFramebuffer, gl_FragCoord.xy * resolution + dir * -0.5 ).xyz +
        texture2D( texFramebuffer, gl_FragCoord.xy * resolution + dir * 0.5 ).xyz );
    float lumaB = dot( rgbB, luma );
    if ( ( lumaB < lumaMin ) || ( lumaB > lumaMax ) ) {
		//gl_FragColor = vec4(1.0,0.0,0.0,1.0);
        gl_FragColor = vec4( rgbA, opacity );
    } else {
		//gl_FragColor = vec4(1.0,1.0,0.0,1.0);
        gl_FragColor = vec4( rgbB, opacity );
    }

	//gl_FragColor = vec4(rgbNW.x, rgbNW.y, rgbNW.z, 1.0);
}
