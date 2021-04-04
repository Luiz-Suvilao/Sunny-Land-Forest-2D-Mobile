using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Projeto.Scripts
{
    public class FadeController : MonoBehaviour
    {
        public static FadeController _FadeController;
        public Image _ImageFade;
        public Color _InitalColor;
        public Color _FinalColor;

        public float _FadeDuration;

        public bool IsFade;

        private float _time;

        private void Awake()
        {
            _FadeController = this;
        }

        IEnumerator InitFade()
        {
            IsFade = true;
            _time = 0;

            while (_time <= _FadeDuration)
            {
                _ImageFade.color = Color.Lerp(_InitalColor, _FinalColor, _time / _FadeDuration);
                _time = _time + Time.deltaTime;
                yield return null;
            }

            IsFade = false;
        }

        void Start()
        {
            StartCoroutine("InitFade");
        }
        
        void Update()
        {
        
        }
    }
}
