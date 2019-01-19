using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class init : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Cursor.SetCursor(null, new Vector2(0f, 0f), CursorMode.Auto);
    }
	
	// Update is called once per frame
	void Update () {
        if (PlayerPrefs.GetInt("Settings.Fullscreen") == 1) Screen.fullScreenMode = FullScreenMode.FullScreenWindow; else Screen.fullScreenMode = FullScreenMode.Windowed;
        Debug.DrawRay(gameObject.transform.position, Vector3.up * 90f, Color.red, 10f);
    }

    string key, value;
    //void OnGUI()
    //{
    //    key = GUILayout.TextField(key);
    //    value = GUILayout.TextField(value);
    //    if (GUILayout.Button("Submit"))
    //    {
    //        PlayerPrefs.SetInt(key, int.Parse(value));
    //        PlayerPrefs.Save();
    //    }
    //}
}
