using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common.Abilities;

namespace Common.Effects
{
    class Wind : Effect
    {
        Vector3 direction;
        public Wind(GameObject @this, int lifeTime, Vector3 direction) : base(@this, lifeTime) {
            this.direction = direction;
        }

        protected override void TestEffective()
        {
            Debug.DrawRay(_this.transform.position, direction.Reverse(), Color.red);
            Ray ray = new Ray(_this.transform.position, direction.Reverse());
            RaycastHit hit;
            status = EffectStatus.Effective;
            if(Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Products") status = EffectStatus.Ineffective;
            }
        }

        protected override void Affect()
        {
            _this.GetComponent<Rigidbody>().AddForce(direction);
        }
    }
}
