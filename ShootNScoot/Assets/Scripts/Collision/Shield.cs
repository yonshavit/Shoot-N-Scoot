using System.Collections.Generic;
using Assets.Scripts.Control;
using Assets.Scripts.GameLogic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Collision
{
    public class Shield : Collidable
    {
        [SerializeField] private float meleeDuration = 0.2f;
        [SerializeField] private float meleeCooldown = 0.4f;
        [SerializeField] private float meleeMoveOffsetDest = 0.5f;
        [SerializeField] private ProjectileManager prefab;
        [SerializeField] private AudioClip[] sfx;

        private AudioSource audio;
        private float lastMeleeStart;

        void Start()
        {
            audio = GetComponent<AudioSource>();
            lastMeleeStart = Time.time;
        }

        void Update()
        {
            // If weapon enabled (this script's enabled flag is true - then this update will be called)
            // and player melees - animate and start parry duration
            if (Time.time - lastMeleeStart >= meleeCooldown && Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(MeleeAnimation());
                lastMeleeStart = Time.time;
            }
        }

        private IEnumerator<WaitForEndOfFrame> MeleeAnimation()
        {
            var startMelee = Time.time;
            var halfDuration = meleeDuration / 2;
            var dest = transform.right * meleeMoveOffsetDest;
            float diff;

            // Move shield back and forth
            while ((diff = Time.time - startMelee) < meleeDuration)
            {
                if (diff < halfDuration)
                {
                    // Push away
                    transform.localPosition =
                        Vector3.Lerp(Vector3.zero, dest, diff / halfDuration);
                }
                else
                {
                    // Retract shield
                    transform.localPosition =
                        Vector3.Lerp(dest, Vector3.zero, diff / meleeDuration);
                }

                yield return HelperFunctions.EndOfFrame;
            }

            // Return shield to original position
            transform.localPosition = Vector3.zero;

            yield return null;
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            PlayerController opponent;
            
            // If hit the opponent during melee
            if (Time.time - lastMeleeStart < meleeDuration &&
                (opponent = collider.gameObject.GetComponent<PlayerController>()) != null)
            {
                opponent.GetHit();
            }
        }

        public override bool HandleCollision(ProjectileHead p, Vector3 hitNormal)
        {
            if (sfx.Length > 0)
            {
                audio.clip = sfx[Random.Range(0, sfx.Length)];
                audio.Play();
            }

            if (Time.time - lastMeleeStart <= meleeDuration)
            {
                // Parry! damage player as well
                // TODO create and handle collider with player. Using isTriggered?
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
