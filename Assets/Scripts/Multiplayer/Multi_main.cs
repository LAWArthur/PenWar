using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Common;
using UnityEngine.Networking;
[DisallowMultipleComponent]
[System.Obsolete("This has been obsoleted. Please use Multyplayer instead.", true)]
class Multi_main : MonoBehaviour
{
    //bool gaming;
    //public GameObject pen, abilityInfo;
    //public List<GameObject> pens;
    //public Queue<GameObject> rounds;
    //public int Point
    //{
    //    get
    //    {
    //        return PlayerPrefs.GetInt("Abilities.General.Point");
    //    }
    //    set
    //    {
    //        PlayerPrefs.SetInt("Abilities.General.Point", value);
    //    }
    //}

    //// Use this for initialization
    //void Start()
    //{
    //    #region Init variable
    //    gaming = false;
    //    pens = new List<GameObject>();
        
    //    Physics.gravity = new Vector3(0f, -9.81f, 0f);
    //    #endregion

    //    #region init ControllerBase
    //    foreach (GameObject i in pens)
    //    {
    //        i.name = "Player " + (pens.IndexOf(i) + 1);
    //    }
    //    #endregion

    //    Invoke("StartGame", 3f);

    //    abilityInfo = GameObject.Find("AbilityInfo");
    //    abilityInfo.SetActive(false);

    //    GameObject.Find("Channel").GetComponent<Text>().text = "游戏即将开始";

        
    //}

    //IEnumerator StartGame()
    //{
    //    yield return new WaitUntil(delegate ()
    //    {
    //        return NetworkManager.singleton.numPlayers >= 4;
    //    });
    //    rounds = new Queue<GameObject>(pens);
    //    foreach (GameObject i in pens)
    //    {
            
    //        i.name = "Player " + (pens.IndexOf(i) + 1);
    //    }
    //    #region Set Cursor
    //    Cursor.lockState = CursorLockMode.Locked;
    //    Cursor.visible = false;
    //    #endregion
    //    gaming = true;
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    #region Inspect Abilities
    //    if (Input.GetKey(KeyCode.BackQuote))
    //    {
    //        string str = "";
    //        foreach (Common.Abilities.Ability i in GameObject.FindGameObjectWithTag("Local").GetComponent<ControllerBase>() ? GameObject.FindGameObjectWithTag("Local").GetComponent<ControllerBase>().abilitiesPool : new List<Common.Abilities.Ability>())
    //        {
    //            str += i.ToString() + "\n";
    //        }
    //        foreach (Common.Effects.Effect i in GameObject.FindGameObjectWithTag("Local").GetComponent<ControllerBase>() ? GameObject.FindGameObjectWithTag("Local").GetComponent<ControllerBase>().effects : new List<Common.Effects.Effect>())
    //        {
    //            str += i.ToString() + "\n";
    //        }
    //        abilityInfo.SetActive(true);
    //        abilityInfo.transform.Find("Viewport/Content/Text").GetComponent<Text>().text = str;
    //    }
    //    if (Input.GetKeyUp(KeyCode.BackQuote))
    //    {
    //        abilityInfo.SetActive(false);
    //    }
    //    #endregion

    //    #region Exit
    //    if (Input.GetKeyDown(KeyCode.Escape)) GameObject.Find("/NetworkingManager").GetComponent<Multiplayer>().StopClient();
    //    #endregion
    //    if (!gaming) return;

    //    #region Test for living players
    //    if (rounds.Count == 1)
    //    {
    //        GameObject.Find("Channel").GetComponent<Text>().text = "三秒钟后返回大厅\n" + rounds.Peek().name + "获胜！\n" + GameObject.Find("Channel").GetComponent<Text>().text;
    //        if (rounds.Peek().tag == "Local")
    //        {

    //            Point++;
    //        }
    //        foreach (Common.Products.Product i in rounds.Peek().GetComponent<ControllerBase>().productsDock)
    //        {
    //            i.Destroy();
    //        }
    //        Destroy(rounds.Peek().GetComponent<Rigidbody>());
    //        Destroy(rounds.Peek().GetComponent<ControllerBase>());
    //        gaming = false;

    //        StartCoroutine(BackToLobby());
    //    }
    //    #endregion

    //    #region Round operator
    //    if (rounds.Peek().GetComponent<ControllerBase>().active) return;
    //    rounds.Enqueue(rounds.Dequeue());
    //    rounds.Peek().GetComponent<ControllerBase>().Activate();
    //    #endregion
    //}

    //IEnumerator BackToLobby()
    //{
    //    yield return new WaitForSeconds(3f);
    //    GameObject.Find("/NetworkingManager").GetComponent<Multiplayer>().StopClient();
    //}

    //void FixedUpdate()
    //{
    //    if (!gaming) return;
    //    Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, Shared.SpectPoint, 0.1f);

    //}
}
