using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personagem_Base : MonoBehaviour
{
    public BarraDeVida barraDeVida;
    [SerializeField]
    short vida, vidaMaxima, velocidade, ataqueFisico;//variaveis dos atributos
    [SerializeField]
    protected Rigidbody2D rigidbody2D; //rigidibody que vai ser usado por todos os personagens pra fazer a movimentacao
    [SerializeField]
    protected SpriteRenderer spriteRenderer; //spriterender por algum bom motivo que eu nao lembro
    [SerializeField]
    String nome; //nome do personagem ou inimigo caso haja necessidade de diferenciar
    [SerializeField]
    float tempoEntreAtaques, distanciaDeAtaque; // tempo pro ataque fisico
    
    #region retornos
    public short RetornaVida() //retornar o atributo vida
    {
        return vida;
    }
    public short RetornaVidaMaxima() // retornar o atributo vida maxima
    {
        return vidaMaxima;
    }
    public short RetornaVelocidade()// retornar o atributo velocidade
    {
        return velocidade;
    }
    public short RetornaAtaque()// retornar o atributo ataque fisico
    {
        return ataqueFisico;
    }
    public String RetornaNome()// retornar o atributo nome
    {
        return nome;
    }

    public Vector2 RetornaPosicao() // retornar a posicao
    {
        return this.transform.position;
    }
    public float RetornaDistanciaDeAtaque() // retornar a distancia de ataque fisico
    {
        return this.distanciaDeAtaque;
    }
    public float RetornaTempoEntreAtaques()  // retornar o tempo do ataque fisico
    {
        return this.tempoEntreAtaques;
    }
    #endregion

    #region inserir
    public void InsereVida(short vida) // insere na vida um valor quando chamado
    {
        this.vida = vida;
    }

    public void InsereVidaMaxima(short vidaMaxima)// insere na vida maxima um valor quando chamado
    {
        this.vidaMaxima = vidaMaxima;
    }

    public void InsereVelocidade(short velocidade)// insere na velocidade um valor quando chamado
    {
        this.velocidade = velocidade;
    }

    public void InsereAtaqueFisico(short ataqueFisico)// insere no ataque fisico um valor quando chamado
    {
        this.ataqueFisico = ataqueFisico;
    }

    public void InsereNome(string nome)// insere um nome quando chamado
    {
        this.nome = nome;
    }
    public void InsereDistanciaDeAtaque(float distanciaDeAtaque) // insere na distancia de ataque um valor quando chamado
    {
        this.distanciaDeAtaque = distanciaDeAtaque;
    }
    public void InsereTempoEntreAtaques(float tempoEntreAtaques) // insere o tempo do ataque fisico
    {
        this.tempoEntreAtaques = tempoEntreAtaques;
    }
    #endregion

    public virtual void ReceberDano(short dano) // ao receber dano a vida é diminuida do dano recebido
    {
        vida -=  dano;
        barraDeVida.AtualizarVidaSlider(vida);
       
    }

    public virtual void Morrer() //se a vida for menor que zero ele é destruido
    {
        if(vida<=0)
        {
            Destroy(this.gameObject);
        }
    }
}
