using PathCreation;
using UnityEngine;

namespace Client.Scripts.Ds
{
    [CreateAssetMenu(menuName = "Create letter", fileName = "Letter")]
    public class Letter : ScriptableObject
    {
        public Stroke[] strokes;
        public GameObject letter;

        public Stroke[] GetStrokes()
        {
            var path = letter.GetComponent<PathCreator>().path;
            var strokess = new Stroke[1];
            var stroke = new Stroke();
            stroke.points = new Vector2[path.NumPoints];
            var points = path.localPoints;
            
            for (int i = 0; i < points.Length; i++)
            {
                stroke.points[i] = points[i] * 3;
            }

            strokess[0] = stroke;

            return strokess;
        }
    }
}