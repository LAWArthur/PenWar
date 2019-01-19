using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common.Abilities;
using Common.Products;
using UnityEngine.Analytics;
using Common;

class Player : Controller {
    
    public bool rounding = false, onAbility = false, onAC = false;

    public override void Start()
    {
        base.Start();
        abilitiesPool = GameObject.Find("Global").GetComponent<Global>().AbilityPool;
        productsDock = new List<Product>();
        Destroy(GameObject.Find("Global"));
    }

    public override void Update()
    {
        base.Update();
        if (active)
        {
            if (rounding)
            {
                foreach(Ability i in abilitiesPool)
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
                    AnalyticsEvent.Custom("Ability-" + current.GetType().Name, new Dictionary<string, object>
                    {
                        {"player", PlayerPrefs.GetString("Settings.Name", "玩家") }, 
                        {"Level", current.Level }
                    });
                    onAC = true;
                }
            }
        }
    }

    public override void Activate()
    {
        base.Activate();
        rounding = true;
        onAbility = false;
        onAC = false;
    }
}
