using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(WindowRaindropsRenderer), PostProcessEvent.AfterStack, "Custom/WindowRaindrops")]
    public sealed class WindowRaindrops : PostProcessEffectSettings
    {
        public FloatParameter size = new FloatParameter { value = 1 };
        public FloatParameter speed = new FloatParameter { value = 1 };
        public FloatParameter distortion = new FloatParameter { value = 1 };
        [Range(0f, 1f)] public FloatParameter blur = new FloatParameter { value = 1 };
        public ColorParameter blurColor = new ColorParameter { value = Color.white };
    }

    internal sealed class WindowRaindropsRenderer : PostProcessEffectRenderer<WindowRaindrops>
    {
        private const string SHADER_NAME_OUTLINE = "Hidden/Custom/WindowRaindrops";
        private const string SIZE_PROPERTY_NAME = "_Size";
        private const string SPEED_PROPERTY_NAME = "_Speed";
        private const string DISTORTION_PROPERTY_NAME = "_Distortion";
        private const string DOWNSAMPLE_PROPERTY_NAME_1 = "_Downsample1";
        private const string DOWNSAMPLE_PROPERTY_NAME_2 = "_Downsample2";
        private const string BLUR_TEXTURE_PROPERTY_NAME = "_BlurTex";
        private const string BLUR_PROPERTY_NAME = "_Blur";
        private const string BLUR_COLOR_PROPERTY_NAME = "_BlurColor";

        private Shader m_shader;
        private int m_sizeID;
        private int m_speedID;
        private int m_distortionID;
        private int m_downsample1;
        private int m_downsample2;
        private int m_blurTexID;
        private int m_blurID;
        private int m_blurColorID;

        public override void Init()
        {
            m_shader = Shader.Find(SHADER_NAME_OUTLINE);
            m_sizeID = Shader.PropertyToID(SIZE_PROPERTY_NAME);
            m_speedID = Shader.PropertyToID(SPEED_PROPERTY_NAME);
            m_distortionID = Shader.PropertyToID(DISTORTION_PROPERTY_NAME);
            m_downsample1 = Shader.PropertyToID(DOWNSAMPLE_PROPERTY_NAME_1);
            m_downsample2 = Shader.PropertyToID(DOWNSAMPLE_PROPERTY_NAME_2);
            m_blurTexID = Shader.PropertyToID(BLUR_TEXTURE_PROPERTY_NAME);
            m_blurID = Shader.PropertyToID(BLUR_PROPERTY_NAME);
            m_blurColorID = Shader.PropertyToID(BLUR_COLOR_PROPERTY_NAME);

            base.Init();
        }

        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(m_shader);
            sheet.properties.SetFloat(m_sizeID, settings.size);
            sheet.properties.SetFloat(m_speedID, settings.speed);
            sheet.properties.SetFloat(m_distortionID, settings.distortion);
            sheet.properties.SetFloat(m_blurID, settings.blur);
            sheet.properties.SetColor(m_blurColorID, settings.blurColor);

            context.GetScreenSpaceTemporaryRT(context.command, m_downsample1);
            context.GetScreenSpaceTemporaryRT(context.command, m_downsample2);

            context.command.BlitFullscreenTriangle(context.source, m_downsample1, sheet, 0);
            context.command.BlitFullscreenTriangle(m_downsample1, m_downsample2, sheet, 1);
            context.command.BlitFullscreenTriangle(m_downsample2, m_downsample1, sheet, 2);
            context.command.BlitFullscreenTriangle(m_downsample1, m_downsample2, sheet, 2);
            context.command.BlitFullscreenTriangle(m_downsample2, m_downsample1, sheet, 3);

            context.command.SetGlobalTexture(m_blurTexID, m_downsample1);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 4);

            context.command.ReleaseTemporaryRT(m_downsample1);
            context.command.ReleaseTemporaryRT(m_downsample2);
        }
    }
}
