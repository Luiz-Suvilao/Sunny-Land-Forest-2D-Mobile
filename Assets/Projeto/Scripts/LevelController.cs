using UnityEngine;
using UnityEngine.SceneManagement;

namespace Projeto.Scripts
{
    public class LevelController : MonoBehaviour
    {
        void Start()
        {
        
        }
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                EnterPressed();
            }
        }

         public void EnterPressed()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
