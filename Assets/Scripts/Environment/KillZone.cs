using UnityEngine;

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
