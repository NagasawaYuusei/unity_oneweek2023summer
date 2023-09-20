using UnityEngine;
using UnityEngine.UI;

namespace KyawaLib
{
    public class FaderCanvasRoot : CanvasRoot
    {
        [SerializeField]
        Image m_image = null;
        public Image Image => m_image;
    }
}