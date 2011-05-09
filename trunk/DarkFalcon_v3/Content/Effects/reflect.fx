extern texture FinalTexture;

sampler TextureSampler = sampler_state
{
	texture = <FinalTexture>;
	magFilter = POINT; minFilter = POINT; mipFilter = POINT;
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
float4 PSReflect2 ( float2 TexCoord : TEXCOORD0 ) : COLOR0
{
  float edge = 0.7;
  float4 Color = tex2D(TextureSampler, TexCoord);
  if (TexCoord.y > edge || TexCoord.y < 1-edge)
  {
  float na =   TexCoord.y + 1 - edge;
  Color.a = na;
    
  } 

  
  return Color;
}

technique PostEffect
{

	pass Blur
	{
		PixelShader = compile ps_2_0 PSReflect();
	}
	pass quadBlur
	{
		PixelShader = compile ps_2_0 PSReflect2();
	}

}