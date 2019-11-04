﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace danijelhusakovic.bubbleshooter
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [SerializeField] private QueueManager _queuedBubbles;
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private BubbleFactory _factory;

        private Bubble _activeBubble;

        private void Awake()
        {
            if (Instance == null) { Instance = this; }

            _inputManager.Clicked.AddListener(OnClick);
        }

        private void Start()
        {
            _activeBubble = _queuedBubbles.GetActive();
        }

        private void OnClick(Vector3 mousePosition)
        {
            _activeBubble.Launch(mousePosition);
            _activeBubble = _queuedBubbles.GetActive();
        }

    }
}