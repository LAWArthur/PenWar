using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportParticle : MonoBehaviour {
    public float distance;

	// Use this for initialization
	void Start () {
        ParticleSystem.MainModule main = GetComponent<ParticleSystem>().main;
        main.startSpeed = distance;
        GetComponent<ParticleSystem>().Play();
        
        StartCoroutine(Coroutine());
    }

    IEnumerator Coroutine()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
