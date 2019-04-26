using Assets.Scripts.GameLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioClip[] sources;

        public static AudioManager Instance = null;
        private AudioSource manager;
        private float lastSourceStartTime;
        private int index;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                manager = GetComponent<AudioSource>();
                sources.Shuffle();
                index = 0;

                if (sources.Length > 0)
                {
                    manager.clip = sources[index];
                    manager.Play();
                    lastSourceStartTime = Time.time;
                }
            }
            else if (Instance != this)
            {
                // Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);
            }

            // Sets this to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.time - lastSourceStartTime >= manager.clip.length)
            {
                index = (index + 1) % sources.Length;
                manager.clip = sources[index];
            }
        }
    }
}