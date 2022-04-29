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
        }
    }
}