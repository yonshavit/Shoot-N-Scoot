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

        public void DecreaseProjectileCounter()
        {
            if (projectileCount-- == 0)
                Destroy(gameObject);
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

        public void InitProjectiles()
        {
            var projectiles = GetComponentsInChildren<Projectile>();
            projectileCount = projectiles.Length;
            var headPosition = projectiles[0].transform.position;

            foreach (var projectile in projectiles)
            {
                projectile.InitProjectile(speed, headPosition, this);
            }
        }

        //private void Start()
        //{
        //    InitProjectiles(transform.eulerAngles.z);
        //}
    }
}
