#version 130                                                              
uniform mat4 M, P;                       /* Transformations-Matrizen */   
in vec3 vPosition, vNormal;
in vec4 vColor;                          /* Vertex-Attribute */           
out vec4 fColor;                         /* Fragment-Farbe */             
void main()                                                               
{  
	vec4 vertex = M * vec4(vPosition, 1);          /* ModelView-Transformation */   
	gl_Position = P * vertex;             /* Projektion */                 
	fColor = vColor;                                                       
}