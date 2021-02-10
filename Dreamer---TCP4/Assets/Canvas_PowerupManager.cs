using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas_PowerupManager : MonoBehaviour
{
    public int filhos;
    public Sprite dano,velArmaBala,invencivel,vida,velocidade;

    public short temporario;

    public List<Sprite> PowerUpsHabilitado;
    // Start is called before the first frame update
    void Start()
    {
        filhos = this.gameObject.transform.childCount;
        Debug.Log(gameObject.transform.GetChild(1).name);
        for (short i = 0; i < gameObject.transform.childCount;i++)
        {
            PowerUpsHabilitado.Add(gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite);
        }
        //Sprite sprite = (Sprite)Resources.Load("Sprites/Temporario_Zumbi");
      //  PowerUpsHabilitado[2] = dano;
     //   gameObject.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = PowerUpsHabilitado[2];
       // GetComponentInChildren<SpriteRenderer>().sprite = sprite;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            RebaixarNaLista();
        }
        if(Input.GetKeyDown(KeyCode.N))
        {
            AdicionarNaLista(temporario);
        }
    }
    // Update is called once per frame
    public void RebaixarNaLista()
    {
        
        PowerUpsHabilitado.RemoveRange(1, 0);
        for(short i = 0; i< gameObject.transform.childCount-1;i++)
        {
            
            PowerUpsHabilitado[i] = PowerUpsHabilitado[i + 1];
            gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = PowerUpsHabilitado[i];
            if(i>=3)
            {
                PowerUpsHabilitado[i+1] = null;
                 gameObject.transform.GetChild(i+1).GetComponent<SpriteRenderer>().sprite = null;
                
            }
        }
    }

    public void AdicionarNaLista(short qualPowerUp)
    {

        Sprite enviar;

        if(qualPowerUp == 0)
        {
            enviar = dano;
        }
        else if(qualPowerUp == 1)
        {
            enviar = velArmaBala;
        }
        else if (qualPowerUp == 2)
        {
            enviar = invencivel;
        }
        else if (qualPowerUp == 3)
        {
            enviar = vida;
        }
        else
        {
            enviar = velocidade;
        }

        if (PowerUpsHabilitado[0] == null)
        {
            PowerUpsHabilitado[0] = enviar;
            
        }
        else if(PowerUpsHabilitado[1] == null)
        {
            PowerUpsHabilitado[1] = enviar;
        }
        else if (PowerUpsHabilitado[2] == null)
        {
            PowerUpsHabilitado[2] = enviar;
        }
        else if (PowerUpsHabilitado[3] == null)
        {
            PowerUpsHabilitado[3] = enviar;
        }
        else if (PowerUpsHabilitado[4] == null)
        {
            PowerUpsHabilitado[4] = enviar;
        }

        for (short i = 0; i < gameObject.transform.childCount; i++)
        {

           
            gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = PowerUpsHabilitado[i];
            gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>().size = new Vector2(47,45);
            
        }

    }
}
