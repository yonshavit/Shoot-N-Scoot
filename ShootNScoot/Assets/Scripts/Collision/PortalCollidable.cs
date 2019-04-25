using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Collision
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class PortalCollidable : Collidable
    {
        [SerializeField] private PortalCollidable secondPortal;
        private BoxCollider2D myBoxCollider;

        void Awake()
        {
            myBoxCollider = GetComponent<BoxCollider2D>();
        }

        public override bool HandleCollision(ProjectileHead p, Vector3 hitNormal)
        {
            var diff = transform.position - p.transform.position;
            Vector3 newPos = secondPortal.transform.position + diff;
            p.HandlePortal(newPos);
            return false;
        }
    }
}
