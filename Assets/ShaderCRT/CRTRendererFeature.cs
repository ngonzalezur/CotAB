using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CRTRendererFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class CRTSettings
    {
        public Material crtMaterial;
        [Range(0, 1)]
        public float scanlineIntensity = 0.3f;
        [Range(0, 1000)]
        public float scanlineFrequency = 480.0f;
        [Range(0, 0.1f)]
        public float distortionAmount = 0.03f;
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
    }

    public CRTSettings settings = new CRTSettings();
    private CRTRenderPass crtRenderPass;

    public override void Create()
    {
        crtRenderPass = new CRTRenderPass(settings);
        crtRenderPass.renderPassEvent = settings.renderPassEvent;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.crtMaterial == null)
            return;

        crtRenderPass.Setup(renderer);
        renderer.EnqueuePass(crtRenderPass);
    }

    protected override void Dispose(bool disposing)
    {
        crtRenderPass.Cleanup();
        base.Dispose(disposing);
    }

    private class CRTRenderPass : ScriptableRenderPass
    {
        private CRTSettings settings;
        private ScriptableRenderer renderer;
        private RenderTextureDescriptor cameraTextureDescriptor;
        private RTHandle tempRTHandle;
        private static readonly string tempRTName = "_TempCRTRT";

        public CRTRenderPass(CRTSettings settings)
        {
            this.settings = settings;
            tempRTHandle = RTHandles.Alloc(tempRTName, name: tempRTName);
        }

        public void Setup(ScriptableRenderer renderer)
        {
            this.renderer = renderer;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            cameraTextureDescriptor = renderingData.cameraData.cameraTargetDescriptor;
            cameraTextureDescriptor.depthBufferBits = 0;
            cameraTextureDescriptor.msaaSamples = 1;

            // Asegurarnos de que el RTHandle tenga el tamaño correcto
            RenderingUtils.ReAllocateIfNeeded(ref tempRTHandle, cameraTextureDescriptor, FilterMode.Bilinear, TextureWrapMode.Clamp, name: tempRTName);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (settings.crtMaterial == null || renderer == null)
                return;

            CommandBuffer cmd = CommandBufferPool.Get("CRT Effect");

            // Actualizar los parámetros del material
            settings.crtMaterial.SetFloat("_ScanlineIntensity", settings.scanlineIntensity);
            settings.crtMaterial.SetFloat("_ScanlineFrequency", settings.scanlineFrequency);
            settings.crtMaterial.SetFloat("_DistortionAmount", settings.distortionAmount);

            // Intentamos obtener el color target de diferentes maneras según la versión de URP
            // Método 1: Usar comandos de blit más básicos para mayor compatibilidad
            RTHandle source = renderer.cameraColorTargetHandle;

            if (source != null && source.rt != null)
            {
                // Blits manuales sin usar Blitter para evitar NullReferenceException
                cmd.Blit(source.nameID, tempRTHandle.nameID, settings.crtMaterial, 0);
                cmd.Blit(tempRTHandle.nameID, source.nameID);
            }
            else
            {
                // Fallback - intento con identificadores globales
                RenderTargetIdentifier sourceIdentifier = new RenderTargetIdentifier("_CameraColorTexture");
                RenderTargetIdentifier destIdentifier = new RenderTargetIdentifier(tempRTHandle.name);

                cmd.Blit(sourceIdentifier, tempRTHandle.nameID, settings.crtMaterial, 0);
                cmd.Blit(tempRTHandle.nameID, sourceIdentifier);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public void Cleanup()
        {
            // Liberar el RTHandle cuando se destruye el feature
            tempRTHandle?.Release();
        }
    }
}