using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Abilities
{
    class Spark : Ability
    {
        public Vector3 oldPos, pos;
        bool OnGetting = false;

        public Spark(int usageWeight, KeyCode key, int level) : base(usageWeight, key, level)
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
                    oldPos = ExternalFunctionality.MousePosition();
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
            yield return new WaitUntil(() => Input.GetKeyDown(Basic));
            yield return new WaitUntil(delegate ()
            {
                return oldPos == ExternalFunctionality.MousePosition() ? false : true;
            });
            float st = Time.time;
            yield return new WaitUntil(() =>
            {
                if (Time.time - st >= 1f) return true;
                Ray point = Camera.main.ScreenPointToRay(ExternalFunctionality.MousePosition());
                RaycastHit raycastHit;
                if (!Physics.Raycast(point, out raycastHit)) return true;
                if (raycastHit.transform.gameObject.GetComponent<ParticleSystem>()) return false;
                if (raycastHit.transform.parent?.name == "Table")
                {
                    GameObject spark = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Particles/Spark"), raycastHit.point + 0.02f * Vector3.up, Quaternion.identity, GameObject.Find("/Table/Spawnspace").transform);
                    gameObject.GetComponent<Controller>().productsDock.Add(new Products.SparkProduct(spark, gameObject, Level));
                    return false;
                }
                return true;
            });

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            ExternalFunctionality.Toggle(ref OnGetting);
            gameObject.GetComponent<Controller>().StartCoroutine(gameObject.GetComponent<Controller>().Coroutine());
        }

        public override void Activate(GameObject gameObject)
        {
    
        }

        public override IEnumerator AI(GameObject gameObject)
        {
            foreach (Collider i in Physics.OverlapSphere(gameObject.transform.position, 0.8f + 0.2f * Level))
            {
                if (i.gameObject.GetComponent<Controller>() != null)
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

