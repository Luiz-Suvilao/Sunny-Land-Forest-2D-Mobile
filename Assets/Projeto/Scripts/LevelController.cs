using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Projeto.Scripts
{
    public class LevelController : MonoBehaviour
    {
        public void Play()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
