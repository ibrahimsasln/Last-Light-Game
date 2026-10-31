using UnityEngine;
using LastLight.Combat;
using LastLight.Core;

namespace LastLight.Environment
{
    public class KillZone : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Health health))
            {
                health.Kill();
            }
            else if (other.TryGetComponent(out IDespawnable despawnable))
            {
                despawnable.Despawn();
            }
            else
            {
                Destroy(other.gameObject);
            }
        }
    }
}
