using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projetil : MonoBehaviour
{
    // public GameObject efeitoDeExplosao; //efeito animado de explosao
    private float DanoProjetil;
    private void Start()
    {
        Destroy(this.gameObject, 5f); //depois de atirar em 5 segundos ele vai ser distruido
    }
    public void efeitoExplosao()
    {
        // GameObject efeito = Instantiate(efeitoDeExplosao, transform.position, Quaternion.identity); // intanciar o efeito e guardar em uma variavel
        // para poder ser chamada apos, pra instanciar é necessario o que é estanciando, onde e sua rotaçao
        // Destroy(efeito, 5f); // destroi o efeito depois de 5 segundos
        
    }
    public void InserirDanoNoProjetil(float valor)
    {
        DanoProjetil = valor;
    }
    public float ReceberDanoDoProjetil()
    {
        return DanoProjetil;
    }
}
