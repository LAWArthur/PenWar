using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;
using Common.Abilities;
using Common.Products;
using Common.Effects;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

//Base class for Player & AI
[DisallowMultipleComponent]
class Controller : NetworkBehaviour {
    
    protected GameObject channel;
    protected Ability current;
    [HideInInspector]
    public bool active = false;
    public List<Ability> abilitiesPool;
    public List<Product> productsDock;
    public List<Effect> effects;
    // Use this for initialization
    public virtual void Start () {
        productsDock = new List<Product>();
        effects = new List<Effect>();
        channel = GameObject.Find("Channel");
        gameObject.transform.Find("Canvas").transform.Find("Text").GetComponent<Text>().text = name;
        abilitiesPool = new List<Ability>
        {
            new Move(0, (KeyCode)PlayerPrefs.GetInt("Settings.Keys.Movement"))
        };
    }
	
	// Update is called once per frame
	public virtual void Update () {
        //Test alive
        if (transform.position.y < -1f)
        {
            channel.GetComponent<Text>().text = name + "被" + Camera.main.GetComponent<main>().rounds.Peek().name + "淘汰了\n" + channel.GetComponent<Text>().text;
            if (Camera.main.GetComponent<main>().rounds.Peek().tag == "Local") Camera.main.GetComponent<main>().Point++;
            for (int i = 0; i < Camera.main.GetComponent<main>().rounds.Count; i++)
            {
                GameObject pen = Camera.main.GetComponent<main>().rounds.Dequeue();
                if (pen == gameObject) continue;
                Camera.main.GetComponent<main>().rounds.Enqueue(pen);
            }
            foreach(Product i in productsDock)
            {
                i.Destroy();
            }
            gameObject.SetActive(false);
        }
        foreach(Effect i in effects)
        {
            i.Activate();
        }
        if (active)
        {
            current.OnActivated(gameObject);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Controller>())
        {
            transform.Find("Crash").GetComponent<AudioSource>().time = 0.3f;
            transform.Find("Crash").GetComponent<AudioSource>().Play();
        }
    }

    public virtual IEnumerator Coroutine()
    {
        float start = Time.time;
        yield return new WaitUntil(delegate ()
        {
            if (Time.time - start > 3) return true;
            foreach(GameObject i in Camera.main.GetComponent<main>().rounds)
            {
                if (i.GetComponent<Rigidbody>().IsSleeping()) continue;
                return false;
            }
            return true;
        });
        active = false;
    }

    public virtual void Activate()
    {
        active = true;
        current = new Ability(0, KeyCode.None, 0);
        foreach (Effect i in effects)
        {
            i.LifeTime--;
        }
    }
}
