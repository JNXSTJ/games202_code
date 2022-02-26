#version 330 core
out vec4 FragColor;

in vec2 TexCoords;
in vec3 vFragPos;
in vec3 vNormal;

uniform vec3 uLightPos;
uniform vec3 uCameraPos;
uniform vec3 uLightIntensity;
uniform vec3 uKs;
uniform vec3 uKd;

uniform sampler2D texture_diffuse1;

vec3 blinnPhong() {
  vec3 color = texture(texture_diffuse1, TexCoords).rgb;
  color = pow(color, vec3(2.2));

  vec3 ambient = 0.05 * color;

  vec3 lightDir = normalize(uLightPos);
  vec3 normal = normalize(vNormal);
  float diff = max(dot(lightDir, normal), 0.0);
  vec3 light_atten_coff =
      uLightIntensity / pow(length(uLightPos - vFragPos), 2.0);
  vec3 diffuse = uKd * diff * light_atten_coff * color;

  vec3 viewDir = normalize(uCameraPos - vFragPos);
  vec3 halfDir = normalize((lightDir + viewDir));
  float spec = pow(max(dot(halfDir, normal), 0.0), 32.0);
  vec3 specular = uKs * light_atten_coff * spec;

  vec3 radiance = (ambient + diffuse + specular);
  // vec3 radiance = diffuse;
  vec3 phongColor = pow(radiance, vec3(1.0 / 2.2));
  return phongColor;
}

void main()
{
    FragColor = vec4(blinnPhong(), 1.0f);
}
