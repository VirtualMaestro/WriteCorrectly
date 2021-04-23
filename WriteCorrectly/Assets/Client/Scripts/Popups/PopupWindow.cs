using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.Popups
{
    public class PopupWindow : MonoBehaviour
    {
        public event Action OnTryAgain;
        public event Action OnNext;
        
        [SerializeField]
        private Text label;
        [SerializeField]
        private Text message;

        [SerializeField]
        private GameObject tryBtn;

        [SerializeField]
        private GameObject nextBtn;
        
        public void ShowCongratulation()
        {
            label.text = "Congratulation!";
            message.text = "You are rock!";
            
            tryBtn.SetActive(true);
            nextBtn.SetActive(true);
        }

        public void ShowIncorrectStartPosition()
        {
            label.text = "Oops!";
            message.text = "You've started from wrong position!";
            
            tryBtn.SetActive(true);
            nextBtn.SetActive(false);
        }

        public void ShowIncorrectEndPosition()
        {
            label.text = "Oops!";
            message.text = "You've finished at wrong position!";
            
            tryBtn.SetActive(true);
            nextBtn.SetActive(false);
        }

        public void ShowWrongDirection()
        {
            label.text = "Oops!";
            message.text = "You moved to wrong direction!";
            
            tryBtn.SetActive(true);
            nextBtn.SetActive(false);
        }

        public void OnTryButtonClick()
        {
            Debug.Log("Try button click");
            OnTryAgain?.Invoke();
        }
        
        public void OnNextButtonClick()
        {
            Debug.Log("Next button click");
            OnNext?.Invoke();
        }
    }
}