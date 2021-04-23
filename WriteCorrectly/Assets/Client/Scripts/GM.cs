using System;
using Client.Scripts.Common;
using Client.Scripts.Ds;
using UnityEngine;

namespace Client.Scripts
{
    public class GM : Singleton<GM>
    {
        public AppSettings AppSettings;
        
        public event Action<Letter> OnDrawingLetterStart;
        public event Action OnDrawingLetterEnd;
        public event Action OnRestart;

        private void Start()
        {
            // IM.I.OnMouseDown += _OnMouseDown;
            // IM.I.OnMouseUp += _OnMouseUp;
            // IM.I.OnMouseMove += _OnMouseMove;

            var letter = AppSettings.Letters[0];
            OnDrawingLetterStart?.Invoke(letter);
        }

        public void ShowChooseLetterPopup()
        {
            
        }

        public void ShowWrongDrawingPopup()
        {
            
        }

        public void Restart()
        {
            OnRestart?.Invoke();
        }

        private void _OnMouseDown(Vector2 mousePosition)
        {
            // Debug.Log("GM: OnMouseDown");
        }
        
        private void _OnMouseUp(Vector2 mousePosition)
        {
            // Debug.Log("GM: _OnMouseUp");
        }

        private void _OnMouseMove(Vector2 mousePosition)
        {
            // Debug.Log($"GM: _OnMouseMove: {mousePosition}");
        }
    }
}