﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Collision;
using Assets.Scripts.Control;
using UnityEngine;

namespace Assets.Scripts.GameLogic.AI
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Turret : MonoBehaviour
    {
        [SerializeField] private PlayerController shooter;
        [SerializeField] private ProjectileManager projectileManager;
        [SerializeField] private float shotCooldownSeconds = 0.5f;
        [SerializeField] private float shotFireOffset = 1.5f;

        private Animator anim;
        private float lastShotTime;
        private bool aiEnabled;

        public void SetControl(bool enable)
        {
            aiEnabled = enable;
        }

        void Start()
        {
            anim = GetComponent<Animator>();
            lastShotTime = Time.time;
            aiEnabled = true;
        }

        void Update()
        {
            if (aiEnabled)
            {
                var targetDirection = (shooter.transform.position - transform.position).normalized;
                var targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

                // Rotate towards the shooter player
                transform.rotation = Quaternion.AngleAxis(targetAngle, Vector3.forward);

                if (Time.time - lastShotTime > shotCooldownSeconds)
                {
                    // Cooldown has passed, turret may shoot again
                    lastShotTime = Time.time;

                    // Create a new projectile and initiate it
                    var newProjectile = Instantiate(projectileManager,
                        transform.position + transform.right * shotFireOffset,
                        Quaternion.AngleAxis(targetAngle, Vector3.forward));

                    newProjectile.InitProjectiles();

                    // Call shoot animation which will return to idle animation after a single animation cycle
                    anim.Play("Shoot");
                }
            }
        }
    }
}