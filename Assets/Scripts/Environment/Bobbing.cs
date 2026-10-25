using UnityEngine;

namespace Tilemania.Environment
{
    public class Bobbing : MonoBehaviour
    {
        [SerializeField] float amplitude = 0.08f;
        [SerializeField] float frequency = 3f;

        Vector3 startPosition;

        private void Awake()
        {
            startPosition = transform.localPosition;
        }

        private void Update()
        {
            transform.localPosition = startPosition + Vector3.up * (Mathf.Sin(Time.unscaledTime * frequency) * amplitude);
        }
    }
}
