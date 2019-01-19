using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inspect : MonoBehaviour {
    ToggleGroup toggles;
    GameObject Introd;
    public int Point
    {
        get
        {
            return PlayerPrefs.GetInt("Abilities.General.Point");
        }
        set
        {
            PlayerPrefs.SetInt("Abilities.General.Point", value);
            transform.Find("points/Text").GetComponent<Text>().text = "" + value;
        }
    }

    // Use this for initialization
    void Start () {
        toggles = GameObject.Find("Base").GetComponent<ToggleGroup>();
        Introd = GameObject.Find("Introd");
	}
	
	// Update is called once per frame
	void Update () {
        if (toggles.AnyTogglesOn())
        {
            foreach(Toggle i in toggles.ActiveToggles())
            {
                if (i.isOn)
                {
                    Introd.transform.Find("Title").GetComponent<Text>().text = i.gameObject.GetComponent<AbilitiesTogCtrl>().introduction.name;
                    Introd.transform.Find("Scroll View/Viewport/Info").GetComponent<Text>().text =
                    #region Text
                        "<size=16>基本信息</size>\n" +
                        i.gameObject.GetComponent<AbilitiesTogCtrl>().introduction.introduction +
                        "\n<size=16>触发方式</size>\n" +
                        i.gameObject.GetComponent<AbilitiesTogCtrl>().introduction.triggerCondition +
                        "\n<size=16>状态信息</size>\n" +
                        i.gameObject.GetComponent<AbilitiesTogCtrl>().introduction.states +
                        "\n<size=16>分级信息</size>\n" +
                        "等级上限" + i.gameObject.GetComponent<AbilitiesTogCtrl>().MaxLevel +
                        "\n出师等级" + i.gameObject.GetComponent<AbilitiesTogCtrl>().ProLevel +
                        "\n" + i.gameObject.GetComponent<AbilitiesTogCtrl>().introduction.levelBasedAddition +
                        "\n<size=16>技令消耗</size>\n" + i.gameObject.GetComponent<AbilitiesTogCtrl>().introduction.badgeConsuming +
                        (i.gameObject.GetComponent<AbilitiesTogCtrl>().introduction.extension == "" ? "" : "\n<size=16>附加效果</size>\n" +
                        i.gameObject.GetComponent<AbilitiesTogCtrl>().introduction.extension);
                    #endregion
                    Introd.transform.Find("Lvl/Lvl").GetComponent<Text>().text = "" + i.gameObject.GetComponent<AbilitiesTogCtrl>().Level;
                    Introd.transform.Find("Lvl/Minus").GetComponent<Button>().onClick.RemoveAllListeners();
                    Introd.transform.Find("Lvl/Minus").GetComponent<Button>().onClick.AddListener(delegate ()
                    {
                        foreach(GameObject j in i.GetComponent<AbilitiesTogCtrl>().next)
                        {
                            if (j.transform.GetChild(0).GetComponent<AbilitiesTogCtrl>().Level > 0 && i.GetComponent<AbilitiesTogCtrl>().Level - 1 < i.GetComponent<AbilitiesTogCtrl>().ProLevel) return;
                        }
                        if (i.GetComponent<AbilitiesTogCtrl>().Level > 0) {
                            i.GetComponent<AbilitiesTogCtrl>().Level--;
                            Point += i.GetComponent<AbilitiesTogCtrl>().GetBadges();
                        }
                    });
                    Introd.transform.Find("Lvl/Add").GetComponent<Button>().onClick.RemoveAllListeners();
                    Introd.transform.Find("Lvl/Add").GetComponent<Button>().onClick.AddListener(delegate ()
                    {
                        if (Point >= i.GetComponent<AbilitiesTogCtrl>().GetBadges() && i.GetComponent<AbilitiesTogCtrl>().Level < i.GetComponent<AbilitiesTogCtrl>().MaxLevel) {
                            Point -= i.GetComponent<AbilitiesTogCtrl>().GetBadges();
                            i.GetComponent<AbilitiesTogCtrl>().Level++;
                        }
                    });
                    Introd.transform.Find("KeySelector/Toggle").GetComponent<KeyTog>().storageName = i.gameObject.GetComponent<AbilitiesTogCtrl>().Name;
                    Introd.transform.Find("KeySelector/Toggle").GetComponent<KeyTog>().Start();
                    Introd.SetActive(true);
                    return;
                }
            }
        }
        Introd.SetActive(false);
    }
}
