using Assets.Scripts.Collision;
using UnityEngine;

namespace Assets.Scripts.Control
{
    public enum Orientation { Left, DownLeft, Down, DownRight, Right, UpRight, Up, UpLeft }

    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerController : Collidable
    {
        public Orientation LastMovedOrientation { get; private set; }
        [SerializeField] private KeyCode Up;
        [SerializeField] private KeyCode Down;
        [SerializeField] private KeyCode Left;
        [SerializeField] private KeyCode Right;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private AudioSource[] sfx;

        private SpriteRenderer player;
        
        void Start()
        {
            player = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            var move = new Vector3();

            if (Input.GetKey(Up))
            {
                move.y += moveSpeed * Time.deltaTime;

                if (Input.GetKey(Left))
                {
                    LastMovedOrientation = Orientation.UpLeft;
                    move.x -= moveSpeed * Time.deltaTime;
                    player.flipX = true;
                }

                else if (Input.GetKey(Right))
                {
                    LastMovedOrientation = Orientation.UpRight;
                    move.x += moveSpeed * Time.deltaTime;
                    player.flipX = false;
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
                    player.flipX = true;
                }

                else if (Input.GetKey(Right))
                {
                    LastMovedOrientation = Orientation.DownRight;
                    move.x += moveSpeed * Time.deltaTime;
                    player.flipX = false;
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
                player.flipX = true;
            }
            else if (Input.GetKey(Right))
            {
                LastMovedOrientation = Orientation.Right;
                move.x += moveSpeed * Time.deltaTime;
                player.flipX = false;
            }

            transform.position += move;
        }

        public override bool HandleCollision(Projectile p)
        {
            if (p.IsHead())
            {
                sfx[Random.Range(0, sfx.Length)].Play();
            }

            p.HandleImmediateAbsorption();

            // TODO complete

            return false;
        }
    }
}
