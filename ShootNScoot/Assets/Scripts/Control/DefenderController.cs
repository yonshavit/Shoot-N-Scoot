using System;
using Assets.Scripts.Collision;
using UnityEngine;

namespace Assets.Scripts.Control
{
    public class DefenderController : PlayerController
    {
        [SerializeField] private EdgeCollider2D TopCollider;
        [SerializeField] private EdgeCollider2D BottomCollider;
        [SerializeField] private EdgeCollider2D LeftCollider;
        [SerializeField] private EdgeCollider2D RightCollider;
        [SerializeField] private BoxCollider2D shieldCollider2D;
        private Vector2[] pointsTop;
        private Vector2[] pointsBottom;
        private Vector2[] pointsLeft;
        private Vector2[] pointsRight;

        public override bool HandleCollision(ProjectileHead p, RaycastHit2D hit)
        {
            // Ignore hit if generated inside defender - determine that by the hit normal and the collider itself
            if (hit.collider == TopCollider &&
                (hit.normal - Vector2.down).sqrMagnitude < Mathf.Epsilon ||
                hit.collider == BottomCollider &&
                (hit.normal - Vector2.up).sqrMagnitude < Mathf.Epsilon ||
                hit.collider == LeftCollider &&
                (hit.normal - Vector2.right).sqrMagnitude < Mathf.Epsilon ||
                hit.collider == RightCollider &&
                (hit.normal - Vector2.left).sqrMagnitude < Mathf.Epsilon)
            {
                return true;
            }

            return base.HandleCollision(p, hit);
        }

        protected override void Start()
        {
            base.Start();

            pointsTop = TopCollider.points.Clone() as Vector2[];
            pointsBottom = BottomCollider.points.Clone() as Vector2[];
            pointsLeft = LeftCollider.points.Clone() as Vector2[];
            pointsRight = RightCollider.points.Clone() as Vector2[];
        }

        protected override void Update()
        {
            base.Update();

            var xOffset = shieldCollider2D.offset.x - (shieldCollider2D.size.x / 2);

            switch (LastMovedOrientation)
            {
                case Orientation.Right:
                    LeftCollider.points = pointsLeft.Clone() as Vector2[];
                    RightCollider.points = new[]
                    {
                        pointsRight[0] + new Vector2(xOffset - RightCollider.offset.x, 0),
                        pointsRight[1] + new Vector2(xOffset - RightCollider.offset.x, 0)
                    };
                    TopCollider.points = new[]
                    {
                        pointsTop[0],
                        new Vector2(xOffset, 0)
                    };
                    BottomCollider.points = new[]
                    {
                        pointsBottom[0],
                        new Vector2(xOffset, 0)
                    };

                    break;
                case Orientation.Up:
                    BottomCollider.points = pointsBottom.Clone() as Vector2[];
                    LeftCollider.points = new[]
                    {
                        pointsLeft[0],
                        new Vector2(0, xOffset)
                    };
                    RightCollider.points = new[]
                    {
                        pointsRight[0],
                        new Vector2(0, xOffset)
                    };
                    TopCollider.points = new[]
                    {
                        pointsTop[0] + new Vector2(0, xOffset - TopCollider.offset.y),
                        pointsTop[1] + new Vector2(0, xOffset - TopCollider.offset.y)
                    };

                    break;
                case Orientation.Left:
                    xOffset = -xOffset;

                    RightCollider.points = pointsRight.Clone() as Vector2[];
                    LeftCollider.points = new[]
                    {
                        pointsLeft[0] + new Vector2(xOffset - LeftCollider.offset.x, 0),
                        pointsLeft[1] + new Vector2(xOffset - LeftCollider.offset.x, 0)
                    };
                    TopCollider.points = new[]
                    {
                        new Vector2(xOffset, 0),
                        pointsTop[1]
                    };
                    BottomCollider.points = new[]
                    {
                        new Vector2(xOffset, 0),
                        pointsBottom[1]
                    };

                    break;
                case Orientation.Down:
                    xOffset = -xOffset;

                    TopCollider.points = pointsTop.Clone() as Vector2[];
                    LeftCollider.points = new[]
                    {
                        new Vector2(0, xOffset),
                        pointsLeft[1]
                    };
                    RightCollider.points = new[]
                    {
                        new Vector2(0, xOffset),
                        pointsRight[1]
                    };
                    BottomCollider.points = new[]
                    {
                        pointsBottom[0] + new Vector2(0, xOffset - BottomCollider.offset.y),
                        pointsBottom[1] + new Vector2(0, xOffset - BottomCollider.offset.y)
                    };

                    break;
            }
        }
    }
}
