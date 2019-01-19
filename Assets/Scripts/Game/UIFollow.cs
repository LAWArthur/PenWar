using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class UIFollow : MonoBehaviour {

    public GameObject UI;
    public Vector2 offset;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);
        UI.GetComponent<RectTransform>().position = pos + offset;
	}
}
