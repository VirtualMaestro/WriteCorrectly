using System.Collections.Generic;
using Client.Scripts.Ds;
using UnityEngine;

namespace Client.Scripts
{
    public class Painter : MonoBehaviour
    {
        private DrawSettings _drawConfig;

        private float _lineSeparationDistance;
        private List<Vector2> _currentLine;
        private LineRenderer _lineRenderer;

        private bool _isDrawingDisable = true;

        void Awake()
        {
            GM.I.OnDrawingLetterStart += _OnDrawingStart;
            GM.I.OnDrawingLetterEnd += _OnDrawingEnd;
        }

        private void _OnDrawingStart()
        {
            _CleaUp();
            
            _lineSeparationDistance = GM.I.AppSettings.mouseSensitivity;
            _drawConfig = GM.I.AppSettings.fillDrawSettings;

            IM.I.OnMouseDown += _OnStartDraw;
            IM.I.OnMouseUp += _OnEndDraw;

            _isDrawingDisable = false;
        }

        private void _OnDrawingEnd()
        {
            IM.I.OnMouseDown -= _OnStartDraw;
            IM.I.OnMouseUp -= _OnEndDraw;
            IM.I.OnMouseMove -= _OnDrawing;
            
            _isDrawingDisable = true;
        }

        private void _OnStartDraw(Vector2 mousePosition)
        {
            if (_isDrawingDisable) return;
            
            IM.I.OnMouseMove += _OnDrawing;

            _currentLine = new List<Vector2>();

            var line = new GameObject("Line");
            line.transform.parent = transform;

            _lineRenderer = line.AddComponent<LineRenderer>();
            _lineRenderer.positionCount = 0;
            _lineRenderer.startWidth = _drawConfig.lineWidth;
            _lineRenderer.endWidth = _drawConfig.lineWidth;
            _lineRenderer.startColor = _drawConfig.lineColor;
            _lineRenderer.endColor = _drawConfig.lineColor;
            _lineRenderer.useWorldSpace = true;
            _lineRenderer.numCapVertices = _drawConfig.capVertices;
            _lineRenderer.numCornerVertices = _drawConfig.cornerVertices;
            _lineRenderer.material = _drawConfig.drawMaterial;
        }

        private void _OnDrawing(Vector2 mousePosition)
        {
            if (_CanPlacePoint(mousePosition))
            {
                _currentLine.Add(mousePosition);
                var positionCount = _lineRenderer.positionCount;
                positionCount++;
                _lineRenderer.positionCount = positionCount;
                _lineRenderer.SetPosition(positionCount - 1, mousePosition);
            }
        }

        private void _OnEndDraw(Vector2 mousePosition)
        {
            IM.I.OnMouseMove -= _OnDrawing;
        }

        private bool _CanPlacePoint(Vector2 point)
        {
            if (_currentLine.Count == 0)
                return true;

            var distance = Vector2.Distance(point, _currentLine[_currentLine.Count - 1]);

            return distance >= _lineSeparationDistance;
        }
        
        private void _CleaUp()
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}