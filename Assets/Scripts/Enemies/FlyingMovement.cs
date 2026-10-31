using UnityEngine;
using LastLight.Combat;

namespace LastLight.Enemies
{
    public class FlyingMovement : MonoBehaviour, IEnemyMovement
    {
        [SerializeField] float speed = 2.5f;
        [SerializeField] float acceleration = 4f;
        [SerializeField] float bobAmplitude = 0.6f;
        [SerializeField] float bobFrequency = 2f;

        Rigidbody2D flyingRB;
        Transform targetTransform;
        Health targetHealth;
        float speedMultiplier = 1f;

        private void Awake()
        {
            flyingRB = GetComponent<Rigidbody2D>();
            flyingRB.gravityScale = 0f;
        }

        private void FixedUpdate()
        {
            Vector2 desiredVelocity = Vector2.zero;

            if (targetHealth != null && !targetHealth.IsDead)
            {
                Vector2 toTarget = targetTransform.position - transform.position;
                desiredVelocity = toTarget.normalized * speed * speedMultiplier;
                desiredVelocity.y += Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;

                transform.localScale = new Vector2(Mathf.Sign(toTarget.x), 1f);
            }

            flyingRB.linearVelocity = Vector2.MoveTowards(flyingRB.linearVelocity, desiredVelocity, acceleration * Time.fixedDeltaTime);
        }

        public void Initialize(Health target, float speedMultiplier)
        {
            targetHealth = target;
            targetTransform = target.transform;
            this.speedMultiplier = speedMultiplier;
            flyingRB.linearVelocity = Vector2.zero;
        }
    }
}
