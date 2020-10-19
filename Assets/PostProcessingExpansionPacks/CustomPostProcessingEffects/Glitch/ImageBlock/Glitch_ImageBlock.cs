using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(Glitch_ImageBlockRenderer), PostProcessEvent.AfterStack, "Custom/Glitch/Image Block")]
    public sealed class Glitch_ImageBlock : PostProcessEffectSettings
    {
        [Range(0.0f, 50.0f)]
        public FloatParameter Speed = new FloatParameter { value = 10f };

        [Range(0.0f, 50.0f)]
        public FloatParameter BlockSize = new FloatParameter { value = 8f };
    }

    internal sealed class Glitch_ImageBlockRenderer : PostProcessEffectRenderer<Glitch_ImageBlock>
    {
		private const string SHADER_NAME = "Hidden/Custom/Glitch/Image Block";
        private Shader m_shader;

        private static class ShaderIDs
        {
            internal static readonly int _Params = Shader.PropertyToID("_Params");
        }

        public override void Init()
        {
            m_shader = Shader.Find(SHADER_NAME);
            
            base.Init();
        }
        
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(m_shader);

            sheet.properties.SetVector(ShaderIDs._Params, new Vector2(settings.Speed, settings.BlockSize));

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
