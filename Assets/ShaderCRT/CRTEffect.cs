using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CRTPostProcessFeature : ScriptableRendererFeature
{
    class CRTRenderPass : ScriptableRenderPass
    {
        private Material material;
        private RTHandle sourceTexture;
        private RTHandle tempTexture;

        private const string profilerTag = "CRT Post Process";

        public CRTRenderPass(Material mat)
        {
            this.material = mat;
            profilingSampler = new ProfilingSampler(profilerTag);
        }

        public void Setup(RTHandle source)
        {
            this.sourceTexture = source;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            // Configurar el buffer temporal
            RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
            descriptor.depthBufferBits = 0;

            // En Unity 6, usamos RenderingUtils.ReAllocateIfNeeded en lugar de GetTemporaryRT
            RenderingUtils.ReAllocateIfNeeded(ref tempTexture, descriptor, FilterMode.Bilinear, TextureWrapMode.Clamp, name: "_TempCRTTexture");
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (material == null)
            {
                Debug.LogError("Material no asignado en CRTRenderPass");
                return;
            }

            CommandBuffer cmd = CommandBufferPool.Get(profilerTag);

            using (new ProfilingScope(cmd, profilingSampler))
            {
                // Aplicar el post-proceso con el material
                Blitter.BlitCameraTexture(cmd, sourceTexture, tempTexture, material, 0);
                Blitter.BlitCameraTexture(cmd, tempTexture, sourceTexture);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            // No es necesario limpiar el tempTexture en Unity 6
            // Los RTHandles se gestionan de forma diferente
        }

        public void Dispose()
        {
            tempTexture?.Release();
        }
    }

    [System.Serializable]
    public class CRTSettings
    {
        public Material material = null;
    }

    public CRTSettings settings = new CRTSettings();
    private CRTRenderPass renderPass;

    public override void Create()
    {
        if (settings.material == null)
        {
            Debug.LogError("Material no asignado en CRTPostProcessFeature.");
            return;
        }

        renderPass = new CRTRenderPass(settings.material)
        {
            // En Unity 6, AfterRenderingTransparents podría haber cambiado
            // Usar un evento de renderizado adecuado
            renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing
        };
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        if (settings.material == null)
            return;

        // En Unity 6, debemos usar cameraColorTargetHandle en lugar de cameraColorTarget
        renderPass.Setup(renderer.cameraColorTargetHandle);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.material == null)
            return;

        renderer.EnqueuePass(renderPass);
    }

    protected override void Dispose(bool disposing)
    {
        renderPass?.Dispose();
    }
}


