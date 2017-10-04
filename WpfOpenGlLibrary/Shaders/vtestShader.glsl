#version 130                                                              
uniform mat4 M, P;                       /* Transformations-Matrizen */   
in vec4 vPosition;
in vec4 vColor;                         /* Vertex-Attribute */           
out vec4 fColor;                         /* Fragment-Farbe */  

void main()
{
  fColor = vec4(1.0,0.0,0.0,1.0);
  vec4 pos = M * vPosition;
  gl_Position = P * pos;
}