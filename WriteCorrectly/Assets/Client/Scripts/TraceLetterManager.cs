using System.Collections.Generic;
using Client.Scripts.Ds;
using Client.Scripts.Utils;
using UnityEngine;

namespace Client.Scripts
{
    public class TraceLetterManager : MonoBehaviour
    {
        private List<Vector3[]> _strokes;

        private float _tracePointSize = 0.5f;
        private float _actionPointSize = 0.5f;

        private PathTracer _pathTracer;

        void Awake()
        {
            GM.I.OnDrawingLetterStart += _OnDrawingStart;
            GM.I.OnDrawingLetterEnd += _OnDrawingEnd;

            _pathTracer = new PathTracer();
        }

        private void _OnDrawingStart(Letter letter)
        {
            _CleaUp();
            
            _pathTracer.OnTraceComplete += _OnTraceComplete;
            _pathTracer.OnWrongDirection += _OnWrongDirection;
            _pathTracer.OnCorrectStrokeStart += _OnCorrectStrokeStart;
            _pathTracer.OnIncorrectStrokeStart += _OnIncorrectStrokeStart;
            _pathTracer.OnCorrectStrokeEnd += _OnCorrectStrokeEnd;
            _pathTracer.OnIncorrectStrokeEnd += _OnIncorrectStrokeEnd;
            
            IM.I.OnMouseDown += _OnStrokeStart;
            IM.I.OnMouseUp += _OnStrokeEnd;

            var appSettings = GM.I.AppSettings;
            _tracePointSize = appSettings.tracePointSize;
            _actionPointSize= appSettings.actionPointSize;
            
            _strokes = DrawUtil.DrawLetter(letter, transform, appSettings.letterTemplateDrawSettings);
            _pathTracer.Init(_strokes, _tracePointSize, _actionPointSize);
        }

        private void _OnDrawingEnd()
        {
            _pathTracer.OnTraceComplete -= _OnTraceComplete; 
            _pathTracer.OnWrongDirection -= _OnWrongDirection;
            _pathTracer.OnCorrectStrokeStart -= _OnCorrectStrokeStart;
            _pathTracer.OnIncorrectStrokeStart -= _OnIncorrectStrokeStart;
            _pathTracer.OnCorrectStrokeEnd -= _OnCorrectStrokeEnd;
            _pathTracer.OnIncorrectStrokeEnd -= _OnIncorrectStrokeEnd;
            
            IM.I.OnMouseDown -= _OnStrokeStart;
            IM.I.OnMouseUp -= _OnStrokeEnd;
        }

        private void _OnTraceComplete()
        {
            Debug.Log("Trace complete!");
        }
        
        private void _OnWrongDirection()
        {
            Debug.LogWarning("Wrong direction!");
        }

        private void _OnCorrectStrokeStart()
        {
            Debug.Log("Stroke starts correctly!");
        }

        private void _OnIncorrectStrokeStart()
        {
            Debug.LogWarning("Stroke starts incorrectly!");
        }

        private void _OnCorrectStrokeEnd()
        {
            Debug.Log("Stroke ends correctly!");
        }

         private void _OnIncorrectStrokeEnd()
        {
            Debug.LogWarning("Stroke ends incorrectly!");
        }

        private void _OnStrokeStart(Vector2 mousePosition)
        {
            _pathTracer.BeginStroke(mousePosition);
            IM.I.OnMouseMove += _OnTrace;
        }

        private void _OnStrokeEnd(Vector2 mousePosition)
        {
            _pathTracer.EndStroke(mousePosition);
            IM.I.OnMouseMove -= _OnTrace;
        }

        private void _OnTrace(Vector2 mousePosition)
        {
            _pathTracer.Trace(mousePosition);
        }

        private void _CleaUp()
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        void OnDrawGizmos()
        {
            if (_strokes != null)
            {
                Gizmos.color = Color.blue;

                foreach (var stroke in _strokes)
                {
                    Gizmos.color = Color.blue;
                    
                    foreach (var vertex in stroke)
                    {
                        Gizmos.DrawSphere(vertex, _tracePointSize);
                    }
                    
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawSphere(stroke[0], _actionPointSize);
                    Gizmos.DrawSphere(stroke[stroke.Length-1], _actionPointSize);
                }
            }
        }
    }
}