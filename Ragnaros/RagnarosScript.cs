using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class RagnarosScript : NetworkBehaviour {
    [SyncVar]
    int hp;

    public AudioClip[] audios = new AudioClip[5];
    public AudioSource audioSource;

    GameObject[] players = new GameObject[4];
    Transform targetPlayer;
    int index = 0;

    public Animator anim;

    int maxhp = 10000;


    //public Slider health;
    public GameObject fireball;
    public GameObject flameTurbine;
    public GameObject ultimate;
    public GameObject fenix;

    float timer;
    float looking_timer;
    float start_delay = 0;
        
    float activateSpell;

    

    bool flameturbine_trigger = false;
    bool flameBrust_trigger = false;
    bool hellfire_trigger = false;
    bool ultimate_trigger = false;
    bool fenix_trigger = false;
	// Use this for initialization
	void Start () {
        hp = maxhp;
        //health.maxValue = maxhp;
        //health.value = health.maxValue;
	}
	
	// Update is called once per frame
	void Update () {
        if (!base.isServer)
            return;

        looking_timer += Time.deltaTime;
        start_delay += Time.deltaTime;

        if(hp <= 0)
        {
            Destroy(gameObject);
        }

        players = GameObject.FindGameObjectsWithTag("Player");//buscando jogadores no game

        if (looking_timer > 4f && players.Length >= 1)//duracao que o boss olha para o player dependendo da qtd d players
        {

            index = Random.Range(0, players.Length);
            looking_timer = 0;
        }
        if (players.Length > 0) //Nao tente olhar para um player q nao existe
        {
            transform.LookAt(players[index].transform);
            targetPlayer = players[index].transform;
        }

        //transform.LookAt(players[0].transform);
        //transform.LookAt(enemy.transform);
        gameObject.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);//eixos n y = 0

        timer += Time.deltaTime;
        controls();//ativadores de spells

        /*if (timer > .3f )
        {

            Instantiate(fireball, new Vector3(transform.position.x, transform.position.y - 10, transform.position.z), transform.rotation);//.GetComponent<Rigidbody>().velocity = transform.forward;
            //Instantiate(fireball, transform).GetComponent<Rigidbody>().velocity = transform.forward;
            timer = 0;
        }*/
        Cmd_hellfire();//spell que sempre esta ativa
        
    }
    void takeDamage(int amount )
    {
        //health.value -= amount;
        hp -= amount;
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 20;
        style.normal.textColor = Color.black;
        

        GUI.Label(new Rect(70, 80, 150, 20), "BOSS HP: " + ((float)hp / maxhp) * 100 + "%");//+ hp );        
        /*if (ultimate_trigger == true)
        {
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2 - 40, 100, 20), "DevouringFlame",style);
        }
        if (flameBrust_trigger == true)
        {
            style.normal.textColor = Color.red;
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2 - 40, 100, 20), "FlameBurst", style);
        }
        if (flameturbine_trigger == true)
        {
            style.normal.textColor = Color.white;
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2 - 40, 100, 20), "FlameTurbine", style);
        }*/

        //Debug.Log("width " + Screen.width + " height " + Screen.height);
    }
    
    
    void flameBrust()
    {
        for (int i = 0; i < 40; i++)
        {
            Quaternion rotacao = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y+10*i-45, transform.eulerAngles.z);


            //Instantiate(fireball, new Vector3(transform.position.x, transform.position.y - 10, transform.position.z), rotacao);
            NetworkServer.Spawn(Instantiate(fireball, new Vector3(transform.position.x, transform.position.y - 10, transform.position.z), rotacao));
        }
    }
    

    void devouringFlame()
    {
        anim.Play("ChannelCastOmni [1]");
        //GameObject ballOfLight = Instantiate(ultimate);
        NetworkServer.Spawn(Instantiate(ultimate));
        //ballOfLight.SetActive(true);
        ultimate_trigger = false;
    }

    [Command]
    void Cmd_hellfire()
    {
        if(start_delay < 5f)
        {
            return;
        }

        if (timer > .3f)
        {

            //Instantiate(fireball, new Vector3(transform.position.x, transform.position.y - 10, transform.position.z), transform.rotation);//.GetComponent<Rigidbody>().velocity = transform.forward;
            

            timer = 0;
            NetworkServer.Spawn(Instantiate(fireball, new Vector3(transform.position.x, transform.position.y - 10, transform.position.z), transform.rotation));
            //NetworkServer.Spawn(Instantiate(fireball, new Vector3(transform.position.x+10, transform.position.y - 10, transform.position.z), transform.rotation));
        }
    }

    void flameturbine()//flameturbine 
    {
        Vector3 center = transform.position;
        for (int i = 0; i < 24; i++)//for (int i = 0; i < 12; i++)
        {

            int a = i * 15;//int a = i * 30;
            float ang = a;

            Vector3 pos;
            pos.x = center.x + Vector3.Distance(targetPlayer.transform.position, transform.position) * Mathf.Sin(ang * Mathf.Deg2Rad);
            pos.y = center.y - 5;
            pos.z = center.z + Vector3.Distance(targetPlayer.transform.position, transform.position) * Mathf.Cos(ang * Mathf.Deg2Rad);

            Vector3 poz = pos;

            //Instantiate(flameTurbine, pos, Quaternion.identity);
            NetworkServer.Spawn(Instantiate(flameTurbine, pos, Quaternion.identity));//nao funfa =/

        }
    }

    IEnumerator DelayedFor()//flameturbine 
    {
        Vector3 center = transform.position;
        for (int i = 0; i < 24; i++)//for (int i = 0; i < 12; i++)
        {

            int a = i * 15;//int a = i * 30;
            float ang = a;

            Vector3 pos;
            pos.x = center.x + Vector3.Distance(targetPlayer.transform.position, transform.position) * Mathf.Sin(ang * Mathf.Deg2Rad);
            pos.y = center.y-5;
            pos.z = center.z + Vector3.Distance(targetPlayer.transform.position, transform.position) * Mathf.Cos(ang * Mathf.Deg2Rad);

            Vector3 poz = pos;

            //Instantiate(flameTurbine, pos, Quaternion.identity);
            NetworkServer.Spawn(Instantiate(flameTurbine, pos, Quaternion.identity));//nao funfa =/
            yield return new WaitForSeconds(0.1f);

        }
    }

    void fenixSpell()
    {
        GameObject spell = Instantiate(fenix,transform.position,transform.rotation);
        spell.GetComponent<Rigidbody>().velocity = transform.forward * 6;
        NetworkServer.Spawn(spell);
    }


    void controls()
    {
        if (flameturbine_trigger == true)// flameturbine spell
        {
            if (activateSpell > 2f)
            {
                Rpc_text("");//removendo o texto flamebrust
                StartCoroutine(DelayedFor());
                //flameturbine();
                flameturbine_trigger = false;
                activateSpell = 0;
            }else { activateSpell += Time.deltaTime; }
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Rpc_text("FlameTurbine");
            Rpc_playerAudio(1);
            flameturbine_trigger = true;
        }

        if (Input.GetKeyDown(KeyCode.B))// flameburst spell
        {
            Rpc_text("FlameBurst");
            Rpc_playerAudio(3);
            flameBrust_trigger = true;
        }
        if (flameBrust_trigger == true)
        {
            if (activateSpell > 2f)
            {
                Rpc_text("");//removendo o texto flamebrust

                flameBrust();
                flameBrust_trigger = false;
                activateSpell = 0;
            }
            else { activateSpell += Time.deltaTime; }
        }

        /*if (hellfire_trigger == true)// hellfire spell
        {
            if (activateSpell > 2f)
            {
                Cmdhellfire();
                hellfire_trigger = false;
                activateSpell = 0;
            }
            else { activateSpell += Time.deltaTime; }
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            hellfire_trigger = true;
        }*/

        if(ultimate_trigger == true)// ultimate stuff
        {
            if (activateSpell > 2f)
            {
                Rpc_text("");//removendo o devouring flame text


                devouringFlame();
                ultimate_trigger = false;
                activateSpell = 0;
            }
            else { activateSpell += Time.deltaTime; }
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            Rpc_text("Devouring Flame");
            Rpc_playerAudio(2);
            ultimate_trigger = true;
        }

        if(fenix_trigger == true)
        {
            if(activateSpell > 2f)
            {
                fenixSpell();
                fenix_trigger = false;
                activateSpell = 0;
            }
            else { activateSpell += Time.deltaTime; }
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            fenix_trigger = true;

        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Black Hole(Clone)")
        {
            takeDamage(1000);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.name == "Galaxy(Clone)")
        {
            takeDamage(1000);
            Destroy(collision.gameObject);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Black Hole(Clone)")
        {
            takeDamage(260);
            Destroy(other.gameObject);
        }
        if (other.gameObject.name == "Galaxy(Clone)")
        {
            takeDamage(350);
            Destroy(other.gameObject);
        }
    }
    
    [ClientRpc]
    void Rpc_text(string _text)//envia mensagens do boss
    {
        

        foreach(GameObject p in players)
        {
            Text txt = GameObject.Find("MeuTexto").GetComponent<Text>();
            txt.text = _text;
            if(_text == "Devouring Flame")
            {
                txt.color = Color.black;
            }
            if(_text == "FlameBurst")
            {
                txt.color = Color.red;
            }
            if(_text == "FlameTurbine")
            {
                txt.color = Color.yellow;
            }
            //FindObjectOfType<Text>().text = "nani ?";
        }
        
    }

    [ClientRpc]
    void Rpc_playerAudio(int audio)//tocador dos audios
    {
        audioSource.clip = audios[audio];
        audioSource.Play();
    }

}
