using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Collision
{
    public interface ICollidable
    {
        bool HandleCollision(Projectile p);
    }
}
