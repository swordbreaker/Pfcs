#version 130                  /* Shader Language Version */
in vec4 vPosition, vColor;    /* Vertex-Attribute */
out vec4 fColor;              /* Fragment-Farbe */
void main() 
{  
    gl_Position = vPosition; 
    fColor = vColor;
}