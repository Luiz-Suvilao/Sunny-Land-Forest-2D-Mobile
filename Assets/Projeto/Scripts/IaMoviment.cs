using UnityEngine;

namespace Projeto.Scripts
{
    public class IaMoviment : MonoBehaviour
    {
        public Transform enemie;
        public Transform[] position;

        private SpriteRenderer _enemieSprit;

        private bool _isRight = true;

        private float _speed = 2.5f;

        private int _idTarget;

        void Start()
        {
            _enemieSprit = enemie.gameObject.GetComponent<SpriteRenderer>();
            enemie.position = position[0].position;
            _idTarget = 1;
        }

        void Update()
        {
            Moviment();
        }

        private void Moviment()
        {
            if (enemie == null) return;

            enemie.position = Vector3.MoveTowards(enemie.position, position[_idTarget].position, _speed * Time.deltaTime);

            if (enemie.position != position[_idTarget].position) return;

            _idTarget += 1;

            if (_idTarget == position.Length)
            {
                _idTarget = 0;
            }

            if (position[_idTarget].position.x < enemie.position.x && _isRight)
            {
                Flip();
            }
            else if (position[_idTarget].position.x > enemie.position.x && !_isRight)
            {
                Flip();
            }
        }

        private void Flip()
        {
            _isRight = !_isRight;
            _enemieSprit.flipX = !_enemieSprit.flipX;
        }
    }
}