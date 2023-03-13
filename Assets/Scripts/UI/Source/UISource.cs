using TMPro;
using UnityEngine;

namespace MineAndRefact.Core.UI
{
    public class UISource : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _recoveryDurationText;


        private void Awake()
        {
            if (_recoveryDurationText == null)
                throw new System.ArgumentNullException(nameof(_recoveryDurationText));

            _recoveryDurationText.gameObject.SetActive(false);
        }


        public void ShowRecoveryDuration()
        {
            _recoveryDurationText.gameObject.SetActive(true);
        }

        public void HideRecoveryDuration()
        {
            _recoveryDurationText.gameObject.SetActive(false);
        }

        public void DisplayRecoveryDuration(float recoveryDuration)
        {
            _recoveryDurationText.text = System.Math.Round(recoveryDuration, 1).ToString();
        }
    }
}