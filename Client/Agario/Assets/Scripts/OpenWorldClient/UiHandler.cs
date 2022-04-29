using System;
using System.Collections;
using System.Collections.Generic;
using OpenWordClient;
using TMPro;
using UnityEngine;

namespace OpenWordClient
{
    public class UiHandler : MonoBehaviour
    {
        [SerializeField] private TMP_Text outputText;
        [SerializeField] private TMP_Text errorText;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private ServerCommunicator serverCommunicator;

        public string GetInputFieldText()
        {
            var text = inputField.text;
            inputField.text = "";
            return text;
        }

        private void SetText(string s)
        {
            outputText.text = s;
        }

        private void Start()
        {
            serverCommunicator.OnUpdateText += SetText;
            serverCommunicator.OnGetUserInput += GetInputFieldText;
            serverCommunicator.OnError += PrintError;
        }

        private void PrintError(string s)
        {
            errorText.text = s;
            StartCoroutine(ResetErrorText());
        }

        private IEnumerator ResetErrorText()
        {
            yield return new WaitForSeconds(2);
            errorText.text = "";
        }
    }
}