using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Points : MonoBehaviour {
    public int Point
    {
        get
        {
            return PlayerPrefs.GetInt("Abilities.General.Point");
        }
        set
        {
            PlayerPrefs.SetInt("Abilities.General.Point", value);
            transform.Find("Text").GetComponent<Text>().text = "" + value;
        }
    }

	// Use this for initialization
	void Start () {
        Point = PlayerPrefs.GetInt("Abilities.General.Point");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
