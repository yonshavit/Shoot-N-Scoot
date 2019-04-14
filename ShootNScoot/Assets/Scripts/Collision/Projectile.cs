using System;
using UnityEngine;

namespace Assets.Scripts.Collision
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private int index;

        public void InitProjectile(float orgOrientation)
        {
            transform.eulerAngles = new Vector3(0, 0, orgOrientation);
        }

        public void HandleAbsorption()
        {
            throw new NotImplementedException();
        }

        public void HandleDeflection()
        {
            throw new NotImplementedException();
        }

        void Start()
        {
            // TODO complete
        }

        void Update()
        {
            // TODO complete
        }
    }
}
