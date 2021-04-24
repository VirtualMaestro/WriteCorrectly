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
        
        public event Action OnDrawingLetterStart;
        public event Action OnDrawingLetterEnd;

        public AppSettings AppSettings => appSettings;

        private int _curLetterIndex;

        public Letter GetCurrentLetter => appSettings.Letters[_curLetterIndex];

        private void Start()
        {
            OnDrawingLetterStart?.Invoke();
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
            popupComponent.OnNext += _OnNext;
            
            OnDrawingLetterEnd?.Invoke();
            return popupComponent;
        }

        private void _HidePopup()
        {
            messageWindow.GetComponent<PopupWindow>().OnTryAgain -= _OnRestart;
            messageWindow.GetComponent<PopupWindow>().OnNext -= _OnNext;
            messageWindow.SetActive(false);
        }

        private void _OnRestart()
        {
            _HidePopup();
            
            StartCoroutine(_StartDrawingLetter());
        }

        private void _OnNext()
        {
            _HidePopup();
            _curLetterIndex++;
            _curLetterIndex = _curLetterIndex % appSettings.Letters.Count;
            StartCoroutine(_StartDrawingLetter());
        }

        IEnumerator _StartDrawingLetter()
        {
            yield return new WaitForEndOfFrame();
            OnDrawingLetterStart?.Invoke();
        }
    }
}