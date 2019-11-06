using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace danijelhusakovic.bubbleshooter
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance;

        [SerializeField] private BubbleFactory _bubbleFactory;
        [SerializeField] private float _snapSpeed;
        private Bubble[,] _grid;
        private Bubble _newestBubble;

        private int _height;
        private int _width;

        private void Awake()
        {
            Instance = this;
            _grid = new Bubble[12, 18];
            _height = _grid.GetLength(0);
            _width = _grid.GetLength(1);
        }

        public void Initialize(int rows)
        {
            for (int rowIndex = 0; rowIndex < rows; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < _width; columnIndex++)
                {
                    AddBubble(rowIndex, columnIndex);
                }
            }
        }

        private void AddBubble(int rowIndex, int columnIndex)
        {
            GameObject go = _bubbleFactory.Create();
            go.GetComponent<CircleCollider2D>().enabled = true;
            go.GetComponent<CircleCollider2D>().isTrigger = false;
            AddToGrid(new Vector2Int(rowIndex, columnIndex), go.GetComponent<Bubble>());
            go.transform.position = new Vector3(columnIndex + 1, _height - rowIndex, 0f);
        }

        public void AddToGrid(Vector2Int position, Bubble bubble)
        {
            _newestBubble = bubble;
            _grid[position.x, position.y] = bubble;
        }

        public bool RemoveFromGrid(Vector2Int position)
        {
            if (_grid[position.x, position.y] == null) { return false; }

            _grid[position.x, position.y] = null;
            return true;
        }

        public void ListenForCollision(Bubble bubble)
        {
            _newestBubble = bubble;
            bubble.HitSomething.AddListener(AddAndSnap);
        }

        private void AddAndSnap(Bubble bubble, Vector3 positionOnCollision)
        {
            Vector2Int newPos = new Vector2Int(Mathf.RoundToInt(positionOnCollision.x), Mathf.RoundToInt(positionOnCollision.y));
            StartCoroutine(SnapToGrid(bubble.transform, newPos));
        }

        private IEnumerator SnapToGrid(Transform bubbleTransform, Vector2Int destination)
        {
            Vector3 finalPosition = new Vector3(destination.x, destination.y, 0f);
            do
            {
                bubbleTransform.position = Vector2.MoveTowards(bubbleTransform.position, destination, _snapSpeed * Time.deltaTime);
                yield return new WaitForSeconds(Time.deltaTime);
            } while (bubbleTransform.position != finalPosition);

            Debug.Log("Snapped to grid");
            Vector2Int posInGrid = new Vector2Int(_height - destination.y - 1, destination.x - 1);
            AddToGrid(posInGrid, _newestBubble);
        }
    }
}