using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class Jogador : Personagem_Base
{


    float moverHorizontal, moverVertical; // movimentaçao horizontal é o valor recebido do joystick pra saber se esta indo pra direita ou esquerda
    // a vertical pro joystick do tiro pra saber se esta mirando pra cima ou pra baixo
   public GameObject objetoMira; // objeto vazio no qual esta vinculado a arma pra fazer ela girar
    public GameObject saidaTiro,balaPrefab; // onde o tiro é intanciado e qual o prefab da bala
   // float contadorAtaque; // contador pra fazer o contdown do ataque fisico
  //  public BarraDeVida barraDeVida;
   // public BoxCollider2D ColisorDaArma; // colisor da arma

    private bool esquerda = false; // pra saber se o personagem esta olhando pra direita ou esquerda

    public Vector3 posAtaque; // posicao do ataque fisico
    public float raioDeAtaque; // raio do ataque fisico
    public LayerMask layerDeAtaque; // layer em que o ataque fisico esta sendo realizado

    Animator animator; // animator
    private bool morto = false;

    #region testeBalasMultiplas
    [Header("Tiros Multiplos")]
    [Space]
    [SerializeField]
    float anguloDasBalas;
    [SerializeField]
    short quantidadeDeBalas;
    #endregion

    #region arma
    [Header("Arma")]
    [Space]
    public bool tirosInfinitos;


    [SerializeField] private float contadorAtirar; // contador pro tiro da bala

    [SerializeField]
    private string nomeArma1, nomeArma2, nomeArmaAtual; //nome da arma vindo do script da arma
    [SerializeField]
    private Sprite imagemArma1, imagemArma2, imagemArmaAtual;//imagem da arma vindo do script da arma
    [SerializeField]
    private short danoArma1, danoArma2, danoArmaAtual;//dano da arma vindo do script da arma
    [SerializeField]
    private short balasMaximasArma1, balasMaximasArma2, balasMaximasArmaAtual;//quantidade maxima de balas da arma vindo do script da arma
    [SerializeField]short balasAtuais;
    [SerializeField]
    private float velocidadeDeAtirarArma1, velocidadeDeAtirarArma2, velocidadeDeAtirarArmaAtual;//velocidade de atirar cada bala da arma vindo do script da arma
    [SerializeField]
    private float velocidadeDoTiro1, velocidadeDoTiro2, velocidadeDoTiroAtual; //velocidade de cada bala da arma vindo do script da arma
    [SerializeField]
    short quantidadeDeTiros1, quantidadeDeTiros2, quantidadeDeTirosAtual;
    public Image imagem1, imagem2, selecionada1, selecionada2;

    public TextMeshProUGUI textoBalas;
    #endregion

    #region PowerUp
    [SerializeField]
    float aumentDano;
    [SerializeField]
    float aumentoVelocidadeDeAtirar, aumentoVelocidadeTiro;
    
    bool invensivel;

    public Canvas_PowerupManager powerupManager;
    #endregion

    #region especial
    [Header("Especial")]
    [Space]
    [SerializeField]
    BarraDeVida especial;
    [SerializeField]
    float BarraEspecialMax, BarraEspecialAtual, fatorMultEspecial;

    #endregion
    public Joystick joystick,joysticMira; // joystick de movimentacao e de mirar
    private Vector3 guardarMira;
    private Inimigo inimigo;
    // Start is called before the first frame update

    [SerializeField] [Space] [Header("Arma equipada")] SpriteRenderer spriteRenderArmaAtual;
    [SerializeField] Sprite spriteRenderArma1, spriteRenderArma2;

    [SerializeField] string movLado, movCima, moveBaixo, danoLado,danoCima,danoBaixo,morteCima,morteLado,morteBaixo,ultimoEstado;
    [SerializeField] bool tomouDano;
    // [SerializeField] Light2D light;

    [SerializeField] bool atirando;
    [SerializeField] bool esquerda_a, direita, cima, baixo;


    #region audio
    [Header("Audio")]
    [Space]
    
    [SerializeField]string SomTiro;
    [SerializeField] string SomMover,somReceberDano,SomEspecial,SomMorrer, somTrocaArma;


    #endregion

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>(); // intanciamento do rigidbody
        spriteRenderer = GetComponent<SpriteRenderer>();// intancianemtno do spriterender
        InsereVidaMaxima(100); 
        InsereVelocidade(2);
        InsereVida(RetornaVidaMaxima());
        InsereAtaqueFisico(10);
        InsereTempoEntreAtaques(3f);
        InsereNome("Jogador");
        barraDeVida.ColocarAVidaMaxima(RetornaVidaMaxima());
        barraDeVida.AtualizarVidaSlider(RetornaVidaMaxima());

        this.gameObject.tag = RetornaNome();
        animator = GetComponent<Animator>();// instanciamento do animator
        guardarMira = Vector3.zero;
        inimigo = FindObjectOfType<Inimigo>();
        aumentoVelocidadeTiro = 1;
        especial.ColocarAVidaMaxima(BarraEspecialMax);
        BarraEspecialAtual = 0;
        especial.AtualizarVidaSlider(BarraEspecialAtual);

    }

    // Update is called once per frame
    void FixedUpdate()//utilizacao do fixed update pq ele fica melhor pra ter uma movimentacao mais fluida
    {
        if (morto == false)
        {
            Movimentacao(); //chamando o metodo de movimentacao

            Mirar(); // chamando metodo de mirar
        }
    }
     void Update()
    {
       // LimitesMirar(); 
        Morrer();



        if (contadorAtirar < 5 && morto == false) // contadores recebendo seus valores, eles param no 5 pra nao ficar somando ate infinito caso alguem esqueca o jogo aberto
        {
            contadorAtirar += Time.fixedDeltaTime;
        }


        if(Input.GetKeyDown(KeyCode.P))
        {
            Especial();
        }
        
    }



    void OnDrawGizmosSelected() //esse é pra fazer um desenho do circulo do que vai servir de colisao na tela da cena
    {
      //  Vector3 temporario = ColisorDaArma.gameObject.transform.position; //mesma logica de cima
      //  temporario += transform.right * posAtaque.x;
        //temporario += transform.up * posAtaque.y;

        //Gizmos.DrawWireSphere(temporario, raioDeAtaque); // desenha uma esfera na posicao do temporario e com o raio do raio de ataque
    }
    public void MiraAtirarPertada()
    {

        if (joysticMira.Direction != Vector2.zero)
        {
            atirando = true;
            
        }
        else
        {
            atirando = false;
        }
    }
        private void Mirar() // movimentaçao da mira
    {

        MiraAtirarPertada();

        Vector3 temp;
        temp = new Vector3(0,180, (Mathf.Atan2(joysticMira.Direction.x, joysticMira.Direction.y) * Mathf.Rad2Deg)+90);

        #region limites
        if (direita)
        {
            if(temp.z >=200)
            {
                temp = new Vector3(0, 180, 200);
            }

            if(temp.z <=130)
            {
                temp = new Vector3(0, 180, 130);
            }

        }
        else if(esquerda_a)
        {
            if (temp.z >= 32)
            {
                temp = new Vector3(0, 180, 32);
            }

            if (temp.z <= -31)
            {
                temp = new Vector3(0, 180, -31);
            }
        }
        else if(cima)
        {
            if (temp.z >= 115)
            {
                temp = new Vector3(0, 180, 115);
            }

            if (temp.z <= 42)
            {
                temp = new Vector3(0, 180, 42);
            }
        }
        else if(baixo)
        {
            if (temp.z <= 215 && temp.z>120)
            {
                temp = new Vector3(0, 180, 215);
              
            }

            if (temp.z >= -60 && temp.z< 210)
            {
                temp = new Vector3(0, 180, -60);
                
            }
        }

        #endregion

        //Debug.Log(temp.z);
        if(guardarMira != temp && atirando)
        {
            guardarMira = temp;
        }
        if (contadorAtirar >= velocidadeDeAtirarArmaAtual - aumentoVelocidadeDeAtirar && atirando) // e o contador do tiro maior que a velocidade de poder atirar da arma
        {
            Atirar(); //chama metodo de atirar
            contadorAtirar = 0; // zera o contador de atirar
            SoundManager.Instance.PlayEfeito(SomTiro);
            // atirando = true;
        }

        objetoMira.transform.eulerAngles = guardarMira;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.transform.CompareTag("Projetil_Inimigo"))//se o zumbi colidir com algo com a tag projetil
        {

            acessoDanoRecebido((short)collision.GetComponent<Projetil>().ReceberDanoDoProjetil()); //ele vai recber dano
            barraDeVida.AtualizarVidaSlider(RetornaVida()); //atualiza o slider
            Destroy(collision.gameObject); // destroi a bala
          //  Morrer();//se a vida for menor que 0 ele morre
        }

    }

    public override void Morrer()
    {
        if(RetornaVida()<=0 && morto == false)
        {
            morto = true;
            rigidbody2D.velocity = Vector2.zero;
            this.GetComponent<BoxCollider2D>().enabled = false;
            animator.ResetTrigger("TomarDano");
            animator.SetBool("MoverCima", false);
            animator.SetBool("MoverBaixo", false);
            animator.SetBool("MoverLado", false);
            animator.SetTrigger("Morrer");
            SoundManager.Instance.PlayEfeito(SomMorrer);
            //animator.Play(morteBaixo);
            
        }    
       // this.gameObject.SetActive(false);
        //base.Morrer();
    }
    public void _Morrer()
    {
        this.gameObject.SetActive(false);
    }

    private void Atirar() //ajeitar pra quando for tiro multiplo e quantos tiros multiplos forem
    {
        
        
        // bala.GetComponent<Rigidbody2D>().AddForce();
        
        

        #region testeTiroMultiplos
       // int i = 0;
        for (int i = 0; i < quantidadeDeTirosAtual; i++)
        {

            GameObject bala = Instantiate(balaPrefab, saidaTiro.transform.position, saidaTiro.transform.rotation); //intanciar a bala pelo prefab, no objeto vazio dizendo onde vai sair o tiro e a rotacao desse objeto
            int angleOffset;
            if (quantidadeDeTirosAtual%2 ==0) //pares
            {
               // angleOffset = i * 15 - (15); // colocar isso como uma variavel dos tiros multiplos
                if(i>((short)(quantidadeDeTirosAtual/2)-1))
                {
                    angleOffset = i * 15 -15;
                }
                else
                {
                    angleOffset = i * -15 - 15;// - (15 * (i - 1));
                }
            }
            else // impares
            {
              //  angleOffset = i * 15 - (15 * (short)(quantidadeDeTirosAtual/2)); // colocar isso como uma variavel dos tiros multiplos
                
                if (i == 0)
                {
                    angleOffset = 0;
                    
                }
                else if( i > ((short)(quantidadeDeTirosAtual / 2)))
                {
                    angleOffset = i * 15 - 15 * (short)(quantidadeDeTirosAtual / 2);
                }
                else
                {
                    angleOffset = i * -15;// - (15 * (i - 1));
                }
            }
            
            bala.GetComponent<Rigidbody2D>().AddForce(Quaternion.AngleAxis(angleOffset, Vector3.forward) *objetoMira.transform.right * velocidadeDoTiroAtual * aumentoVelocidadeTiro, ForceMode2D.Impulse);
            bala.transform.name ="bala "+ i;
        }


        #endregion



        if (!tirosInfinitos && imagemArmaAtual != imagemArma1)
        {
            balasAtuais--;
            textoBalas.text = "Balas: " + balasAtuais;
        }
        else
        {
            textoBalas.text = "Balas: ∞";
        }

        if(balasAtuais == 0)
        {
            balasMaximasArmaAtual = balasMaximasArma1;
            danoArmaAtual = danoArma1;
            imagemArmaAtual = imagemArma1;
            nomeArmaAtual = nomeArma1;
            velocidadeDeAtirarArmaAtual = velocidadeDeAtirarArma1;
            velocidadeDoTiroAtual = velocidadeDoTiro1;
            quantidadeDeTirosAtual = quantidadeDeTiros1;
            //selecionada2.gameObject.SetActive(false);
            selecionada1.gameObject.SetActive(true);
            imagem1.sprite = imagemArma1;
            textoBalas.text = "Balas: ∞";
            spriteRenderArmaAtual.sprite = spriteRenderArma1;
            
            balasMaximasArma2 = 0;
            danoArma2 = 0;
            imagemArma2 = null;
            nomeArma2 = "";
            velocidadeDeAtirarArma2 = 0;
            imagem2.sprite = null;

        }
        // a força que o projetil vai receber, depende da velocidade que vem da arma, multiplicada pelo transform do objeto, esse forcemod.impulse acho que faz com que faz ir pra frente de onde o tiro sair
    }
    public void _ResetTrigerAnimacao()
    {
        animator.ResetTrigger("TomarDano");
    }
    private void TocarAnimacoes(string posicao)
    {
        if(posicao != ultimoEstado)
        {
            animator.Play(posicao);
            ultimoEstado = posicao; //ultimaPos serve pra dizer se foi cima baixo ou lado
        }
    }

    private void Movimentacao() //movimentacao
    {
        moverHorizontal = joystick.Horizontal * RetornaVelocidade(); // a movimentacao do joystick recebe um valor vindo do joystick entre -1 e 1 e multiplica pela velocidade do jogador
        moverVertical = joystick.Vertical * RetornaVelocidade();

        if (math.abs(moverVertical) > math.abs(moverHorizontal))
        {
            this.rigidbody2D.velocity = Vector2.up * moverVertical;
            SoundManager.Instance.PlayEfeito(SomMover);
            if (rigidbody2D.velocity.y > 0)
            {

                animator.SetBool("MoverCima", true);
               animator.SetBool("MoverBaixo", false);
                animator.SetBool("MoverLado", false);
                cima = true;
                baixo = false;
                esquerda_a = false;
                direita = false;

                if (!atirando)
                {
                    guardarMira = new Vector3(0, 180, 90);
                    objetoMira.transform.eulerAngles = guardarMira;
                }
              if (!tomouDano)
                    TocarAnimacoes(movCima);
              else
                {
                    animator.Play(danoCima);
                    tomouDano = false;
                }

            }
            else
            {
                if (!atirando)
                {
                    guardarMira = new Vector3(0, 180, 270);
                    objetoMira.transform.eulerAngles = guardarMira;
                }
                baixo = true;
                cima = false;
                esquerda_a = false;
                direita = false;
                animator.SetBool("MoverBaixo", true);
                animator.SetBool("MoverCima", false);
                animator.SetBool("MoverLado", false);
              if(!tomouDano)
                    TocarAnimacoes(moveBaixo);
              else
                {
                    
                    animator.Play(danoBaixo);
                    tomouDano = false;
                }
            }
           // animator.ResetTrigger("TomarDano");
        }
        else if (math.abs(moverVertical) < math.abs(moverHorizontal))
        {
            this.rigidbody2D.velocity = Vector2.right * moverHorizontal; // o rigidbody recebe essa velocidade pra fazer o personagem se mover
            animator.SetBool("MoverCima", false);
            animator.SetBool("MoverBaixo", false);
            animator.SetBool("MoverLado", true);
            SoundManager.Instance.PlayEfeito(SomMover);

            if (!atirando && !esquerda)
            {
                guardarMira = new Vector3(0, 180, 180); //direita
                objetoMira.transform.eulerAngles = guardarMira;
                direita = true;
                cima = false;
                baixo = false;
                esquerda_a = false;
            }
            else if(!atirando && esquerda)
            {
                guardarMira = new Vector3(0, 180, 0); //esquerda
                objetoMira.transform.eulerAngles = guardarMira;
                esquerda_a = true;
                cima = false;
                baixo = false;
                direita = false;
            }

            if (!tomouDano)
                TocarAnimacoes(movLado);
          else
            {
                animator.Play(danoLado);
                tomouDano = false;
            }
           // animator.ResetTrigger("TomarDano");
        }
        else
        {
            this.rigidbody2D.velocity = Vector2.zero;
            if (!tomouDano)
            {
                animator.SetBool("MoverBaixo", false);
                animator.SetBool("MoverCima", false);
                animator.SetBool("MoverLado", false);
            }
            else
            {
                if (ultimoEstado == "AndarLado")
                {
                    animator.Play(danoLado);
                }
                else if(ultimoEstado == "AndarCima")
                {
                    animator.Play(danoCima);
                }
                else
                {
                    animator.Play(danoBaixo);
                }
                tomouDano = false;
            }
           
          //  animator.ResetTrigger("TomarDano");
        }
        FlipImagem();//faz a troca da imagem se vai pra esquerda ou direita
    }

    private void FlipImagem()
    {
        if(moverHorizontal > 0) //caso ele esteja se movendo pra direita o mover horizontal vai ser positivo
        {
            transform.eulerAngles = new Vector3(0, 0, 0); // o angulo é o valor original do sprite
            esquerda = false; //a booleana esquerda recebe falso
        }
        else if(moverHorizontal < 0) //caso ele esteja se movendo pra esquerda o mover horizontal vai ser negativo
        {
            transform.eulerAngles = new Vector3(0, 180, 0); // altera o angulo em y em 180 pra fazer o giro
            esquerda = true; //a booleana esquerda recebe verdadeiro
        }
    }

    #region armas

    public void acessoDanoRecebido(short danoRecebido) // metodo pra quando receber dano
    {
        if (invensivel == false)
        {
            ReceberDano(danoRecebido); //o metodo receber dano envia o dano recebido
            if (RetornaVida() - danoRecebido > 0)
            {
                CameraShake.Instance.ShakeCamera(1, 0.2f);
                barraDeVida.AtualizarVidaSlider(RetornaVida()); //atualiza o canvas da barra de vida
                tomouDano = true;
                animator.SetTrigger("TomarDano");
                SoundManager.Instance.PlayEfeito(somReceberDano);
            }
            
           // Morrer();
        }
    }



    public void EquiparArmaNInfinito(Armas arma) // pega as armas e armazena em cada slot, caso ambas estejam equipadas e se encontre uma nova a que estiver equipada é substituida
    {
        /*if (imagemArma2 == arma.imagem && imagemArma2 != null)
        {
             balasMaximasArma2 += arma.balasMaximas;
            textoBalas.text = "Balas: " + balasMaximasArmaAtual;
        }*/

        if (this.imagemArma1 == null) //se a imagem, foi usada a imagem pq foi o primeiro valor que achei que podia receber null
        {//caso nao possua imagem no slot1 ele recebe o valor da arma que encostar
            this.nomeArma1 = arma.nome;
            this.imagemArma1 = arma.imagem;
            danoArma1 = arma.danoArma;
            velocidadeDeAtirarArma1 = arma.velocidadeDeAtirar;
            balasMaximasArma1 = arma.balasMaximas;
            velocidadeDoTiro1 = arma.velocidadeDoTiro;
            quantidadeDeTiros1 = arma.quantidadeDeTiros;
            spriteRenderArma1 = arma.ArmaNoJogo;
            imagem1.sprite = imagemArma1;
            spriteRenderArmaAtual.sprite = spriteRenderArma1;
        }
        else if (this.imagemArma2 == null && arma.imagem != this.imagemArma1)
        {//caso nao possua imagem no slot2 ele recebe o valor da arma que encostar
            nomeArmaAtual = nomeArma2 = arma.nome;
            imagem1.sprite = imagemArmaAtual = imagemArma2 = arma.imagem;
            imagem2.sprite = imagemArma1;
            danoArmaAtual = danoArma2 = arma.danoArma;
            spriteRenderArmaAtual.sprite = spriteRenderArma2 = arma.ArmaNoJogo;

            velocidadeDeAtirarArmaAtual = velocidadeDeAtirarArma2 = arma.velocidadeDeAtirar;
            balasMaximasArmaAtual = balasMaximasArma2 += arma.balasMaximas;
            balasAtuais = balasMaximasArmaAtual;
            velocidadeDoTiroAtual = velocidadeDoTiro2 = arma.velocidadeDoTiro;
            quantidadeDeTirosAtual = quantidadeDeTiros2 = arma.quantidadeDeTiros;
           // selecionada2.gameObject.SetActive(true);
            selecionada1.gameObject.SetActive(true);
            
            textoBalas.text = "Balas: " + balasMaximasArmaAtual;


        }
        else if (this.imagemArma2 != null && this.imagemArma1 != null && arma.imagem != this.imagemArma1)
        {
            nomeArmaAtual = nomeArma2 = arma.nome;
            imagem1.sprite = imagemArmaAtual = imagemArma2 = arma.imagem;
            imagem2.sprite = imagemArma1;
            spriteRenderArmaAtual.sprite = spriteRenderArma2 = arma.ArmaNoJogo;
            danoArmaAtual = danoArma2 = arma.danoArma;
            velocidadeDeAtirarArmaAtual = velocidadeDeAtirarArma2 = arma.velocidadeDeAtirar;
            balasMaximasArmaAtual = balasMaximasArma2 = arma.balasMaximas;
            balasAtuais += balasMaximasArmaAtual;
            velocidadeDoTiroAtual = velocidadeDoTiro2 = arma.velocidadeDoTiro;
            quantidadeDeTirosAtual = quantidadeDeTiros2 = arma.quantidadeDeTiros;
          //  selecionada2.gameObject.SetActive(true);
            selecionada1.gameObject.SetActive(true);
            textoBalas.text = "Balas: " + balasAtuais;
        }


        if (imagemArmaAtual == null)
        {//faz o primeiro equip da arma
            imagemArmaAtual = arma.imagem;
            nomeArmaAtual = arma.nome;
            danoArmaAtual = arma.danoArma;
            velocidadeDeAtirarArmaAtual = arma.velocidadeDeAtirar;
            balasMaximasArmaAtual = arma.balasMaximas;
            velocidadeDoTiroAtual = arma.velocidadeDoTiro;
            quantidadeDeTirosAtual = arma.quantidadeDeTiros;
            spriteRenderArmaAtual.sprite = arma.ArmaNoJogo;
            selecionada1.gameObject.SetActive(true);
            selecionada2.gameObject.SetActive(false);
        }
    }
    public void EquiparArmaInfinito(Armas arma)
    {
        if (this.imagemArma1 == null) //se a imagem, foi usada a imagem pq foi o primeiro valor que achei que podia receber null
        {//caso nao possua imagem no slot1 ele recebe o valor da arma que encostar
            this.nomeArma1 = arma.nome;
            this.imagemArma1 = arma.imagem;
            danoArma1 = arma.danoArma;
            velocidadeDeAtirarArma1 = arma.velocidadeDeAtirar;
            balasMaximasArma1 = arma.balasMaximas;
            velocidadeDoTiro1 = arma.velocidadeDoTiro;
            quantidadeDeTiros1 = arma.quantidadeDeTiros;
            imagem1.sprite = imagemArma1;
        }
        else if (this.imagemArma2 == null && arma.imagem != this.imagemArma1)
        {//caso nao possua imagem no slot2 ele recebe o valor da arma que encostar
            this.imagemArma2 = arma.imagem;
            this.nomeArma2 = arma.nome;
            danoArma2 = arma.danoArma;
            velocidadeDeAtirarArma2 = arma.velocidadeDeAtirar;
            balasMaximasArma2 = arma.balasMaximas;
            velocidadeDoTiro2 = arma.velocidadeDoTiro;
            imagem2.sprite = imagemArma2;
            quantidadeDeTiros1 = arma.quantidadeDeTiros;
            
        }
        else if (this.imagemArma2 != null && this.imagemArma2 != null && imagemArma1 != arma.imagem)
        {//se ambas as armas estiverem equipadas 
            if (imagemArmaAtual == imagemArma1)
            {//aqui faz a substituiçao de uma arma nova que nao seja nenhuma das duas equipadas
                nomeArmaAtual = nomeArma1 = arma.nome;
                imagem1.sprite = imagemArmaAtual = imagemArma1 = arma.imagem;
                danoArmaAtual = danoArma1 = arma.danoArma;
                velocidadeDeAtirarArmaAtual = velocidadeDeAtirarArma1 = arma.velocidadeDeAtirar;
                balasMaximasArmaAtual = balasMaximasArma1 = arma.balasMaximas;
                velocidadeDoTiroAtual = velocidadeDoTiro1 = arma.velocidadeDoTiro;
                quantidadeDeTirosAtual = quantidadeDeTiros1 = arma.quantidadeDeTiros;


            }
            else if (imagemArmaAtual == imagemArma2)
            {
                nomeArmaAtual = nomeArma2 = arma.nome;
                imagem2.sprite = imagemArmaAtual = imagemArma2 = arma.imagem;
                danoArmaAtual = danoArma2 = arma.danoArma;
                velocidadeDeAtirarArmaAtual = velocidadeDeAtirarArma2 = arma.velocidadeDeAtirar;
                balasMaximasArmaAtual = balasMaximasArma2 = arma.balasMaximas;
                velocidadeDoTiroAtual = velocidadeDoTiro2 = arma.velocidadeDoTiro;
                quantidadeDeTirosAtual = quantidadeDeTiros2 = arma.quantidadeDeTiros;

            }
        }

        if (imagemArmaAtual == null)
        {//faz o primeiro equip da arma
            imagemArmaAtual = arma.imagem;
            nomeArmaAtual = arma.nome;
            danoArmaAtual = arma.danoArma;
            velocidadeDeAtirarArmaAtual = arma.velocidadeDeAtirar;
            balasMaximasArmaAtual = arma.balasMaximas;
            velocidadeDoTiroAtual = arma.velocidadeDoTiro;
            quantidadeDeTirosAtual = arma.quantidadeDeTiros;
            selecionada1.gameObject.SetActive(true);
            selecionada2.gameObject.SetActive(false);
        }
    }
    public void TrocarArma() // trocar as armas que ja foram pegas entre si
    {
        if (this.nomeArmaAtual == this.nomeArma1 && imagemArma2 != null)
        {
            nomeArmaAtual = nomeArma2;
            imagemArmaAtual = imagemArma2;
            danoArmaAtual = danoArma2;
            velocidadeDeAtirarArmaAtual = velocidadeDeAtirarArma2;
            balasMaximasArmaAtual = balasMaximasArma2;
            velocidadeDoTiroAtual = velocidadeDoTiro2;
            quantidadeDeTirosAtual = quantidadeDeTiros2;

            spriteRenderArmaAtual.sprite = spriteRenderArma2;
           // selecionada2.gameObject.SetActive(true);
            selecionada1.gameObject.SetActive(true);
            imagem1.sprite = imagemArma2;
            imagem2.sprite = imagemArma1;
            textoBalas.text = "Balas: " + balasAtuais;

            

        }
        else if (this.nomeArmaAtual == this.nomeArma2)
        {
            nomeArmaAtual = nomeArma1;
            imagemArmaAtual = imagemArma1;
            danoArmaAtual = danoArma1;
            velocidadeDeAtirarArmaAtual = velocidadeDeAtirarArma1;
            balasMaximasArmaAtual = balasMaximasArma1;
            velocidadeDoTiroAtual = velocidadeDoTiro1;
            quantidadeDeTirosAtual = quantidadeDeTiros1;
            selecionada1.gameObject.SetActive(true);
            spriteRenderArmaAtual.sprite = spriteRenderArma1;
            //  selecionada2.gameObject.SetActive(false);
            textoBalas.text = "Balas: ∞";
            imagem1.sprite = imagemArma1;
            imagem2.sprite = imagemArma2;

        }
        SoundManager.Instance.PlayEfeito(somTrocaArma);

    }

    private void AtualRecebeArma1()
    {

    }

    public short RetornaDanoArma() // retornar o dano da arma
    {
        return (short)(danoArmaAtual + aumentDano);
    }
    #endregion


    #region PowerUp
    public IEnumerator ReceberPowerUp(PowerUp1 powerup)
    {
        if (powerup.aumentaDano > 0)
        {
            this.aumentDano = powerup.aumentaDano; //>0   ---> 0
            powerupManager.AdicionarNaLista(0);
        }
        else if (powerup.aumentaVida > 1)
        {
            InsereVida((short)(RetornaVida() * powerup.aumentaVida)); // >1,0 ---->3
            InsereVidaMaxima((short)(RetornaVidaMaxima() * powerup.aumentaVida));
            powerupManager.AdicionarNaLista(3);

        }
        else if (powerup.aumentaVelocidadeDoTiro>0)
        {
            aumentoVelocidadeDeAtirar = powerup.aumentaVelocidadeDeAtirar; // >0 ---->1
            aumentoVelocidadeTiro = powerup.aumentaVelocidadeDoTiro;// >0 ------>1
            powerupManager.AdicionarNaLista(1);
        }
        
        else if(powerup.invencivel == true)
        {
            invensivel = powerup.invencivel;//true ----->2
            powerupManager.AdicionarNaLista(2);
        }
       else if(powerup.aumentaVelocidade>1)
        {
            InsereVelocidade((short)(RetornaVelocidade() * powerup.aumentaVelocidade));// >1,0 ----->4
            powerupManager.AdicionarNaLista(4);
        }
        barraDeVida.AtualizarVidaSlider(RetornaVida());
        
        yield return new WaitForSeconds(powerup.tempo);



        if (powerup.aumentaDano > 0)
        {
            this.aumentDano = 0;
        }
        else if (powerup.aumentaVida > 1)
        {
            InsereVidaMaxima((short)((RetornaVidaMaxima() / powerup.aumentaVida) + 1));
            if (RetornaVida() > RetornaVidaMaxima())
            {
                InsereVida(RetornaVidaMaxima());
            }

        }
        else if (powerup.aumentaVelocidadeDoTiro > 0)
        {
            aumentoVelocidadeDeAtirar = 0;
            aumentoVelocidadeTiro = 1;
        }

        else if (powerup.invencivel == true)
        {
            invensivel = false;
        }
        else if (powerup.aumentaVelocidade > 1)
        {
            InsereVelocidade((short)((RetornaVelocidade() / powerup.aumentaVelocidade) + 1));
        }

        

       // InsereVida((short)((RetornaVida() / powerup.aumentaVida)+1));
       
        
       
        
        barraDeVida.AtualizarVidaSlider(RetornaVida());
        powerupManager.RebaixarNaLista();
    }

    #endregion

    #region Especial
    public void Especial()
    {
        DestroyAll();
    }

    void DestroyAll()
    {
        Inimigo[] enemies = FindObjectsOfType<Inimigo>();// FindGameObjectsWithTag(tag);
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].ReceberDano(999);
            //Destroy(enemies[i]);
        }
        BarraEspecialAtual = 0;
        especial.AtualizarVidaSlider(BarraEspecialAtual);
        SoundManager.Instance.PlayEfeito(SomEspecial);
    }
    public void AdicionaPontosEspecial()
    {
        BarraEspecialAtual += fatorMultEspecial;
        especial.AtualizarVidaSlider(BarraEspecialAtual);
    }
    #endregion
    public void ResetJogador()
    {
        
        InsereVida(100);
        barraDeVida.AtualizarVidaSlider(RetornaVida());
        this.gameObject.SetActive(true);
        this.GetComponent<BoxCollider2D>().enabled = true;
        morto = false;
    }
}
