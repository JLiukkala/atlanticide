﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Atlanticide.UI;

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

        [Header("WORLD CONFIG")]

        public float gravity = 1f;
        public float pushSpeed = 1f;

        [Range(0f, 1f)]
        public float minWalkingSpeedRatio = 0.2f;

        [SerializeField, Range(1, 20)]
        private int _maxEnergyCharges = 5;

        [SerializeField, Range(1f, 10f)]
        private float _interactRange = 3f;

        [SerializeField, Range(0f, 5f)]
        private float _playerInteractStrength = 1f;

        [Header("INVENTORY")]

        public List<int> keyCodes = new List<int>();

        private UIController _ui;
        private bool _gamePaused;

        public int MaxEnergyCharges { get { return _maxEnergyCharges; } }

        public int CurrentEnergyCharges { get; set; }

        public float InteractRange { get { return _interactRange; } }

        public float PlayerInteractStrength { get { return _playerInteractStrength; } }

        public bool GamePaused { get { return _gamePaused; } }

        public float DeltaTime
        {
            get { return (GamePaused ? 0f : Time.deltaTime); }
        }

        public bool ShieldBashing { get; set; }

        public bool EnergyCollectorIsActive
        {
            get { return DrainingEnergy || EmittingEnergy; }
        }

        public bool EmittingEnergy { get; set; }

        public bool DrainingEnergy { get; set; }


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
        public void Init()
        {
            _ui = FindObjectOfType<UIController>();
        }

        /// <summary>
        /// Updates the object once per frame.
        /// </summary>
        private void Update()
        {
            // ...
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
            //else
            //{
            //    Debug.Log("Key code [" + keyCode + "] is already active");
            //}

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

            if (!pause || playerName.Length > 0)
            {
                GameManager.Instance.ActivatePauseScreen(GamePaused, playerName);
            }
        }

        public void SetEnergyChargesAndUpdateUI(int charges)
        {
            CurrentEnergyCharges = Utils.Clamp(charges, 0, MaxEnergyCharges);
            //float ratio = GetEnergyRatio();
            //_ui.UpdateEnergyBar(ratio);
            //Debug.Log(string.Format("Energy charges: {0} ({1} %)",
            //    CurrentEnergyCharges, ratio * 100));
        }

        public float GetEnergyRatio()
        {
            return (float) CurrentEnergyCharges / MaxEnergyCharges;
        }

        /// <summary>
        /// Resets the world to its default state.
        /// </summary>
        public void ResetWorld()
        {
            keyCodes.Clear();
            EmittingEnergy = false;
            ShieldBashing = false;
            DrainingEnergy = false;
        }
    }
}
