using System;
using UnityEngine;

namespace Assets.Scripts.Collision
{
    [RequireComponent(typeof(SpriteRenderer))]
    //[RequireComponent(typeof(BoxCollider2D))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private int index;
        [SerializeField] private bool head;
        [SerializeField] private LayerMask BlockingLayer;
        //private BoxCollider2D myCollider;
        private SpriteRenderer mySpriteRenderer;
        private float speed;
        private Vector3 myOrigin;
        private float distanceToHead;
        private ProjectileManager manager;

        public void InitProjectile(float orgOrientation, float speed, Vector3 origin, ProjectileManager manager)
        {
            transform.eulerAngles = new Vector3(0, 0, orgOrientation);
            this.speed = speed;
            this.myOrigin = transform.position;
            this.distanceToHead = Vector3.Distance(origin, myOrigin);
            this.manager = manager;
        }

        public bool IsHead()
        {
            return head;
        }

        public void HandleAbsorption()
        {
            // TODO Animation
            manager.DecreaseProjectileCounter();
            Destroy(gameObject);
        }

        public void HandleDeflection(Vector3 wallNormal)
        {
            index++;
            if (index == 5)
            {
                manager.DecreaseProjectileCounter();
                Destroy(gameObject);
                return;
            }

            mySpriteRenderer.sprite = manager.GetProjectileSprite(index);

            transform.right = Vector3.Reflect(transform.right, wallNormal);

        }

        private void EnableSprite()
        {
            if (!mySpriteRenderer.enabled && Vector3.Distance(transform.position, myOrigin) > distanceToHead)
            {
                mySpriteRenderer.enabled = true;
            }
        }

        private void Move()
        {
            var moveDirAbsolute = speed * transform.right * Time.deltaTime;
            var start = transform.position;
            var end = start + moveDirAbsolute;
            RaycastHit2D hit;

            //myCollider.enabled = false;
            hit = Physics2D.Linecast(start, end, BlockingLayer);
            //myCollider.enabled = true;

            // Move if did not hit anything or react if did
            if (hit.transform == null)
            {
                transform.position = end;
            }
            else
            {
                var collidable = hit.transform.GetComponent<ICollidable>();

                // If collided with a collidable object, let it handle this collision and if returns true then move anyways
                if (collidable.HandleCollision(this))
                {
                    transform.position = end;
                }
            }

        }

        void Start()
        {
            mySpriteRenderer = GetComponent<SpriteRenderer>();
            //myCollider = GetComponent<BoxCollider2D>();
        }

        void Update()
        {
            Move();
            EnableSprite();
        }
    }
}
