using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawn : MonoBehaviour
{
    public float raio;
   // public GameObject inimigo1,inimigo2,inimigo3,inimigo4;

   // public List<GameObject> objetosAdicionadosLista;
    [SerializeField]
    List<GameObject> ObjetosLista;

    int angulo;
    public float contador, contadorMax;

    public Vector2 PosSpawn;
    public Jogador jogador;

    [SerializeField] short QuantidadeInimigoSpawnadoPorVez;

    void OnDrawGizmosSelected() //esse é pra fazer um desenho do circulo do que vai servir de colisao na tela da cena
    {

        Gizmos.DrawWireSphere(this.gameObject.transform.position, 4);
        Gizmos.DrawWireSphere(this.gameObject.transform.position, raio); // desenha uma esfera na posicao do temporario e com o raio do raio de ataque
    }
    // Start is called before the first frame update
    void Start()
    {
        jogador = GetComponentInParent<Jogador>();
        contador = 0;
        //angulo = Random.Range(0, 361);
       /// Instantiate(inimigo1, new Vector2(raio * Mathf.Cos(angulo), raio * Mathf.Sin(angulo)), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        Contar();
        PosSpawn = jogador.transform.position;
    }

    private void Contar()
    {
        contador += Time.fixedDeltaTime *Time.timeScale;

        if (contador >= contadorMax)
        {
            for (int i = 0; i < QuantidadeInimigoSpawnadoPorVez; i++)
            {
                Spawnar();
                
            }
            contador = 0;
            contadorMax = Random.Range(2.5f, 5);
        }
        
    }

    private void Spawnar()
    {

        GameObject objeto = Instantiate(EscolhaInimigo(), NovaPosicao(), Quaternion.identity);

    }
    public Vector2 NovaPosicao()
    {
         angulo = Random.Range(0, 361);
       // float tempX = 0 - (quadrado.x / 2);
        //float tempY = 0 - (quadrado.y / 2);
        //Vector2 posFinal = new Vector2(Random.Range(tempX + this.transform.position.x, quadrado.x / 2 + this.transform.position.x), Random.Range(tempY + this.transform.position.y, quadrado.y / 2 + this.transform.position.y));
        Vector2 pos = new Vector2((Random.Range(4,raio) * Mathf.Cos(angulo))+PosSpawn.x , (Random.Range(4, raio) * Mathf.Sin(angulo))+PosSpawn.y );
        return pos;
    }

    private GameObject EscolhaInimigo()
    {
        int temp = Random.Range(0, ObjetosLista.Capacity);
        Debug.Log("objeto de numero " + temp + " adicionado");
        return ObjetosLista[temp];

    }
}
