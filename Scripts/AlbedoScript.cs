using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.Networking;

public class AlbedoScript : NetworkBehaviour {
    bool isPaused = false;//variavel que checa se esta em foco

    int maxHealth = 3000;//hp maximo
    [SyncVar]
    int health;//hp atual
    
    
    public Slider healthSlider;
    Slider myslider;

    public GameObject boss;//boss atual
    
    public GameObject galaxySpell;
    public GameObject blackHoleSpell;
    bool galaxy_trigger=false;
    bool black_trigger = false;

    public GameObject aura;
    float auraCoolDown =0f;


    //Coisas do cursor
    public Texture2D cursorTexture;
    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero;
    

    bool cursorSelected = false;//trigger para spells instantaneas no boss (m)
    public GameObject Firstspell;
    Rigidbody playerRigidbody;
    float timer;
    Vector3 newPos = new Vector3(0, 0, 0);
    bool invunerable = false;

    //ativadores das spells
    bool spellAttack1 = false;


    public override void OnStartLocalPlayer()//mandando meu nome para o servidor
    {
        Cmd_SetMyId("Player" + GetComponent<NetworkIdentity>().netId.ToString());


        /*try
        {
            healthSlider = GameObject.Find("HealthSlider").GetComponent<Slider>();

            myslider = (Slider)Instantiate(healthSlider, healthSlider.transform);
            myslider.transform.name = "NovoSlider";
        }
        catch
        {
            healthSlider = GameObject.Find("NovoSlider").GetComponent<Slider>();

            myslider = (Slider)Instantiate(healthSlider, healthSlider.transform);
            myslider.transform.name = "NovoSlider";
        }

        myslider.transform.parent = GameObject.Find("HealthUI").GetComponent<Transform>();

        healthSlider.maxValue = maxHealth;
        healthSlider.value = healthSlider.maxValue;*/
        

    }
    public override void OnStartClient()//atualizando meu nome na propria maquina
    {
        transform.name = "Player" + GetComponent<NetworkIdentity>().netId.ToString();
    }


    // Use this for initialization
    void Start () {


        //myslider = (Slider)Instantiate(healthSlider,healthSlider.transform);
        //myslider.transform.name = "NovoSlider";

        //myslider.transform.parent =  GameObject.Find("HealthUI").GetComponent<Transform>();

        

        //myslider.transform.position = new Vector2(120,Screen.height - 250);



        playerRigidbody = GetComponent<Rigidbody>();
        health = maxHealth;
        
    }
    
    // Update is called once per frame
    void Update () {

        if (!isLocalPlayer)
            return;

        if(health <= 0)
        {
            Destroy(gameObject);
        }


        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject p in Players)
        {
            if(p.GetComponent<AlbedoScript>().health <= 0)
            {
                Destroy(p);
            }
        }

        timer += Time.deltaTime;
        

        if(health > maxHealth)
        {
            health = maxHealth;
        }
        if(invunerable == true)//tirando invunerabilidade
        {
            auraCoolDown += Time.deltaTime;
            if(auraCoolDown > 1.5f)
            {
                invunerable = false;
                aura.SetActive(false);
                auraCoolDown = 0;
            }
        }

        //gambiarra para ele nao voar
        Vector3 posicao = transform.position;
        posicao.y = 0;
        transform.position = posicao;

        if (isPaused == false)//detecta se o jogo esta em foco
        {
            //rotacao do personagem
            RaycastHit pontoRotacional;
            Ray raio = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(raio, out pontoRotacional))
            {
                transform.LookAt(pontoRotacional.point);//olhando mouse            
                gameObject.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);//eixos n y = 0
            }


            movimentacao();
            targetting();

            /*Cmd_BlackHole();
            Cmd_Galaxy();*/
            //Cmd_spellCast();
            
        }

        if (Input.GetKey(KeyCode.D))
        {
            invunerable = true;
            aura.SetActive(true);

        }

        if (Input.GetKey(KeyCode.M))
        {
            cursorSelected = true;
        }

        if (Input.GetKey(KeyCode.Z))
        {
            spellAttack1 = true;
        }

        /*if (Input.GetKey(KeyCode.K))//spell
        {
            if(timer > 1.5f)
            {
                timer = 0;
                GameObject spell = Instantiate(Firstspell, transform);
                //Instantiate(Firstspell, new Vector3(transform.position.x,transform.position.y,transform.position.z),pontoRotacional.transform.rotation);
                
            }
        }*/
        if (Input.GetKey(KeyCode.Q))
        {
            galaxy_trigger = true;
            //Cmd_Galaxy();
        }
        if (Input.GetKey(KeyCode.W))
        {
            black_trigger = true;
            //Cmd_BlackHole();
        }

        transform.position = Vector3.MoveTowards(transform.position, newPos, 0.30f);//andar
    }
    [Command]
    void Cmd_SetMyId(string newName)
    {
        transform.name = newName;
        
    }

    /*[Command]
    public void Cmd_spellCast()
    {
        if(spellAttack1 == true)
        {
            NetworkServer.Spawn( (GameObject) Instantiate(boss, new Vector3(0, 25, -60), Quaternion.identity));
            spellAttack1 = false;
        }
    }*/


    [Command]
    void Cmd_Galaxy()
    {

        /*var bullet = (GameObject)Instantiate(
             galaxySpell,
             new Vector3(transform.position.x, transform.position.y + 7, transform.position.z),
             Quaternion.identity);*/

        var bullet = (GameObject)Instantiate(
             galaxySpell,
             new Vector3(transform.position.x, transform.position.y + 7, transform.position.z),
             transform.rotation);

        bullet.GetComponent<Rigidbody>().velocity = transform.forward * 15;
        NetworkServer.Spawn(bullet);

        
        timer = 0;
        Destroy(bullet, 5.0f);
    }

    [Command]
    void Cmd_BlackHole()
    {
        /*var bullet = (GameObject)Instantiate(
             blackHoleSpell,
             new Vector3(transform.position.x, transform.position.y + 7, transform.position.z),
             Quaternion.identity);*/
        var bullet = (GameObject)Instantiate(
             blackHoleSpell,
             new Vector3(transform.position.x, transform.position.y + 7, transform.position.z),
             transform.rotation);

        bullet.GetComponent<Rigidbody>().velocity = transform.forward * 15;
        NetworkServer.Spawn(bullet);

        
        timer = 0;
        Destroy(bullet, 5.0f);
    }


    public void targetting()//spells target on boss
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))//raycasting no ponto para saber se ha algo
        {
            if (hit.collider.name == "ragnaros" || hit.collider.name == "ragnaros(Clone)")//cliquei no boss
            {
                Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
                if (Input.GetKey(KeyCode.Mouse0))//clicou para ativar spell
                {
                    if (timer > 2 && cursorSelected == true)//so dando o cooldown da spell
                    {
                        Instantiate(Firstspell, hit.transform);//criando o meteoro
                        hit.collider.gameObject.SendMessage("takeDamage", 33);
                        timer = 0;
                        cursorSelected = false;
                    }

                    if (timer > 2 && galaxy_trigger == true)//spell da galaxia
                    {
                        Cmd_Galaxy();
                        galaxy_trigger = false;

                    }

                    if (timer > 2 && black_trigger == true)//spell do buraco negro
                    {
                        Cmd_BlackHole();
                        black_trigger = false;
                    }

                }
            }

            else
            {
                Cursor.SetCursor(null, hotSpot, cursorMode);
            }
        }
    }

    public void movimentacao()
    {
        
        if (Input.GetKey(KeyCode.Mouse1))//movimentacao do usuario
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                newPos = hit.point;
                //print(hit.collider.name + " toquei vc");
            }
            cursorSelected = false;
            galaxy_trigger = false;
            black_trigger = false;
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*if (collision.gameObject.name != "bush" || collision.gameObject.name != "Plane")
        {
            Debug.Log(collision.gameObject.name);
        }*/
        if (collision.transform.name == "FireballCollider")
        {
            harm(250);
            Destroy(collision.gameObject);

        }
        if (collision.gameObject.name == "Flamethrower(Clone)")
        {
            harm(1000);
            Destroy(collision.gameObject);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.gameObject.name != "bush" || other.gameObject.name != "Plane")
        {
            Debug.Log(other.gameObject.name);
        }*/
        if (other.transform.name == "FireballCollider")
        {
            harm(250);
            Destroy(other.transform.parent.gameObject);
            //Destroy(other.gameObject);
            
        }
        if (other.gameObject.name == "Flamethrower(Clone)")
        {
            harm(500);
            Destroy(other.gameObject);

        }
    }

    void harm(int amount)
    {
        if (invunerable == false)
        {
            health -= amount;
        }
        //myslider.value -= amount;
        //healthSlider.value -= amount;
    }
    

    private void OnGUI()
    {
        /*if (!isLocalPlayer)
        {
            return;
        }*/
        

        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);//conversao da transform do espaco 3d para 2d
        
        GUIStyle healthStyle = new GUIStyle(GUI.skin.box);//green part of the health
        GUIStyle backStyle = new GUIStyle(GUI.skin.box);//harm part of the health        

        healthStyle.normal.background = MakeTex(2, 2, new Color(0f, 1f, 0f, 1f));
        backStyle.normal.background = MakeTex(2, 2, new Color(1f, 0f, 0f, 1.0f));

        //healthSlider.foxyst vascodagama123
        
        GUI.Box(new Rect(pos.x - 26, Screen.height - pos.y + 20, maxHealth / 50, 7), "popopo", backStyle);
        if (health > 0)
        {
            GUI.Box(new Rect(pos.x - 25, Screen.height - pos.y + 21, health / 50, 5), "poker face", healthStyle);
        }


        //Vector3 cursorPoint = Camera.main.WorldToScreenPoint(Input.mousePosition);
        //GUI.Box(new Rect(cursorPoint.x - 25, Screen.height - cursorPoint.y + 21, health / 50, 5), "poker face", healthStyle);

        if (isLocalPlayer)
        {
            GUIStyle greenColor = new GUIStyle();//pintando o texto hp de verde
            greenColor.normal.textColor = Color.green;
            GUI.Label(new Rect(50, 120, 100, 50), "Albedo HP:" + health + "/" + maxHealth, greenColor);
        }
        
        if (cursorSelected == true)
        {
            GUI.Label(new Rect(70, 90, 100, 20), "Selecao Ativa");
            
        }
        if(black_trigger == true)
        {
            GUI.Label(new Rect(70, 90, 100, 20), "Black Hole active");
        }
        if (galaxy_trigger == true)
        {
            GUI.Label(new Rect(70, 90, 100, 20), "Galaxy active");
        }
    }

    private void OnApplicationFocus(bool focus)//serve para checar se a página está em foco
    {
        isPaused = !focus;
    }

    Texture2D MakeTex(int width, int height, Color col)//method used to paint the health bar
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
}
