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
        [SerializeField] public PortalCollidable secondPortal;
        private BoxCollider2D myBoxCollider;
        private Vector3 portalBottom;

        public Vector3 portalOffset;

        void Awake()
        {
            myBoxCollider = GetComponent<BoxCollider2D>();
        }

        public override bool HandleCollision(ProjectileHead p, Vector3 hitNormal)
        {
            //if you want different variations of portals (not both on the y axis) you'll need to change this!
            Vector3 newPos = new Vector3(secondPortal.transform.position.x+secondPortal.portalOffset.x, p.transform.position.y, 0);
            p.HandlePortal(newPos);
            return false;
        }
    }
}
