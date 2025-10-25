using SimplePopupManager;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Zenject;

namespace ButtonsScripts
{
    public class LeaderboardOpenButton : MonoBehaviour
    {
        [SerializeField] private Button m_Button;
        [Inject] private IPopupManagerService m_PopupManagerService;

        private void Start()
        {
            m_Button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            m_Button.interactable = false;
            m_PopupManagerService.OpenPopup(Constants.Popups.LeaderboardPopup, null);
            Invoke(nameof(ActivateButton), 1f);
        }

        public void ActivateButton()
        {
            m_Button.interactable = true;
        }
    }
}