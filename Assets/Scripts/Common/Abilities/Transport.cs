using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Abilities
{
    class Transport: Ability
    {
        public Vector3 oldPos, pos;
        GameObject area;
        bool OnGetting = false;
        
        public Transport(int usageWeight, KeyCode key, int level):base(usageWeight, key, level)
        {
            Status = AbilityStatus.Cooling;
            CoolTemp = 1;
            StateTemp = CoolTemp;
        }

        public override void OnRounding()
        {
            if(Status == AbilityStatus.Cooling && StateTemp == 0)
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
                    area.GetComponent<SpriteRenderer>().color = new Color(1f / 255 * 57, 1, 0, 1f / 255 * 130);
                    area.transform.localScale = new Vector3(1f + 0.2f * Level, 1f + 0.2f * Level, 1f);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    Cursor.SetCursor(CursorIcon, Vector2.zero, CursorMode.Auto);
                    Status = AbilityStatus.Cooling;
                    StateTemp = CoolTemp;
                    return true;
                }
            }
            else if (Mathf.FloorToInt(Random.value * AIUsageWeight) == 0 && Status == AbilityStatus.Ready)
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
            if(Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == area)
                {
                    pos = hit.point;
                    Activate(gameObject);
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    pos = area.GetComponent<Collider>().ClosestPointOnBounds(hit.point);
                    Activate(gameObject);
                    yield return new WaitForSeconds(1f);
                }
            }
            ExternalFunctionality.Toggle(ref OnGetting);
            gameObject.GetComponent<Controller>().StartCoroutine(gameObject.GetComponent<Controller>().Coroutine());
        }

        public override void Activate(GameObject gameObject)
        {
            oldPos = gameObject.transform.position;
            gameObject.transform.position = pos + new Vector3(0f, 0.2f, 0f);
            gameObject.GetComponent<Rigidbody>().WakeUp();
            Object.Destroy(area);
            GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Particles/Transport"), oldPos + new Vector3(0f, 0.2f, 0f), Quaternion.LookRotation(pos, Vector3.up)).GetComponent<TransportParticle>().distance = Vector3.Distance(pos, oldPos);
        }

        public override IEnumerator AI(GameObject gameObject)
        {
            Vector2 OriPos = Random.insideUnitCircle * Random.value * (1f + 0.2f * Level);
            pos = new Vector3(OriPos.x, 0f, OriPos.y);
            Activate(gameObject);
            yield return new WaitForSeconds(1f);
            gameObject.GetComponent<Controller>().StartCoroutine(gameObject.GetComponent<Controller>().Coroutine());
        }

        public override void OnActivated(GameObject gameObject)
        {
            if (OnGetting)
            {
                Shared.SpectPoint = gameObject.transform.position + Vector3.up + Vector3.up * 0.2f * Level;
            }
            else
            {
                Shared.SpectPoint = gameObject.transform.position + Vector3.up;
            }
        }
    }
}
