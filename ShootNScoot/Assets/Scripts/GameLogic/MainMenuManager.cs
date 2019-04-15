using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.GameLogic
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] Camera bgCamera;

        public void LoadByIndex(int sceneIndex)
        {
            DontDestroyOnLoad(bgCamera);

            SceneManager.LoadScene(sceneIndex);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
