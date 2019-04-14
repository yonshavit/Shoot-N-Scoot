using Assets.Scripts.Collision;
using UnityEngine;

namespace Assets.Scripts.Control
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class GunControlHandler : MonoBehaviour
    {
        [SerializeField] private float shotCooldownSeconds = 0.4f;
        [SerializeField] private ProjectileManager projectileManager;

        private float lastShotTime;
        private SpriteRenderer gun;

        void Start()
        {
            gun = GetComponent<SpriteRenderer>();
            lastShotTime = Time.time;
        }

        void Update()
        {
            #region HandleRotation
            var origin = Camera.main.WorldToScreenPoint(transform.parent.position);
            var cursor = Input.mousePosition;

            var targetDirection = (cursor - origin).normalized;
            var targetAngle = (Mathf.Atan2(targetDirection.y, targetDirection.x) - Mathf.PI) *
                              Mathf.Rad2Deg;

            transform.rotation = Quaternion.AngleAxis(targetAngle, Vector3.forward);

            // Flip to maintain weapon orientation
            var z = transform.rotation.eulerAngles.z;
            gun.flipY = z > 90 && z < 270;
            #endregion

            #region HandleShoot
            if (Input.GetMouseButtonDown(0))
            {
                if (Time.time - lastShotTime > shotCooldownSeconds)
                {
                    // Cooldown has passed, player may shoot again
                    lastShotTime = Time.time;

                    // Create a new projectile and initiate it
                    var newProjectile = Instantiate(projectileManager, transform.position, Quaternion.AngleAxis(targetAngle, Vector3.forward));

                    newProjectile.InitProjectiles();
                }
            }
            #endregion
        }
    }
}
