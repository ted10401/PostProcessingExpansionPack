using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(FataMorganaRenderer), PostProcessEvent.AfterStack, "Custom/FataMorgana")]
    public sealed class FataMorgana : PostProcessEffectSettings
    {
        public FloatParameter depthMultiplier = new FloatParameter { value = 1f };
        public Vector2Parameter depthRange = new Vector2Parameter { value = new Vector2(0f, 1f) };
        public TextureParameter distortionTexture = new TextureParameter { value = null };
        public FloatParameter distortionStrength = new FloatParameter { value = 1f };
        public FloatParameter distortionSpeed = new FloatParameter { value = 1f };
        public BoolParameter debugDepth = new BoolParameter { value = false };
        public BoolParameter debugDistortion = new BoolParameter { value = false };
    }

    internal sealed class FataMorganaRenderer : PostProcessEffectRenderer<FataMorgana>
    {
		private const string SHADER_NAME = "Hidden/Custom/FataMorgana";
        private readonly int DEPTH_MULTIPLIER_ID = Shader.PropertyToID("_DepthMutiplier");
        private readonly int DEPTH_RANGE_ID = Shader.PropertyToID("_DepthRange");
        private readonly int DISTORTION_TEXTURE_ID = Shader.PropertyToID("_DistortionTex");
        private readonly int DISTORTION_STRENGTH_ID = Shader.PropertyToID("_DistortionStrength");
        private readonly int DISTORTION_SPEED_ID = Shader.PropertyToID("_DistortionSpeed");
        private Shader m_shader;
        
        public override void Init()
        {
            m_shader = Shader.Find(SHADER_NAME);
            
            base.Init();
        }
        
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(m_shader);

            sheet.properties.SetFloat(DEPTH_MULTIPLIER_ID, settings.depthMultiplier);
            sheet.properties.SetVector(DEPTH_RANGE_ID, settings.depthRange);

            if(settings.distortionTexture.value != null)
            {
                sheet.properties.SetTexture(DISTORTION_TEXTURE_ID, settings.distortionTexture);
            }
            
            sheet.properties.SetFloat(DISTORTION_STRENGTH_ID, settings.distortionStrength);
            sheet.properties.SetFloat(DISTORTION_SPEED_ID, settings.distortionSpeed);

            if(settings.debugDepth.value)
            {
                context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 2);
            }
            else
            {
                context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, settings.debugDistortion.value ? 1 : 0);
            }
        }
    }
}
