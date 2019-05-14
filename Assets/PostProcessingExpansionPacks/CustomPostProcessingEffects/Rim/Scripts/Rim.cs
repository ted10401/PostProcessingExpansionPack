using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(RimRenderer), PostProcessEvent.AfterStack, "Custom/Rim")]
    public sealed class Rim : PostProcessEffectSettings
    {
        [Header("Debugs")]
        public BoolParameter debugPrepass = new BoolParameter { value = false };
    }

    internal sealed class RimRenderer : PostProcessEffectRenderer<Rim>
    {
        private const string SHADER_NAME = "Hidden/Custom/Rim";

        private const string PROPERTY_NAME_RIM_RT = "_RimRT";
        private const string PROPERTY_NAME_RIM_TEX = "_RimTex";

        private Shader m_shader;
        private int m_rimRT;
        private int m_rimTexID;

        public override DepthTextureMode GetCameraFlags()
        {
            return DepthTextureMode.Depth;
        }

        public override void Init()
        {
            m_shader = Shader.Find(SHADER_NAME);
            m_rimRT = Shader.PropertyToID(PROPERTY_NAME_RIM_RT);
            m_rimTexID = Shader.PropertyToID(PROPERTY_NAME_RIM_TEX);

            base.Init();
        }

        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(m_shader);

            context.command.GetTemporaryRT(m_rimRT, context.camera.pixelWidth, context.camera.pixelHeight);
            context.command.SetRenderTarget(m_rimRT);
            context.command.ClearRenderTarget(false, true, Color.clear);

            RimManager.Instance.ExecuteCommandBuffer(context.command);

            if (settings.debugPrepass)
            {
                context.command.BlitFullscreenTriangle(m_rimRT, context.destination);
                return;
            }

            context.command.SetGlobalTexture(m_rimTexID, m_rimRT);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
