using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(CustomMotionBlurRenderer), PostProcessEvent.AfterStack, "Custom/Kernels/CustomMotionBlur")]
    public sealed class CustomMotionBlur : PostProcessEffectSettings
    {
        public Vector2Parameter center = new Vector2Parameter { value = new Vector2(0.5f, 0.5f) };
        [Range(0f, 1f)]
        public FloatParameter nearClip = new FloatParameter { value = 1.0f };
        public Vector2Parameter distanceStrength = new Vector2Parameter { value = Vector2.one };
        [Range(0f, 1f)]
        public FloatParameter strength = new FloatParameter { value = 1 };
    }

    internal sealed class CustomMotionBlurRenderer : PostProcessEffectRenderer<CustomMotionBlur>
    {
        private Shader m_shader;
        
        public override void Init()
        {
            m_shader = Shader.Find("Hidden/Custom/Kernels/CustomMotionBlur");
        }
        
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(m_shader);

            sheet.properties.SetVector("_Center", settings.center);
            sheet.properties.SetFloat("_NearClip", settings.nearClip);
            sheet.properties.SetVector("_DistanceStrength", settings.distanceStrength);
            sheet.properties.SetFloat("_Strength", settings.strength);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
