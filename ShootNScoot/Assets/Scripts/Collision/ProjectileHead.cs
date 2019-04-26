using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Collision
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ProjectileHead : MonoBehaviour
    {
        [SerializeField] private LayerMask blockingLayer;

        private SpriteRenderer mySpriteRenderer;
        private int index;
        
        private ProjectileManager manager;

        public void InitProjectile(float speed, ProjectileManager manager, int index)
        {
            this.manager = manager;
            this.index = index;
            mySpriteRenderer.sprite = manager.GetProjectileSprite(index);
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

        public void HandlePortal(Vector3 exitPosition)
        {
            manager.AddHit(new HitData(transform.position, transform.right, ActionType.Portaled, exitPosition));
            this.transform.position = exitPosition;
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

        public void HandleMelee(Vector3 hitNormal, ProjectileManager prefab)
        {
            // Create a new projectile and destroy this one
            transform.right = Vector3.Reflect(transform.right, hitNormal);

            var newProjectile = Instantiate(prefab,
                transform.position + transform.right * 0.4f,
                transform.rotation);

            newProjectile.InitProjectiles();

            HandleImmediateAbsorption();
        }

        private void Move()
        {
            var start = transform.position;
            var end = start + manager.GetFrameMoveSpeed(transform.right);
            var hit = Physics2D.Linecast(start, end, blockingLayer);

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

        void Awake()
        {
            mySpriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            Move();
        }
    }
}
