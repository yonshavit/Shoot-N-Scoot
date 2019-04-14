using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Collision
{
    public class AbsorbingTile : Collidable
    {
        [SerializeField] private AudioSource[] sfx;

        public override bool HandleCollision(Projectile p)
        {
            if (p.IsHead())
            {
                sfx[Random.Range(0, sfx.Length)].Play();
            }

            p.HandleAbsorption();

            return false;
        }
    }
}
