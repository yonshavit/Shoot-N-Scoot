using System;
using UnityEngine;

namespace Assets.Scripts.Collision
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private int index;
        [SerializeField] private bool head;
        [SerializeField] private LayerMask BlockingLayer;
        [SerializeField] private float speed;
        
        private SpriteRenderer mySpriteRenderer;
        private Vector3 myOrigin;
        private float distanceToHead;
        private ProjectileManager manager;

        public void InitProjectile(float speed, Vector3 origin, ProjectileManager manager)
        {
            this.speed = speed;
            this.myOrigin = transform.position;
            this.distanceToHead = Vector3.Distance(origin, myOrigin);
            this.manager = manager;
        }

        public bool IsHead()
        {
            return head;
        }

        public void HandleImmediateAbsorption()
        {
            Destroy(manager.gameObject);
        }

        public void HandleAbsorption()
        {
            // TODO Animation
            manager.DecreaseProjectileCounter();
            Destroy(gameObject);
        }

        public void HandleDeflection(Vector3 hitNormal)
        {
            // Save hit normal of head for the rest of the crew
            // Or obtain hit normal
            //if (IsHead())
            //{
            //    manager.headHitNormal = hitNormal;
            //}
            //else
            //{
            //    hitNormal = manager.headHitNormal;
            //}

            index++;

            if (index == 5)
            {
                manager.DecreaseProjectileCounter();
                Destroy(gameObject);
                return;
            }
            
            mySpriteRenderer.sprite = manager.GetProjectileSprite(index);

            transform.right = Vector3.Reflect(transform.right, hitNormal);
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
            var hit = Physics2D.Linecast(start, end, BlockingLayer);

            // Move if did not hit anything or react if did
            if (hit.transform == null)
            {
                transform.position = end;
            }
            else
            {
                // The transform will always be the parent - use the collider to get the actual object that was hit
                var collidable = hit.collider.GetComponent<Collidable>();

                // If collided with a collidable object, let it handle this collision and if returns true then move anyways
                if (collidable != null && collidable.HandleCollision(this, hit.normal))
                {
                    moveDirAbsolute = speed * transform.right * Time.deltaTime;
                    transform.position = start + moveDirAbsolute;
                }
            }
        }

        void Start()
        {
            mySpriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            Move();
            EnableSprite();
        }
    }
}
