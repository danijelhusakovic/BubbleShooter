using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace danijelhusakovic.bubbleshooter
{
    public enum BubbleType
    {
        Red,
        Orange,
        Blue,
        Yellow,
        Green
    }

    [RequireComponent(typeof(Rigidbody2D))]
    public class Bubble : MonoBehaviour, IPooledObject
    {
        [SerializeField] private float _speed;
        private Transform _transform;
        private Rigidbody2D _rigidbody;
        private BubbleType _type;
        private bool _hasHitSomething;

        public BubbleType Type { get { return _type; } set { _type = value; } }

        public UnityEvent ExitedBottomArea;

        [System.Serializable] public class PositionEvent : UnityEvent<Bubble, Vector3> { }
        public PositionEvent HitSomething;

        private void Awake()
        {
            _transform = transform;
            _rigidbody = GetComponent<Rigidbody2D>();
            ExitedBottomArea = new UnityEvent();
            HitSomething = new PositionEvent();
            _hasHitSomething = false;
        }

        public void Launch(Vector3 mousePos)
        {
            Vector3 direction = (mousePos - _transform.position);
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
            _rigidbody.AddForce(direction * _speed);
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.gameObject.tag.Equals("BottomArea") == false) { return; }
            GetComponent<CircleCollider2D>().isTrigger = false;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Bubble bubble = collision.gameObject.GetComponent<Bubble>();
            bool hitTopWall = collision.gameObject.tag.Equals("TopWall");

            if (bubble == null && hitTopWall == false) { return; }
            _rigidbody.bodyType = RigidbodyType2D.Static;
            if (_hasHitSomething == false)
            {
                HitSomething.Invoke(this, transform.position);
                _hasHitSomething = true;
            }
        }

        public void OnObjectSpawn()
        {
            _type = (BubbleType) Random.Range(0, 5);
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            Color newColor = Color.black;

            switch (_type)
            {
                case BubbleType.Blue:
                    newColor = Color.blue;
                    break;
                case BubbleType.Green:
                    newColor = Color.green;
                    break;
                case BubbleType.Orange:
                    newColor = new Color(0.9764f, 0.4157f, 0.0549f);
                    break;
                case BubbleType.Red:
                    newColor = Color.red;
                    break;
                case BubbleType.Yellow:
                    newColor = Color.yellow;
                    break;
                default:
                    newColor = Color.black;
                    break;
            }

            spriteRenderer.color = newColor;
        }

        public bool CompareTypes(Bubble other)
        {
            return _type == other.Type;
        }

        public void Pop()
        {
            GetComponent<SpriteRenderer>().color = Color.black;
        }
    }
}