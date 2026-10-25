using UnityEngine;
using Tilemania.Combat;
using Tilemania.Core;

namespace Tilemania.Enemies
{
    public class SlimeMovement : MonoBehaviour, IEnemyMovement
    {
        enum State { Waiting, Jumping }

        [SerializeField] float waitTime = 1.5f;
        [SerializeField] Vector2 jumpForce = new Vector2(3f, 6f);
        [SerializeField] float wallJumpMultiplier = 1.6f;
        [SerializeField] float wallCheckDistance = 1.5f;
        [SerializeField] float groundCheckDistance = 0.6f;
        [SerializeField] float groundCheckRadius = 0.25f;
        [SerializeField] float maxAirTime = 3f;

        Rigidbody2D slimeRB;
        Transform playerTransform;
        Health playerHealth;
        LayerMask groundLayer;

        State state = State.Waiting;
        float nextJumpTime;
        float forceLandTime;
        float jumpVelocityX;
        float speedMultiplier = 1f;

        private void Awake()
        {
            slimeRB = GetComponent<Rigidbody2D>();
            groundLayer = LayerMask.GetMask(LayerNames.Ground);

            GameObject player = GameObject.FindWithTag(Tags.Player);
            playerTransform = player.transform;
            playerHealth = player.GetComponent<Health>();

            nextJumpTime = Time.time + waitTime;
        }

        private void FixedUpdate()
        {
            if (playerHealth.IsDead) return;

            switch (state)
            {
                case State.Waiting:
                    UpdateWaiting();
                    break;
                case State.Jumping:
                    UpdateJumping();
                    break;
            }
        }

        private void UpdateWaiting()
        {
            slimeRB.linearVelocityX = 0f;

            if (Time.time >= nextJumpTime)
            {
                JumpTowardsPlayer();
            }
        }

        private void UpdateJumping()
        {
            slimeRB.linearVelocityX = jumpVelocityX;

            bool isFalling = slimeRB.linearVelocityY <= 0f;
            bool isStuck = Time.time >= forceLandTime;

            if ((isFalling && IsGrounded()) || isStuck)
            {
                Land();
            }
        }

        private void JumpTowardsPlayer()
        {
            float direction = Mathf.Sign(playerTransform.position.x - transform.position.x);
            float jumpHeight = IsWallAhead(direction) ? jumpForce.y * wallJumpMultiplier : jumpForce.y;

            jumpVelocityX = direction * jumpForce.x * speedMultiplier;

            transform.localScale = new Vector2(direction, 1f);
            slimeRB.linearVelocity = new Vector2(jumpVelocityX, jumpHeight);
            forceLandTime = Time.time + maxAirTime;

            state = State.Jumping;
        }

        private void Land()
        {
            slimeRB.linearVelocityX = 0f;
            nextJumpTime = Time.time + waitTime / speedMultiplier;

            state = State.Waiting;
        }

        public void SetSpeedMultiplier(float multiplier)
        {
            speedMultiplier = multiplier;
        }

        private bool IsGrounded()
        {
            float castDistance = groundCheckDistance - groundCheckRadius;
            bool hit = Physics2D.CircleCast(transform.position, groundCheckRadius, Vector2.down, castDistance, groundLayer);
            Debug.DrawRay(transform.position, Vector2.down * groundCheckDistance, hit ? Color.green : Color.red);
            return hit;
        }

        private bool IsWallAhead(float direction)
        {
            bool hit = Physics2D.Raycast(transform.position, Vector2.right * direction, wallCheckDistance, groundLayer);
            Debug.DrawRay(transform.position, Vector2.right * direction * wallCheckDistance, hit ? Color.green : Color.red);
            return hit;
        }
    }
}
