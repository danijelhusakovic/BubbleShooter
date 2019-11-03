using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace danijelhusakovic.bubbleshooter
{
    public class InputManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        public UnityEvent Clicked;

        public void OnPointerClick(PointerEventData eventData)
        {
            Clicked.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {

        }

        public void OnPointerUp(PointerEventData eventData)
        {

        }
    }
}