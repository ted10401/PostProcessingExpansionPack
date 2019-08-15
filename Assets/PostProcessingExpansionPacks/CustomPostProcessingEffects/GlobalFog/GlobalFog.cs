using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(GlobalFogRenderer), PostProcessEvent.AfterStack, "Custom/GlobalFog")]
    public sealed class GlobalFog : PostProcessEffectSettings
    {
        [Range(0f, 1f)] public FloatParameter weight = new FloatParameter { value = 1, overrideState = true };
        public ColorParameter fogDepthColor = new ColorParameter { value = Color.white };
        [Range(0f, 1f)] public FloatParameter fogDepthDensity = new FloatParameter { value = 0.5f };
        public FloatParameter fogDepthStrength = new FloatParameter { value = 1f };
        public FloatParameter fogDepthPower = new FloatParameter { value = 1 };
        public ColorParameter fogHeightColor = new ColorParameter { value = Color.white };
        public FloatParameter fogHeightStart = new FloatParameter { value = 0f };
        public FloatParameter fogHeightRange = new FloatParameter { value = 10f };
        [Range(0f, 1f)] public FloatParameter fogHeightDensity = new FloatParameter { value = 1f };
        public TextureParameter noiseTexture = new TextureParameter { value = null };
        public Vector2Parameter fogSpeed = new Vector2Parameter { value = Vector2.one * 0.1f };
        [Range(0f, 1f)]
        public FloatParameter noiseAmount = new FloatParameter { value = 0.5f };
    }

    internal sealed class GlobalFogRenderer : PostProcessEffectRenderer<GlobalFog>
    {
        private Shader m_shader;
        private int m_weightID;
        private int m_frustumCornersID;
        private int m_fogDepthColorID;
        private int m_fogDepthDensityID;
        private int m_fogDepthStrengthID;
        private int m_fogDepthPowerID;
        private int m_fogHeightColorID;
        private int m_fogHeightStartID;
        private int m_fogHeightRangeID;
        private int m_fogHeightDensityID;
        private int m_noiseTexID;
        private int m_fogSpeedXID;
        private int m_fogSpeedYID;
        private int m_noiseAmountID;

        public override void Init()
        {
            m_shader = Shader.Find("Hidden/Custom/GlobalFog");
            m_weightID = Shader.PropertyToID("_Weight");
            m_frustumCornersID = Shader.PropertyToID("_FrustumCornersRay");
            m_fogDepthColorID = Shader.PropertyToID("_FogDepthColor");
            m_fogDepthDensityID = Shader.PropertyToID("_FogDepthDensity");
            m_fogDepthStrengthID = Shader.PropertyToID("_FogDepthStrength");
            m_fogDepthPowerID = Shader.PropertyToID("_FogDepthPower");
            m_fogHeightColorID = Shader.PropertyToID("_FogHeightColor");
            m_fogHeightStartID = Shader.PropertyToID("_FogHeightStart");
            m_fogHeightRangeID = Shader.PropertyToID("_FogHeightRange");
            m_fogHeightDensityID = Shader.PropertyToID("_FogHeightDensity");
            m_noiseTexID = Shader.PropertyToID("_NoiseTex");
            m_fogSpeedXID = Shader.PropertyToID("_FogXSpeed");
            m_fogSpeedYID = Shader.PropertyToID("_FogYSpeed");
            m_noiseAmountID = Shader.PropertyToID("_NoiseAmount");
        }
        
        public override void Render(PostProcessRenderContext context)
        {
            if(settings.weight == 0)
            {
                context.command.BlitFullscreenTriangle(context.source, context.destination);
                return;
            }

            var sheet = context.propertySheets.Get(m_shader);

            float fov = context.camera.fieldOfView;
            float near = context.camera.nearClipPlane;
            float far = context.camera.farClipPlane;
            float aspect = context.camera.aspect;

            float halfHeight = near * Mathf.Tan(0.5f * fov * Mathf.Deg2Rad);
            Vector3 toRight = context.camera.transform.right * halfHeight * aspect;
            Vector3 toTop = context.camera.transform.up * halfHeight;

            Vector3 topLeft = context.camera.transform.forward * near + toTop - toRight;
            Vector3 topRight = context.camera.transform.forward * near + toTop + toRight;
            Vector3 bottomLeft = context.camera.transform.forward * near - toTop - toRight;
            Vector3 bottomRight = context.camera.transform.forward * near - toTop + toRight;

            float scale = topLeft.magnitude / near;

            topLeft.Normalize();
            topLeft *= scale;
            topRight.Normalize();
            topRight *= scale;
            bottomLeft.Normalize();
            bottomLeft *= scale;
            bottomRight.Normalize();
            bottomRight *= scale;

            Matrix4x4 frustumCorners = Matrix4x4.identity;
            frustumCorners.SetRow(0, bottomLeft);
            frustumCorners.SetRow(1, bottomRight);
            frustumCorners.SetRow(2, topRight);
            frustumCorners.SetRow(3, topLeft);

            sheet.properties.SetFloat(m_weightID, settings.weight);
            sheet.properties.SetMatrix(m_frustumCornersID, frustumCorners);
            sheet.properties.SetColor(m_fogDepthColorID, settings.fogDepthColor);
            sheet.properties.SetFloat(m_fogDepthDensityID, settings.fogDepthDensity);
            sheet.properties.SetFloat(m_fogDepthStrengthID, settings.fogDepthStrength);
            sheet.properties.SetFloat(m_fogDepthPowerID, settings.fogDepthPower);
            sheet.properties.SetColor(m_fogHeightColorID, settings.fogHeightColor);
            sheet.properties.SetFloat(m_fogHeightStartID, settings.fogHeightStart);
            sheet.properties.SetFloat(m_fogHeightRangeID, settings.fogHeightRange);
            sheet.properties.SetFloat(m_fogHeightDensityID, settings.fogHeightDensity);
            if(settings.noiseTexture.value != null)
            {
                sheet.properties.SetTexture(m_noiseTexID, settings.noiseTexture.value);
            }
            sheet.properties.SetFloat(m_fogSpeedXID, settings.fogSpeed.value.x);
            sheet.properties.SetFloat(m_fogSpeedYID, settings.fogSpeed.value.y);
            sheet.properties.SetFloat(m_noiseAmountID, settings.noiseAmount);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
