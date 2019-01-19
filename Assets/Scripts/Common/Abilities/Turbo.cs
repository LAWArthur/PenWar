using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Abilities
{
    class Turbo : Ability
    {
        public Vector3 oldPos, pos;
        bool OnGetting = false;

        public Turbo(int usageWeight, KeyCode key, int level) : base(usageWeight, key, level)
        {
            Status = AbilityStatus.Cooling;
            CoolTemp = 1;
            StateTemp = CoolTemp;
        }

        public override void OnRounding()
        {
            if (Status == AbilityStatus.Cooling && StateTemp == 0)
            {
                Status = AbilityStatus.Ready;

            }
            StateTemp--;
        }

        public override bool OnUsing(GameObject gameObject)
        {
            if (AIUsageWeight == 0)
            {
                if (Status == AbilityStatus.Ready && Input.GetKey(Key) && Input.GetKey(Basic))
                {
                    oldPos = Input.mousePosition;
                    Status = AbilityStatus.Cooling;
                    StateTemp = CoolTemp;
                    return true;
                }
            }
            else if (Mathf.FloorToInt(Random.value * AIUsageWeight) == 0 && Status == AbilityStatus.Ready && Physics.OverlapSphere(gameObject.transform.position, 0.8f + 0.2f * Level).Length > 1)
            {
                Status = AbilityStatus.Cooling;
                StateTemp = CoolTemp;
                return true;
            }
            return false;
        }

        public override IEnumerator Coroutine(GameObject gameObject)
        {
            ExternalFunctionality.Toggle(ref OnGetting);
            yield return new WaitUntil(() => Input.GetKeyUp(Basic));
            Activate(gameObject);

            ExternalFunctionality.Toggle(ref OnGetting);
            gameObject.GetComponent<Controller>().StartCoroutine(gameObject.GetComponent<Controller>().Coroutine());
        }

        public override void Activate(GameObject gameObject)
        {
            foreach(GameObject i in Camera.main.GetComponent<main>().rounds)
            {
                Object.Instantiate(Resources.Load<GameObject>("Prefabs/Particles/Turbo"), new Vector3(0f, 0.5f, 0f), Quaternion.identity);
                i.GetComponent<Rigidbody>().AddForce((new Vector3(0f, i.transform.position.y, 0f) - i.transform.position).normalized * (20f + Level));
            }
        }

        public override IEnumerator AI(GameObject gameObject)
        {
            foreach (Collider i in Physics.OverlapSphere(gameObject.transform.position, 0.8f + 0.2f * Level))
            {
                if (i.gameObject.GetComponent<Controller>())
                {
                    Vector2 ori = Random.insideUnitCircle * 0.05f;
                    pos = new Vector3(ori.x, 0f, ori.y) + i.transform.position;
                }
            }
            Activate(gameObject);
            yield return new WaitForSeconds(1f);
            gameObject.GetComponent<Controller>().StartCoroutine(gameObject.GetComponent<Controller>().Coroutine());
        }

        public override void OnActivated(GameObject gameObject)
        {
            if (OnGetting)
            {
                Shared.SpectPoint = Vector3.up * 3;
            }
            else
            {
                Shared.SpectPoint = gameObject.transform.position + Vector3.up;
            }
        }
    }
}


