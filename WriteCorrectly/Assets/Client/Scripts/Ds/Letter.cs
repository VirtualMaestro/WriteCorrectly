using UnityEngine;

namespace Client.Scripts.Ds
{
    [CreateAssetMenu(menuName = "Create letter", fileName = "Letter")]
    public class Letter : ScriptableObject
    {
        public Stroke[] strokes;
    }
}