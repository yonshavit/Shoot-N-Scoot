using UnityEngine;

namespace Assets.Scripts.Control
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class GunControlHandler : MonoBehaviour
    {
        private SpriteRenderer gun;

        void Start()
        {
            gun = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            var origin = Camera.main.WorldToScreenPoint(transform.parent.position);
            var cursor = Input.mousePosition;

            var targetDirection = (cursor - origin).normalized;
            var targetAngle = (Mathf.Atan2(targetDirection.y, targetDirection.x) - Mathf.PI) *
                              Mathf.Rad2Deg;

            transform.rotation = Quaternion.AngleAxis(targetAngle, Vector3.forward);

            // Flip to maintain weapon orientation
            var z = transform.rotation.eulerAngles.z;
            gun.flipY = z > 90 && z < 270;
        }
    }
}
