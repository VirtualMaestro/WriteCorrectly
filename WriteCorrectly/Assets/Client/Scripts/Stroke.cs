using System;
using UnityEngine;

namespace Client.Scripts
{
    [Serializable]
    public class Stroke
    {
        public Vector2[] points;

        public Vector3[] GetVertices()
        {
            var vertices = new Vector3[points.Length];
            for (var i = 0; i < points.Length; i++)
            {
                vertices[i] = points[i];
            }

            return vertices;
        }
    }
}