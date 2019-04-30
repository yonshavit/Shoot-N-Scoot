using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Control;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameLogic
{
    public class PlayerScoreManager : MonoBehaviour
    {
        public static PlayerScoreManager Instance;

        private Text shooterScoreText;
        private Text defenderScoreText;
        private int shooterWinCount;
        private int defenderWinCount;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                Init();
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        private void Init()
        {
            shooterWinCount = 0;
            defenderWinCount = 0;
        }

        public void MatchStart()
        {
            // The only text component in player game objects
            shooterScoreText = GameObject.Find("Shooter").GetComponentInChildren<Text>();
            defenderScoreText = GameObject.Find("Defender").GetComponentInChildren<Text>();

            // Load current score into texts
            shooterScoreText.text = $"{shooterWinCount}";
            defenderScoreText.text = $"{defenderWinCount}";
        }

        public void UpdateScore(bool shooterWon)
        {
            if (shooterWon)
            {
                shooterWinCount++;
            }
            else
            {
                defenderWinCount++;
            }

            shooterScoreText.text = $"{shooterWinCount}";
            defenderScoreText.text = $"{defenderWinCount}";
        }
    }
}
