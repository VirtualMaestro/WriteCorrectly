using Client.Scripts.Ds;
using UnityEngine;

namespace Client.Scripts
{
    public class Entry : MonoBehaviour
    {
        [SerializeField]
        private AppSettings _appSettings;
        
        private void Awake()
        {
            GM.I.AppSettings = _appSettings;
        }

        private void Start()
        {
            Destroy(gameObject);
        }
    }
}