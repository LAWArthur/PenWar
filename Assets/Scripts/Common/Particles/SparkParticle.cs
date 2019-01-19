using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkParticle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider c)
    {
        c.transform.parent?.gameObject?.GetComponent<Controller>()?.effects?.Add(new Common.Effects.Fire(c.transform.parent.gameObject, 2));
    }
}
