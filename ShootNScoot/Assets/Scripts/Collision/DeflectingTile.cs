using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Collision
{
    public class DeflectingTile : Collidable
    {
        //[SerializeField] private Vector3 deflectingNormal;
        [SerializeField] private AudioClip[] sfx;

        private AudioSource audio;

        void Start()
        {
            audio = GetComponent<AudioSource>();
        }

        //public void SetDeflectingNormal(Vector3 normal)
        //{
        //    deflectingNormal = normal;
        //}

        public override bool HandleCollision(Projectile p, Vector3 hitNormal)
        {
            if (p.IsHead())
            {
                if (sfx.Length > 0)
                {
                    audio.clip = sfx[Random.Range(0, sfx.Length)];
                    audio.Play();
                }
            }

            p.HandleDeflection(hitNormal);

            return true;
        }
    }
}
