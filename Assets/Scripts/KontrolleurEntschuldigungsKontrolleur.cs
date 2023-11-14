using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KontrolleurEntschuldigungsKontrolleur : MonoBehaviour
{
    [SerializeField] private KontrolleurEntschuldigung entschuldigungs;
    [SerializeField] private RandomTextPlacement textPLacer;
    // Start is called before the first frame update

    private int entschuldigungsId = 0;
    
    void Start()
    {
        IceController.OnVerspaedungKassiert += IceControllerOnOnVerspaedungKassiert; 
    }

    private void OnDestroy()
    {
        IceController.OnVerspaedungKassiert -= IceControllerOnOnVerspaedungKassiert;
    }

    private void IceControllerOnOnVerspaedungKassiert(int verspaedunginsegunda)
    {
        textPLacer.PlaceTextSomewhere(entschuldigungs.entschuldigungen[entschuldigungsId++]);
    }
}
