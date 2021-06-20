using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Projeto.Scripts
{
    public class LevelController : MonoBehaviour
    {
        public void Play()
        {
            SceneManager.LoadScene("Level1");
        }

        public void ReturnToMenu()
        {
            SceneManager.LoadScene("Menu");
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}
