using Assets.Scripts.Control;
using Assets.Scripts.GameLogic.AI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.GameLogic
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private PlayerController[] players;
        [SerializeField] private Turret[] turrets;
        private SpriteRenderer screenBlocker;
        private Text gameOverText;
        private bool gameOver = false;
        private Resolution resolution;

        public void HandleGameOver(string playerName)
        {
            DarkenScreen();

            foreach (var player in players)
            {
                player.SetControl(false);
            }

            foreach (var turret in turrets)
            {
                turret.SetControl(false);
            }

            // TODO replace with something generic you lazy fuck
            gameOverText.text = (playerName == "Red" ? "Blue" : "Red") + " player won!\nPress space to replay";
        }

        private void DarkenScreen()
        {
            screenBlocker.color = new Color(1, 1, 1, 0.5f);
        }

        void Start()
        {
            screenBlocker = GetComponent<SpriteRenderer>();
            gameOverText = GetComponentInChildren<Text>();
            resolution = Screen.currentResolution;
            screenBlocker.transform.localScale = new Vector3(resolution.width, resolution.height, 1);
            screenBlocker.color = new Color(1,1,1,0);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);
            }

            if (gameOver && Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(1);
            }

            if (resolution.width != Screen.currentResolution.width ||
                resolution.height != Screen.currentResolution.height)
            {
                resolution = Screen.currentResolution;
                screenBlocker.transform.localScale = new Vector3(resolution.width, resolution.height, 1);
            }
        }
    }
}
