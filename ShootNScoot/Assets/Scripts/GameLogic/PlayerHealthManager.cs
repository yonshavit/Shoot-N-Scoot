using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameLogic
{
    public class PlayerHealthManager : MonoBehaviour
    {
        public OnHealthEmptyHandler onHealthEmptyHandler;

        [SerializeField] private Image heartFull;
        [SerializeField] private Image heartHalf;
        [SerializeField] private Image heartEmpty;
        [SerializeField] private int currHealthValue = 6;
        [SerializeField] [Range(0, 1)] private float heightOffsetRatio = 0.05f;
        [SerializeField] [Range(0, 1)] private float initWidthOffsetRatio = 0.02f;
        [SerializeField] [Range(0, 1)] private float widthDeltaRatio = 0.01f;
        [SerializeField] private bool flipSide = false;

        private Canvas canvas;
        private Image[] lives;
        private float heightOffset;
        private float initWidthOffset;
        private float widthDelta;


        void Start()
        {
            canvas = GetComponentInChildren<Canvas>();

            heightOffset = heightOffsetRatio * canvas.pixelRect.height;
            initWidthOffset = initWidthOffsetRatio * canvas.pixelRect.width;
            widthDelta = widthDeltaRatio * canvas.pixelRect.width;

            // Make sure health value starts as an even number!
            currHealthValue = currHealthValue + Mathf.CeilToInt((currHealthValue % 2) / 2.0f);

            lives = new Image[currHealthValue];

            initWidthOffset = flipSide ? canvas.pixelRect.xMax - initWidthOffset : initWidthOffset;

            for (var i = 0; i < lives.Length / 2; i++)
            {
                lives[i] = Instantiate(heartFull, Vector3.zero, Quaternion.identity);
                lives[i].transform.SetParent(canvas.transform, false);
                lives[i].rectTransform.position = new Vector3(initWidthOffset + widthDelta * i * (flipSide ? -1 : 1), 
                    canvas.pixelRect.yMax - heightOffset, 0);
            }
        }

        private void ManageLives()
        {
            for (var i = 0; i < lives.Length / 2; i++)
            {
                var currHeartMinHealth = (i + 1) * 2;

                if (currHealthValue >= currHeartMinHealth)
                {
                    if (lives[i].sprite != heartFull.sprite)
                    {
                        lives[i].sprite = heartFull.sprite;
                    }
                }
                else if (currHealthValue >= currHeartMinHealth - 1)
                {
                    if (lives[i].sprite != heartHalf.sprite)
                    {
                        lives[i].sprite = heartHalf.sprite;
                    }
                }
                else
                {
                    if (lives[i].sprite != heartEmpty.sprite)
                    {
                        lives[i].sprite = heartEmpty.sprite;
                    }
                }
            }
        }

        public int Score
        {
            get => currHealthValue;
            set
            {
                currHealthValue = value;
                ManageLives();

                if (currHealthValue == 0)
                {
                    onHealthEmptyHandler.Invoke(name);
                }
            }
        }

        [Serializable]
        public class OnHealthEmptyHandler : UnityEngine.Events.UnityEvent<string> { }
    }
}
