using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace danijelhusakovic.bubbleshooter
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [SerializeField] private QueueManager _queuedBubbles;
        [SerializeField] private GridManager _gridManager;
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private BubbleFactory _factory;

        private Bubble _activeBubble;
        public Bubble ActiveBubble { set { _activeBubble = value; } }

        private void Awake()
        {
            if (Instance == null) { Instance = this; }

            _inputManager.Clicked.AddListener(OnClick);
        }

        private void Start()
        {
            _activeBubble = _queuedBubbles.GetActive();
            _gridManager.Initialize(5);
        }

        private void OnClick(Vector3 mousePosition)
        {
            _activeBubble.Launch(mousePosition);
            _queuedBubbles.ReplenishAndShift();
            _gridManager.ListenForCollision(_activeBubble);
            _activeBubble = _queuedBubbles.GetActive();
        }

    }
}