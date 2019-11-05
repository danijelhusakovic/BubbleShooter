using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace danijelhusakovic.bubbleshooter
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private BubbleFactory _bubbleFactory;
        private Bubble[,] _grid;

        private int _height;
        private int _width;

        private void Awake()
        {
            _grid = new Bubble[12, 18];
            _height = _grid.GetLength(0);
            _width = _grid.GetLength(1);
        }

        private void Start()
        {
            Initialize(5);
        }

        private void Initialize(int rows)
        {
            for (int rowIndex = 0; rowIndex < rows; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < _grid.GetLength(1); columnIndex++)
                {
                    GameObject go = _bubbleFactory.Create();
                    go.GetComponent<CircleCollider2D>().enabled = true;
                    go.GetComponent<CircleCollider2D>().isTrigger = false;
                    _grid[rowIndex, columnIndex] = go.GetComponent<Bubble>();
                    go.transform.position = new Vector3(columnIndex + 1, _height - rowIndex, 0f);
                }
            }
        }

        public void AddToGrid(Vector2Int position, Bubble bubble)
        {
            _grid[position.x, position.y] = bubble;
        }

        public bool RemoveFromGrid(Vector2Int position)
        {
            if (_grid[position.x, position.y] == null) { return false; }

            _grid[position.x, position.y] = null;
            return true;
        }
    }
}