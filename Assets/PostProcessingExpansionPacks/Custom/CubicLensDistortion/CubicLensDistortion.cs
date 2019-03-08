using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(CubicLensDistortionRenderer), PostProcessEvent.AfterStack, "Custom/CubicLensDistortion")]
    public sealed class CubicLensDistortion : PostProcessEffectSettings
    {
        [Tooltip("K value")]
        public FloatParameter k = new FloatParameter { value = 0f };

        [Tooltip("KCube value")]
        public FloatParameter kCube = new FloatParameter { value = 0f };
    }

    internal sealed class CubicLensDistortionRenderer : PostProcessEffectRenderer<CubicLensDistortion>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/CubicLensDistortion"));

            sheet.properties.SetFloat("_K", settings.k);
            sheet.properties.SetFloat("_KCube", settings.kCube);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
