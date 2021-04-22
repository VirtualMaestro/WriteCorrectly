using UnityEngine;

namespace Client.Scripts
{
    [CreateAssetMenu(menuName = "DrawSettings", fileName = "DrawSettingsSO")]
    public class DrawSettingsSo : ScriptableObject
    {
        public DrawSettings settings;
    }
}