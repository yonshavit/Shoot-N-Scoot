using System.Collections.Generic;
using Assets.Scripts.Collision;
using Assets.Scripts.GameLogic;
using UnityEngine;

namespace Assets.Scripts.Control
{
    public enum Orientation { Right, UpRight, Up, UpLeft, Left, DownLeft, Down, DownRight }

    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(PlayerHealthManager))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerController : Collidable
    {
        public Orientation LastMovedOrientation { get; private set; }
        [SerializeField] private KeyCode Up;
        [SerializeField] private KeyCode Down;
        [SerializeField] private KeyCode Left;
        [SerializeField] private KeyCode Right;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private AudioClip[] sfx;
        [SerializeField] private float iframesTimeSeconds = 3f;
        [SerializeField] private float iframesBlinkRateSeconds = 0.3f;
        [SerializeField] private BoxCollider2D playerMoveCollider;
        [SerializeField] private LayerMask movementBlockMask;
        [SerializeField] private Color iframeColor = new Color(1,1,1, 0.5f);

        private float lastiFramesStart;
        private SpriteRenderer playerSprite;
        private SpriteRenderer weaponRenderer;
        private AudioSource audio;
        private PlayerHealthManager health;
        private Canvas playerCanvas;

        void Start()
        {
            playerSprite = GetComponent<SpriteRenderer>();
            weaponRenderer = GetComponentInChildren<WeaponController>().GetComponent<SpriteRenderer>();
            health = GetComponent<PlayerHealthManager>();
            playerCanvas = GetComponentInChildren<Canvas>();
            audio = GetComponent<AudioSource>();

            // Don't blink on game start
            lastiFramesStart = -iframesTimeSeconds;
        }

        void Update()
        {
            #region HandleMove
            var move = new Vector3();

            if (Input.GetKey(Up))
            {
                move.y += moveSpeed * Time.deltaTime;

                if (Input.GetKey(Left))
                {
                    LastMovedOrientation = Orientation.UpLeft;
                    move.x -= moveSpeed * Time.deltaTime;
                    playerSprite.flipX = true;
                }

                else if (Input.GetKey(Right))
                {
                    LastMovedOrientation = Orientation.UpRight;
                    move.x += moveSpeed * Time.deltaTime;
                    playerSprite.flipX = false;
                }
                else
                {
                    LastMovedOrientation = Orientation.Up;
                }
            }
            else if (Input.GetKey(Down))
            {
                move.y -= moveSpeed * Time.deltaTime;

                if (Input.GetKey(Left))
                {
                    LastMovedOrientation = Orientation.DownLeft;
                    move.x -= moveSpeed * Time.deltaTime;
                    playerSprite.flipX = true;
                }

                else if (Input.GetKey(Right))
                {
                    LastMovedOrientation = Orientation.DownRight;
                    move.x += moveSpeed * Time.deltaTime;
                    playerSprite.flipX = false;
                }
                else
                {
                    LastMovedOrientation = Orientation.Down;
                }
            }
            else if (Input.GetKey(Left))
            {
                LastMovedOrientation = Orientation.Left;
                move.x -= moveSpeed * Time.deltaTime;
                playerSprite.flipX = true;
            }
            else if (Input.GetKey(Right))
            {
                LastMovedOrientation = Orientation.Right;
                move.x += moveSpeed * Time.deltaTime;
                playerSprite.flipX = false;
            }

            playerMoveCollider.enabled = false;
            var hit = Physics2D.Linecast(transform.position, transform.position + move, movementBlockMask);
            playerMoveCollider.enabled = true;

            // Move only if allowed
            if (hit.transform == null)
            {
                transform.position += move;
            }
            #endregion
        }

        private IEnumerator<WaitForSeconds> HandleiFrames(float startTime)
        {
            var weapon = weaponRenderer.GetComponent<WeaponController>();

            weapon.WeaponEnable(false);

            while (Time.time - startTime < iframesTimeSeconds)
            {
                var currColor = playerSprite.material.color;

                // Flip between showing and hiding the player during iframes
                playerSprite.material.color = (Color.white == currColor) ? iframeColor : Color.white;
                weaponRenderer.material.color = (Color.white == currColor) ? iframeColor : Color.white;

                yield return new WaitForSeconds(iframesBlinkRateSeconds);
            }

            weapon.WeaponEnable(true);

            // Make sure the player is shown by the end
            if (!playerSprite.enabled)
            {
                playerSprite.enabled = true;
                weaponRenderer.enabled = true;
            }

            yield return null;
        }

        public bool IsIniFrame()
        {
            return Time.time - lastiFramesStart < iframesTimeSeconds;
        }

        public override bool HandleCollision(ProjectileHead p, Vector3 hitNormal)
        {
            if (!IsIniFrame())
            {
                if (sfx.Length > 0)
                {
                    audio.clip = sfx[Random.Range(0, sfx.Length)];
                    audio.Play();
                }

                // Handle being hit by a projectile
                p.HandleImmediateAbsorption();
                lastiFramesStart = Time.time;
                StartCoroutine(HandleiFrames(lastiFramesStart));
                return false;
            }

            // Consider not being hit - let the projectile pass through
            return true;
        }
    }
}
