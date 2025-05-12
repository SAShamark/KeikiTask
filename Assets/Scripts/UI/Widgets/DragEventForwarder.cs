using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Widgets
{
    public class DragEventForwarder : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler,
        IPointerUpHandler
    {
        private bool _isDragging;

        public void OnPointerDown(PointerEventData eventData)
        {
            _isDragging = false;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_isDragging)
            {
                CancelClick(eventData);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _isDragging = true;
            ForwardToParent(eventData, ExecuteEvents.beginDragHandler);
        }

        public void OnDrag(PointerEventData eventData)
        {
            ForwardToParent(eventData, ExecuteEvents.dragHandler);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ForwardToParent(eventData, ExecuteEvents.endDragHandler);
        }

        private void ForwardToParent<T>(PointerEventData eventData, ExecuteEvents.EventFunction<T> function)
            where T : IEventSystemHandler
        {
            Transform parent = transform.parent;
            while (parent != null)
            {
                if (ExecuteEvents.Execute(parent.gameObject, eventData, function))
                {
                    break;
                }
                parent = parent.parent;
            }
        }

        private void CancelClick(PointerEventData eventData) => eventData.eligibleForClick = false;
    }
}