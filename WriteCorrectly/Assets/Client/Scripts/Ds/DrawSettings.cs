using System;
using UnityEngine;

namespace Client.Scripts.Ds
{
    [Serializable]
    public class DrawSettings
    {
        public float lineSeparationDistance = 0.2f;
        public float lineWidth = 0.1f;
        public Color lineColor = Color.black;
        public int lineCapVertices = 5;
        public Material drawMaterial;
    }
}