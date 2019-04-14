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
        public int projectileCount;

        public void InitProjectiles(float orgOrientation)
        {
            var projectiles = GetComponentsInChildren<Projectile>();
            projectileCount = projectiles.Length;

            foreach (var projectile in projectiles)
            {
                projectile.InitProjectile(orgOrientation);
            }
        }

        void Update()
        {
            if (projectileCount == 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
