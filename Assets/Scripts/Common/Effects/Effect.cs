using UnityEngine;
using UnityEngine.UI;
using Common.Abilities;

namespace Common.Effects
{
    enum EffectStatus
    {
        Effective, 
        Ineffective,
        Invalid
    }

    class Effect
    {
        public int LifeTime { get; set; }
        protected EffectStatus status = EffectStatus.Effective;
        protected GameObject _this;
        public Effect(GameObject @this, int lifeTime)
        {
            _this = @this;
            this.LifeTime = lifeTime;

        }

        public void Activate()
        {
            if (status == EffectStatus.Invalid) return;
            TestEffective();
            if (status == EffectStatus.Effective) Affect();
            if (LifeTime == 0)
            {
                Finally();
            }
        }

        protected virtual void TestEffective()
        {

        }

        protected virtual void Affect()
        {

        }

        public override string ToString()
        {
            return status == EffectStatus.Invalid ? null : "Effect name: " + this.GetType().Name + "  Effect Status: " + System.Enum.GetName(typeof(EffectStatus), status);
        }

        public virtual void Finally()
        {
            status = EffectStatus.Invalid;
        }
    }
}
