using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Collision
{
    public abstract class Collidable : MonoBehaviour
    {
        public abstract bool HandleCollision(ProjectileHead p, RaycastHit2D hit);
    }
}
