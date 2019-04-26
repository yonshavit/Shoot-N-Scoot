using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Collision
{
    [RequireComponent(typeof(AudioSource))]
    public class AbsorbingTile : Collidable
    {
        [SerializeField] private AudioClip[] sfx;

        private AudioSource audio;

        void Start()
        {
            audio = GetComponent<AudioSource>();
        }

        public override bool HandleCollision(ProjectileHead p, Vector3 hitNormal)
        {
            if (sfx.Length > 0)
            {
                audio.clip = sfx[Random.Range(0, sfx.Length)];
                audio.Play();
            }

            p.HandleAbsorption();

            return false;
        }
    }
}
