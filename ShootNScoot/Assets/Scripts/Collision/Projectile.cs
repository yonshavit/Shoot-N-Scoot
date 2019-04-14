using System;
using UnityEngine;

namespace Assets.Scripts.Collision
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private int index;
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

        private void Move()
        {
            transform.position += speed * transform.right * Time.deltaTime;
            
            if(!mySpriteRenderer.enabled && Vector3.Distance(transform.position, myOrigin) > distanceToHead)
            {
                mySpriteRenderer.enabled = true;
            }
            
        }

        void Start()
        {
            mySpriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            this.Move();
        }
    }
}
