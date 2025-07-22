using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject OptionPanel;
    
    void Start()
    {
        
    }

    void Update()
    {
        if (OptionPanel.activeSelf == true)
            return;
        
    }
}
