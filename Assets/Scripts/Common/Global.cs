using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Abilities;
using UnityEngine.UI;

class Global : MonoBehaviour {
    public List<GameObject> pens;

    public Queue<GameObject> peninsp;

    [HideInInspector]
    public List<Ability> AbilityPool { get; set; }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        peninsp = new Queue<GameObject>(pens);
        Next();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit) && hit.transform.name == "pen")
        {
            Next();
        }
    }

    public void GeneratePool()
    {
        
        AbilityPool = new List<Ability>
        {
            new Spin(0, (KeyCode)PlayerPrefs.GetInt("Settings.Keys.Spin")),
            new Hook(0, (KeyCode)PlayerPrefs.GetInt("Settings.Keys.Hook")),
            new Move(0, (KeyCode)PlayerPrefs.GetInt("Settings.Keys.Movement"))
        };
        PPush(new Transport(0, (KeyCode)PlayerPrefs.GetInt("Settings.Keys.Transport"), PlayerPrefs.GetInt("Abilities.Transport.Level")));
        PPush(new Explosion(0, (KeyCode)PlayerPrefs.GetInt("Settings.Keys.Explosion"), PlayerPrefs.GetInt("Abilities.Explosion.Level")));
        PPush(new Flow(0, (KeyCode)PlayerPrefs.GetInt("Settings.Keys.Flow"), PlayerPrefs.GetInt("Abilities.Flow.Level")));
        PPush(new Mountain(0, (KeyCode)PlayerPrefs.GetInt("Settings.Keys.Mountain"), PlayerPrefs.GetInt("Abilities.Mountain.Level")));
        PPush(new Wind(0, (KeyCode)PlayerPrefs.GetInt("Settings.Keys.Wind"), PlayerPrefs.GetInt("Abilities.Wind.Level")));
        PPush(new Platform(0, (KeyCode)PlayerPrefs.GetInt("Settings.Keys.Platform"), PlayerPrefs.GetInt("Abilities.Platform.Level")));
        PPush(new Spark(0, (KeyCode)PlayerPrefs.GetInt("Settings.Keys.Sparkline"), PlayerPrefs.GetInt("Abilities.Sparkline.Level")));
        PPush(new Turbo(0, (KeyCode)PlayerPrefs.GetInt("Settings.Keys.Turbo"), PlayerPrefs.GetInt("Abilities.Turbo.Level")));
        PPush(new Iceblock(0, (KeyCode)PlayerPrefs.GetInt("Settings.Keys.Iceblock"), PlayerPrefs.GetInt("Abilities.Iceblock.Level")));
    }

    void PPush(Ability ability)
    {
        if(ability.Level != 0)
        {
            AbilityPool.Insert(0, ability);
        }
    }

    public GameObject Next()
    {
        Destroy(GameObject.Find("pen"));
        peninsp.Enqueue(peninsp.Dequeue());
        GameObject i = peninsp.Peek();
        i = Instantiate(i);
        i.transform.Find("Canvas/Text").GetComponent<Text>().text = peninsp.Peek().name;
        Transform insp = GameObject.Find("Inspector").transform;
        insp.Find("Mass").Find("Text").GetComponent<Text>().text = i.GetComponent<Rigidbody>().mass * 500 + "g";
        insp.Find("Drag").Find("Text").GetComponent<Text>().text = i.GetComponent<Rigidbody>().drag + "";
        i.name = "pen";
        
        return i;
    }
}
