using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace danijelhusakovic.bubbleshooter
{
    [System.Serializable] public class ClickedEvent: UnityEvent<Vector3> { }

    public class InputManager : MonoBehaviour
    {
        public ClickedEvent Clicked;

        private bool _isPressed;

        private void Awake()
        {
            _isPressed = false;
            Clicked = new ClickedEvent();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isPressed = true;
            }
            if (_isPressed && Input.GetMouseButtonUp(0))
            {
                Clicked.Invoke(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                _isPressed = false;
            }

        }
    }
}