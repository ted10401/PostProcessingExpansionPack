using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(GameBoyRenderer), PostProcessEvent.AfterStack, "Custom/GameBoy")]
    public sealed class GameBoy : PostProcessEffectSettings
    {
        [Range(0.001f, 0.1f)]
        public FloatParameter pixelSize = new FloatParameter { value = 0.001f };
    }

    internal sealed class GameBoyRenderer : PostProcessEffectRenderer<GameBoy>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/GameBoy"));

            sheet.properties.SetFloat("_PixelSize", settings.pixelSize);
            
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
