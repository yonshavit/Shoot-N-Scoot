using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Collision
{
    public class ProjectileManager : MonoBehaviour
    {
        public float speed;
        [SerializeField] private int projectileCount;
        [SerializeField] private Sprite proj1;
        [SerializeField] private Sprite proj2;
        [SerializeField] private Sprite proj3;
        [SerializeField] private Sprite proj4;

        private List<HitData> hits;

        public void DecreaseProjectileCounter()
        {
            projectileCount--;

            if (projectileCount == 0)
            {
                Destroy(gameObject);
            }
        }

        public Sprite GetProjectileSprite(int index)
        {
            switch (index)
            {
                case 1: return proj1;
                case 2: return proj2;
                case 3: return proj3;
                case 4: return proj4;
                default: return proj1;
            }
        }

        public Vector3 GetFrameMoveSpeed(Vector3 dir)
        {
            return speed * dir * Time.deltaTime;
        }

        public void AddHit(HitData data)
        {
            hits.Add(data);
        }

        public HitData? GetHit(int index)
        {
            if (hits.Count > index)
                return hits[index];

            return null;
        }

        public void InitProjectiles()
        {
            hits = new List<HitData>();
            var head = GetComponentInChildren<ProjectileHead>();
            var projectiles = GetComponentsInChildren<ProjectileTail>();
            projectileCount = projectiles.Length + 1; // +1 for head

            head.InitProjectile(speed, this);

            foreach (var projectile in projectiles)
            {
                projectile.InitProjectile(speed, head.transform.position, this);
            }
        }
    }
}
