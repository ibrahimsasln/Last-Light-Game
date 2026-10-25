using UnityEngine;
using Tilemania.Combat;
using Tilemania.Core;

namespace Tilemania.Enemies
{
    public class ZombieMovement : MonoBehaviour, IEnemyMovement
    {
        [SerializeField] float speed = 2f;
        [SerializeField] float jumpPower = 7f;
        [SerializeField] float wallCheckDistance = 0.45f;
        [SerializeField] float groundCheckDistance = 0.6f;

        Rigidbody2D zombieRB;
        Transform playerTransform;
        Health playerHealth;
        LayerMask groundLayer;
        float speedMultiplier = 1f;

        private void Awake()
        {
            zombieRB = GetComponent<Rigidbody2D>();
            groundLayer = LayerMask.GetMask(LayerNames.Ground);

            GameObject player = GameObject.FindWithTag(Tags.Player);
            playerTransform = player.transform;
            playerHealth = player.GetComponent<Health>();
        }

        private void FixedUpdate()
        {
            if (playerHealth.IsDead)
            {
                zombieRB.linearVelocityX = 0f;
                return;
            }

            float direction = Mathf.Sign(playerTransform.position.x - transform.position.x);
            zombieRB.linearVelocityX = direction * speed * speedMultiplier;
            transform.localScale = new Vector2(direction, 1f);

            if (IsGrounded() && IsWallAhead(direction))
            {
                zombieRB.linearVelocityY = jumpPower;
            }
        }

        public void SetSpeedMultiplier(float multiplier)
        {
            speedMultiplier = multiplier;
        }

        private bool IsGrounded()
        {
            return CastRay(transform.position, Vector2.down, groundCheckDistance);
        }

        private bool IsWallAhead(float direction)
        {
            return CastRay(transform.position, Vector2.right * direction, wallCheckDistance);
        }

        private bool CastRay(Vector2 origin, Vector2 direction, float distance)
        {
            bool hit = Physics2D.Raycast(origin, direction, distance, groundLayer);
            Debug.DrawRay(origin, direction * distance, hit ? Color.green : Color.red);
            return hit;
        }
    }
}
