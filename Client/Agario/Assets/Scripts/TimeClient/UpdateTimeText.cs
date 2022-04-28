using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateTimeText : MonoBehaviour
{
    [SerializeField] private TMP_Text textObject;
    
    public delegate void SetTime(string timeString);
    
    public void SetText(string text)
    {
        textObject.text = text;
    }
}
