using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject.Find("Cube").GetComponent<Rigidbody>().AddRelativeTorque(GameObject.Find("Cube").transform.up * 50f);
	}
	
	// Update is called once per frame
	void Update () {
    }
}
