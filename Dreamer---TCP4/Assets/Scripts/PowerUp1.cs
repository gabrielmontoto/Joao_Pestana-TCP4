using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Insira o nome do Power Up aqui", menuName = "Power Ups")]//pra poder criar mais facil na unity
public class PowerUp1 : ScriptableObject
{
    public string nome;              //nome
    public Sprite imagem;            //imagem
    public float aumentaDano;           //dano que a arma vai causar
    public float aumentaVida;       //maximo de balas que essa arma vai ter antes de recarregar caso exista recarregamento se nao o maximo de balas que vai ter pra poder usar
    public float aumentaVelocidadeDeAtirar; //de quanto em quanto tempo vai poder ser disparado um tiro
    public float aumentaVelocidadeDoTiro;  //velocidade que o tiro (projetil) vai ter
    public float aumentaVelocidade;
    public bool invencivel;
    public float tempo;
}
