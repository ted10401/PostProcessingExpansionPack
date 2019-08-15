using System.Collections.Generic;

namespace UnityEngine.Rendering.PostProcessing
{
    public class OutlineData
    {
        public GameObject parent;
        public Renderer[] renderers;
        public Color color;
        public OutlinePrepassType outlinePrepassType;
        public Material prepassMaterial { get; private set; }

        private Color m_lastColor;
        private OutlinePrepassType m_lastOutlinePrepassType;

        public OutlineData(GameObject parent, Color color, OutlinePrepassType outlinePrepassType)
        {
            this.parent = parent;

            List<Renderer> cacheRenderers = new List<Renderer>();

            LODGroup lODGroup = parent.GetComponent<LODGroup>();
            if(lODGroup != null)
            {
                LOD[] lODs = lODGroup.GetLODs();
                cacheRenderers.AddRange(lODs[0].renderers);
            }
            else
            {
                cacheRenderers.AddRange(parent.GetComponentsInChildren<SkinnedMeshRenderer>());
            }
            
            cacheRenderers.AddRange(parent.GetComponentsInChildren<MeshRenderer>());
            cacheRenderers.AddRange(parent.GetComponentsInChildren<SpriteRenderer>());
            renderers = cacheRenderers.ToArray();

            this.color = color;
            this.outlinePrepassType = outlinePrepassType;

            UpdatePrepassMaterial();
        }

        public void SetColor(Color color)
        {
            this.color = color;
            UpdatePrepassMaterial();
        }

        public void SetPrepassType(OutlinePrepassType outlinePrepassType)
        {
            this.outlinePrepassType = outlinePrepassType;
            UpdatePrepassMaterial();
        }

        private void UpdatePrepassMaterial()
        {
            if (prepassMaterial == null || m_lastOutlinePrepassType != outlinePrepassType)
            {
                m_lastOutlinePrepassType = outlinePrepassType;

                if (prepassMaterial == null)
                {
                    prepassMaterial = new Material(OutlineManager.Instance.GetPrepassShader(m_lastOutlinePrepassType));
                }
                else
                {
                    prepassMaterial.shader = OutlineManager.Instance.GetPrepassShader(m_lastOutlinePrepassType);
                }

                m_lastColor = color;
                prepassMaterial.SetColor(OutlineManager.Instance.COLOR_ID, m_lastColor);
            }
            else if (m_lastColor != color)
            {
                m_lastColor = color;
                prepassMaterial.SetColor(OutlineManager.Instance.COLOR_ID, m_lastColor);
            }
        }
    }
}