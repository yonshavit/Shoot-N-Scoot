using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Collision
{
    [RequireComponent(typeof(AudioSource))]
    public class DeflectingTile : Collidable
    {
        [SerializeField] private AudioClip[] sfx;

        private AudioSource audio;

        void Start()
        {
            audio = GetComponent<AudioSource>();
        }

        public override bool HandleCollision(ProjectileHead p, RaycastHit2D hit)
        {
            if (sfx.Length > 0)
            {
                audio.clip = sfx[Random.Range(0, sfx.Length)];
                audio.Play();
            }

            p.HandleDeflection(hit.normal);

            return true;
        }
    }
}
