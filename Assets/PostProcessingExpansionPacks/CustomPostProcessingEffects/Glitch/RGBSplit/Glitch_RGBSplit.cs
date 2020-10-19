using System;
using System.Xml.Linq;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(Glitch_RGBSplitRenderer), PostProcessEvent.AfterStack, "Custom/Glitch/RGB Split")]
    public sealed class Glitch_RGBSplit : PostProcessEffectSettings
    {
        public TextureParameter NoiseTexutre = new TextureParameter { };

        [Range(0.0f, 5.0f)]
        public FloatParameter Amplitude = new FloatParameter { value = 3f };

        [Range(0.0f, 1.0f)]
        public FloatParameter Speed = new FloatParameter { value = 0.1f };
    }

    internal sealed class Glitch_RGBSplitRenderer : PostProcessEffectRenderer<Glitch_RGBSplit>
    {
		private const string SHADER_NAME = "Hidden/Custom/Glitch/RGB Split";
        private Shader m_shader;

        private static class ShaderIDs
        {
            internal static readonly int _NoiseTex = Shader.PropertyToID("_NoiseTex");
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

            if(settings.NoiseTexutre != null)
            {
                sheet.properties.SetTexture(ShaderIDs._NoiseTex, settings.NoiseTexutre);
            }

            sheet.properties.SetVector(ShaderIDs._Params, new Vector2(settings.Amplitude, settings.Speed));

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
