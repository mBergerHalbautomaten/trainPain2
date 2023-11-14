using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VerspaetungsKontrolleur : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textLabel;
    private int totalDelay = 0;
    
    private void Awake()
    {
        IceController.OnVerspaedungKassiert += IceControllerOnOnVerspaedungKassiert;
    }

    private void IceControllerOnOnVerspaedungKassiert(int verspaedunginsegunda)
    {
        totalDelay += verspaedunginsegunda;
        TimeSpan t = TimeSpan.FromSeconds(totalDelay);
        string delayText = string.Format("{0:D2}h:{1:D2}m:{2:D2}s", 
            t.Hours, 
            t.Minutes, 
            t.Seconds);
        textLabel.text = delayText;
        if (totalDelay >= 5)
        {
            textLabel.color = Color.red;
        }
    }
}
