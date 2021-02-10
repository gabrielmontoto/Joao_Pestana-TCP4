using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;
using Pathfinding;

public class Inimigo : Personagem_Base //tipo de script base
{

    Jogador jogador; //pra pegar as propriedades do jogador
    float contadorAtaque = 3; // contador pro ataque fisico setado em 3 permitindo que ele ja ataque

    //public Collider2D cabeca;// colisores de cabeca perna peito(bracos)


    [SerializeField] Vector2 posicaoCaminhar; // um valor inicial de onde o personagem vai poder caminhar

    [SerializeField]
    float distanciaDoRadar; //distancia pra perceber o jogador

    public bool distancia;
    // public bool virarHorizontal;

    public GameObject armaASerSpawnada;
    public GameObject PowerUpASerSpawnado;
   // public float velocidadeDoTiro;
    [SerializeField]
    GameManager gm;

    public short velocidadeInicial;

    Seeker seeker;
    Path path;
    int waypointAtual = 0;
   // float distanciaAteOWaypoint = 0.83f;
    bool chegouAoUltimoWaypoint = false;
    [SerializeField]
    Vector3 alvo;
   
    public GameObject graficos;

    public Animator animator;

    private Spawn spawn;

    public bool podeAtacar = true;

    [Space]
    [Header("Distancia")]
    [SerializeField] GameObject balaPrefab;
    [SerializeField] float VelocidadeTiro;
    public GameObject saidaDoTiro;
    public GameObject ObjetoASerDisparado;
    public bool esquerda;

    [SerializeField][Range(-180,180)]
    float temporario1, temporario2;

    private bool EstaMorto;

    [SerializeField] CircleCollider2D boxCol;


    [Header("Animacoes")]
    [Space]
    [SerializeField] string movLado;
    [SerializeField] string movCima, moveBaixo, danoLado, danoCima, danoBaixo, morteCima, morteLado, morteBaixo, ultimoEstado, ataqueLado, ataqueCima, ataqueBaixo;



    #region audio
    [Header("Audio")]
    [Space]

    [SerializeField] string SomAtaque;
    [SerializeField] string SomMover, somReceberDano, SomMorrer;


    #endregion
    // Start is called before the first frame update
    void Start()
    {
        boxCol = GetComponent<CircleCollider2D>();
        jogador = FindObjectOfType<Jogador>();  //procurar na cena algo do tipo jogador
        gm = FindObjectOfType<GameManager>();
       // InsereVelocidade(400);                    //inserir velocidade
        //InsereVidaMaxima(20);                   //inserir a vida maxima que vai atualizar a vida atual automaticamente
        InsereVida(RetornaVidaMaxima());
        //InsereAtaqueFisico(5);                  //inserir ataque do zumbi
       // InsereNome("Inimigo Sombra Fisico");                 //nome do zumbi
        velocidadeInicial = RetornaVelocidade();

      /*  if (distancia == true)
        {
            InsereDistanciaDeAtaque(1.7f);
        }
        else if (distancia == false)
        {
            InsereDistanciaDeAtaque(0.83f);             //distancia do ataque do zumbi ao jogador
        }*/
        InsereTempoEntreAtaques(3f);
        rigidbody2D = GetComponent<Rigidbody2D>(); //o rigidibody recebe o componente vinculado ja a ele
        barraDeVida.ColocarAVidaMaxima(RetornaVidaMaxima());
        barraDeVida.AtualizarVidaSlider(RetornaVidaMaxima());
        //  estados = Estados.Parado;

        posicaoCaminhar = new Vector2(Random.Range(rigidbody2D.position.x - 10, rigidbody2D.position.x + 10), 0);
        //a posicao recebe um valor randomico em X do ponto em que ele for instanciado pra andar pra direita e esquerda
        //so pra nao ficar parado esperando o player e ter algo tipo vida

        seeker = GetComponent<Seeker>();
        alvo = GameObject.FindGameObjectWithTag("Jogador").transform.position;
        
        InvokeRepeating("UpdatePath", 0f, 0.5f);
        animator = GetComponentInChildren<Animator>();
        spawn = FindObjectOfType<Spawn>();

        if(graficos.GetComponent<SpriteRenderer>().flipX == true)
        {
            esquerda = false;
        }
        else
        {
            esquerda = true;
        }
        EstaMorto = false;
       
    }
    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rigidbody2D.position, alvo, PathCompleto);
        }

        Morrer();
    }

    void PathCompleto(Path p)
    {
        if (!p.error)
        {
            path = p;
            waypointAtual = 0;
        }
    }
    public Vector3 RetornaAlvo()
    {
        return alvo;
    }

    private void Girar()
    {
        // Quaternion rotation;
        Vector2 direcao;
        float angulo;

        if (animator.GetBool("Baixo"))
        {

            //    if(graficos.transform.eulerAngles.z>332 || graficos.transform.eulerAngles.z<32)
            

                direcao = graficos.transform.position - alvo;
                angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg + 100;
                rigidbody2D.rotation = angulo;
          //  graficos.GetComponent<SpriteRenderer>().flipY = true;
                // print(rotation.eulerAngles);

                //quando cima for true -> +90 em z
                //quando for lado ta ok
                //quando for baixo -> - 90 em z

                //graficos.transform.LookAt(graficos.transform.position, alvo);
            
        }
        else if (animator.GetBool("Lado"))
        {
          //  graficos.GetComponent<SpriteRenderer>().flipY = false;
            //esquerda =  normal
            //direita = -90
            direcao = graficos.transform.position - alvo;
            if (alvo.x < graficos.transform.position.x)
            {

                angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;

            }
            else //(alvo.x > graficos.transform.position.x)
            {
                angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg - 180;
            }
           /* else
            {
                angulo = 0;
            }*/

            rigidbody2D.rotation = angulo;
        }
        else if (animator.GetBool("Cima"))
        {
           // graficos.GetComponent<SpriteRenderer>().flipY = false;
            direcao = graficos.transform.position - alvo;
            angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg - 90;
            rigidbody2D.rotation = angulo;

        }

       
    }



    void FixedUpdate()
    {
        if (jogador.gameObject.activeSelf != false) // se houver um jogador na cena
        {
            alvo = GameObject.FindGameObjectWithTag("Jogador").transform.position;
            Perseguir();
            TrocaAnimacoes();
            saidaDoTiro.transform.rotation = graficos.transform.rotation;


            if (Vector2.Distance(jogador.transform.position, rigidbody2D.transform.position) < RetornaDistanciaDeAtaque() && distancia)
            {
                Girar();

            }
            else if(distancia == false)
            {
                graficos.transform.rotation = Quaternion.Euler(0,180,0);
               // graficos.GetComponent<SpriteRenderer>().flipX = true;
            }
           
            else
            {
                graficos.transform.rotation = new Quaternion(0, 0, 0, 0);
            }
        }


    }

    private void AnimacaoDistancia()
    {
        if(graficos.transform.eulerAngles.z <202 && graficos.transform.eulerAngles.z>146)
        {
            /*
             *             animator.SetBool("Cima", true);
            animator.SetBool("Baixo", false);
            animator.SetBool("Lado", false);
             * 
             * */
            animator.SetBool("Baixo", true);
            animator.SetBool("Lado", false);
            animator.SetBool("Cima", false);
        }
        else if (graficos.transform.eulerAngles.z < 107 && graficos.transform.eulerAngles.z > 36)
        {
            animator.SetBool("Baixo", false);
            animator.SetBool("Lado", false);
            animator.SetBool("Cima", true);
        }
        else
        {
            animator.SetBool("Baixo", false);
            animator.SetBool("Lado", true);
            animator.SetBool("Cima", false);
        }

        print(animator.GetBool("Baixo") + " /" + animator.GetBool("Cima") + " /" + animator.GetBool("Lado"));
    }

    private void TocarAnimacoes(string posicao)
    {
        if (posicao != ultimoEstado)
        {
            animator.Play(posicao);
            ultimoEstado = posicao; //ultimaPos serve pra dizer se foi cima baixo ou lado
        }
    }

    private void Perseguir()
    {


                                                 //faz a movimentacao*/
        if (path == null)
        {
            return;
        }


        if (waypointAtual >= path.vectorPath.Count)
        {
            chegouAoUltimoWaypoint = true;
            return;
        }
        else
        {
            chegouAoUltimoWaypoint = false;
        }

        Vector2 direcao = ((Vector2)path.vectorPath[waypointAtual] - rigidbody2D.position).normalized;
        Vector2 force = direcao * RetornaVelocidade() * Time.deltaTime;

        SoundManager.Instance.PlayEfeito(SomMover);

        float distancia = Vector2.Distance(rigidbody2D.position, path.vectorPath[waypointAtual]);


        if (distancia < RetornaDistanciaDeAtaque())
        {
            waypointAtual++;
        }

        /*   if (Vector2.Distance(rigidbody2D.position, alvo) < RetornaDistanciaDeAtaque())
           {
               atacar(alvo);
           }*/
        else
        {
            rigidbody2D.AddForce(force);
        }



    }

    private void TrocaAnimacoes()
    {
       // Debug.Log(rigidbody2D.velocity.x + " Velocidade em x / velocidade em y " + rigidbody2D.velocity.y);
        if (rigidbody2D.velocity.y > 0.1f && rigidbody2D.velocity.x < rigidbody2D.velocity.y)
        {
            animator.SetBool("Cima", false);
            animator.SetBool("Baixo", true);
            animator.SetBool("Lado", false);
            TocarAnimacoes(moveBaixo);
           // Debug.Log("teste 1 ---");

        }
        else if (rigidbody2D.velocity.y < -0.1f && rigidbody2D.velocity.x > rigidbody2D.velocity.y)
        {
            animator.SetBool("Cima", true);
            animator.SetBool("Baixo", false);
            animator.SetBool("Lado", false);
            TocarAnimacoes(movCima);
           // Debug.Log("teste 2 ---");
        }

        else if (rigidbody2D.velocity.x > 0.1f && rigidbody2D.velocity.x > rigidbody2D.velocity.y)
        {
            animator.SetBool("Cima", false);
            animator.SetBool("Baixo", false);
            animator.SetBool("Lado", true);
            TocarAnimacoes(movLado);
         //   Debug.Log("teste 3 ---");
            GirarAnimacoes();
        }
        else if(rigidbody2D.velocity.x < -0.1f && rigidbody2D.velocity.x < rigidbody2D.velocity.y)
        {
            animator.SetBool("Cima", false);
            animator.SetBool("Baixo", false);
            animator.SetBool("Lado", true);
           TocarAnimacoes(movLado);
          //  Debug.Log("teste 4 ---");
            GirarAnimacoes();
        }
    }

    public void GirarAnimacoes()
    {
        if (distancia)
        {
            if (rigidbody2D.velocity.x >= 0.1f)
            {
                //  graficos.transform.localScale = new Vector3(3, 3, 1);
                graficos.GetComponent<SpriteRenderer>().flipX = false;
                esquerda = false;
            }
            else if (rigidbody2D.velocity.x <= -0.1f)
            {
                esquerda = true;
                graficos.GetComponent<SpriteRenderer>().flipX = true;
                //  graficos.transform.localScale = new Vector3(-3, 3, 1);
            }
        }
        else
        {

            if (rigidbody2D.velocity.x >= 0.1f)
            {
                
                //    graficos.transform.localScale = new Vector3(3, 3, 1);
                graficos.GetComponent<SpriteRenderer>().flipX = false;
                  esquerda = false;
            }
            else if (rigidbody2D.velocity.x <= -0.1f)
            {
                
                  esquerda = true;
                graficos.GetComponent<SpriteRenderer>().flipX = true;
                //   graficos.transform.localScale = new Vector3(-3, 3, 1);
            }
        }
    }
    public void atacar(Vector2 alvo) // funcao de atacar e causar dano
    {
        if (Vector2.Distance(this.transform.position, alvo) <= RetornaDistanciaDeAtaque() && podeAtacar && jogador.gameObject.activeSelf == true) //se a distancia entre o zumbi e o algo (player( for menor que a distancia de ataque do zumbi ele vai atacar
        {
            contadorAtaque += Time.fixedDeltaTime; // contador pra saber se estar permitido dar o dano

            if (contadorAtaque >= RetornaTempoEntreAtaques() && !distancia) //se o contador for maior que o tempo estipulado esta permitido atacar
            {
              //  jogador.acessoDanoRecebido(RetornaAtaque()); // da o dano no jogador

                contadorAtaque = 0; // zera o contaador
                animator.SetTrigger("Atacar");

                InsereVelocidade(0);
            }
            else if (contadorAtaque >= RetornaTempoEntreAtaques() && distancia) //se o contador for maior que o tempo estipulado esta permitido atacar
            {

             /*   GameObject bala = Instantiate(ObjetoASerDisparado, saidaDoTiro.transform.position, saidaDoTiro.transform.rotation); //intanciar a bala pelo prefab, no objeto vazio dizendo onde vai sair o tiro e a rotacao desse objeto
                bala.tag = "ProjetilZumbi";
                bala.GetComponent<Rigidbody2D>().AddForce(saidaDoTiro.transform.right * -1 * velocidadeDoTiro, ForceMode2D.Impulse);
                //jogador.acessoDanoRecebido(RetornaAtaque()); // da o dano no jogador
                */
                contadorAtaque = 0; // zera o contaador
            }

        }
    }

    public void ResetarVelocidade()
    {
        InsereVelocidade(velocidadeInicial);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.transform.CompareTag("Projetil"))//se o zumbi colidir com algo com a tag projetil
        {

            ReceberDano(jogador.RetornaDanoArma()); //ele vai recber dano
            barraDeVida.AtualizarVidaSlider(RetornaVida()); //atualiza o slider
            TrocaAnimacoes();
            
            Destroy(collision.gameObject); // destroi a bala

            // Morrer();//se a vida for menor que 0 ele morre
        }


    }



    public override void ReceberDano(short dano)
    {
        if(RetornaVida()>0)
        animator.SetTrigger("ReceberDano");
       // else
           // Morrer();
        base.ReceberDano(dano);
        SoundManager.Instance.PlayEfeito(somReceberDano);
       
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.layer != 11)
            transform.position = new Vector2(spawn.NovaPosicao().x, spawn.NovaPosicao().y);
    }
    private void Update()
    {
        
    /*    if(boxCol.IsTouchingLayers(11) == false && RetornaVida()>0)
        {
            print(boxCol.IsTouchingLayers());
            transform.position = new Vector2(spawn.NovaPosicao().x,spawn.NovaPosicao().y);
        }*/
        
       

        
        
    }

    void OnDrawGizmosSelected()
    {


        Gizmos.DrawWireSphere(graficos.transform.position, RetornaDistanciaDeAtaque()) ;
        //Gizmos.DrawCube(graficos.transform.position,new Vector3(RetornaDistanciaDeAtaque() +1.5f,0.5f,1));
    }

    public override void Morrer()
    {

        if(RetornaVida()<=0 && EstaMorto == false)
        {
            gm.AdicionaPontos(50);
            AnimacaoDeMorte();
            graficos.GetComponent<BoxCollider2D>().enabled = false;
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            EstaMorto = true;

            SoundManager.Instance.PlayEfeito(SomMorrer);
            Destroy(graficos.GetComponent<AtaqueInimigo>().pai, 3);
                       /// morte();
        }
    }

    private void AnimacaoDeMorte()
    {
        animator.SetTrigger("Morte");
    }

    public void InstanciarPowerUpEArmas()
    {
        GameObject objeto = Instantiate(armaASerSpawnada, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);// Resources.Load("Prefabs/ArmaPlaceHolder");
        GameObject powerUp = Instantiate(PowerUpASerSpawnado, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);

    }

    public GameObject RetornaBalaPrefab()
    {
        return balaPrefab;
    }
   public float RetornaVelocidadeBala()
    {
        return VelocidadeTiro;
    }

    public string retornaSomAtaque()
    {
        return SomAtaque;
    }
}

//enum Estados {Atacar,Andando,Parado} // estados da maquina de estaddos