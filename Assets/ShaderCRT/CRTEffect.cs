using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CRTEffect : MonoBehaviour
{
    [SerializeField] private Material crtMaterial;

    [Range(0, 1)]
    public float scanlineIntensity = 0.3f;
    [Range(0, 1000)]
    public float scanlineFrequency = 480.0f;
    [Range(0, 0.1f)]
    public float distortionAmount = 0.03f;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (crtMaterial != null)
        {
            // Actualizar los parámetros del material
            crtMaterial.SetFloat("_ScanlineIntensity", scanlineIntensity);
            crtMaterial.SetFloat("_ScanlineFrequency", scanlineFrequency);
            crtMaterial.SetFloat("_DistortionAmount", distortionAmount);

            // Aplicar el efecto
            Graphics.Blit(source, destination, crtMaterial);
        }
        else
        {
            // Si no hay material, simplemente copia la textura
            Graphics.Blit(source, destination);
        }
    }
}





