using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverBlackHole : MonoBehaviour
{

    bool hovering = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnGUI()
    {
        if (hovering == true)
        {

            GUI.Label(new Rect(290, Screen.height - 200, 200, 80), "deals 250 damage to the boss \n hotkey:W");
        }
    }

    void entreiaki()
    {

        hovering = true;
    }
    void saiaki()
    {

        hovering = false;
    }
}
