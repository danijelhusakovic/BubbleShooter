using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace danijelhusakovic.bubbleshooter
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [SerializeField] private QueueManager _queuedBubbles;
        [SerializeField] private InputManager _inputManager;

        private Bubble _activeBubble;

        private void Awake()
        {
            if (Instance == null) { Instance = this; }

            _inputManager.Clicked.AddListener(OnClick);
        }

        private void OnClick()
        {
            Debug.Log("Clicked");
        }

    }
}