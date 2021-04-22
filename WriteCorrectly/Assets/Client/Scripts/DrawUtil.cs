using System.Collections.Generic;
using UnityEngine;

namespace Client.Scripts
{
    public static class DrawUtil
    {
        public static List<Vector3[]> DrawLetter(Letter letter, Transform parent = null, DrawSettings settings = null)
        {
            var result = new List<Vector3[]>();
            
            foreach (var stroke in letter.strokes)
            {
                result.Add(DrawLine(stroke.GetVertices(), parent, settings));
            }

            return result; 
        }
        
        public static Vector3[] DrawLine(Vector3[] vertices, Transform parent = null, DrawSettings settings = null)
        {
            vertices = LineSmoother.SmoothLine(vertices, 0.8f);
            var lineRenderer = _CreateLineRenderer(parent, settings);

            foreach (var vertex in vertices)
            {
                _DrawLineVertex(vertex, lineRenderer);
            }

            return vertices;
        }
        
        private static LineRenderer _CreateLineRenderer(Transform parent = null, DrawSettings settings = null)
        {
            var line = new GameObject("Line"); 
            if (parent != null) 
                line.transform.SetParent(parent, true);
            
            var lineRenderer = line.AddComponent<LineRenderer>();
            lineRenderer.positionCount = 0;
            lineRenderer.startWidth = settings?.lineWidth ?? 0.1f;
            lineRenderer.endWidth = settings?.lineWidth ?? 0.1f;
            lineRenderer.startColor = settings?.lineColor ?? Color.red;
            lineRenderer.endColor = settings?.lineColor ?? Color.red;
            lineRenderer.useWorldSpace = false;
            lineRenderer.numCapVertices = settings?.lineCapVertices ?? 5;
            lineRenderer.numCornerVertices = settings?.lineCapVertices ?? 5;
            lineRenderer.material = settings?.drawMaterial ? settings?.drawMaterial : null;

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