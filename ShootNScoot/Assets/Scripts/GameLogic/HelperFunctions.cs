using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GameLogic
{
    public static class HelperFunctions
    {
        public readonly static WaitForEndOfFrame EndOfFrame = new WaitForEndOfFrame();

        public static int RoundForSortingOrder(float number)
        {
            return (int)((number * 10) + 0.5);
        }

        #region Extension Methods
        public static void Shuffle(this AudioClip[] sources)
        {
            // Knuth shuffle algorithm :: courtesy of Wikipedia :)
            for (var t = 0; t < sources.Length; t++)
            {
                var tmp = sources[t];
                int r = UnityEngine.Random.Range(t, sources.Length);
                sources[t] = sources[r];
                sources[r] = tmp;
            }
        }
        #endregion
    }
}
