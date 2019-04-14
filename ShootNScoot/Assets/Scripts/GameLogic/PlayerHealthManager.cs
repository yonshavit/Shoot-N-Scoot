﻿using System;
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

        private Image[] lives;

        void Start()
        {
            lives = new Image[Mathf.CeilToInt(currHealthValue / 2.0f)];

            foreach (var life in lives)
            {
                
            }
        }

        public int Score
        {
            get => currHealthValue;
            set
            {
                currHealthValue = value;
                
            }
        }
    }
}
