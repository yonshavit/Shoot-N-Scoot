using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Collision;
using UnityEngine;

namespace Assets.Scripts.Control
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(DeflectingTile))]
    public class ShieldControlHandler : MonoBehaviour
    {
        private SpriteRenderer shield;
        private PlayerController player;
        private DeflectingTile deflecting;

        void Start()
        {
            shield = GetComponent<SpriteRenderer>();
            deflecting = GetComponent<DeflectingTile>();
            player = transform.parent.GetComponent<PlayerController>();
        }

        void Update()
        {
            transform.rotation = Quaternion.Euler(0, 0, 45f * (int)player.LastMovedOrientation);

            // Flip to maintain weapon orientation
            var z = transform.rotation.eulerAngles.z;
            shield.flipY = z > 90 && z < 270;

            // The opposite of the right direction will be the normal of the shield
            deflecting.SetDeflectingNormal(-transform.right);
        }
    }
}
