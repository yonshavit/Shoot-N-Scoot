using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Control
{
    public enum Orientation { Left, DownLeft, Down, DownRight, Right, UpRight, Up, UpLeft }

    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private KeyCode Up;
        [SerializeField] private KeyCode Down;
        [SerializeField] private KeyCode Left;
        [SerializeField] private KeyCode Right;
        public Orientation LastMovedOrientation { get; private set; }
        public float moveSpeed = 5f;

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
    }
}
