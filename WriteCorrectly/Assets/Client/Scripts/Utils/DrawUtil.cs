using System.Collections.Generic;
using Client.Scripts.Ds;
using UnityEngine;

namespace Client.Scripts.Utils
{
    public static class DrawUtil
    {
        public static List<Vector3[]> DrawLetter(Letter letter, DrawSettings settings, Transform parent = null, float smoothness = 0.6f)
        {
            var result = new List<Vector3[]>();
            
            // foreach (var stroke in letter.strokes)
            foreach (var stroke in letter.GetStrokes())
            {
                result.Add(DrawLine(stroke.GetVertices(), settings, parent, smoothness));
            }

            return result; 
        }
        
        public static Vector3[] DrawLine(Vector3[] vertices, DrawSettings settings, Transform parent = null, float smoothness = 0.6f)
        {
            vertices = LineSmootherUtil.SmoothLine(vertices, smoothness);
            var lineRenderer = _CreateLineRenderer(settings, parent);

            foreach (var vertex in vertices)
            {
                _DrawLineVertex(vertex, lineRenderer);
            }

            return vertices;
        }
        
        private static LineRenderer _CreateLineRenderer(DrawSettings settings, Transform parent = null)
        {
            var line = new GameObject("Line"); 
            if (parent != null) 
                line.transform.SetParent(parent, true);
            
            var lineRenderer = line.AddComponent<LineRenderer>();
            lineRenderer.positionCount = 0;
            lineRenderer.startWidth = settings.lineWidth;
            lineRenderer.endWidth = settings.lineWidth;
            lineRenderer.startColor = settings.lineColor;
            lineRenderer.endColor = settings.lineColor;
            lineRenderer.useWorldSpace = false;
            lineRenderer.numCapVertices = settings.capVertices;
            lineRenderer.numCornerVertices = settings.cornerVertices;
            lineRenderer.material = settings.drawMaterial;

            return lineRenderer;
        }
        
        private static void _DrawLineVertex(Vector3 point, LineRenderer lineRenderer)
        {
            var positionCount = lineRenderer.positionCount;
            positionCount++;
            lineRenderer.positionCount = positionCount;
            lineRenderer.SetPosition(positionCount-1, point);
        }
    }
}