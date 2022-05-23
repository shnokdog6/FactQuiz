using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class SceneController : MonoBehaviour
    {
        public void Play()
        {
            SceneManager.LoadScene(1);
        }
        public void Exit()
        {
            Application.Quit();
        }
    }
}