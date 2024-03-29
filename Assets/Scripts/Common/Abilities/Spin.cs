﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Abilities
{
    class Spin : Ability
    {
        public Vector3 oldPos, torque;

        public Spin(int usageWeight, KeyCode key) : base(usageWeight, key, 0, "Appearance/Cursors/spin")
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
            yield return new WaitUntil(delegate ()
            {
                return oldPos == ExternalFunctionality.MousePosition() ? false : true;
            });
            yield return new WaitForSecondsRealtime(0.2f);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            torque = Vector3.up * 50000f;
            Debug.Log(torque);
            Debug.DrawRay(gameObject.transform.position, torque, Color.red, 1000f);
            Activate(gameObject);
            gameObject.GetComponent<Controller>().StartCoroutine(gameObject.GetComponent<Controller>().Coroutine());
        }

        public override void Activate(GameObject gameObject)
        {
            gameObject.GetComponent<Controller>().StartCoroutine(ConstActive(gameObject));
        }

        IEnumerator ConstActive(GameObject gameObject)
        {
            float s_t = Time.time;
            for (; Time.time - s_t < 0.5;)
            {
                gameObject.GetComponent<Rigidbody>().AddTorque(torque);
                yield return new WaitForEndOfFrame();
            }
        }

        public override IEnumerator AI(GameObject gameObject)
        {
            torque = new Vector3(0f, 50000f, 0f);
            Activate(gameObject);
            gameObject.GetComponent<Controller>().StartCoroutine(gameObject.GetComponent<Controller>().Coroutine());
            yield return null;
        }
    }
}
