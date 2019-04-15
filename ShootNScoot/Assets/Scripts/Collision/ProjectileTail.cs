using System;
using UnityEngine;

namespace Assets.Scripts.Collision
{
    public enum ActionType { Deflected, Absorbed }

    public struct HitData
    {
        public Vector3 hitOrigin;
        public Vector3 rightDir;
        public ActionType type;

        public HitData(Vector3 hitOrigin, Vector3 rightDir, ActionType type)
        {
            this.hitOrigin = hitOrigin;
            this.rightDir = rightDir;
            this.type = type;
        }
    }

    public class ProjectileTail : MonoBehaviour
    {
        [SerializeField] private int spriteIndex;
        private int hitIndex;
        private float initDistanceToHead;
        private Vector3 initOrigin;
        private ProjectileManager manager;
        private SpriteRenderer mySpriteRenderer;

        public void InitProjectile(float speed, Vector3 headOrgPos, ProjectileManager manager)
        {
            this.manager = manager;
            initOrigin = transform.position;
            initDistanceToHead = Vector3.Distance(headOrgPos, initOrigin);
            hitIndex = 0;
        }

        private void EnableSprite()
        {
            if (!mySpriteRenderer.enabled && Vector3.Distance(transform.position, initOrigin) > initDistanceToHead)
            {
                mySpriteRenderer.enabled = true;
            }
        }

        private void HandleAbsorption()
        {
            manager.DecreaseProjectileCounter();
            Destroy(gameObject);
        }

        private void HandleDeflection(HitData newHit)
        {
            spriteIndex++;
            hitIndex++;

            if (spriteIndex == 5)
            {
                manager.DecreaseProjectileCounter();
                Destroy(gameObject);
                return;
            }

            mySpriteRenderer.sprite = manager.GetProjectileSprite(spriteIndex);

            // Follow the leader! Move to the next hit
            transform.right = newHit.rightDir;
        }

        private void Move()
        {
            // Try checking for another hit
            var newHit = manager.GetHit(hitIndex);

            // Null meaning the next hit hasn't happened yet! Just keep sailing in your general direction
            if (!newHit.HasValue || Vector3.Distance(newHit.Value.hitOrigin, transform.position) >=
                manager.GetFrameMoveSpeed(transform.right).magnitude)
            {
                // Move in current direction if no new hit yet or if not yet close enough to hit location
                transform.position += manager.GetFrameMoveSpeed(transform.right);
            }
            else
            {
                // Fix on exact position as head
                transform.position = newHit.Value.hitOrigin;

                switch (newHit.Value.type)
                {
                    case ActionType.Deflected:
                        HandleDeflection(newHit.Value);
                        break;
                    case ActionType.Absorbed:
                        HandleAbsorption();
                        break;
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
