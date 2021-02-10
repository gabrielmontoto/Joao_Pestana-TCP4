using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsScript : MonoBehaviour
{
    // Start is called before the first frame update

    public PowerUp1 powerUp; // armas sendo declaradas
    string[] caminhoSalvoDasArmas;
    public GameObject pai;
    // Start is called before the first frame update
    void Start()
    {
        string resultado;
        caminhoSalvoDasArmas = new string[5];
        for (short i = 0; i < caminhoSalvoDasArmas.Length; i++)
        {
            caminhoSalvoDasArmas[i] = "PowerUps/PowerUp" + (i + 1);
        }
        int aleatorio = Random.Range(0, caminhoSalvoDasArmas.Length);

        resultado = caminhoSalvoDasArmas[aleatorio];
        //Debug.Log()
        powerUp = (PowerUp1)Resources.Load(resultado);
        this.gameObject.GetComponent<SpriteRenderer>().sprite = powerUp.imagem; // quando surgir vai receber a imagem padrao da arma
                                                                              //dessa forma pode aparecer uma arma aleatoria e ele vai pegar a imagem referente a essa arma


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Jogador"))// quando o jogador encostar
        {
            Jogador jogador = collision.GetComponent<Jogador>(); //chama a funçao de receber armas transferindo essa arma ao jogador

            // jogador.ReceberPowerUp(powerUp);
            pai.GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(jogador.ReceberPowerUp(powerUp));
                // jogador.EquiparArmaNInfinito(armas);
            

            Destroy(pai, powerUp.tempo+1); // destroi esse objeto
        }
    }
}
