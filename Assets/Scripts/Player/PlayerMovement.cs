using UnityEngine;
using UnityEngine.InputSystem;
using Tilemania.Combat;
using Tilemania.Core;

namespace Tilemania.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        static readonly int IsRunningHash = Animator.StringToHash("isRunning");

        [SerializeField] float moveSpeed = 8f;
        [SerializeField] float jumpPower = 13f;
        [SerializeField] Vector2 knockbackForce = new Vector2(6f, 5f);
        [SerializeField] float knockbackDuration = 0.25f;

        Health health;
        Rigidbody2D playerRB;
        BoxCollider2D feetCollider;
        Animator playerAnimator;
        LayerMask groundLayer;
        Vector2 moveInput;
        float knockbackEndTime;

        bool IsGrounded => feetCollider.IsTouchingLayers(groundLayer);
        bool IsKnockedBack => Time.time < knockbackEndTime;

        private void Awake()
        {
            health = GetComponent<Health>();
            playerRB = GetComponent<Rigidbody2D>();
            feetCollider = GetComponent<BoxCollider2D>();
            playerAnimator = GetComponent<Animator>();
            groundLayer = LayerMask.GetMask(LayerNames.Ground);
        }

        private void OnEnable()
        {
            health.Damaged += ApplyKnockback;
        }

        private void OnDisable()
        {
            health.Damaged -= ApplyKnockback;
        }

        private void FixedUpdate()
        {
            if (health.IsDead || IsKnockedBack) return;

            playerRB.linearVelocityX = moveInput.x * moveSpeed;

            bool isMoving = Mathf.Abs(playerRB.linearVelocityX) > Mathf.Epsilon;
            playerAnimator.SetBool(IsRunningHash, isMoving);

            if (isMoving)
            {
                transform.localScale = new Vector2(Mathf.Sign(playerRB.linearVelocityX), 1f);
            }
        }

        private void OnMove(InputValue value)
        {
            moveInput = value.Get<Vector2>();
        }

        private void OnJump(InputValue value)
        {
            if (health.IsDead || IsKnockedBack) return;

            if (value.isPressed && IsGrounded)
            {
                playerRB.linearVelocityY = jumpPower;
            }
        }

        private void ApplyKnockback(Vector2 hitFrom)
        {
            float awayFromHit = Mathf.Sign(transform.position.x - hitFrom.x);
            playerRB.linearVelocity = new Vector2(awayFromHit * knockbackForce.x, knockbackForce.y);
            knockbackEndTime = Time.time + knockbackDuration;
        }
    }
}
