using UnityEngine;

namespace Client.Scripts.Ds
{
    [CreateAssetMenu(menuName = "DrawSettings", fileName = "DrawSettingsSO")]
    public class DrawSettingsSo : ScriptableObject
    {
        public DrawSettings settings;
    }
}