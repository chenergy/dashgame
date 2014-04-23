Shader "Custom/MobileAlphaDiscard" {
   Properties {
      _MainTex ("RGBA Texture Image", 2D) = "white" {} 
      //_Cutoff ("Alpha Cutoff", Float) = 0.5
   }

   SubShader {
      Pass {    
         Cull Off // since the front is partially transparent, 
            // we shouldn't cull the back

         GLSLPROGRAM

         uniform sampler2D _MainTex;    
         //uniform float _Cutoff;
         varying vec4 textureCoordinates; 
         
         #ifdef VERTEX
         void main()
         {
            textureCoordinates = gl_MultiTexCoord0;
            gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
         }
         #endif

         #ifdef FRAGMENT
         void main()
         {
            gl_FragColor = texture2D(_MainTex, vec2(textureCoordinates));
            if (gl_FragColor.a < 0.5)
               // alpha value less than user-specified threshold?
            {
               discard; // yes: discard this fragment
            }
         }
         #endif
         ENDGLSL
      }
   }
   // The definition of a fallback shader should be commented out 
   // during development:
   // Fallback "Unlit/Transparent Cutout"
}