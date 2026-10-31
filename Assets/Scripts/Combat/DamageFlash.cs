using System.Collections;
using UnityEngine;

namespace LastLight.Combat
{
    public class DamageFlash : MonoBehaviour
    {
        [SerializeField] Color flashColor = new Color(1f, 0.35f, 0.35f);
        [SerializeField] float flashDuration = 0.1f;

        Health health;
        SpriteRenderer spriteRenderer;
        Coroutine flashRoutine;
        Color baseColor;

        private void Awake()
        {
            health = GetComponent<Health>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            health.Damaged += OnDamaged;
        }

        private void OnDisable()
        {
            health.Damaged -= OnDamaged;

            if (flashRoutine != null)
            {
                spriteRenderer.color = baseColor;
                flashRoutine = null;
            }
        }

        private void OnDamaged(Vector2 hitFrom)
        {
            if (flashRoutine != null)
            {
                StopCoroutine(flashRoutine);
                spriteRenderer.color = baseColor;
            }
            flashRoutine = StartCoroutine(Flash());
        }

        private IEnumerator Flash()
        {
            baseColor = spriteRenderer.color;
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = baseColor;
            flashRoutine = null;
        }
    }
}
