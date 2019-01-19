using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Common;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class Multiplayer : NetworkManager {
    bool gaming, start;
    public GameObject pen, abilityInfo;
    public List<GameObject> pens;
    public Queue<GameObject> rounds;
    public int Point
    {
        get
        {
            return PlayerPrefs.GetInt("Abilities.General.Point");
        }
        set
        {
            PlayerPrefs.SetInt("Abilities.General.Point", value);
        }
    }
    MatchInfo match;

    public override void OnStartServer()
    {
        base.OnStartServer();
        
        #region Init variable
        gaming = false;
        pens = new List<GameObject>();
        Physics.gravity = new Vector3(0f, -9.81f, 0f);
        #endregion

        StartCoroutine(StartGame());
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        abilityInfo = GameObject.Find("AbilityInfo");
        abilityInfo.SetActive(false);
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
    }

    public IEnumerator StartGame()
    {
        GameObject.Find("Global").GetComponent<Global>().GeneratePool();
        yield return new WaitForSeconds(0.5f);
        start = true;
        GameObject.Find("Channel").GetComponent<Text>().text = "游戏即将开始";
        //yield return new WaitUntil(delegate ()
        //{
        //    return numPlayers >= 4;
        //});
        rounds = new Queue<GameObject>(pens);
        GameObject.Find("Channel").GetComponent<Text>().text = "游戏开始";
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log(rounds);
        rounds.Peek().GetComponent<Controller>().Activate();
    }

    public void Next()
    {
        rounds.Enqueue(rounds.Dequeue());
        rounds.Peek().GetComponent<Controller>().Activate();
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
    }

    public void Match()
    {
        StartMatchMaker();
        matchMaker.ListMatches(0, 10, "", false, 0, 0, Matching);
    }

    public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        match = matchInfo;
        base.OnMatchCreate(success, extendedInfo, matchInfo);
        #region Init variable
        gaming = false;
        pens = new List<GameObject>();
        Physics.gravity = new Vector3(0f, -9.81f, 0f);
        #endregion

        StartCoroutine(StartGame());
    }

    public override void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        match = matchInfo;
        base.OnMatchJoined(success, extendedInfo, matchInfo);
    }

    public void Matching(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {
        OnMatchList(success, extendedInfo, matchList);
        if (success && (matchList == null || matchList.Count == 0))
        {
            matchMaker.CreateMatch(GenID(8), 4, true, "", "", "", 0, 0, OnMatchCreate);
        }
        else
        {
            matchMaker.JoinMatch(matchList[0].networkId, "", "", "", 0, 0, OnMatchJoined);
        }
    }

    public string GenID(int n)
    {
        string ab = "qwertyuiopasdfghjklzxcvbnm1234567890_";
        string s = "";
        for(int i = 0; i < n; i++)
        {
            s += ab[Random.Range(0, ab.Length - 1)];
        }
        return s;
    }

    void Update()
    {
        if (!start) return;
        if (Input.GetKeyDown(KeyCode.Escape)) matchMaker.DropConnection(match.networkId, match.nodeId, 0, OnDropConnection);
    }

    public override void OnDropConnection(bool success, string extendedInfo)
    {
        base.OnDropConnection(success, extendedInfo);
    }
}
