using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Control
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ShieldControlHandler : MonoBehaviour
    {
        private SpriteRenderer shield;
        private PlayerController player;

        void Start()
        {
            shield = GetComponent<SpriteRenderer>();
            player = transform.parent.GetComponent<PlayerController>();
        }

        void Update()
        {
            transform.rotation = Quaternion.Euler(0, 0, 45f * (int)player.LastMovedOrientation);

            // Flip to maintain weapon orientation
            var z = transform.rotation.eulerAngles.z;
            shield.flipY = z > 90 && z < 270;
        }
    }
}
