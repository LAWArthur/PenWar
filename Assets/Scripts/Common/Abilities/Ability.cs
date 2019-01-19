using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Abilities
{

    public enum AbilityStatus
    {
        Preparing,
        Cooling,
        Inactive,
        Ready
    }

    public class Ability
    {
        #region Properties
        protected int AIUsageWeight { get; set; }
        public KeyCode Key { get; set; }
        public KeyCode Basic { get; set; }
        public float Sensitivity { get; set; }
        public int Level { get; set; }
        public AbilityStatus Status { get; set; }
        protected int StateTemp { get; set; }
        protected int PrepTemp { get; set; }
        protected int CoolTemp { get; set; }
        protected Texture2D CursorIcon { get; set; }
        protected string cursorPath;
        #endregion

        //The following functions contains a template
        public Ability(int usageWeight, KeyCode key, int level, string cursorPath = null)
        {
            AIUsageWeight = usageWeight;
            Key = key;
            Level = level;
            Basic = (KeyCode)PlayerPrefs.GetInt("Settings.Keys.Movement");
            Sensitivity = (float)PlayerPrefs.GetInt("Settings.MouseSensitivity", 100) / 1000;
            CursorIcon = Resources.Load<Texture2D>(cursorPath);
            this.cursorPath = cursorPath;
        }

        public virtual void OnRounding()
        {
            switch (Status)
            {
                case AbilityStatus.Preparing:
                    if (--StateTemp == 0) {
                        Status = AbilityStatus.Ready;
                    }
                    break;
                case AbilityStatus.Cooling:
                    if(--StateTemp == 0)
                    {
                        Status = AbilityStatus.Ready;
                    }
                    break;
            }
        }

        public virtual bool OnUsing(GameObject gameObject)
        {
            return true;
        }

        public virtual IEnumerator Coroutine(GameObject gameObject)
        {
            yield return new WaitForSecondsRealtime(0.2f);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Activate(gameObject);
            gameObject.GetComponent<Controller>().StartCoroutine(gameObject.GetComponent<Controller>().Coroutine());
        }

        public virtual void Activate(GameObject gameObject)
        {

        }

        public virtual IEnumerator AI(GameObject gameObject)
        {
            yield return null;
        }

        public virtual void OnActivated(GameObject gameObject)
        {
            Shared.SpectPoint = gameObject.transform.position + Vector3.up;
        }

        public override string ToString()
        {
            return "Ability name: " + this.GetType().Name + "  Key code: " + System.Enum.GetName(typeof(KeyCode), Key) + "  Status: " + System.Enum.GetName(typeof(AbilityStatus), Status);       
        }
    }
}
