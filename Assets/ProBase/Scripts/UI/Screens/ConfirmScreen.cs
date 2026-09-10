using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProBase
{
    public class ConfirmScreen : UiScreenWithResult<ConfirmArgs, bool>
    {
        [SerializeField] private TMP_Text _messageLabel;
        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _declineButton;

        protected override void OnArgsReceived()
        {
            _messageLabel.text = Args.Message;
        }

        protected override void OnOpened()
        {
            _confirmButton.onClick.AddListener(OnConfirmClicked);
            _declineButton.onClick.AddListener(OnDeclineClicked);
        }

        protected override void OnClosing()
        {
            _confirmButton.onClick.RemoveListener(OnConfirmClicked);
            _declineButton.onClick.RemoveListener(OnDeclineClicked);
        }

        private void OnConfirmClicked()
        {
            SetResult(true);
        }

        private void OnDeclineClicked()
        {
            SetResult(false);
        }
    }
}
