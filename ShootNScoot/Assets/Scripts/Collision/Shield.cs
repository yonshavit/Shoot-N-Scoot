using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Collision
{
    public class Shield : Collidable
    {
        [SerializeField] private float parryDuration = 0.2f;
        [SerializeField] private float parryCooldown = 0.4f;
        [SerializeField] private ProjectileManager prefab;
        [SerializeField] private AudioClip[] sfx;

        private AudioSource audio;
        private float lastParryStart;

        void Start()
        {
            audio = GetComponent<AudioSource>();
            lastParryStart = Time.time;
        }

        void Update()
        {
            // If weapon enabled and player parries - animate and start parry duration
            if (Time.time - lastParryStart >= parryCooldown && Input.GetKeyDown(KeyCode.Space))
            {
                // TODO add animation
                lastParryStart = Time.time;
            }
        }

        public override bool HandleCollision(ProjectileHead p, Vector3 hitNormal)
        {
            if (sfx.Length > 0)
            {
                audio.clip = sfx[Random.Range(0, sfx.Length)];
                audio.Play();
            }

            if (Time.time - lastParryStart <= parryDuration)
            {
                // Parry! damage player as well
                // TODO create and handle collision with player. Using isTriggered?
                p.HandleParry(hitNormal, prefab);
            }
            else
            {
                // Normal deflect
                p.HandleDeflection(hitNormal);
            }

            return true;
        }
    }
}
