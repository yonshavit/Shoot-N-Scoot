using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GameLogic
{
    static class HelperFunctions
    {
        static public int RoundForSortingOrder(float number)
        {
            return(int)((number * 10) + 0.5);
        }

        public static WaitForEndOfFrame EndOfFrame = new WaitForEndOfFrame();
    }
}
