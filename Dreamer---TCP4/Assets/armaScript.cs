using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class armaScript : MonoBehaviour
{
    //script para as armas que forem dropagas no jogo


    public Armas armas; // armas sendo declaradas
    string[] caminhoSalvoDasArmas;
    public GameObject pai;
    // Start is called before the first frame update
    void Start()
    {
        string resultado;
        caminhoSalvoDasArmas = new string[3];
        for (short i = 0; i < caminhoSalvoDasArmas.Length; i++)
        {
            caminhoSalvoDasArmas[i] = "Armas/ArmaTeste0"+(i+1);
        }
        int aleatorio = Random.Range(0, caminhoSalvoDasArmas.Length);
        
        resultado = caminhoSalvoDasArmas[aleatorio];
        //Debug.Log()
        armas = (Armas)Resources.Load(resultado);
        this.gameObject.GetComponent<SpriteRenderer>().sprite = armas.imagem; // quando surgir vai receber a imagem padrao da arma
        //dessa forma pode aparecer uma arma aleatoria e ele vai pegar a imagem referente a essa arma
        
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Jogador"))// quando o jogador encostar
        {
            Jogador jogador = collision.GetComponent<Jogador>(); //chama a funçao de receber armas transferindo essa arma ao jogador
            if(jogador.tirosInfinitos == false)
            {
                jogador.EquiparArmaNInfinito(armas);
            }
            else
            {
                jogador.EquiparArmaInfinito(armas);
            }
            Destroy(pai); // destroi esse objeto
        }
    }
}
