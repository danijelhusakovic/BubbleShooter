using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace danijelhusakovic.bubbleshooter
{
    public class ArrowMovement : MonoBehaviour
    {
        private Transform _transform;
        private float _distanceToCamera;

        private void Awake()
        {
            _transform = transform;
            _distanceToCamera = _transform.position.z - Camera.main.transform.position.z;
        }

        private void Update()
        {
            MoveToMouse();
        }

        private void MoveToMouse()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _distanceToCamera));
            mousePos -= _transform.position;
            float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

            // If is used to combat the flipping of the arrow.
            if (angle < -90f) { angle = 180f; }
            else
            {
                angle = Mathf.Clamp(angle, 0f, 180f);
            }

            _transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        }
    }
}