using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common.Abilities;

namespace Common.Products
{
    class Product
    {
        protected GameObject TargetObj { get; set; }
        protected GameObject This { get; set; }

        public Product(GameObject tar, GameObject This)
        {
            TargetObj = tar;
            this.This = This;
        }

        public virtual void OnRounding()
        {

        }

        public virtual void Destroy()
        {
            Object.Destroy(TargetObj);
        }
    }
}
