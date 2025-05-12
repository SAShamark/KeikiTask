using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Widgets
{
    public class NestedScrollRectHandler : ScrollRect
    {
        private bool _shouldRouteToParent;

        public override void OnBeginDrag(PointerEventData eventData)
        {
            _shouldRouteToParent = ShouldDelegateToParent(eventData);
            if (_shouldRouteToParent)
            {
                DelegateDragEventToParent(eventData, ExecuteEvents.beginDragHandler);
            }
            else
            {
                base.OnBeginDrag(eventData);
            }
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (_shouldRouteToParent)
            {
                DelegateDragEventToParent(eventData, ExecuteEvents.dragHandler);
            }
            else
            {
                base.OnDrag(eventData);
            }
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            if (_shouldRouteToParent)
            {
                DelegateDragEventToParent(eventData, ExecuteEvents.endDragHandler);
            }
            else
            {
                base.OnEndDrag(eventData);
            }

            _shouldRouteToParent = false;
        }

        private bool ShouldDelegateToParent(PointerEventData eventData)
        {
            if (!horizontal && !vertical)
            {
                return true;
            }

            Vector2 delta = eventData.delta;
            var isHorizontalDrag = Mathf.Abs(delta.x) > Mathf.Abs(delta.y);

            if (horizontal && isHorizontalDrag)
            {
                return false;
            }

            return !vertical || isHorizontalDrag;
        }

        private void DelegateDragEventToParent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> eventFunction)
            where T : IEventSystemHandler
        {
            Transform currentParent = transform.parent;

            while (currentParent != null)
            {
                if (ExecuteEvents.Execute(currentParent.gameObject, data, eventFunction))
                {
                    break;
                }

                currentParent = currentParent.parent;
            }
        }
    }
}