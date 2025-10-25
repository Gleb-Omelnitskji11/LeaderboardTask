using System;
using SimplePopupManager;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class PreloaderInstaller : MonoInstaller
    {
        [SerializeField] private Preloader.Preloader m_Preloader;

        private Action[] m_InitializationActions;

        public override void InstallBindings()
        {
            SetInitializationCommand();
            Loading();
        }

        private void SetInitializationCommand()
        {
            m_InitializationActions = new Action[]
            {
                () =>
                {
                    StaticContext.Container.Bind<IPopupManagerService>().To<PopupManagerServiceService>().AsSingle();
                }
            };
        }

        private void Loading()
        {
            int length = m_InitializationActions.Length - 1;
            m_Preloader.SetSlider(0.3f); // fake loading for 1 binding

            for (int i = 0; i < m_InitializationActions.Length; i++)
            {
                m_InitializationActions[i]();
                m_Preloader.SetSlider((float)i / length);
            }

            m_Preloader.GoToGameMenu();
        }
    }
}