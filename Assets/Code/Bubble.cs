﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace danijelhusakovic.bubbleshooter
{
    public class Bubble : MonoBehaviour
    {
        [SerializeField] private float _speed;
        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }

        public void Launch(Vector3 mousePos)
        {
            Vector3 direction = (mousePos - _transform.position);
            StartCoroutine(MoveCoroutine(direction));
        }

        private IEnumerator MoveCoroutine(Vector2 dir)
        {
            do
            {
                _transform.position += (Vector3) dir * _speed;
                yield return new WaitForSeconds(Time.deltaTime);
            }
            while (true);
        }
    }
}