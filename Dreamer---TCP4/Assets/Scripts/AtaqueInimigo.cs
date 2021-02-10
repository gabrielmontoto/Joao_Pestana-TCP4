using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaqueInimigo : MonoBehaviour
{
    public Vector3 attackOffset;
    public float attackRange;// = 1f;
    public LayerMask attackMask;
    Inimigo inimigo;
    short dano;
    public GameObject pai;

    [SerializeField] bool distancia;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        inimigo = GetComponentInParent<Inimigo>();
        if (distancia)
            attackRange = inimigo.RetornaDistanciaDeAtaque() / 1.2f;
        else
            attackRange = inimigo.RetornaDistanciaDeAtaque() * 1.3f;
        distancia = inimigo.distancia;
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame

    public void SomAtaque()
    {
        SoundManager.Instance.PlayEfeito(inimigo.retornaSomAtaque());
    }
    public void ataque()
    {
        Vector3 pos = transform.position;
        if (distancia)
        {
            PosAtaquesADistancia();
        }
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;


       
        /*  if(inimigo.distancia)
          {
              GameObject bala = Instantiate(inimigo.RetornaBalaPrefab(), inimigo.saidaDoTiro.transform.position, inimigo.saidaDoTiro.transform.rotation); //intanciar a bala pelo prefab, no objeto vazio dizendo onde vai sair o tiro e a rotacao desse objeto
              if (inimigo.animator.GetBool("Cima"))
              {
                  bala.GetComponent<Rigidbody2D>().AddForce(inimigo.saidaDoTiro.transform.up * inimigo.RetornaVelocidadeBala() * -1, ForceMode2D.Impulse);

              }
              else if (inimigo.animator.GetBool("Baixo"))
              {
                  bala.GetComponent<Rigidbody2D>().AddForce(inimigo.saidaDoTiro.transform.up * inimigo.RetornaVelocidadeBala(), ForceMode2D.Impulse);

              }
              else
              {
                  if(inimigo.esquerda)
                  bala.GetComponent<Rigidbody2D>().AddForce(inimigo.saidaDoTiro.transform.right * inimigo.RetornaVelocidadeBala() * -1, ForceMode2D.Impulse);

                  else
                      bala.GetComponent<Rigidbody2D>().AddForce(inimigo.saidaDoTiro.transform.right * inimigo.RetornaVelocidadeBala(), ForceMode2D.Impulse);

              }
              bala.GetComponent<Projetil>().InserirDanoNoProjetil(inimigo.RetornaAtaque());
              bala.tag = "Projetil_Inimigo";
              bala.transform.name = "bala Inimigo";
          }*/
        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colInfo != null)// && !inimigo.distancia)
        {
            dano = inimigo.RetornaAtaque();
            colInfo.GetComponent<Jogador>().acessoDanoRecebido(dano);
            inimigo.GirarAnimacoes();
            // colInfo.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
        }

    }
    public void ResetarVelocidade()
    {
        inimigo.ResetarVelocidade();
    }

    void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Gizmos.DrawWireSphere(pos, attackRange);
    }

    public void morte()
    {
       
        inimigo.velocidadeInicial = 0;
        inimigo.InstanciarPowerUpEArmas();
        
        Destroy(pai);

    }
    private void PosAtaquesADistancia()
    {
        if (animator.GetBool("Baixo"))
        {
            attackOffset.y = 0.97f;
            attackOffset.x = 0;
        }
        else if (animator.GetBool("Cima"))
        {
            attackOffset.y = -0.97f;
            attackOffset.x = 0;
        }
        else
        {
            if (sr.flipX == true)
            {
                attackOffset.x = -0.92f;
                attackOffset.y = 0;
            }
            else
            {
                attackOffset.x = 0.92f;
                attackOffset.y = 0;
            }
        }
    }
}

/*    public void ataque()
    {
        Vector3 pos = transform.position;
       pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;
        // x 0.73 y -0.65 -> esquerda cima
        colInfo = Physics2D.OverlapBox(pos, new Vector2(attackRange*2,0.3f), attackMask);
         colInfo2 = Physics2D.OverlapBox(pos,new Vector2( 0.3f,attackRange*2), attackMask);
        //Physics2D.OverlapBox(pos, attackRange, attackMask);
        if (colInfo.transform.CompareTag("Jogador") || colInfo2.transform.CompareTag("Jogador"))
        {
            print(colInfo.transform.name + "-----------------------------------------0");
            dano = inimigo.RetornaAtaque();
            // Debug.Log(colInfo);
            // Debug.Log(dano);
            if (colInfo.transform.CompareTag("Jogador"))
                colInfo.GetComponent<Jogador>().ReceberDano(dano);
            else if (colInfo2.transform.CompareTag("Jogador"))
                colInfo2.GetComponent<Jogador>().ReceberDano(dano);
            inimigo.GirarAnimacoes();
            // colInfo.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
        }

    }
    public void ResetarVelocidade()
    {
        inimigo.ResetarVelocidade();
    }

    void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

       // Gizmos.DrawWireSphere(pos, attackRange);
        Gizmos.DrawCube(pos, new Vector3(0.3f, attackRange *2, 1));
        Gizmos.DrawCube(pos, new Vector3(attackRange *2 , 0.3f, 1));
    }
}
*/