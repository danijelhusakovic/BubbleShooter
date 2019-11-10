using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace danijelhusakovic.bubbleshooter
{
    public enum StateType
    {
        Playing,
        Paused,
        GameOver,
        GameWon
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [SerializeField] private QueueManager _queuedBubbles;
        [SerializeField] private GridManager _gridManager;
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private BubbleFactory _factory;

        private Bubble _activeBubble;
        public Bubble ActiveBubble { set { _activeBubble = value; } }
        public StateType GameState;

        private void Awake()
        {
            if (Instance == null) { Instance = this; }
            GameState = StateType.Playing;
        }

        private void Start()
        {
            _activeBubble = _queuedBubbles.GetActive();
            _gridManager.Initialize(5);

            GridManager.Instance.OnHit.AddListener(OnHit);
            _inputManager.Clicked.AddListener(OnClick);
        }

        private void OnClick(Vector3 mousePosition)
        {
            _activeBubble.Launch(mousePosition);
            _queuedBubbles.ReplenishAndShift();
            _gridManager.ListenForCollision(_activeBubble);
            _activeBubble = _queuedBubbles.GetActive();
        }

        private void OnHit(bool isPopping)
        {
            if (isPopping == false)
            {
                GridManager.Instance.ShiftDown(1);
                GridManager.Instance.AddRowsFromTop(1);
            }
        }

        private void Update()
        {
            Debug.Log(GameState);
        }
    }
}