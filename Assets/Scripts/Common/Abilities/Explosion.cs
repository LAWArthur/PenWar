using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Abilities
{
    class Explosion : Ability
    {
        public Vector3 oldPos, pos;
        GameObject area;
        bool OnGetting = false;

        public Explosion(int usageWeight, KeyCode key, int level) : base(usageWeight, key, level)
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
                    area = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/AbilityArea"), gameObject.transform.position, Quaternion.Euler(90f, 0f, 0f));
                    area.GetComponent<SpriteRenderer>().color = new Color(1f / 255 * 253, 1f / 255 * 74, 1f / 255 * 2, 1f / 255 * 130);
                    area.transform.localScale = new Vector3(0.8f + 0.2f * Level, 0.8f + 0.2f * Level, 1f);
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
            yield return new WaitUntil(delegate ()
            {
                return oldPos == Input.mousePosition ? false : true;
            });
            yield return new WaitForSecondsRealtime(0.2f);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == area)
                {
                    pos = hit.point + Vector3.down * 0.05f;
                    Activate(gameObject);
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    pos = area.GetComponent<Collider>().ClosestPointOnBounds(hit.point) + Vector3.down * 0.05f;
                    Activate(gameObject);
                    yield return new WaitForSeconds(1f);
                }
            }
            
            ExternalFunctionality.Toggle(ref OnGetting);
            gameObject.GetComponent<Controller>().StartCoroutine(gameObject.GetComponent<Controller>().Coroutine());
        }

        public override void Activate(GameObject gameObject)
        {
            foreach(Collider i in Physics.OverlapSphere(pos, 0.1f + 0.02f * Level))
            {
                if(i.attachedRigidbody)
                    i.attachedRigidbody.AddExplosionForce(20f, pos, 0.1f + 0.02f * Level);
            }
            GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Particles/Explosion"), pos + Vector3.up * 0.3f, Quaternion.Euler(-90f, 0f, 0f));
            Object.Destroy(area);
        }

        public override IEnumerator AI(GameObject gameObject)
        {
            foreach(Collider i in Physics.OverlapSphere(gameObject.transform.position, 0.8f + 0.2f * Level))
            {
                if(i.gameObject.transform.parent?.GetComponent<Controller>() && i.gameObject.transform.parent?.name.IndexOf(gameObject.name) == -1)
                {
                    Vector2 ori = Random.insideUnitCircle * 0.05f;
                    pos = new Vector3(ori.x, -0.05f, ori.y) + i.gameObject.transform.parent.position;
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
                Shared.SpectPoint = gameObject.transform.position + Vector3.up + Vector3.up * 0.4f * Level;
            }
            else
            {
                Shared.SpectPoint = gameObject.transform.position + Vector3.up;
            }
        }
    }
}
