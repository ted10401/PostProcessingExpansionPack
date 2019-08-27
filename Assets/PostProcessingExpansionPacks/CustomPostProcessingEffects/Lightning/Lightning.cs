using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(LightningRenderer), PostProcessEvent.AfterStack, "Custom/Lightning")]
    public sealed class Lightning : PostProcessEffectSettings
    {
        public FloatParameter lightningFlicker = new FloatParameter { value = 10 };
        public FloatParameter lightningFlash = new FloatParameter { value = 2 };
    }

    internal sealed class LightningRenderer : PostProcessEffectRenderer<Lightning>
    {
        private const string LIGHTNING_FLICKER_PROPERTY_NAME = "_LightningFlicker";
        private const string LIGHTNING_FLASH_PROPERTY_NAME = "_LightningFlash";

        private Shader m_shader;
        private int m_lightningFlickerID;
        private int m_lightningFlashID;
        
        public override void Init()
        {
            m_shader = Shader.Find("Hidden/Custom/Lightning");
            m_lightningFlickerID = Shader.PropertyToID(LIGHTNING_FLICKER_PROPERTY_NAME);
            m_lightningFlashID = Shader.PropertyToID(LIGHTNING_FLASH_PROPERTY_NAME);

            base.Init();
        }
        
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(m_shader);
            sheet.properties.SetFloat(m_lightningFlickerID, settings.lightningFlicker);
            sheet.properties.SetFloat(m_lightningFlashID, settings.lightningFlash);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
