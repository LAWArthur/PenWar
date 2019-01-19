using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ButtonCtrl : MonoBehaviour {
    public List<ButtonEvent> buttonEvents;

	public void Listen()
    {
        foreach(ButtonEvent i in buttonEvents)
        {
            i.Listen();
        }
    }
    public void Sleep()
    {
        foreach (ButtonEvent i in buttonEvents)
        {
            i.Sleep();
        }
    }
    void Start()
    {
        Listen();
    }

    [System.Serializable]
    public class ButtonEvent
    {
        public Button button;
        public System.Delegate Delegate;
        [System.Serializable]
        public class OnClick : UnityEvent { }
        public OnClick onClick;

        public void Listen()
        {
            button.onClick.AddListener(onClick.Invoke);
        }
        public void Sleep()
        {
            button.onClick.RemoveAllListeners();
        }
    }
}
