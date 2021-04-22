using System.Collections.Generic;
using UnityEngine;

namespace Client.Scripts
{
    [CreateAssetMenu(menuName = "Create letter", fileName = "Letter")]
    public class Letter : ScriptableObject
    {
        public Stroke[] strokes;

        // public List<Vector3> GetVertices()
        // {
        //     var list = new List<Vector3>();
        //     
        //     foreach (var stroke in strokes)
        //     {
        //         foreach (var point in stroke.points)
        //         {
        //             list.Add(new Vector3(point.x, point.y));
        //         }
        //     }
        //      
        //     return list;
        // }
    }
}