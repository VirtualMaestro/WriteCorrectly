using System;
using System.Collections.Generic;
using Client.Scripts.Utils;
using UnityEngine;

namespace Client.Scripts
{
    public class PathTracer
    {
        public event Action OnWrongDirection;
        public event Action OnTraceComplete;
        public event Action OnIncorrectStrokeStart;
        public event Action OnIncorrectStrokeEnd;
        public event Action OnCorrectStrokeStart;
        public event Action OnCorrectStrokeEnd;
        
        private List<Vector3[]> _strokes;
        private int _curNumStroke;
        private Vector3[] _currentStroke;
        private int _reachedPoint;
        
        private float _tracePointSize = 0.5f;
        private float _actionPointSize = 0.5f;

        private Vector2 _currentMousePosition;

        public void Init(List<Vector3[]> strokes)
        {
            _reachedPoint = 0;
            _curNumStroke = 0;
            _strokes = strokes;
        }

        public void BeginStroke(Vector2 mousePosition)
        {
            _currentMousePosition = mousePosition;
            _currentStroke = _strokes[_curNumStroke];
            
            if (_IsStartActionPointRight())
                OnCorrectStrokeStart?.Invoke();
            else
                OnIncorrectStrokeStart?.Invoke();
        }

        public void Trace(Vector2 mousePosition)
        {
            _currentMousePosition = mousePosition;
            
            while (_reachedPoint < _currentStroke.Length-1)
            {
                var lenBetweenCurrentAndNextPoints = Vector2.Distance(_currentStroke[_reachedPoint],
                    _currentStroke[_reachedPoint + 1]);
                var lenBetweenCurrentAndMouse =
                    Vector2.Distance(_currentStroke[_reachedPoint], _currentMousePosition);
                    
                if (lenBetweenCurrentAndMouse >= lenBetweenCurrentAndNextPoints)
                {
                    var isIntersect = IntersectionUtil.IsIntersectLineToCircle(_currentStroke[_reachedPoint], _currentMousePosition,
                        _currentStroke[_reachedPoint + 1], _tracePointSize);

                    if (isIntersect)
                    {
                        _reachedPoint++;
                    }
                    else
                    {
                        OnWrongDirection?.Invoke();
                        break;
                    }
                }
                else break;
            }
        }


        public void EndStroke(Vector2 mousePosition)
        {
            _currentMousePosition = mousePosition;
            
            if (_IsEndActionPointRight())
            {
                OnCorrectStrokeEnd?.Invoke();
                
                _curNumStroke++;

                if (_curNumStroke >= _strokes.Count)
                {
                    // Letter is correctly written
                    OnTraceComplete?.Invoke();
                }
            }
            else
                OnIncorrectStrokeEnd?.Invoke();
        }
                
        private bool _IsStartActionPointRight()
        {
            Debug.LogWarning($"MousePosition: {_currentMousePosition}, ActionPointPosition: {_currentStroke[0]}" +
                             $"Distance: {Vector2.Distance(_currentMousePosition, _currentStroke[0])}, ActionPointSize: {_actionPointSize}");
            return Vector2.Distance(_currentMousePosition, _currentStroke[0]) <= _actionPointSize; 
        }

        private bool _IsEndActionPointRight()
        {
            var lastStrokePoint = _currentStroke[_currentStroke.Length-1];
            return Vector2.Distance(_currentMousePosition, lastStrokePoint) <= _actionPointSize;
        }
    }
}