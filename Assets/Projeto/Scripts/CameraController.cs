using UnityEngine;

namespace Projeto.Scripts
{
    public class CameraController : MonoBehaviour
    {
        private float _playerX;
        private float _playerY;
        private float _offSetX = 3f;
        private float _smooth = 0.1f;

        public float _limetedUp;
        public float _limetedDown;
        public float _limetedLeft;
        public float _limetedRight;

        private Transform _playerTransform;

        private void Start()
        {
            _playerTransform = FindObjectOfType<PlayerController>().transform;
        }
    
        private void FixedUpdate()
        {
            if (_playerTransform == null) return;

            var position = _playerTransform.position;

            _playerX = Mathf.Clamp(position.x + _offSetX, _limetedLeft, _limetedRight);
            _playerY = Mathf.Clamp(position.y, _limetedDown, _limetedUp);

            transform.position = Vector3.Lerp(transform.position, new Vector3(_playerX, _playerY, transform.position.z), _smooth);
        }
    }
}
