using Platformer.Mechanics;
using Platformer.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer.UI
{
    [DisallowMultipleComponent]
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField]
        GameObject pauseMenuVisibleRoot;

        public bool Paused { get; private set; }

        void Update()
        {
            if (Input.GetButtonDown("Pause"))
            {
                _TogglePuase(!Paused);
            }
            if (Paused)
            {
                if (Input.GetKey(KeyCode.R))
                {
                    // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    SceneTransition.Instance.LoadNowScene();
                }
                else if (Input.GetKey(KeyCode.Q))
                {
                    // SceneManager.LoadScene("Chimneis");
                    SceneTransition.Instance.LoadNextScene("StageSelect");
                }
            }
        }
        void OnEnable()
        {
            _TogglePuase(false);
        }

        void _TogglePuase(bool pause)
        {
            Paused = pause;
            if (pause)
            {
                Time.timeScale = 0;
                if (pauseMenuVisibleRoot != null)
                    pauseMenuVisibleRoot.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                if (pauseMenuVisibleRoot != null)
                    pauseMenuVisibleRoot.SetActive(false);
            }
        }
    }
}
