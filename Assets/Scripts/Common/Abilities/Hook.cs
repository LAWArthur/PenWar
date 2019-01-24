using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Abilities
{
    class Hook : Ability
    {
        public Vector3 oldPos, force;

        public Hook(int usageWeight, KeyCode key) : base(usageWeight, key, 0, "Appearance/Cursors/hook")
        {

        }

        public override void OnRounding()
        {

        }

        public override bool OnUsing(GameObject gameObject)
        {
            if (AIUsageWeight == 0)
            {
                if (Input.GetKey(Key) && Input.GetKey(Basic))
                {
                    oldPos = ExternalFunctionality.MousePosition();
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    Cursor.SetCursor(CursorIcon, Vector2.zero, CursorMode.Auto);
                    return true;
                }
            }
            else if (Mathf.FloorToInt(Random.value * AIUsageWeight) == 0)
            {
                foreach(GameObject i in Camera.main.GetComponent<main>().rounds)
                {
                    if(i != gameObject)
                    {
                        if (i.transform.position.y > gameObject.transform.position.y) return true;
                    }
                }
            }
            return false;
        }

        public override IEnumerator Coroutine(GameObject gameObject)
        {
            yield return new WaitUntil(delegate ()
            {
                return oldPos == ExternalFunctionality.MousePosition() ? false : true;
            });
            yield return new WaitForSecondsRealtime(0.2f);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            force = new Vector3(0f, Vector3.Distance(oldPos, ExternalFunctionality.MousePosition()), 0f) * Sensitivity;
            Activate(gameObject);
            gameObject.GetComponent<Controller>().StartCoroutine(gameObject.GetComponent<Controller>().Coroutine());
        }

        public override void Activate(GameObject gameObject)
        {
            gameObject.GetComponent<Rigidbody>().AddForce(force);
        }

        public override IEnumerator AI(GameObject gameObject)
        {
            force = new Vector3(0f, 50f, 0f);
            Activate(gameObject);
            gameObject.GetComponent<Controller>().StartCoroutine(gameObject.GetComponent<Controller>().Coroutine());
            yield return null;
        }
    }
}
