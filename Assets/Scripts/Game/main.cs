using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Common;
using UnityEngine.Analytics;
[DisallowMultipleComponent]
class main : MonoBehaviour {
    protected bool gaming;
    public int numPlayer;
    float scaleSensitivity;
    public GameObject pen, abilityInfo;
    public List<GameObject> pens;
    public Queue<GameObject> rounds;
    bool gamepause = false;
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

    // Use this for initialization
    protected virtual void Start () {
        AnalyticsEvent.GameStart(new Dictionary<string, object> {
            {"player", PlayerPrefs.GetString("Settings.Name", "玩家") }
        });
        scaleSensitivity = (float)PlayerPrefs.GetInt("Settings.ScaleSensitivity") / 500;
        #region Init variable
        gaming = false;
        pens = new List<GameObject>();
        pens.Add(Instantiate(GameObject.Find("Global").GetComponent<Global>().peninsp.Peek(), new Vector3(Random.Range(-0.7f, 0.7f), 1f, Random.Range(-0.7f, 0.7f)), Random.rotation));
        for (int i = 1; i < numPlayer; i++)
        {
            pens.Add(Instantiate(GameObject.Find("Global").GetComponent<Global>().pens[Random.Range(0, GameObject.Find("Global").GetComponent<Global>().pens.Count)], new Vector3(Random.Range(-0.7f, 0.7f), 1f, Random.Range(-0.7f, 0.7f)), Random.rotation));
        }
        rounds = new Queue<GameObject>(pens);
        Physics.gravity = new Vector3(0f, -9.81f, 0f);
        #endregion

        #region init controller
        foreach (GameObject i in pens)
        {
            i.name = ExternalFunctionality.GenID(6);
            if (pens.IndexOf(i) == 0)
            {
                i.AddComponent<Player>();
                i.tag = "Local";
                i.name = PlayerPrefs.GetString("Settings.Name", "玩家");
            }
            else
            {
                i.AddComponent<AI>();
            }
        }
        #endregion

        Invoke("StartGame", 3f);

        abilityInfo = GameObject.Find("AbilityInfo");
        abilityInfo.SetActive(false);

        GameObject.Find("Channel").GetComponent<Text>().text = "游戏即将开始";

        #region Set Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        #endregion
    }

    void StartGame()
    {
        gaming = true;
    }

	// Update is called once per frame
	protected virtual void Update () {
        Shared.SpectPointOffset -= Input.mouseScrollDelta.y * Vector3.up * scaleSensitivity;
        #region Inspect Abilities
        if (Input.GetKey(KeyCode.BackQuote))
        {
            string str = "";
            foreach(Common.Abilities.Ability i in GameObject.FindGameObjectWithTag("Local").GetComponent<Controller>()? GameObject.FindGameObjectWithTag("Local").GetComponent<Controller>().abilitiesPool:new List<Common.Abilities.Ability>())
            {
                str += i.ToString() + "\n";
            }
            foreach (Common.Effects.Effect i in GameObject.FindGameObjectWithTag("Local").GetComponent<Controller>() ? GameObject.FindGameObjectWithTag("Local").GetComponent<Controller>().effects : new List<Common.Effects.Effect>())
            {
                str += i.ToString() == null ? i.ToString() + "\n" : "";
            }
            abilityInfo.SetActive(true);
            abilityInfo.transform.Find("Viewport/Content/Text").GetComponent<Text>().text = str;
        }
        if (Input.GetKeyUp(KeyCode.BackQuote))
        {
            abilityInfo.SetActive(false);
        }
        #endregion

        #region Exit
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0.0f;
            gamepause = true;
        }
        #endregion
        if (!gaming) return;

        #region Test for living players
        if(rounds.Count == 1)
        {
            GameObject.Find("Channel").GetComponent<Text>().text = "三秒钟后返回大厅\n" + rounds.Peek().name + "获胜！\n" + GameObject.Find("Channel").GetComponent<Text>().text;
            if(rounds.Peek().tag == "Local")
            {
                AnalyticsEvent.GameOver("Win", new Dictionary<string, object> {
            {"player", PlayerPrefs.GetString("Settings.Name", "玩家") }
        });
                
            }
            else
            {
                AnalyticsEvent.GameOver("Lose", new Dictionary<string, object> {
            {"player", PlayerPrefs.GetString("Settings.Name", "玩家") }
        });
            }
            foreach(Common.Products.Product i in rounds.Peek().GetComponent<Controller>().productsDock)
            {
                i.Destroy();
            }
            Destroy(rounds.Peek().GetComponent<Rigidbody>());
            Destroy(rounds.Peek().GetComponent<Controller>());
            gaming = false;

            StartCoroutine(BackToLobby());
        }
        #endregion

        #region Round operator
        if (rounds.Peek().GetComponent<Controller>().active) return;
        rounds.Enqueue(rounds.Dequeue());
        rounds.Peek().GetComponent<Controller>().Activate();
        #endregion
    }
    
    protected IEnumerator BackToLobby()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
    }

    void FixedUpdate()
    {
        if (!gaming) return;
        Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, Shared.SpectPoint + Shared.SpectPointOffset, 0.1f);
    }

    void OnGUI()
    {
        if (gamepause)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.MiddleCenter;
            style.fontSize = 18;
            GUIStyleState normal = style.normal;
            normal.textColor = Color.black;
            style.normal = normal;

            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 60, 100, 30), "游戏暂停", style);
            if (GUI.Button(new Rect(Screen.width/2-50,Screen.height/2-30,100,30),"继续游戏"))
            {
                gamepause = false;
                Time.timeScale = 1.0f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2, 100, 30), "退出游戏"))
            {
                Time.timeScale = 1.0f;
                SceneManager.LoadScene(0);
                AnalyticsEvent.GameOver("Quit", new Dictionary<string, object> {
                    {"player", PlayerPrefs.GetString("Settings.Name", "玩家") }
                });
            }
        }
    }
}
