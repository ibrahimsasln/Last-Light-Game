using System.Collections;
using UnityEngine;
using Tilemania.Combat;
using Tilemania.Managers;

namespace Tilemania.Player
{
    public class PlayerDeath : MonoBehaviour
    {
        static readonly int DyingHash = Animator.StringToHash("Dying");

        [SerializeField] float deathBounce = 10f;
        [SerializeField] float gameOverDelay = 2f;

        Health health;
        Rigidbody2D playerRB;
        Animator playerAnimator;

        private void Awake()
        {
            health = GetComponent<Health>();
            playerRB = GetComponent<Rigidbody2D>();
            playerAnimator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            health.Died += OnDied;
        }

        private void OnDisable()
        {
            health.Died -= OnDied;
        }

        private void OnDied()
        {
            playerAnimator.SetTrigger(DyingHash);
            playerRB.linearVelocity = new Vector2(0f, deathBounce);
            StartCoroutine(GameOverAfterDelay());
        }

        private IEnumerator GameOverAfterDelay()
        {
            yield return new WaitForSeconds(gameOverDelay);
            GameManager.Instance.GameOver();
        }
    }
}
