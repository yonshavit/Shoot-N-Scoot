using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GameLogic
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class BackgroundManager : MonoBehaviour
    {
        private SpriteRenderer backgroundImage;
        private Resolution resolution;

        void Start()
        {
            backgroundImage = GetComponent<SpriteRenderer>();
            resolution = Screen.currentResolution;
            backgroundImage.transform.localScale = new Vector3(resolution.width, resolution.height, 1);
        }

        void Update()
        {
            if (resolution.width != Screen.currentResolution.width ||
                resolution.height != Screen.currentResolution.height)
            {
                resolution = Screen.currentResolution;
                backgroundImage.transform.localScale = new Vector3(resolution.width, resolution.height, 1);
            }
        }


    }
}
