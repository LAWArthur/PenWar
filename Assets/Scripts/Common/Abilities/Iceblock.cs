using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Abilities
{
    class Iceblock : Ability
    {
        public GameObject selected;
        bool OnGetting = false;

        public Iceblock(int usageWeight, KeyCode key, int level) : base(usageWeight, key, level)
        {
            Status = AbilityStatus.Cooling;
            CoolTemp = 2;
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
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    Cursor.SetCursor(CursorIcon, Vector2.zero, CursorMode.Auto);
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

            yield return new WaitUntil(() =>
            {
                foreach(GameObject i in Camera.main.GetComponent<main>().rounds)
                {
                    i.GetComponentInChildren<Renderer>().material.color = Color.white;
                }
                Ray point = Camera.main.ScreenPointToRay(ExternalFunctionality.MousePosition());
                RaycastHit raycastHit;
                if(Physics.Raycast(point, out raycastHit) && raycastHit.transform.GetComponent<Controller>())
                {
                    raycastHit.transform.GetComponentInChildren<Renderer>().material.color = new Color(0.8f, 0.8f, 1.0f);
                    if (Input.GetKeyDown(Basic))
                    {
                        selected = raycastHit.transform.gameObject;
                        return true;
                    }
                }
                return false;
            });

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Activate(gameObject);
            ExternalFunctionality.Toggle(ref OnGetting);
            gameObject.GetComponent<Controller>().StartCoroutine(gameObject.GetComponent<Controller>().Coroutine());
        }

        public override void Activate(GameObject gameObject)
        {
            selected.GetComponent<Controller>().effects.Add(new Effects.Frozen(selected, Level + 1, 100));
        }

        public override IEnumerator AI(GameObject gameObject)
        {
            foreach (Collider i in Physics.OverlapSphere(gameObject.transform.position, 0.8f + 0.2f * Level))
            {
                if (i.gameObject.GetComponent<Controller>() != null)
                {
                    Vector2 ori = Random.insideUnitCircle * 0.05f;
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

