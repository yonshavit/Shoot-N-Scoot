using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameLogic
{
    public class PlayerHealthManager : MonoBehaviour
    {
        [SerializeField] private Image heartFull;
        [SerializeField] private Image heartHalf;
        [SerializeField] private Image heartEmpty;
        [SerializeField] private int currHealthValue = 6;
        [SerializeField] private float heightOffset = 50;
        [SerializeField] private float initWidthOffset = 30;
        [SerializeField] private float widthDelta = 20;
        [SerializeField] private bool flipSide = false;
        private Canvas canvas;
        private Image[] lives;
        

        void Start()
        {
            canvas = GetComponentInChildren<Canvas>();

            // Make sure health value starts as an even number!
            currHealthValue = currHealthValue + Mathf.CeilToInt((currHealthValue % 2) / 2.0f);

            lives = new Image[currHealthValue];

            initWidthOffset = flipSide ? canvas.pixelRect.xMax - initWidthOffset : initWidthOffset;

            for (var i = 0; i < lives.Length / 2; i++)
            {
                lives[i] = Instantiate(heartFull, Vector3.zero, Quaternion.identity);
                lives[i].transform.parent = canvas.transform;
                lives[i].rectTransform.position = new Vector3(initWidthOffset + widthDelta * i * (flipSide ? -1 : 1), 
                    canvas.pixelRect.yMax - heightOffset, 0);
            }
        }

        private void ManageLives()
        {
            // TODO decrease lives by changing icons well
        }

        public int Score
        {
            get => currHealthValue;
            set
            {
                currHealthValue = value;
                ManageLives();
            }
        }
    }
}
