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

            Vector2Int posInGrid = new Vector2Int(_height - destination.y, destination.x - 1);
            AddToGrid(posInGrid, bubbleTransform.GetComponent<Bubble>());

            List<Bubble> toDestroy = FindAllRecursiveNeighbors(posInGrid);

            foreach (Bubble bubble in toDestroy)
            {
                bubble.Explode();
                RemoveFromGrid(FindPositionOfBubble(bubble));
            }
        }

        private List<Bubble> FindAllRecursiveNeighbors(Vector2Int originPosition, List<Bubble> result = null)
        {
            List<Bubble> allNeighbors = FindNeighbors(originPosition);

            if (result == null)
                // Mark (see bellow)
                result = new List<Bubble>();

            List<Bubble> newBubbles = new List<Bubble>();
            foreach (Bubble bubble in allNeighbors)
            {
                if (!result.Contains(bubble))
                {
                    result.Add(bubble);
                    newBubbles.Add(bubble);
                }
            }

            // Recursion starts here.
            foreach (Bubble bubble in newBubbles)
            {
                List<Bubble> neighbors = FindAllRecursiveNeighbors(FindPositionOfBubble(bubble), result);
            }

            return result;
        }

        private void DestroyBubble(Bubble bubble)
        {
            bubble.Explode();
            Vector2Int positionOfBubble = FindPositionOfBubble(bubble);
            RemoveFromGrid(positionOfBubble);
        }

        private List<Bubble> FindNeighbors(Vector2Int location)
        {
            List<Bubble> results = new List<Bubble>();

            Bubble thisBubble = _grid[location.x, location.y];
            if (thisBubble == null) { return null; }

            // Check up.
            if (location.x > 0)
            {
                Bubble up = _grid[location.x - 1, location.y];
                if (up != null)
                {
                    bool isMatch = thisBubble.CompareTypes(up);
                    if (isMatch) { results.Add(up); }
                }
            }

            // Check down.
            if(location.x < _height - 1)
            {
                Bubble down = _grid[location.x + 1, location.y];
                if (down != null)
                {
                    bool isMatch = thisBubble.CompareTypes(down);
                    if (isMatch) { results.Add(down); }
                }
            }

            // Check left.
            if (location.y > 0)
            {
                Bubble left = _grid[location.x, location.y - 1];
                if (left != null)
                {
                    bool isMatch = thisBubble.CompareTypes(left);
                    if (isMatch) { results.Add(left); }
                }
            }

            // Check right.
            if (location.y < _width - 1)
            {
                Bubble right = _grid[location.x, location.y + 1];
                if (right != null)
                {
                    bool isMatch = thisBubble.CompareTypes(right);
                    if (isMatch) { results.Add(right); }
                }
            }

            // Check diagonal up right.
            if (location.x > 0 && location.y < _width - 1)
            {
                Bubble upRight = _grid[location.x - 1, location.y + 1];
                if (upRight != null)
                {
                    bool isMatch = thisBubble.CompareTypes(upRight);
                    if (isMatch) { results.Add(upRight); }
                }

            }

            // Check diagonal down right.
            if (location.x < _height - 1 && location.y < _width - 1)
            {
                Bubble downRight = _grid[location.x + 1, location.y + 1];
                if (downRight != null)
                {
                    bool isMatch = thisBubble.CompareTypes(downRight);
                    if (isMatch) { results.Add(downRight); }
                }
            }

            // Check diagonal down left.
            if (location.x < _height - 1 && location.y > 0)
            {
                Bubble downLeft = _grid[location.x + 1, location.y - 1];
                if (downLeft != null)
                {
                    bool isMatch = thisBubble.CompareTypes(downLeft);
                    if (isMatch) { results.Add(downLeft); }
                }
            }

            // Check diagonal up left.
            if (location.x > 0 && location.y > 0)
            {
                Bubble upLeft = _grid[location.x - 1, location.y - 1];
                if (upLeft != null)
                {
                    bool isMatch = thisBubble.CompareTypes(upLeft);
                    if (isMatch) { results.Add(upLeft); }
                }
            }

            return results;
        }

        private Vector2Int FindPositionOfBubble(Bubble bubble)
        {
            Vector2Int result = Vector2Int.zero;

            for (int rowIndex = 0; rowIndex < _height; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < _width; columnIndex++)
                {
                    if (_grid[rowIndex, columnIndex] == bubble)
                    {
                        result = new Vector2Int(rowIndex, columnIndex);
                    }
                }
            }

            return result;
        }
    }
}