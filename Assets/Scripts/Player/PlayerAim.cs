using UnityEngine;
using UnityEngine.InputSystem;
using LastLight.Combat;

namespace LastLight.Player
{
    public class PlayerAim : MonoBehaviour
    {
        [SerializeField] Transform gun;

        Camera mainCamera;
        Health health;

        public Vector2 AimDirection { get; private set; } = Vector2.right;
        public Vector2 GunPosition => gun.position;

        private void Awake()
        {
            mainCamera = Camera.main;
            health = GetComponent<Health>();
        }

        private void Update()
        {
            if (health.IsDead || Time.timeScale == 0f || Mouse.current == null) return;

            Vector2 mouseWorld = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 toMouse = mouseWorld - (Vector2)gun.position;
            if (toMouse.sqrMagnitude < 0.01f) return;

            AimDirection = toMouse.normalized;

            float facing = AimDirection.x >= 0f ? 1f : -1f;
            transform.localScale = new Vector2(facing, 1f);

            float angle = Mathf.Atan2(AimDirection.y, AimDirection.x * facing) * Mathf.Rad2Deg;
            gun.localRotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}
