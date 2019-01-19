using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common.Abilities;

namespace Common.Products
{
    class Mountain : Product
    {
        int LifeTime { get; set; }

        public Mountain(GameObject tar, GameObject This, int lifeTime) : base(tar, This)
        {
            LifeTime = lifeTime;
        }

        public override void OnRounding()
        {
            LifeTime--;
            if (LifeTime == 0) This.GetComponent<Controller>().StartCoroutine(Coroutine());
        }

        IEnumerator Coroutine()
        {
            for(int i = 0; i < 6; i++)
            {
                yield return new WaitForSeconds(0.1f);
                TargetObj.transform.position += Vector3.down * 0.1f;
            }
            Destroy();
        }
    }
}
