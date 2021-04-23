using System.Collections;
using System.Collections.Generic;
using Client.Scripts.Ds;
using UnityEngine;

namespace Client.Scripts
{
    public class Painter : MonoBehaviour
    {
        [SerializeField] 
        private DrawSettingsSo drawConfig;

        private List<Vector2> _currentLine;
        private LineRenderer _lineRenderer;

        private bool _isDrawing;

        private Camera _camera;
    
        void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                _StartDraw();
            
            if (Input.GetMouseButtonUp(0))
                _EndDraw();
        }

        private void _StartDraw()
        {
            _isDrawing = true;
            
            StartCoroutine(_Drawing());
        }
    
        private void _EndDraw()
        {
            _isDrawing = false;
        }

        IEnumerator _Drawing()
        {
            _StartLine();

            while (_isDrawing)
            {
                _AddPoint(_GetCurrentWorldPoint());
                yield return null;
            }
        }

        private void _StartLine()
        {
            _currentLine = new List<Vector2>();
            
            var line = new GameObject("Line"); 
            line.transform.parent = transform;

            _lineRenderer = line.AddComponent<LineRenderer>();
            _lineRenderer.positionCount = 0;
            _lineRenderer.startWidth = drawConfig.settings?.lineWidth ?? 0.1f;
            _lineRenderer.endWidth = drawConfig.settings?.lineWidth ?? 0.1f;
            _lineRenderer.startColor = drawConfig.settings?.lineColor ?? Color.green;
            _lineRenderer.endColor = drawConfig.settings?.lineColor ?? Color.green;
            _lineRenderer.useWorldSpace = true;
            _lineRenderer.numCapVertices = drawConfig.settings?.lineCapVertices ?? 5;
            _lineRenderer.numCornerVertices = drawConfig.settings?.lineCapVertices ?? 5;
            _lineRenderer.material = drawConfig.settings?.drawMaterial ? drawConfig.settings?.drawMaterial : null;
        }

        private Vector2 _GetCurrentWorldPoint()
        {
            return _camera.ScreenToWorldPoint(Input.mousePosition);
        }

        private void _AddPoint(Vector2 point)
        {
            if (_PlacePoint(point))
            {
                _currentLine.Add(point);
                var positionCount = _lineRenderer.positionCount;
                positionCount++;
                _lineRenderer.positionCount = positionCount;
                _lineRenderer.SetPosition(positionCount - 1, point);
            }
        }

        private bool _PlacePoint(Vector2 point)
        {
            if (_currentLine.Count == 0)
                return true;
            
            var distance = Vector2.Distance(point, _currentLine[_currentLine.Count - 1]);

            return distance >= drawConfig.settings.lineSeparationDistance;
        }
    }
}
