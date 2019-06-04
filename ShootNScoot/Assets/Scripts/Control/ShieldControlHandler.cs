﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Collision;
using UnityEngine;

namespace Assets.Scripts.Control
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Shield))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class ShieldControlHandler : WeaponController
    {
        private float rotationAngle;
        private Shield shield;
        private SpriteRenderer shieldRenderer;
        private PlayerController player;
        private BoxCollider2D shieldCollider2D;

        void Start()
        {
            rotationAngle = 360f / Enum.GetNames(typeof(Orientation)).Length; 
            shield = GetComponent<Shield>();
            shieldRenderer = GetComponent<SpriteRenderer>();
            shieldCollider2D = GetComponent<BoxCollider2D>();
            player = transform.parent.GetComponent<PlayerController>();
        }

        void Update()
        {
            transform.rotation = Quaternion.Euler(0, 0, rotationAngle * (int)player.LastMovedOrientation);

            // Flip to maintain weapon orientation
            var z = transform.rotation.eulerAngles.z;
            shieldRenderer.flipY = z > 90 && z < 270;
        }

        public override void WeaponEnable(bool enabled)
        {
            shieldCollider2D.enabled = enabled;
            shield.enabled = enabled;
        }
    }
}
