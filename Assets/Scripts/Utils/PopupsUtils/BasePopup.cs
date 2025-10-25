using System;
using System.Threading.Tasks;
using SimplePopupManager;
using UnityEngine;
using Zenject;

namespace Utils.PopupsUtils
{
    public class BasePopup : MonoBehaviour, IPopupInitialization
    {
        [SerializeField] protected Animator m_Animator;
        
        public string PopupName;

        private IPopupManagerService m_PopupManagerService;
        private readonly int HideAnimatorParameter = Animator.StringToHash("Hide");
        private readonly int ShownAnimatorParameter = Animator.StringToHash("Shown");

        [Inject]
        public void Construct(IPopupManagerService popupManagerService)
        {
            m_PopupManagerService = popupManagerService;
        }

        public virtual void ShowPopup()
        {
            m_Animator.SetTrigger(HideAnimatorParameter);
        }

        public void DoHide()
        {
            m_Animator.SetTrigger(HideAnimatorParameter);
            m_PopupManagerService.ClosePopup(PopupName);
        }

        public void OnHide()
        {
            gameObject.SetActive(false);
            m_PopupManagerService.ClosePopup(PopupName);
        }

        public virtual Task Init(object param)
        {
            ShowPopup();
            return Task.CompletedTask;
        }
    }
}