using UnityEngine;

namespace Client.Scripts.Ds
{
    [CreateAssetMenu(menuName = "Create draw settings", fileName = "DrawSettingsSO")]
    public class DrawSettings : ScriptableObject
    {
        public float lineWidth = 0.1f;
        public Color lineColor = Color.black;
        public int capVertices = 5;
        public int cornerVertices = 5;
        public Material drawMaterial;
    }
}