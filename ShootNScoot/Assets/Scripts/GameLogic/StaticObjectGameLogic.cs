using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GameLogic
{
    [RequireComponent(typeof(SpriteRenderer))]
    class StaticObjectGameLogic : MonoBehaviour
    {
        private SpriteRenderer mySpriteRenderer;

        void Start()
        {
            mySpriteRenderer = GetComponent<SpriteRenderer>();
            mySpriteRenderer.sortingOrder = -HelperFunctions.RoundForSortingOrder(transform.position.y);
        }
    }
}
