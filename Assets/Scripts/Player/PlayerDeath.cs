using UnityEngine;
using LastLight.Combat;

namespace LastLight.Player
{
    public class PlayerDeath : MonoBehaviour
    {
        static readonly int DyingHash = Animator.StringToHash("Dying");

        [SerializeField] float deathBounce = 10f;

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
        }
    }
}
