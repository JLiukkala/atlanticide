﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atlanticide
{
    public class World : MonoBehaviour
    {
        #region Statics
        private static World instance;
        public static World Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<World>();

                    if (instance == null)
                    {
                        Debug.LogError("A World object could not be found in the scene.");
                    }
                }

                return instance;
            }
        }
        #endregion Statics

        public float gravity = 1f;
        public float pushSpeed = 1f;

        [SerializeField, Range(0.1f, 5f)]
        public float telegrabRadius = 1f;

        [SerializeField, Range(0.1f, 5f)]
        public float energyCollectRadius = 1f;

        public List<int> keyCodes = new List<int>();
        private bool _gamePaused;

        public bool GamePaused
        {
            get { return _gamePaused; }
        }

        public float DeltaTime
        {
            get { return (GamePaused ? 0f : Time.deltaTime); }
        }

        /// <summary>
        /// Initializes the singleton instance.
        /// </summary>
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Initializes the object.
        /// </summary>
        private void Start()
        {
        }

        /// <summary>
        /// Updates the object once per frame.
        /// </summary>
        private void Update()
        {
        }

        public bool TryActivateNewKeyCode(int keyCode, bool allowDuplicates)
        {
            bool added = false;

            if (allowDuplicates)
            {
                keyCodes.Add(keyCode);
                added = true;
            }
            else
            {
                added = keyCodes.AddIfNew(keyCode);
            }

            if (added)
            {
                Debug.Log("Key code [" + keyCode + "] activated");
            }
            else
            {
                Debug.Log("Key code [" + keyCode + "] is already active");
            }

            return added;
        }

        public bool DeactivateKeyCode(int keyCode)
        {
            bool removed = keyCodes.Remove(keyCode);

            if (removed)
            {
                Debug.Log("First instance of key code [" + keyCode + "] deactivated");
            }
            else
            {
                Debug.Log("No key code [" + keyCode + "] to deactivate");
            }

            return removed;
        }

        /// <summary>
        /// Pauses or unpauses the game.
        /// </summary>
        /// <param name="pause">Should the game be paused</param>
        /// <param name="playerName">[Optional] The name of the player
        /// who paused the game</param>
        public void PauseGame(bool pause, string playerName = "")
        {
            _gamePaused = pause;
            GameManager.Instance.ActivatePauseScreen(GamePaused, playerName);
        }

        /// <summary>
        /// Resets the world to its default state.
        /// </summary>
        public void ResetWorld()
        {
            keyCodes.Clear();
        }
    }
}
