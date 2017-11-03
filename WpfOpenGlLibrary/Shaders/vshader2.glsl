#version 130                                         /* Shader Language Version */                
/*  -------- Input/Output Variabeln  ----------- */                                               
                                                                                                 
uniform mat4 M, P;                                   /* Transformations-Matrizen */               
uniform vec4 lightPosition;                          /* Position Lichtquelle (im Cam.System) */   
uniform int shadingLevel;                            /* 0 ohne Beleucht, 1 diffuse Reflexion */   
uniform float ambient;                               /* ambientes Licht */                        
uniform float diffuse;                               /* diffuse Reflexion */   
in vec4 vPosition;
in vec4 gl_Color;
in vec4 gl_Normal;

out vec4 fColor;                                     /* Fragment-Farbe */                         
void main()                                                                                       
{ 
    vec4 vertex = M * vPosition;                      /* ModelView=Transformation */               
    gl_Position = P * vertex;                         /* Projektion */                                                                                                            
    float Id;                                         /* Helligkeit diffuse Reflexion */           
	fColor = gl_Color;
    if (shadingLevel >= 1)                                                                         
    { 
        vec3 normal = normalize((M * gl_Normal).xyz);                                                  
        vec3 toLight = normalize(lightPosition.xyz - vertex.xyz);                                    
        Id = diffuse * dot(toLight, normal);            /* Gesetz von Lambert */                     
        if ( Id < 0.0 ) Id = 0.0;
        vec3 whiteColor = vec3(1,1,1);
        vec3 reflectedLight =  (ambient + Id) * gl_Color.rgb;
        fColor.rgb = min(reflectedLight, whiteColor);
    }
}