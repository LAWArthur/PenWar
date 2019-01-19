using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common.Abilities;

namespace Common.Effects
{
    class Fire : Effect
    {
        public Fire(GameObject @this, int lifeTime) : base(@this, lifeTime)
        {
            @this.GetComponent<Rigidbody>().mass *= 0.95f;
        }

        protected override void TestEffective()
        {
            
        }

        protected override void Affect()
        {
            
        }

        public override void Finally()
        {
            base.Finally();
            _this.GetComponent<Rigidbody>().mass /= 0.95f;
        }
    }
}
