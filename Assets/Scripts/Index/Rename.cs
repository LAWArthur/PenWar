using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rename : MonoBehaviour
{
    InputField inputField;
    // Start is called before the first frame update
    void Start()
    {
        inputField = GetComponent<InputField>();
        inputField.text = PlayerPrefs.GetString("Settings.Name", "玩家");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndEdit()
    {
        PlayerPrefs.SetString("Settings.Name", inputField.text);
    }
}
