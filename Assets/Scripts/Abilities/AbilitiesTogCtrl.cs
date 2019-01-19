using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
[System.Serializable]
public struct Introduction
{
    public string name;
    [TextArea]
    public string introduction;
    [TextArea]
    public string triggerCondition;
    [TextArea]
    public string states;
    [TextArea]
    public string levelBasedAddition;
    [TextArea]
    public string badgeConsuming;
    [TextArea]
    public string extension;
}

public class AbilitiesTogCtrl : MonoBehaviour  {
    public string Name;
    [SerializeField]
    public Introduction introduction;
    public List<GameObject> next;
    public int Level {
        get
        {
            return PlayerPrefs.GetInt("Abilities." + Name + ".Level");
        }
        set
        {
            OnDataUpdate();
            PlayerPrefs.SetInt("Abilities." + Name + ".Level", value);
        }
    }
    public int MaxLevel;
    public int ProLevel;
    public int badges;
    public int badgesLevelAdditive;

    // Use this for initialization
    void Start () {
        next = next ?? new List<GameObject>();
        OnDataUpdate();
	}

    // Update is called once per frame
    void Update()
    {
        if (Level >= ProLevel)
        {
            foreach (GameObject i in next)
            {
                i.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject i in next)
            {
                i.SetActive(false);
            }
        }
        transform.Find("KeyLog").GetComponent<Text>().text = System.Enum.GetName(typeof(KeyCode), PlayerPrefs.GetInt("Settings.Keys." + Name, (int)KeyCode.Mouse0));
    }

    void OnDataUpdate()
    {

    }

    public int GetBadges()
    {
        return badges + Level * badgesLevelAdditive;
    }
}
