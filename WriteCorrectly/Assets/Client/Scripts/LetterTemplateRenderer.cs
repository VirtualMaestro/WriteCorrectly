using System.Collections.Generic;
using Client.Scripts.Ds;
using Client.Scripts.Utils;
using UnityEngine;

namespace Client.Scripts
{
    public class LetterTemplateRenderer : MonoBehaviour
    {
        [SerializeField] private Letter letter;
        [SerializeField] private DrawSettingsSo drawConfig;

        private GM _gm;
        private IM _im;

        private List<Vector3[]> _strokes;

        private float _tracePointSize = 0.5f;
        private float _actionPointSize = 0.5f;

        private PathTracer _pathTracer;

        void Awake()
        {
            _gm = GM.I;
            _im = IM.I;
            _im.OnMouseDown += _OnStrokeStart;
            _im.OnMouseUp += _OnStrokeEnd;

            _pathTracer = new PathTracer();
            _pathTracer.OnTraceComplete += () => Debug.Log("Trace complete!");
            _pathTracer.OnWrongDirection += () => Debug.LogWarning("Wrong direction!");
            _pathTracer.OnCorrectStrokeStart += () => Debug.Log("Stroke starts correctly!");
            _pathTracer.OnIncorrectStrokeStart += () => Debug.LogWarning("Stroke starts incorrectly!");
            _pathTracer.OnCorrectStrokeEnd += () => Debug.Log("Stroke ends correctly!");
            _pathTracer.OnIncorrectStrokeEnd += () => Debug.LogWarning("Stroke ends incorrectly!");
        }

        void Start()
        {
            _strokes = DrawUtil.DrawLetter(letter, transform, drawConfig.settings);
            _pathTracer.Init(_strokes);
        }

        private void _OnStrokeStart(Vector2 mousePosition)
        {
            _pathTracer.BeginStroke(mousePosition);
            _im.OnMouseMove += _OnTrace;
        }

        private void _OnStrokeEnd(Vector2 mousePosition)
        {
            _pathTracer.EndStroke(mousePosition);
            _im.OnMouseMove -= _OnTrace;
        }

        private void _OnTrace(Vector2 mousePosition)
        {
            _pathTracer.Trace(mousePosition);
        }

        private void _ReStart()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }

            _strokes = DrawUtil.DrawLetter(letter, transform, drawConfig.settings);
            _pathTracer.Init(_strokes);
        }

        void OnDrawGizmos()
        {
            if (_strokes != null)
            {
                Gizmos.color = Color.blue;

                foreach (var stroke in _strokes)
                {
                    foreach (var vertex in stroke)
                    {
                        Gizmos.DrawSphere(vertex, _tracePointSize);
                    }
                }
            }
        }
    }
}