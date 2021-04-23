using System;
using Client.Scripts.Common;
using UnityEngine;

namespace Client.Scripts
{
    public class IM : Singleton<IM>
    {
        public event Action<Vector2> OnMouseDown;
        public event Action<Vector2> OnMouseUp;
        public event Action<Vector2> OnMouseMove;
        
        private Vector2 _prevMousePosition;
        private Camera _camera;
        private float _mouseSensitivity = 0.1f;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                OnMouseDown?.Invoke(_GetMousePosition());
            
            if (Input.GetMouseButtonUp(0))
                OnMouseUp?.Invoke(_GetMousePosition());

            if (_MouseMoved(out var mouseWorldPosition))
                OnMouseMove?.Invoke(mouseWorldPosition);                
        }
        
        private bool _MouseMoved(out Vector2 mouseWorldPosition)
        {
            var currentMouseScreenPosition = Input.mousePosition;
            if (Vector2.Distance(_prevMousePosition, currentMouseScreenPosition) >= _mouseSensitivity)
            {
                _prevMousePosition = currentMouseScreenPosition;
                mouseWorldPosition = _camera.ScreenToWorldPoint(currentMouseScreenPosition);
                return true;
            }

            mouseWorldPosition = new Vector2();
            return false;
        }

        private Vector2 _GetMousePosition() => _camera.ScreenToWorldPoint(Input.mousePosition);
    }
}