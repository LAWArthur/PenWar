using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Abilities
{
    class Platform : Ability
    {
        public Vector3 oldPos, pos;
        GameObject area, platform;
        bool OnGetting = false;

        public Platform(int usageWeight, KeyCode key, int level) : base(usageWeight, key, level)
        {
            Status = AbilityStatus.Cooling;
            CoolTemp = 3;
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
                    area.GetComponent<SpriteRenderer>().color = new Color(0.6320f, 0.6270f, 0f, 0.5176f);
                    area.transform.localScale = new Vector3(0.8f + 0.2f * Level, 0.8f + 0.2f * Level, 1f);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    Cursor.SetCursor(CursorIcon, Vector2.zero, CursorMode.Auto);
                    Status = AbilityStatus.Cooling;
                    StateTemp = CoolTemp;
                    return true;
                }
            }
            else if (Mathf.FloorToInt(Random.value * AIUsageWeight) == 0)
            {
                foreach (GameObject i in Camera.main.GetComponent<main>().rounds)
                {
                    if (i != gameObject)
                    {
                        if (i.transform.position.y > gameObject.transform.position.y) return true;
                    }
                }
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
                }
                else
                {
                    pos = area.GetComponent<Collider>().ClosestPointOnBounds(hit.point) + Vector3.down * 0.05f;
                }
                Object.Destroy(area);
                platform = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Platform"), pos + Vector3.down, Quaternion.Euler(180f, 0f, 0f));
                gameObject.GetComponent<Controller>().productsDock.Add(new Products.Mountain(platform, gameObject,2 * Level));
                for (int i = 0; i < 5; i++)
                {
                    Activate(gameObject);
                    yield return new WaitForSeconds(0.1f);
                }

                yield return new WaitForSeconds(1f);
            }

            ExternalFunctionality.Toggle(ref OnGetting);
            gameObject.GetComponent<Controller>().StartCoroutine(gameObject.GetComponent<Controller>().Coroutine());
        }

        public override void Activate(GameObject gameObject)
        {
            platform.transform.position = Vector3.MoveTowards(platform.transform.position, new Vector3(platform.transform.position.x, -0.07f, platform.transform.position.z), 0.30f);
        }

        public override IEnumerator AI(GameObject gameObject)
        {
            Activate(gameObject);
            gameObject.GetComponent<Controller>().StartCoroutine(gameObject.GetComponent<Controller>().Coroutine());
            yield return null;
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
