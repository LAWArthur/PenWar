using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Abilities
{
    class Move : Ability
    {
        public Vector3 oldPos, force;

        public Move(int usageWeight, KeyCode key) : base(usageWeight, key, 0, "Appearance/Cursors/normal")
        {

        }

        public override void OnRounding()
        {

        }

        public override bool OnUsing(GameObject gameObject)
        {
            if (AIUsageWeight == 0)
            {
                if (Input.GetKey(Key))
                {
                    oldPos = Input.mousePosition;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    Cursor.SetCursor(CursorIcon, Vector2.zero, CursorMode.Auto);
                    return true;
                }
            }
            else if (Mathf.FloorToInt(Random.value * AIUsageWeight) == 0)
            {
                return true;
            }
            return false;
        }

        public override IEnumerator Coroutine(GameObject gameObject)
        {
            yield return new WaitUntil(delegate ()
            {
                return oldPos == Input.mousePosition ? false : true;
            });
            yield return new WaitForSecondsRealtime(0.2f);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            force = new Vector3((Input.mousePosition - oldPos).x, 0f, (Input.mousePosition - oldPos).y) * Sensitivity;
            Activate(gameObject);
            gameObject.GetComponent<Controller>().StartCoroutine(gameObject.GetComponent<Controller>().Coroutine());
        }

        public override void Activate(GameObject gameObject)
        {
            gameObject.GetComponent<Rigidbody>().AddForce(force);
        }

        public override IEnumerator AI(GameObject gameObject)
        {
            force = gameObject.transform.position - new Vector3(Random.value * 40 - 20, 0f, Random.value * 40 - 20) * 2;
            Activate(gameObject);
            gameObject.GetComponent<Controller>().StartCoroutine(gameObject.GetComponent<Controller>().Coroutine());
            yield return null;
        }
    }
}
