using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common.Abilities;

namespace Common.Effects
{
    class Frozen : Effect
    {
        float force;

        public Frozen(GameObject @this, int lifeTime, float force) : base(@this, lifeTime)
        {
            @this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            foreach(Renderer i in @this.GetComponentsInChildren<Renderer>())
            {
                i.material.color = new Color(0.5f, 0.5f, 1f);
            }
        }

        protected override void TestEffective()
        {
            if (force < 0) Finally();
        }

        protected override void Affect()
        {
            force -= _this.GetComponent<Rigidbody>().velocity.magnitude;
        }

        public override void Finally()
        {
            base.Finally();
            _this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            _this.GetComponentInChildren<Renderer>().material.color = Color.white;
        }
    }
}
