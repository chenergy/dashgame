Shader "Custom/Fresnel Outline" {
    Properties {
        _MainTex ("Diffuse (RGB)", 2D) = "white" {}
        _OutlineColor ("Outline Colour", Color) = ( 0.0, 0.0, 0.0, 0.0)
        _OutlineAdjust ("Outline Adjust", Range(0.0, 1.0)) = 0.25
    }
    SubShader {
        Tags { "RenderType"="Opaque" "IgnoreProjector"="True" "Queue"="Geometry" }
        Lighting Off
        LOD 200

        Pass {
            Tags { "LightMode" = "Always" }
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile_builtin
                #pragma fragmentoption ARB_precision_hint_fastest

                #include "UnityCG.cginc"

                struct v2f {
                    float4  pos : SV_POSITION;
                    fixed2  uv : TEXCOORD0;
                    fixed       outline : TEXCOORD1;
                };


                v2f vert (appdata_base v) {
                    v2f o;
                    o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                    o.uv = v.texcoord.xy; // UVs.
                    o.outline = dot(ObjSpaceViewDir(v.vertex), v.normal) / o.pos.w/ unity_Scale.w; // Ramp UV coord.
                    return o;
                }

                sampler2D _MainTex;
                fixed4 _OutlineColor;
                fixed _OutlineAdjust;

                fixed4 frag(v2f i) : COLOR {
                    fixed4 c = tex2D ( _MainTex, i.uv ); // Diffuse.
                    fixed o = step ( _OutlineAdjust, i.outline ); //Outline.
                    c *= o; // Apply outline to diffuse.
                    c += _OutlineColor * (1 - o); // Apply colour to outline, add to diffuse.
                    return c;
                }
            ENDCG
        }
    }
    FallBack Off

}