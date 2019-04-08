using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(DistortionRenderer), PostProcessEvent.AfterStack, "Custom/Distortion")]
    public sealed class Distortion : PostProcessEffectSettings
    {
        [Range(0, 4)]
        public IntParameter downscale = new IntParameter { value = 0 };

        [Range(0f, 10.0f)]
        public FloatParameter magnitude = new FloatParameter { value = 1.0f };

        public BoolParameter debugView = new BoolParameter { value = false };
    }

    internal sealed class DistortionRenderer : PostProcessEffectRenderer<Distortion>
    {
        private Shader m_shader;
        private int m_magnitudeID;
        private int m_globalDistortionTexID;

        public override DepthTextureMode GetCameraFlags()
        {
            return DepthTextureMode.Depth;
        }

        public override void Init()
        {
            m_shader = Shader.Find("Hidden/Custom/Distortion");
            m_magnitudeID = Shader.PropertyToID("_Magnitude");
            m_globalDistortionTexID = Shader.PropertyToID("_GlobalDistortionTex");

            base.Init();
        }
        
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(m_shader);
            sheet.properties.SetFloat(m_magnitudeID, settings.magnitude);

            context.command.GetTemporaryRT(m_globalDistortionTexID, context.camera.pixelWidth >> settings.downscale, context.camera.pixelHeight >> settings.downscale, 0, FilterMode.Bilinear, RenderTextureFormat.RGFloat);
            context.command.SetRenderTarget(m_globalDistortionTexID);
            context.command.ClearRenderTarget(false, true, Color.clear);

            DistortionManager.Instance.ExecuteCommandBuffer(context.command);

            if (settings.debugView)
            {
                context.command.BlitFullscreenTriangle(m_globalDistortionTexID, context.destination);
                return;
            }

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
