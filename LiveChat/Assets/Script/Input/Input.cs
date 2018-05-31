using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Input : MonoBehaviour {
    public Client client;
    private TouchScreenKeyboard mobileKeys;

    void OnInputEvent()
    {
        mobileKeys = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false);
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (UnityEngine.Input.GetKeyDown("return") || mobileKeys.done)
        {
            client.OnSendButton();
        }

    }
}
