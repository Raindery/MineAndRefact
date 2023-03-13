using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MineAndRefact.Core.UI
{
    public class UISpot : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _amountRequiredResourceText;
        [SerializeField] private TextMeshProUGUI _recyclingProgressText;


        private void Awake()
        {
            if (_amountRequiredResourceText == null)
                throw new System.ArgumentNullException(nameof(_amountRequiredResourceText));

            if (_recyclingProgressText == null)
                throw new System.ArgumentNullException(nameof(_recyclingProgressText));
        }


        public void ShowAmountRequired()
        {
            _amountRequiredResourceText.gameObject.SetActive(true);
        }

        public void UpdateAmountResource(int amount)
        {
            _amountRequiredResourceText.text = amount.ToString();
        }

        public void HideAmountRequired()
        {
            _amountRequiredResourceText.gameObject.SetActive(false);
        }

        public void ShowRecyclingProgress()
        {
            _recyclingProgressText.gameObject.SetActive(true);
        }

        public void UpdateRecyclingProgress(float progress)
        {
            _recyclingProgressText.text = System.Math.Round(progress, 1).ToString();
        }

        public void HideRecyclingProgress()
        {
            _recyclingProgressText.gameObject.SetActive(false);
        }


    }
}

