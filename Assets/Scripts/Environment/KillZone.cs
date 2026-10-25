using UnityEngine;
using Tilemania.Combat;

namespace Tilemania.Environment
{
    public class KillZone : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Health health))
            {
                health.Kill();
            }
            else
            {
                Destroy(other.gameObject);
            }
        }
    }
}
