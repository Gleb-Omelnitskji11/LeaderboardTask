using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace Preloader
{
    public class Preloader : MonoBehaviour
    {
        [SerializeField] private Slider m_Slider;

        private bool m_ReadyToChangeScene;
        private bool m_IsInitialized;

        public void SetSlider(float progress)
        {
            m_Slider.value = progress;
        }

        public void GoToGameMenu()
        {
            m_IsInitialized = true;
            ChangeScene();
        }

        private void Start()
        {
            m_ReadyToChangeScene = true;
            ChangeScene();
        }

        private void ChangeScene()
        {
            if (m_IsInitialized && m_ReadyToChangeScene)
            {
                SceneManager.LoadScene(Constants.Scenes.MenuScene);
            }
        }
    }
}