using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Projeto.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        private Animator _playerAnimator;
        private Rigidbody2D _playerRigidbody2D;
        private GameController _gameController;
        private SpriteRenderer _spriteRenderer;

        public Transform GroundCheck;
        public AudioSource fxGame;
        public AudioClip fxJump;
        public AudioClip fxCollectedItem;
        public AudioClip fxEnemieDead;
        public AudioClip fxPlayerDie;
        public AudioClip fxPlayerDamage;
        public Color noHitColor;
        public GameObject PlayerDie;
        public ParticleSystem Particle;
        public GameObject Warning;
        public GameObject InfoNPC;
        
        private bool _facingRight = true;
        private bool _isGround;
        private bool _jump;
        private bool _invincible;

        private const float Speed = 5f;
        private const float JumpForce = 600f;
        private float _touchRun = 0.0f;

        private int _numberJumps;
        private int _numberEnemies;
        private const int MaximumJumps = 2;

        private int lifes = 3;

        private void Start()
        {
            _playerAnimator = GetComponent<Animator>();
            _playerRigidbody2D = GetComponent<Rigidbody2D>();
            _gameController = FindObjectOfType(typeof(GameController)) as GameController;
        }

        private void Update()
        {
            _numberEnemies = GameObject.FindGameObjectsWithTag("Inimigo").Length;
            _isGround = Physics2D.Linecast(transform.position, GroundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
            _playerAnimator.SetBool("isGrounded", _isGround);

            _touchRun = Input.GetAxisRaw("Horizontal");

            if (Input.GetButtonDown("Jump"))
            {
                _jump = true;
            }

            ResolveAnimations();
        }

        private void MovePlayer(float moviment)
        {
            _playerRigidbody2D.velocity = new Vector2(moviment * Speed, _playerRigidbody2D.velocity.y);

            if (moviment < 0 && _facingRight || (moviment > 0 && !_facingRight))
            {
                Flip();
            }
        }

        private void FixedUpdate()
        {
            MovePlayer(_touchRun);

            if (_jump)
            {
                JumPlayer();
            }
        }

        private void Flip()
        {
            CreateParticle();
            _facingRight = !_facingRight;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        private void ResolveAnimations()
        {
            _playerAnimator.SetBool("Walk", _playerRigidbody2D.velocity.x != 0 && _isGround);
            _playerAnimator.SetBool("Jump", !_isGround);
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void JumPlayer()
        {
            if (_isGround)
            {
                _numberJumps = 0;
                CreateParticle();
            }

            if (_isGround || _numberJumps < MaximumJumps)
            {
                _playerRigidbody2D.AddForce(new Vector2(0f, JumpForce));
                _isGround = false;
                _numberJumps++;
                fxGame.PlayOneShot(fxJump);
                CreateParticle();
            }

            _jump = false;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            switch (col.gameObject.tag)
            {
                case "Coletavel":
                    CollectedItems(col.gameObject);
                break;
                
                case "Inimigo":
                    KillEnemie(col.gameObject);
                break;
                
                case "Damage":
                    EnvironmentDamage();
                break;
                
                case "NextLevel":
                    NextLevel();
                break;
                
                case "InfoNPC":
                    InfoNPC.SetActive(true);
                break;
            }
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            switch (col.gameObject.tag)
            {
                case "NextLevel":
                    Warning.SetActive(false);
                break;

                case "InfoNPC":
                    InfoNPC.SetActive(false);
                break;
            }
        }

        private void KillEnemie(GameObject obj)
        {
            GameObject explosion = Instantiate(_gameController.enemieDeathPrefab, transform.position, transform.localRotation);
            Destroy(explosion, 0.5f);
            
            Rigidbody2D rb = GetComponentInParent<Rigidbody2D>();
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, 800f));
            
            fxGame.PlayOneShot(fxEnemieDead);
            
            Destroy(obj);
        }

        private void EnvironmentDamage()
        {
            _gameController.ChangeLifeImg(1);
            GameObject pDie = Instantiate(PlayerDie, transform.position, Quaternion.identity);
            Rigidbody2D rbDie = pDie.GetComponent<Rigidbody2D>();
            rbDie.AddForce(new Vector2(150f, 500f));

            fxGame.PlayOneShot(fxPlayerDie);
            Invoke("LoadGame", 4f);
            gameObject.SetActive(false);
        }

        private void NextLevel()
        {
            if (_numberEnemies > 0)
            {
                Warning.SetActive(true);
            }

            if (_numberEnemies == 0)
            {
                Warning.SetActive(false);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        private void CollectedItems(GameObject obj)
        {
            _gameController.Score(1);

            fxGame.PlayOneShot(fxCollectedItem);
            
            Destroy(obj);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            switch (collision.gameObject.tag)
            {
                case "Inimigo":
                    TakeDamage();
                break;

                case "Plataform":
                    this.transform.parent = collision.transform;
                break;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            switch (collision.gameObject.tag)
            {
                case "Plataform":
                    this.transform.parent = null;
                break;
            }
        }

        private void TakeDamage()
        {
            if (!_invincible)
            {
                _invincible = true;

                lifes--;
                fxGame.PlayOneShot(fxPlayerDamage);

                StartCoroutine("Damage");

                _gameController.ChangeLifeImg(lifes);

                if (lifes < 1)
                {
                    GameObject pDie = Instantiate(PlayerDie, transform.position, Quaternion.identity);
                    Rigidbody2D rbDie = pDie.GetComponent<Rigidbody2D>();
                    rbDie.AddForce(new Vector2(150f, 500f));

                    fxGame.PlayOneShot(fxPlayerDie);
                    Invoke("LoadGame", 4f);
                    gameObject.SetActive(false);
                }
            }
        }

        private void LoadGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        IEnumerator Damage()
        {
            _spriteRenderer.color = noHitColor;
            yield return new WaitForSeconds(0.1f);

            for (float i = 0; i < 1; i += 0.1f)
            {
                _spriteRenderer.enabled = false;
                yield return new WaitForSeconds(0.1f);
                _spriteRenderer.enabled = true;
                yield return new WaitForSeconds(0.1f);
            }

            _spriteRenderer.color = Color.white;
            _invincible = false;
        }

        void CreateParticle()
        {
            Particle.Play();
        }
    }
}
