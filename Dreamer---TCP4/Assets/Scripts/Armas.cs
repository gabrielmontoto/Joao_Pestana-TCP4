using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Insira o nome da arma aqui", menuName = "Armas")]//pra poder criar mais facil na unity
//clicar com o botao direito>new>armas como opçao de criaçao
public class Armas : ScriptableObject //um tipo de script pra armazenar dados
{

   public string nome;              //nome
   public Sprite imagem;            //imagem
   public short danoArma;           //dano que a arma vai causar
   public short balasMaximas;       //maximo de balas que essa arma vai ter antes de recarregar caso exista recarregamento se nao o maximo de balas que vai ter pra poder usar
   public float velocidadeDeAtirar; //de quanto em quanto tempo vai poder ser disparado um tiro
    public float velocidadeDoTiro;  //velocidade que o tiro (projetil) vai ter
    public short quantidadeDeTiros; //quantidade de tiros disparados
    public Sprite ArmaNoJogo;
}
