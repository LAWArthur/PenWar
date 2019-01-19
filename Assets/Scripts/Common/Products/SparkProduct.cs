using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common.Abilities;

namespace Common.Products
{
    class SparkProduct : Product
    {
        int LifeTime { get; set; }

        public SparkProduct(GameObject tar, GameObject This, int lifeTime) : base(tar, This)
        {
            LifeTime = lifeTime;
        }

        public override void OnRounding()
        {
            LifeTime--;
            if (LifeTime == 0) Destroy();
        }
    }
}
