using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Abilities
{
    class Wind : Ability
    {
        GameObject linear;
        bool OnGetting = false;

        public Wind(int usageWeight, KeyCode key, int level) : base(usageWeight, key, level)
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
                    linear = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/AbilityLinear"), GameObject.Find("/Canvas").transform);
                    linear.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    linear.GetComponent<Image>().color = new Color(1f / 255 * 57, 1, 0, 1f / 255 * 130);
                    linear.transform.localScale = new Vector3(0.5f, 2.5f, 1f);
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
                return Input.GetKey(Basic) ? false : true;
            });
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            ExternalFunctionality.Toggle(ref OnGetting);
            Activate(gameObject);
            yield return new WaitForSeconds(1f);
            gameObject.GetComponent<Controller>().StartCoroutine(gameObject.GetComponent<Controller>().Coroutine());
        }

        public override void Activate(GameObject gameObject)
        {
            Vector3 direction = linear.transform.TransformDirection(Vector3.up);
            Ray ray = new Ray(gameObject.transform.position, new Vector3(direction.x, 0f, direction.y));
            foreach(GameObject i in Camera.main.GetComponent<main>().rounds)
            {
                i.GetComponent<Controller>().effects.Add(new Effects.Wind(i, Level, ray.GetPoint(0.5f + 0.02f * Level)));
            }
            Object.Destroy(linear);
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
                Shared.SpectPoint = gameObject.transform.position + Vector3.up * 2;
                linear.GetComponent<RectTransform>().localRotation = Quaternion.Euler(Vector3.forward * Mathf.Rad2Deg * Mathf.Atan2(Input.mousePosition.y - linear.transform.position.y, Input.mousePosition.x - linear.transform.position.x) + new Vector3(0f, 0f, -90f));
            }
            else
            {
                Shared.SpectPoint = gameObject.transform.position + Vector3.up;
            }
        }
    }
}
