using UnityEngine;
using UnityEngine.UI;

namespace Projeto.Scripts
{
    public class GameController : MonoBehaviour
    {
        public Text txtScore;
        public GameObject enemieDeathPrefab;
        public Sprite[] imageLife;
        public Image life;

        private int _score;

        public void Score(int numberOfPoints)
        {
            _score += numberOfPoints;
            txtScore.text = _score.ToString();
        }

        public void ChangeLifeImg(int health)
        {
            life.sprite = imageLife[health];
        }
    }
}
