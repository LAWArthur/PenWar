using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class KeyTog : MonoBehaviour {

    public string storageName;
    Toggle toggle;
    Text key;

	// Use this for initialization
	public void Start () {
        toggle = GetComponent<Toggle>();
        key = transform.Find("Key").GetComponent<Text>();
        key.text = Enum.GetName(typeof(KeyCode), (KeyCode)PlayerPrefs.GetInt("Settings.Keys." + storageName, (int)KeyCode.Mouse0));
	}
	
	// Update is called once per frame
	void Update () {
        if (toggle.isOn)
        {
            toggle.interactable = false;
            foreach(KeyCode i in Enum.GetValues(typeof(KeyCode))){
                if (Input.GetKeyDown(i))
                {
                    toggle.isOn = false;
                    toggle.interactable = true;
                    PlayerPrefs.SetInt("Settings.Keys." + storageName, (int)i);
                    key.text = Enum.GetName(typeof(KeyCode), i);
                    break;
                }
            }
            
        }
	}
}
