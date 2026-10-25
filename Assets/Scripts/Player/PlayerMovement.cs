using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Tilemania.Combat;
using Tilemania.Core;

namespace Tilemania.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        static readonly int IsRunningHash = Animator.StringToHash("isRunning");
        static readonly int RunDirectionHash = Animator.StringToHash("RunDirection");

        [Header("Movement")]
        [SerializeField] float moveSpeed = 8f;
        [SerializeField] float jumpPower = 13f;

        [Header("Knockback")]
        [SerializeField] Vector2 knockbackForce = new Vector2(6f, 5f);
        [SerializeField] float knockbackDuration = 0.25f;

        [Header("Dodge")]
        [SerializeField] float dodgeSpeed = 16f;
        [SerializeField] float dodgeDuration = 0.18f;
        [SerializeField] float dodgeCooldown = 0.8f;

        Health health;
        Rigidbody2D playerRB;
        BoxCollider2D feetCollider;
        Animator playerAnimator;
        LayerMask groundLayer;
        Vector2 moveInput;
        float controlLockedUntil;
        float nextDodgeTime;
        float speedMultiplier = 1f;
        bool hasRunDirectionParameter;

        public bool IsDodgeUnlocked { get; private set; }

        bool IsGrounded => feetCollider.IsTouchingLayers(groundLayer);
        bool IsControlLocked => Time.time < controlLockedUntil;
        float Facing => Mathf.Sign(transform.localScale.x);

        private void Awake()
        {
            health = GetComponent<Health>();
            playerRB = GetComponent<Rigidbody2D>();
            feetCollider = GetComponent<BoxCollider2D>();
            playerAnimator = GetComponent<Animator>();
            groundLayer = LayerMask.GetMask(LayerNames.Ground);
            hasRunDirectionParameter = HasParameter(playerAnimator, RunDirectionHash);
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
            if (health.IsDead || IsControlLocked) return;

            playerRB.linearVelocityX = moveInput.x * moveSpeed * speedMultiplier;

            bool isMoving = Mathf.Abs(playerRB.linearVelocityX) > Mathf.Epsilon;
            playerAnimator.SetBool(IsRunningHash, isMoving);

            if (hasRunDirectionParameter)
            {
                bool isMovingBackwards = isMoving && Mathf.Sign(playerRB.linearVelocityX) != Facing;
                playerAnimator.SetFloat(RunDirectionHash, isMovingBackwards ? -1f : 1f);
            }
        }

        private void OnMove(InputValue value)
        {
            moveInput = value.Get<Vector2>();
        }

        private void OnJump(InputValue value)
        {
            if (health.IsDead || IsControlLocked) return;

            if (value.isPressed && IsGrounded)
            {
                playerRB.linearVelocityY = jumpPower;
            }
        }

        private void OnSprint(InputValue value)
        {
            if (!value.isPressed || !IsDodgeUnlocked || health.IsDead) return;
            if (IsControlLocked || Time.time < nextDodgeTime) return;

            float direction = Mathf.Abs(moveInput.x) > 0.1f ? Mathf.Sign(moveInput.x) : Facing;
            StartCoroutine(Dodge(direction));
        }

        public void AddSpeedBonus(float bonus)
        {
            speedMultiplier += bonus;
        }

        public void UnlockDodge()
        {
            IsDodgeUnlocked = true;
        }

        private IEnumerator Dodge(float direction)
        {
            nextDodgeTime = Time.time + dodgeCooldown;
            controlLockedUntil = Time.time + dodgeDuration;
            health.MakeInvulnerable(dodgeDuration);

            float gravityScale = playerRB.gravityScale;
            playerRB.gravityScale = 0f;
            playerRB.linearVelocity = new Vector2(direction * dodgeSpeed, 0f);

            yield return new WaitForSeconds(dodgeDuration);
            playerRB.gravityScale = gravityScale;
        }

        private void ApplyKnockback(Vector2 hitFrom)
        {
            float awayFromHit = Mathf.Sign(transform.position.x - hitFrom.x);
            playerRB.linearVelocity = new Vector2(awayFromHit * knockbackForce.x, knockbackForce.y);
            controlLockedUntil = Time.time + knockbackDuration;
        }

        private static bool HasParameter(Animator animator, int nameHash)
        {
            foreach (AnimatorControllerParameter parameter in animator.parameters)
            {
                if (parameter.nameHash == nameHash) return true;
            }
            return false;
        }
    }
}
