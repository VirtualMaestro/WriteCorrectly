using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.Popups
{
    public class PopupWindow : MonoBehaviour
    {
        public event Action OnTryAgain;
        
        [SerializeField]
        private Text label;
        [SerializeField]
        private Text message;

        public void ShowCongratulation()
        {
            label.text = "Congratulation!";
            message.text = "You are rock!";
        }

        public void ShowIncorrectStartPosition()
        {
            label.text = "Oops!";
            message.text = "You've started from wrong position!";
        }

        public void ShowIncorrectEndPosition()
        {
            label.text = "Oops!";
            message.text = "You've finished at wrong position!";
        }

        public void ShowWrongDirection()
        {
            label.text = "Oops!";
            message.text = "You moved to wrong direction!";
        }

        public void OnTryButtonClick()
        {
            Debug.Log("Try button click");
            OnTryAgain?.Invoke();
        }
    }
}