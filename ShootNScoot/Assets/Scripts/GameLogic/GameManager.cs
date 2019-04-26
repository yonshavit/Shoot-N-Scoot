using Assets.Scripts.Audio;
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
        [SerializeField] private PlayerController shooter;
        [SerializeField] private PlayerController defender;
        [SerializeField] private Turret[] turrets;
        private SpriteRenderer screenBlocker;
        private Text gameOverText;
        private bool gameOver = false;
        private Resolution resolution;
        private AudioManager audioManager;

        public void HandleGameOver(string playerName)
        {
            // Handle once per match
            if (!gameOver)
            {
                gameOver = true;

                DarkenScreen();

                shooter.ControllerEnabled = false;
                defender.ControllerEnabled = false;

                foreach (var turret in turrets)
                {
                    turret.SetControl(false);
                }

                gameOverText.text = (playerName == shooter.name ? defender.name : shooter.name) + " won!\nPress 'F' to replay";
            }
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
            audioManager = FindObjectOfType<AudioManager>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Stop playing da music!
                Destroy(audioManager);
                SceneManager.LoadScene(0);
            }

            if (gameOver && Input.GetKeyDown(KeyCode.F))
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
