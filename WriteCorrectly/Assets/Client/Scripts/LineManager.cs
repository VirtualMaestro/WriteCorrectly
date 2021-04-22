using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client.Scripts
{
    public class LineManager : MonoBehaviour
    {
        [SerializeField] 
        private DrawSettingsSo drawConfig;
        
        private List<GameObject> _lines;
        private List<Vector2> _currentLine;
        private LineRenderer _lineRenderer;

        private bool drawing = false;

        private Camera _camera;
    
        void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                _OnStartDraw();
            
            if (Input.GetMouseButtonUp(0))
                _OnEndDraw();
        }

        private void _OnStartDraw()
        {
            drawing = true;
            
            StartCoroutine(_Drawing());
        }
    
        private void _OnEndDraw()
        {
            drawing = false;
        }

        IEnumerator _Drawing()
        {
            _StartLine();

            while (drawing)
            {
                _AddPoint(_GetCurrentWorldPoint());
                yield return null;
            }

            _EndLine();
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
            if (PlacePoint(point))
            {
                _currentLine.Add(point);
                var positionCount = _lineRenderer.positionCount;
                positionCount++;
                _lineRenderer.positionCount = positionCount;
                _lineRenderer.SetPosition(positionCount - 1, point);
            }
        }

        private bool PlacePoint(Vector2 point)
        {
            if (_currentLine.Count == 0)
                return true;
            
            var distance = Vector2.Distance(point, _currentLine[_currentLine.Count - 1]);

            return distance >= drawConfig.settings.lineSeparationDistance;
        }

        private void _EndLine()
        {

        }
    }
}
