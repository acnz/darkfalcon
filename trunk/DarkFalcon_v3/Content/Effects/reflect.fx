extern texture FinalTexture;

sampler TextureSampler = sampler_state
{
	texture = <FinalTexture>;
	magFilter = POINT; minFilter = POINT; mipFilter = POINT;
};

const float NUM_PASSES = 8;

const float2 BlurPasses[8] =
{
	float2(0.003, 0),
	float2(0.004, 0),
	float2(0, 0.003),
	float2(0, 0.004),
	
	float2(-0.0033, 0),
	float2(-0.0043, 0),
	float2(0, -0.0033),
	float2(0, -0.0043) 
};


float4 PSReflect ( float2 TexCoord : TEXCOORD0 ) : COLOR0
{
  float edge = 0.7;
  float4 Color = tex2D(TextureSampler, TexCoord);
  if (TexCoord.y > edge)
  {
  float na =   TexCoord.y-0.7;
  Color.a = na;
    return Color;
  }else{
  Color.a = 0;
  return Color;}
}

technique PostEffect
{

	pass Blur
	{
		PixelShader = compile ps_2_0 PSReflect();
	}

}