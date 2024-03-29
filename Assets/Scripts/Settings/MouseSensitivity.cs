﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivity : MonoBehaviour {

	public void OnValueChange()
    {
        PlayerPrefs.SetInt("Settings.MouseSensitivity", (int)GetComponent<Slider>().value);
    }
    void Start()
    {
        GetComponent<Slider>().value = PlayerPrefs.GetInt("Settings.MouseSensitivity", 100);
    }
    void Update()
    {
        transform.Find("Text").GetComponent<Text>().text = (int)GetComponent<Slider>().value + "%";
    }
}
