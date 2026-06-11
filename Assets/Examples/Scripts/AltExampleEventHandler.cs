using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class AltExampleEventHandler : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler
    {
        private float _minMoveForDrag;

        public PointerHandler MainPointer => _mainPointer;

        private PointerHandler _mainPointer;

        private void Awake()
        {
            _minMoveForDrag = 0f;
            _mainPointer = new PointerHandler("Main", _minMoveForDrag);
        }

        private void LateUpdate()
        {
            _mainPointer.ResetState();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_mainPointer.PointerID == PointerHandler.MinTouchID)
                _mainPointer.OnPress(eventData);
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (eventData.pointerId == _mainPointer.PointerID && _mainPointer.DragActive)
                _mainPointer.OnMove(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.pointerId == _mainPointer.PointerID)
                _mainPointer.OnRelease(eventData);
        }
    }

    public class PointerHandler
    {
        public const int MinTouchID = -10;
        public bool DragActive { get; private set; }
        public bool DragStarted { get; private set; }
        public bool DragInProcess { get; private set; }
        public bool DragFinished { get; private set; }
        public Vector2 Position { get; private set; }
        public int PointerID { get; private set; }
        public GameObject Object { get; private set; }

        private readonly string _name;
        private readonly float _minDragMovement;

        private Vector2 _downPosition;
        private bool _canStartDrag;
        private bool _isDragging;

        public PointerHandler(string name, float minDragMovement)
        {
            _name = name;
            _minDragMovement = minDragMovement;
            PointerID = MinTouchID;
        }

        public void ResetState()
        {
            if (DragFinished)
                Object = null;

            DragStarted = false;
            DragInProcess = false;
            DragFinished = false;
        }

        public void OnPress(PointerEventData eventData)
        {
            PointerID = eventData.pointerId;
            _isDragging = false;
            _canStartDrag = true;
            _downPosition = eventData.position;
            DragActive = true;
            Object = eventData.pointerEnter;
        }

        public void OnMove(PointerEventData eventData)
        {
            if (!_canStartDrag)
                return;

            Position = eventData.position;

            if (!_isDragging)
            {
                if (Vector2.SqrMagnitude(eventData.position - _downPosition) > _minDragMovement * _minDragMovement)
                {
                    DragStarted = true;
                    _isDragging = true;
                }
                else return;
            }

            DragInProcess = true;
        }

        public void OnRelease(PointerEventData eventData)
        {
            DragActive = false;
            DragFinished = true;
            Position = eventData.position;
            _canStartDrag = false;
            PointerID = MinTouchID;
        }
    }
}