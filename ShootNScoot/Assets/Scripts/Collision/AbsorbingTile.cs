using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Collision
{
    public class AbsorbingTile : ICollidable
    {
        [SerializeField] private AudioSource[] sfx;

        public void HandleCollision(Projectile p)
        {
            sfx[Random.Range(0, sfx.Length)].Play();
            p.HandleAbsorption();
        }
    }
}
