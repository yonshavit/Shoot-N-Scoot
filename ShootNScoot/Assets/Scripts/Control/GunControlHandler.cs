using System.Collections.Generic;
using Assets.Scripts.Collision;
using UnityEngine;

namespace Assets.Scripts.Control
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(AudioSource))]
    public class GunControlHandler : WeaponController
    {
        [SerializeField] private float shotCooldownSeconds = 0.4f;
        [SerializeField] private float shotFireOffset = 1.5f;
        [SerializeField] private ProjectileManager projectileManager;
        [SerializeField] private AudioClip[] sfx;

        private AudioSource audio;
        private float lastShotTime;
        private SpriteRenderer gun;
        private PlayerController player;
        private bool weaponEnabled = true;

        void Start()
        {
            gun = GetComponent<SpriteRenderer>();
            player = GetComponentInParent<PlayerController>();
            audio = GetComponent<AudioSource>();
            lastShotTime = Time.time;
        }

        void Update()
        {
            if (player.ControllerEnabled)
            {
                #region HandleRotation

                var origin = Camera.main.WorldToScreenPoint(transform.parent.position);
                var cursor = Input.mousePosition;

                var targetDirection = (cursor - origin).normalized;
                var targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

                transform.rotation = Quaternion.AngleAxis(targetAngle, Vector3.forward);

                // Flip to maintain weapon orientation
                var z = transform.rotation.eulerAngles.z;
                gun.flipY = z > 90 && z < 270;

                #endregion

                #region HandleShoot

                if (Input.GetMouseButtonDown(0) && weaponEnabled)
                {
                    if (Time.time - lastShotTime > shotCooldownSeconds /*&& !player.IsIniFrame()*/)
                    {
                        // Cooldown has passed, player may shoot again
                        lastShotTime = Time.time;

                        // Create a new projectile and initiate it
                        var newProjectile = Instantiate(projectileManager,
                            transform.position + transform.right * shotFireOffset,
                            Quaternion.AngleAxis(targetAngle, Vector3.forward));

                        newProjectile.InitProjectiles();

                        // Play shoot sound
                        if (sfx.Length > 0)
                        {
                            audio.clip = sfx[Random.Range(0, sfx.Length)];
                            audio.Play();
                        }
                    }
                }

                #endregion
            }
        }

        public override void WeaponEnable(bool enabled)
        {
            weaponEnabled = enabled;
        }
    }
}
