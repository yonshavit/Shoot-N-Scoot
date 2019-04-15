using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Collision
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ProjectileHead : MonoBehaviour
    {
        [SerializeField] private int index;
        [SerializeField] private LayerMask BlockingLayer;
        
        private SpriteRenderer mySpriteRenderer;
        
        private ProjectileManager manager;

        public void InitProjectile(float speed, ProjectileManager manager)
        {
            this.manager = manager;
        }
        
        public void HandleImmediateAbsorption()
        {
            Destroy(manager.gameObject);
        }

        public void HandleAbsorption()
        {
            // TODO Animation
            manager.AddHit(new HitData(transform.position, transform.right, ActionType.Absorbed));
            manager.DecreaseProjectileCounter();
            Destroy(gameObject);
        }

        public void HandleDeflection(Vector3 hitNormal)
        {
            index++;

            if (index == 5)
            {
                manager.DecreaseProjectileCounter();
                Destroy(gameObject);
                return;
            }
            
            mySpriteRenderer.sprite = manager.GetProjectileSprite(index);
            transform.right = Vector3.Reflect(transform.right, hitNormal);

            manager.AddHit(new HitData(transform.position, transform.right, ActionType.Deflected));
        }

        private void Move()
        {
            var start = transform.position;
            var end = start + manager.GetFrameMoveSpeed(transform.right);
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
                    transform.position = start + manager.GetFrameMoveSpeed(transform.right);
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
        }
    }
}
