using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace danijelhusakovic.bubbleshooter
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Bubble : MonoBehaviour, IPooledObject
    {
        [SerializeField] private float _speed;
        private Transform _transform;
        private Rigidbody2D _rigidbody;
        public UnityEvent ExitedBottomArea;

        private void Awake()
        {
            _transform = transform;
            _rigidbody = GetComponent<Rigidbody2D>();
            ExitedBottomArea = new UnityEvent();
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
        }

        public void OnObjectSpawn()
        {
            GetComponent<SpriteRenderer>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        }
    }
}