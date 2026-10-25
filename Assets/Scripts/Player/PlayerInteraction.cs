using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Tilemania.Combat;
using Tilemania.Interaction;

namespace Tilemania.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] float interactRadius = 1.5f;

        Health health;
        ContactFilter2D filter;
        readonly List<Collider2D> nearbyColliders = new List<Collider2D>();
        IInteractable currentInteractable;

        private void Awake()
        {
            health = GetComponent<Health>();
            filter = ContactFilter2D.noFilter;
        }

        private void Update()
        {
            IInteractable nearest = health.IsDead ? null : FindNearestInteractable();

            if (nearest != currentInteractable)
            {
                currentInteractable?.SetHighlighted(false);
                currentInteractable = nearest;
                currentInteractable?.SetHighlighted(true);
            }
        }

        private void OnInteract(InputValue value)
        {
            if (health.IsDead) return;

            currentInteractable?.Interact();
        }

        private IInteractable FindNearestInteractable()
        {
            Physics2D.OverlapCircle(transform.position, interactRadius, filter, nearbyColliders);

            IInteractable nearest = null;
            float nearestDistance = float.MaxValue;

            foreach (Collider2D nearbyCollider in nearbyColliders)
            {
                if (!nearbyCollider.TryGetComponent(out IInteractable interactable)) continue;

                float distance = Vector2.Distance(transform.position, nearbyCollider.transform.position);
                if (distance < nearestDistance)
                {
                    nearest = interactable;
                    nearestDistance = distance;
                }
            }

            return nearest;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, interactRadius);
        }
    }
}
