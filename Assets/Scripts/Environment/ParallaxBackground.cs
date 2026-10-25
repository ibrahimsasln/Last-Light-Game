using UnityEngine;

namespace Tilemania.Environment
{
    public class ParallaxBackground : MonoBehaviour
    {
        [SerializeField] float parallaxSpeed;

        Transform cameraTransform;
        float startXPos;
        float bgLength;

        private void Awake()
        {
            cameraTransform = Camera.main.transform;
            startXPos = transform.position.x;
            bgLength = GetComponent<SpriteRenderer>().bounds.size.x;
        }

        private void LateUpdate()
        {
            float cameraX = cameraTransform.position.x;
            float distance = cameraX * parallaxSpeed;
            float movement = cameraX * (1 - parallaxSpeed);

            transform.position = new Vector2(startXPos + distance, transform.position.y);

            if (movement > startXPos + bgLength)
            {
                startXPos += bgLength;
            }
            else if (movement < startXPos - bgLength)
            {
                startXPos -= bgLength;
            }
        }
    }
}
