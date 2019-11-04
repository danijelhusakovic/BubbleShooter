using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace danijelhusakovic.bubbleshooter
{
    public class QueueManager : MonoBehaviour
    {
        [SerializeField] private int _numBubbles;
        [SerializeField] private Transform _rightmostTransform;
        [SerializeField] private Transform _arrow;
        [Range(0f, 1f)][SerializeField] private float _padding;

        private Queue<Bubble> _bubbles;
        private Queue<GameObject> _bubblesGOs;
        private Vector3 _firstPosition;

        private void Awake()
        {
            _bubbles = new Queue<Bubble>();
            _bubblesGOs = new Queue<GameObject>();
            _numBubbles = 3;
            _firstPosition = _rightmostTransform.position;
        }

        private void Start()
        {
            InitBubblez();
        }

        private void InitBubblez()
        {
            for (int bubbleIndex = 0; bubbleIndex < _numBubbles; bubbleIndex++)
            {
                AddBubble();
            }

            GetActive();
        }

        private void AddBubble()
        {
            GameObject go = BubbleFactory.Instance.Create();
            Bubble newBubble = go.GetComponent<Bubble>();

            go.transform.position = _firstPosition + Vector3.left * _bubblesGOs.Count + Vector3.left * _padding * _bubblesGOs.Count;

            _bubblesGOs.Enqueue(go);
            _bubbles.Enqueue(newBubble);

        }

        public Bubble GetActive()
        {
            Bubble result = null;
            result = _bubbles.Dequeue();
            GameObject go = _bubblesGOs.Dequeue();
            go.transform.position = _arrow.position;
            ShiftBubbles();
            AddBubble();

            return result;
        }

        private void ShiftBubbles()
        {
            foreach (GameObject go in _bubblesGOs)
            {
                go.transform.position += Vector3.right + Vector3.right * _padding;
            }
        }
    }
}