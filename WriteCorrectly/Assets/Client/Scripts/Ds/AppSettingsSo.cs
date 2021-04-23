using System.Collections.Generic;
using UnityEngine;

namespace Client.Scripts.Ds
{
    [CreateAssetMenu(menuName = "Create app settings", fileName = "AppSettingsSO")]
    public class AppSettingsSo : ScriptableObject
    {
        public float mouseSensitivity = 0.1f;
        public float tracePointSize = 0.5f;
        public float actionPointSize = 0.5f;
        // public drawSeFulFil
        public List<Letter> Letters;
    }
}