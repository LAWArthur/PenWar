using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Abilities;

class AI : Controller {
    public override void Start()
    {
        base.Start();
        abilitiesPool = new List<Ability>
        {
            new Explosion(3, KeyCode.None, 1),
            new Transport(3, KeyCode.None, 1),
            new Hook(3, KeyCode.None),
            new Move(1, KeyCode.None)
        };
    }

    public override void Activate()
    {
        base.Activate();
        foreach (Ability i in abilitiesPool)
        {
            i.OnRounding();
        }
        StartCoroutine(Corout());
    }
    IEnumerator Corout()
    {
        yield return new WaitForSeconds(1f);
        foreach (Ability i in abilitiesPool)
        {
            if (i.OnUsing(gameObject))
            {
                StartCoroutine(i.AI(gameObject));
                current = i;
                break;
            }
        }
    }
}
