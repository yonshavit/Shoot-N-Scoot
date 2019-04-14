using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameLogic
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SplashScreenManager : MonoBehaviour
    {
        public OnSplashFinishedHandler onSplashFinished;
        private SpriteRenderer splashIcon;
        private Resolution resolution;
        private float threshold = 0.95f;

        public void DarkenScreen()
        {
            splashIcon.color = new Color(1, 1, 1, 0.01f);
        }

        public void LightenScreen()
        {
            splashIcon.color = Color.white;
        }

        void Start()
        {
            splashIcon = GetComponent<SpriteRenderer>();
            resolution = Screen.currentResolution;
            DarkenScreen();
            splashIcon.transform.localScale = new Vector3(resolution.width, resolution.height, 1);

            StartCoroutine(SplashFadeIn());
        }

        void Update()
        {
            if (resolution.width != Screen.currentResolution.width ||
                resolution.height != Screen.currentResolution.height)
            {
                resolution = Screen.currentResolution;
                splashIcon.transform.localScale = new Vector3(resolution.width, resolution.height, 1);
            }
        }

        private IEnumerator<WaitForEndOfFrame> SplashFadeIn()
        {
            while (splashIcon.color.a < threshold)
            {
                splashIcon.color = Color.Lerp(splashIcon.color, Color.white, Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }

            StartGame();
        }

        void StartGame()
        {
            LightenScreen();
            onSplashFinished.Invoke();
        }
    }

    [Serializable]
    public class OnSplashFinishedHandler : UnityEngine.Events.UnityEvent { }
}
