﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Atlanticide.UI
{
    public class PauseScreen : MonoBehaviour
    {
        [SerializeField]
        public Text title;

        [SerializeField]
        public Text pausingPlayerText;

        public InputController Input { get; set; }

        /// <summary>
        /// Initializes the object.
        /// </summary>
        private void Start()
        {
            title.text = "Game Paused";
        }

        public void Activate(bool activate)
        {
            gameObject.SetActive(activate);
        }

        public void ResumeGame()
        {
            World.Instance.PauseGame(false);
        }

        public void SwapInputDevices()
        {
            Input.SwapInputDevices();
        }

        public void RestartLevel()
        {
            GameManager.Instance.StartSceneReset();
        }

        public void ReturnToMainMenu()
        {
            GameManager.Instance.LoadMainMenu();
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
