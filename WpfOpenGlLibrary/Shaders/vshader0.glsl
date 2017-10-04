#version 130                  /* Shader Language Version */
in vec3 vPosition
in vec4 vColor;
out vec4 fColor;
void main() 
{  
    gl_Position = vPosition; 
    fColor = vColor;
}