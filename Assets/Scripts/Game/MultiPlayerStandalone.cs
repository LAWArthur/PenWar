using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Common;
using UnityEngine.Analytics;

class MultiPlayerStandalone : main
{
    // Start is called before the first frame update
    protected override void Start()
    {
        AnalyticsEvent.GameStart();
        #region Init variable
        gaming = false;
        pens = new List<GameObject>
        {
            Instantiate(pen, new Vector3(0.1f, 1f, 0.1f), Quaternion.Euler(90f,45f,0f)),
            Instantiate(pen, new Vector3(-0.1f, 1f, 0.1f), Quaternion.Euler(90f,90f,0f)),
            Instantiate(pen, new Vector3(0.1f, 1f, -0.1f), Quaternion.Euler(90f,-90f,0f)),
            Instantiate(pen, new Vector3(-0.1f, 1f, -0.1f), Quaternion.Euler(90f,-45f,0f))
        };
        rounds = new Queue<GameObject>(pens);
        Physics.gravity = new Vector3(0f, -9.81f, 0f);
        #endregion

        #region init controller
        foreach (GameObject i in pens)
        {
            i.AddComponent<Player>();
            i.name = "Player " + (pens.IndexOf(i) + 1);
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

    protected override void Update()
    {
        #region Inspect Abilities
        if (Input.GetKey(KeyCode.BackQuote))
        {
            string str = "";
            foreach (Common.Abilities.Ability i in rounds.Peek().GetComponent<Controller>().abilitiesPool)
            {
                str += i.ToString() + "\n";
            }
            foreach (Common.Effects.Effect i in rounds.Peek().GetComponent<Controller>().effects)
            {
                str += i.ToString() + "\n";
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
            SceneManager.LoadScene(0);
            AnalyticsEvent.GameOver("Quit");
        }
        #endregion
        if (!gaming) return;

        #region Test for living players
        if (rounds.Count == 1)
        {
            GameObject.Find("Channel").GetComponent<Text>().text = "三秒钟后返回大厅\n" + rounds.Peek().name + "获胜！\n" + GameObject.Find("Channel").GetComponent<Text>().text;
            foreach (Common.Products.Product i in rounds.Peek().GetComponent<Controller>().productsDock)
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
}
