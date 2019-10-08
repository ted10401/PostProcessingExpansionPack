using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(CloudRenderer), PostProcessEvent.AfterStack, "Custom/Cloud")]
    public sealed class Cloud : PostProcessEffectSettings
    {
        public FloatParameter cloudScale = new FloatParameter { value = 5.0f };
        public Vector2Parameter cloudSpeed = new Vector2Parameter { value = new Vector2(0.1f, 0.03f) };
        public FloatParameter cloudCover = new FloatParameter { value = 0.1f };
        public FloatParameter cloudAlpha = new FloatParameter { value = 15.0f };
        public ColorParameter cloudDarkColor = new ColorParameter { value = new Color(190f / 255, 190f / 255, 190f / 255) };
        public ColorParameter cloudLightColor = new ColorParameter { value = new Color(1.0f, 1.0f, 1.0f) };
        public ColorParameter skyUpColor = new ColorParameter { value = new Color(102f / 255, 178f / 255, 255f / 255) };
        public ColorParameter skyDownColor = new ColorParameter { value = new Color(51f / 255, 102f / 255, 153f / 255) };
    }

    internal sealed class CloudRenderer : PostProcessEffectRenderer<Cloud>
    {
		private const string SHADER_NAME = "Hidden/Custom/Cloud";
        private readonly int CLOUD_SCALE_ID = Shader.PropertyToID("_CloudScale");
        private readonly int CLOUD_SPEED_ID = Shader.PropertyToID("_CloudSpeed");
        private readonly int CLOUD_COVER_ID = Shader.PropertyToID("_CloudCover");
        private readonly int CLOUD_ALPHA_ID = Shader.PropertyToID("_CloudAlpha");
        private readonly int CLOUD_DARK_COLOR_ID = Shader.PropertyToID("_CloudDarkColor");
        private readonly int CLOUD_LIGHT_COLOR_ID = Shader.PropertyToID("_CloudLightColor");
        private readonly int SKY_UP_COLOR_ID = Shader.PropertyToID("_SkyUpColor");
        private readonly int SKY_DOWN_COLOR_ID = Shader.PropertyToID("_SkyDownColor");
        private Shader m_shader;
        
        public override void Init()
        {
            m_shader = Shader.Find(SHADER_NAME);
            
            base.Init();
        }
        
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(m_shader);
            sheet.properties.SetFloat(CLOUD_SCALE_ID, settings.cloudScale);
            sheet.properties.SetVector(CLOUD_SPEED_ID, settings.cloudSpeed);
            sheet.properties.SetFloat(CLOUD_COVER_ID, settings.cloudCover);
            sheet.properties.SetFloat(CLOUD_ALPHA_ID, settings.cloudAlpha);
            sheet.properties.SetColor(CLOUD_DARK_COLOR_ID, settings.cloudDarkColor);
            sheet.properties.SetColor(CLOUD_LIGHT_COLOR_ID, settings.cloudLightColor);
            sheet.properties.SetColor(SKY_UP_COLOR_ID, settings.skyUpColor);
            sheet.properties.SetColor(SKY_DOWN_COLOR_ID, settings.skyDownColor);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
