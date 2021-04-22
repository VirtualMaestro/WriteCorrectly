using System;
using System.Collections.Generic;
using UnityEngine;

namespace Client.Scripts
{
    public class LetterTemplateRenderer : MonoBehaviour
    {
        [SerializeField]
        private Letter letter;
        [SerializeField]
        private DrawSettingsSo drawConfig;

        private List<Vector3[]> _strokes;

        private int _curNumStroke = 0;
        private Vector3[] _currentStroke;
        private bool _isStrokeStart;

        private int _reachedPoint = 0;
        
        private Camera _camera;
        private Vector2 _prevMousePosition;
        private float _mouseSensitivity = 0.1f;
        
        private float _tracePointSize = 0.5f;
        private float _actionPointSize = 0.5f;
    
        void Awake()
        {
            _camera = Camera.main;
        }
        
        void Start()
        {
            _strokes = DrawUtil.DrawLetter(letter, transform, drawConfig.settings);
        }

        private void _ReStart()
        {
            _reachedPoint = 0;
            _curNumStroke = 0;
            _prevMousePosition = Vector2.zero;

            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            
            _strokes = DrawUtil.DrawLetter(letter, transform, drawConfig.settings);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                _OnStartStroke();
            
            if (Input.GetMouseButtonUp(0))
                _OnEndStroke();
            
            if (Input.GetMouseButtonUp(1))
                _ReStart();
            
            if (_isStrokeStart && _MouseMoved(out var mouseWorldPosition))
            {
                while (_reachedPoint < _currentStroke.Length-1)
                {
                    var lenBetweenCurrentAndNextPoints = Vector2.Distance(_currentStroke[_reachedPoint],
                        _currentStroke[_reachedPoint + 1]);
                    var lenBetweenCurrentAndMouse =
                        Vector2.Distance(_currentStroke[_reachedPoint], mouseWorldPosition);
                    
                    if (lenBetweenCurrentAndMouse >= lenBetweenCurrentAndNextPoints)
                    {
                        var isIntersect = IsIntersectLineToCircle(_currentStroke[_reachedPoint], mouseWorldPosition,
                            _currentStroke[_reachedPoint + 1], _tracePointSize);

                        if (isIntersect)
                        {
                            _reachedPoint++;
                        }
                        else
                        {
                            Debug.Log("Wrong direction");
                            break;
                        }
                    }
                    else break;
                }
            }
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

        private void _OnStartStroke()
        {
            Debug.Log($"_OnStartStroke");

            _isStrokeStart = true;
            
            _currentStroke = _strokes[_curNumStroke];

            if (_IsStartActionPointRight())
            {
                Debug.Log($"Right on the start");
            }
            else
            {
                Debug.LogWarning($"Wrong start! Try again!");
            }
        }

        private bool _IsStartActionPointRight()
        {
            return Vector2.Distance(_GetMouseWorldPoint(), _currentStroke[0]) <= _actionPointSize; 
        }

        private bool _IsEndActionPointRight()
        {
            var mousePosition = _GetMouseWorldPoint();
            var lastStrokePoint = _currentStroke[_currentStroke.Length-1];
            return Vector2.Distance(mousePosition, lastStrokePoint) <= _actionPointSize;
        }
        
        private void _OnEndStroke()
        {
            _isStrokeStart = false;

            if (_IsEndActionPointRight())
            {
                // TODO: Everything is good! Move to the next stroke
                Debug.Log("Everything is good! Move to the next stroke");
                
                _curNumStroke++;

                if (_curNumStroke >= _strokes.Count)
                {
                    // TODO: Letter is correctly written
                    Debug.Log("Letter is correctly written");
                }
            }
            else
            {
                // TODO: Wrong! Try again!
                Debug.LogWarning("Wrong action point! Try again!");
            }
        }
        
        private Vector2 _GetMouseWorldPoint()
        {
            return _camera.ScreenToWorldPoint(Input.mousePosition);
        }
        
        public static bool IsIntersectLineToCircle(Vector2 startLine, Vector2 endLine, Vector2 circlePosition, float radius)
        {
            var cpX = circlePosition.x;
            var cpY = circlePosition.y;
            var x1 = startLine.x - cpX;
            var y1 = startLine.y - cpY;
            var x2 = endLine.x - cpX;
            var y2 = endLine.y - cpY;

            var dx = x2 - x1;
            var dy = y2 - y1;

            var a = dx * dx + dy * dy;
            var b = 2.0 * (x1 * dx + y1 * dy);
            var c = x1 * x1 + y1 * y1 - radius * radius;

            if (-b < 0) return (c < 0);
            if (-b < (2.0 * a)) return (((4.0 * a * c) - b * b) < 0);

            return ((a + b + c) < 0);
        }
        
        void OnDrawGizmos()
        {
            
            if( _strokes != null )
            {
                Gizmos.color = Color.blue;
                if (_currentStroke != null) Gizmos.DrawWireSphere(_currentStroke[0], _actionPointSize);
                foreach (var stroke in _strokes)
                {
                    foreach (var vertex in stroke)
                    {
                        Gizmos.DrawSphere(vertex, _tracePointSize );
                    }
                }
            }
        }
    }
}
