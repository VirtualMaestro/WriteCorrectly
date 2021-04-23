using System;
using System.Collections;
using Client.Scripts.Common;
using Client.Scripts.Ds;
using Client.Scripts.Popups;
using UnityEngine;

namespace Client.Scripts
{
    public class GM : Singleton<GM>
    {
        [SerializeField]
        private AppSettings appSettings;

        [SerializeField] 
        private GameObject messageWindow;
        
        public event Action<Letter> OnDrawingLetterStart;
        public event Action OnDrawingLetterEnd;

        public AppSettings AppSettings => appSettings;

        private void Start()
        {
            var letter = appSettings.Letters[0];
            OnDrawingLetterStart?.Invoke(letter);
        }

        public void IncorrectStrokeStart()
        {
            var popupWindow = _ShowPopup();
            popupWindow.ShowIncorrectStartPosition();
        }

        public void IncorrectStrokeEnd()
        {
            var popupWindow = _ShowPopup();
            popupWindow.ShowIncorrectEndPosition();
        }

        public void WrongDirection()
        {
            var popupWindow = _ShowPopup();
            popupWindow.ShowWrongDirection();
        }

        public void TraceComplete()
        {
            var popupWindow = _ShowPopup();
            popupWindow.ShowCongratulation();
        }

        private PopupWindow _ShowPopup()
        {
            messageWindow.SetActive(true);
            var popupComponent = messageWindow.GetComponent<PopupWindow>();
            popupComponent.OnTryAgain += _OnRestart;
            
            OnDrawingLetterEnd?.Invoke();
            return popupComponent;
        }

        private void _HidePopup()
        {
            messageWindow.GetComponent<PopupWindow>().OnTryAgain -= _OnRestart;
            messageWindow.SetActive(false);
        }

        private void _OnRestart()
        {
            _HidePopup();
            
            StartCoroutine(_StartDrawingLetter());
        }

        IEnumerator _StartDrawingLetter()
        {
            yield return new WaitForEndOfFrame();
            
            var letter = appSettings.Letters[0];
            OnDrawingLetterStart?.Invoke(letter);
        }
    }
}