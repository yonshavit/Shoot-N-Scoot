﻿using System.Collections.Generic;
using Assets.Scripts.Collision;
using Assets.Scripts.GameLogic;
using UnityEngine;

namespace Assets.Scripts.Control
{
    //public enum Orientation { Right, UpRight, Up, UpLeft, Left, DownLeft, Down, DownRight }
    public enum Orientation { Right, Up, Left, Down }

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
        [SerializeField] private Vector3 playerMovementBlockOffset;
        [SerializeField] private LayerMask movementBlockMask;
        [SerializeField] private Color iframeColor = new Color(1,1,1, 0.5f);

        public Color playerColor;
        public bool ControllerEnabled { get; set; }
        private float lastiFramesStart;
        private SpriteRenderer playerSprite;
        private SpriteRenderer weaponRenderer;
        private AudioSource audio;
        private PlayerHealthManager health;
        private BoxCollider2D playerBodyCollider;
        private bool isPressing;

        protected virtual void Start()
        {
            isPressing = false;
            playerSprite = GetComponent<SpriteRenderer>();
            weaponRenderer = GetComponentInChildren<WeaponController>().GetComponent<SpriteRenderer>();
            health = GetComponent<PlayerHealthManager>();
            audio = GetComponent<AudioSource>();
            playerBodyCollider = GetComponent<BoxCollider2D>();
            ControllerEnabled = true;

            // Don't blink on game start
            lastiFramesStart = -iframesTimeSeconds;
        }

        protected virtual void Update()
        {
            #region HandleMove

            if (ControllerEnabled)
            {
                // Free pressing flag if the orientation button is released
                if (isPressing)
                {
                    isPressing =
                        !((Input.GetKeyUp(Up) && LastMovedOrientation.Equals(Orientation.Up)) ||
                          (Input.GetKeyUp(Down) && LastMovedOrientation.Equals(Orientation.Down)) ||
                          (Input.GetKeyUp(Left) && LastMovedOrientation.Equals(Orientation.Left)) ||
                          (Input.GetKeyUp(Right) &&
                           LastMovedOrientation.Equals(Orientation.Right)));
                }

                // Set orientation if currently not set (ignore key up! that means they stop pressing that button)
                if (!isPressing)
                {
                    if (Input.GetKey(Up) && !Input.GetKeyUp(Up))
                    {
                        isPressing = true;
                        LastMovedOrientation = Orientation.Up;
                    }
                    else if (Input.GetKey(Down) && !Input.GetKeyUp(Down))
                    {
                        isPressing = true;
                        LastMovedOrientation = Orientation.Down;
                    }
                    else if (Input.GetKey(Left) && !Input.GetKeyUp(Left))
                    {
                        isPressing = true;
                        LastMovedOrientation = Orientation.Left;
                    }
                    else if (Input.GetKey(Right) && !Input.GetKeyUp(Right))
                    {
                        isPressing = true;
                        LastMovedOrientation = Orientation.Right;
                    }
                }

                var move = new Vector3();

                if (Input.GetKey(Up))
                {
                    move.y += moveSpeed * Time.deltaTime;

                    if (Input.GetKey(Left))
                    {
                        //LastMovedOrientation = Orientation.UpLeft;
                        move.x -= moveSpeed * Time.deltaTime;
                        playerSprite.flipX = true;
                    }
                    else if (Input.GetKey(Right))
                    {
                        //LastMovedOrientation = Orientation.UpRight;
                        move.x += moveSpeed * Time.deltaTime;
                        playerSprite.flipX = false;
                    }
                    //else
                    //{
                    //    LastMovedOrientation = Orientation.Up;
                    //}
                }
                else if (Input.GetKey(Down))
                {
                    move.y -= moveSpeed * Time.deltaTime;

                    if (Input.GetKey(Left))
                    {
                        //LastMovedOrientation = Orientation.DownLeft;
                        move.x -= moveSpeed * Time.deltaTime;
                        playerSprite.flipX = true;
                    }
                    else if (Input.GetKey(Right))
                    {
                        //LastMovedOrientation = Orientation.DownRight;
                        move.x += moveSpeed * Time.deltaTime;
                        playerSprite.flipX = false;
                    }
                    //else
                    //{
                    //    LastMovedOrientation = Orientation.Down;
                    //}
                }
                else if (Input.GetKey(Left))
                {
                    //LastMovedOrientation = Orientation.Left;
                    move.x -= moveSpeed * Time.deltaTime;
                    playerSprite.flipX = true;
                }
                else if (Input.GetKey(Right))
                {
                    //LastMovedOrientation = Orientation.Right;
                    move.x += moveSpeed * Time.deltaTime;
                    playerSprite.flipX = false;
                }

                // Move collider according to flip
                if ((playerSprite.flipX && playerBodyCollider.offset.x < 0) ||
                    (!playerSprite.flipX && playerBodyCollider.offset.x > 0))
                {
                    playerBodyCollider.offset *= -1;
                }

                // Move only if allowed
                if (Physics2D.Linecast(transform.position + playerMovementBlockOffset,
                        transform.position + playerMovementBlockOffset + move,
                        movementBlockMask).transform == null)
                {
                    transform.position += move;
                }
                // Try moving in a single direction if trying to move diagonally
                else if (Mathf.Abs(move.x) > Mathf.Epsilon && Mathf.Abs(move.y) > Mathf.Epsilon)
                {
                    // First, attempt moving left/right                    
                    var altMove = Vector3.Scale(move, Vector3.right);

                    if (Physics2D.Linecast(transform.position + playerMovementBlockOffset,
                            transform.position + playerMovementBlockOffset + altMove,
                            movementBlockMask).transform == null)
                    {
                        transform.position += altMove;
                    }
                    else
                    {
                        // First, attempt moving up/down
                        altMove = Vector3.Scale(move, Vector3.up);

                        if (Physics2D.Linecast(transform.position + playerMovementBlockOffset,
                                transform.position + playerMovementBlockOffset + altMove,
                                movementBlockMask).transform == null)
                        {
                            transform.position += altMove;
                        }
                    }
                }
            }
            #endregion
        }

        private IEnumerator<WaitForSeconds> HandleiFrames(float startTime)
        {
            //var weapon = weaponRenderer.GetComponent<WeaponController>();
            var currColor = playerSprite.material.color;

            //weapon.WeaponEnable(false);

            while (Time.time - startTime < iframesTimeSeconds)
            {
                currColor = playerSprite.material.color;

                // Flip between showing and hiding the player during iframes
                playerSprite.material.color = (Color.white == currColor) ? iframeColor : Color.white;
                weaponRenderer.material.color = (Color.white == currColor) ? iframeColor : Color.white;

                yield return new WaitForSeconds(iframesBlinkRateSeconds);
            }

            //weapon.WeaponEnable(true);

            // Make sure the player is shown by the end
            playerSprite.material.color = Color.white;
            weaponRenderer.material.color = Color.white;

            yield return null;
        }

        public bool IsIniFrame()
        {
            return Time.time - lastiFramesStart < iframesTimeSeconds;
        }

        public override bool HandleCollision(ProjectileHead p, RaycastHit2D hit)
        {
            if (!IsIniFrame())
            {
                // Handle being hit by a projectile
                p.HandleImmediateAbsorption();
                GetHit();
                return false;
            }

            // Consider not being hit - let the projectile pass through
            return true;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            var portal = collider.gameObject.GetComponent<PortalCollidable>();

            if (portal != null)
            {
                // If you want different variations of portals (not both on the y axis) you'll need to change this!
                Vector3 newPos = new Vector3(portal.secondPortal.transform.position.x + portal.secondPortal.portalOffset.x, transform.position.y, 0);
                transform.position = newPos;
            }
        }

        public void GetHit()
        {
            if (!IsIniFrame())
            {
                if (sfx.Length > 0)
                {
                    audio.clip = sfx[Random.Range(0, sfx.Length)];
                    audio.Play();
                }

                health.Score--;
                lastiFramesStart = Time.time;
                StartCoroutine(HandleiFrames(lastiFramesStart));
            }
        }
    }
}
