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
        [SerializeField] private Texture2D gameCursor;
        private SpriteRenderer screenBlocker;
        private Text gameOverText;
        private bool gameOver = false;
        private Resolution resolution;

        public void HandleGameOver(string loserName)
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

                gameOverText.text = (loserName == shooter.name ? defender.name : shooter.name) + " won!\nPress 'F' to replay";

                // Set winner's color to text
                gameOverText.color = (loserName == shooter.name ? defender.playerColor : shooter.playerColor);

                // Update win count
                PlayerScoreManager.Instance.UpdateScore(loserName == defender.name);
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

            // Change cursor for this scene
            Cursor.SetCursor(gameCursor, new Vector2(gameCursor.width / 2.0f, gameCursor.height / 2.0f), CursorMode.Auto);

            // Stop playing menu music!
            if (MenuMusicSingleton.Instance != null)
            {
                Destroy(MenuMusicSingleton.Instance.gameObject);
            }

            // Inform score manager that match has started
            PlayerScoreManager.Instance.MatchStart();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Stop playing da music!
                Destroy(AudioManager.Instance.gameObject);

                // Stop Counting score!
                Destroy(PlayerScoreManager.Instance.gameObject);

                // Restore cursor
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
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
