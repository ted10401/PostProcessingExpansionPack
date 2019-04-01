using System;

namespace UnityEngine.Rendering.PostProcessing
{
    [Serializable]
    [PostProcess(typeof(VerticalFogRenderer), PostProcessEvent.AfterStack, "Custom/VerticalFog")]
    public sealed class VerticalFog : PostProcessEffectSettings
    {
        [Range(0f, 1f)]
        public FloatParameter fogDensity = new FloatParameter { value = 0f };
        public ColorParameter fogColor = new ColorParameter { value = Color.white };
        public FloatParameter fogStart = new FloatParameter { value = 0f };
        public FloatParameter fogEnd = new FloatParameter { value = 0f };
    }

    internal sealed class VerticalFogRenderer : PostProcessEffectRenderer<VerticalFog>
    {
        private Shader m_shader;
        private int m_frustumCornersID;
        private int m_fogDensityID;
        private int m_fogColorID;
        private int m_fogStartID;
        private int m_fogEndID;

        public override void Init()
        {
            m_shader = Shader.Find("Hidden/Custom/VerticalFog");
            m_frustumCornersID = Shader.PropertyToID("_FrustumCornersRay");
            m_fogDensityID = Shader.PropertyToID("_FogDensity");
            m_fogColorID = Shader.PropertyToID("_FogColor");
            m_fogStartID = Shader.PropertyToID("_FogStart");
            m_fogEndID = Shader.PropertyToID("_FogEnd");
        }
        
        public override void Render(PostProcessRenderContext context)
        {
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

            sheet.properties.SetMatrix(m_frustumCornersID, frustumCorners);
            sheet.properties.SetFloat(m_fogDensityID, settings.fogDensity);
            sheet.properties.SetColor(m_fogColorID, settings.fogColor);
            sheet.properties.SetFloat(m_fogStartID, settings.fogStart);
            sheet.properties.SetFloat(m_fogEndID, settings.fogEnd);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
