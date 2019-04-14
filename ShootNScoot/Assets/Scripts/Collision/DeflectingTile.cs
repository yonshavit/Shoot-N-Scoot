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
        [SerializeField] private Vector3 deflectingNormal;
        [SerializeField] private AudioSource[] sfx;

        public void SetDeflectingNormal(Vector3 normal)
        {
            deflectingNormal = normal;
        }

        public override bool HandleCollision(Projectile p)
        {
            if (p.IsHead())
            {
                sfx[Random.Range(0, sfx.Length)].Play();
            }

            p.HandleDeflection(deflectingNormal);

            return true;
        }
    }
}
