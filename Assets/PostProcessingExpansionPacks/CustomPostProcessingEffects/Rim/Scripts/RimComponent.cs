
namespace UnityEngine.Rendering.PostProcessing
{
    public class RimComponent : MonoBehaviour
    {
        [SerializeField] private RimType m_rimType = RimType.Normal;
        [SerializeField] private Color m_color = Color.white;
        [SerializeField] private float m_power = 5;
        [SerializeField] private float m_intensity = 2;
        private RimData m_rimData;

        private void Awake()
        {
            m_rimData = new RimData(gameObject, m_rimType, m_color, m_power, m_intensity);
        }

        private void OnValidate()
        {
            if (m_rimData == null)
            {
                m_rimData = new RimData(gameObject, m_rimType, m_color, m_power, m_intensity);
            }

            m_rimData.SetRimType(m_rimType);
            m_rimData.SetColor(m_color);
            m_rimData.SetPower(m_power);
            m_rimData.SetIntensity(m_intensity);
        }

        private void OnEnable()
        {
            RimManager.Instance.Register(m_rimData);
        }

        private void OnDisable()
        {
            RimManager.Instance.Unregister(m_rimData);
        }
    }
}