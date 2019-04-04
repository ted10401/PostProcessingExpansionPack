using UnityEngine;

public class OutlineComponent : MonoBehaviour
{
    [SerializeField] private Color m_color = Color.white;
    [SerializeField] private OutlinePrepassType m_outlinePrepassType = OutlinePrepassType.SolidColor;
    private OutlineData m_outlineData;

    private void Awake()
    {
        m_outlineData = new OutlineData(gameObject, m_color, m_outlinePrepassType);
    }

    private void OnValidate()
    {
        if(m_outlineData == null)
        {
            m_outlineData = new OutlineData(gameObject, m_color, m_outlinePrepassType);
        }

        m_outlineData.SetColor(m_color);
        m_outlineData.SetPrepassType(m_outlinePrepassType);
    }

    private void OnEnable()
    {
        OutlineManager.Instance.Register(m_outlineData);
    }

    private void OnDisable()
    {
        OutlineManager.Instance.Unregister(m_outlineData);
    }
}
