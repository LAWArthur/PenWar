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

class Multi_controller : Controller {
    public bool rounding = false, onAbility = false, onAC = false;
    public override void Start()
    {
        productsDock = new List<Product>();
        effects = new List<Effect>();
        channel = GameObject.Find("Channel");
        gameObject.transform.Find("Canvas").transform.Find("Text").GetComponent<Text>().text = name;
        abilitiesPool = new List<Ability>
        {
            new Move(0, (KeyCode)PlayerPrefs.GetInt("Settings.Keys.Movement"))
        };
        if (isLocalPlayer)
            ((Multiplayer)NetworkManager.singleton).pens.Add(gameObject);
        abilitiesPool = GameObject.Find("Global").GetComponent<Global>().AbilityPool;
        productsDock = new List<Product>();
        Destroy(GameObject.Find("Global"));
        Debug.Log(abilitiesPool);
    }

    public override void Update()
    {

        //Test alive
        if (transform.position.y < -1f)
        {
            channel.GetComponent<Text>().text = name + "被淘汰了\n" + channel.GetComponent<Text>().text;
            for (int i = 0; i < ((Multiplayer)NetworkManager.singleton).rounds.Count; i++)
            {
                GameObject pen = ((Multiplayer)NetworkManager.singleton).rounds.Dequeue();
                if (pen == gameObject) continue;
                ((Multiplayer)NetworkManager.singleton).rounds.Enqueue(pen);
            }
            foreach (Product i in productsDock)
            {
                i.Destroy();
            }
            gameObject.SetActive(false);
        }

        foreach (Effect i in effects)
        {
            i.Activate();
        }
        if (active)
        {
            current.OnActivated(gameObject);
        }
        if (isLocalPlayer && active)
        {
            if (rounding)
            {
                foreach (Ability i in abilitiesPool)
                {
                    i.OnRounding();
                }
                foreach (Product i in productsDock)
                {
                    i.OnRounding();
                }
                rounding = false;
            }
            if (!onAbility)
            {
                foreach (Ability i in abilitiesPool)
                {
                    if (i.OnUsing(gameObject))
                    {
                        onAbility = true;
                        current = i;
                        break;
                    }
                }
            }
            else
            {
                if (!onAC)
                {
                    StartCoroutine(current.Coroutine(gameObject));
                    onAC = true;
                }
            }
        }
    }

    public override void Activate()
    {
        active = true;
        current = new Ability(0, KeyCode.None, 0);
        foreach (Effect i in effects)
        {
            i.LifeTime--;
        }
        rounding = true;
        onAbility = false;
        onAC = false;
    }

    public override IEnumerator Coroutine()
    {
        float start = Time.time;
        yield return new WaitUntil(delegate ()
        {
            if (Time.time - start > 3) return true;
            foreach (GameObject i in ((Multiplayer)NetworkManager.singleton).rounds)
            {
                if (i.GetComponent<Rigidbody>().IsSleeping()) continue;
                return false;
            }
            return true;
        });
        //active = false;
        ((Multiplayer)NetworkManager.singleton).Next();
    }
}
