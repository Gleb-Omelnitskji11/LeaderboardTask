using SimplePopupManager;
using UnityEngine;
using Zenject;

namespace Utils.PopupsUtils
{
    public class PopupsCanvas : MonoBehaviour
    {
        [Inject] private IPopupManagerService m_PopupManagerService;
        private void Start()
        {
            DontDestroyOnLoad(this);
            m_PopupManagerService.SetCanvas(this.GetComponent<Canvas>());
        }
    }
}
