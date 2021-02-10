using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdentificadorERandomizador : MonoBehaviour
{
    //   public float raio;
    public bool usarRotacao;
    public Vector3 quadrado;

    [SerializeField]
    List<GameObject> ObjetosLista;
   // int angulo;

      
    [SerializeField] int objetosNesseCenario;
    // public Jogador jogador;
    public SpriteRenderer sr;

    #region regiaoTeste

    public List<GameObject> objetosAdicionadosLista;
    #endregion

  //  [SerializeField] LayerMask layerMask;




   // public GameObject algo;
    public Collider2D[] collider2Ds;
    public ContactFilter2D contactFilter2;
    public int contatos;
    public float radios;

  

    // Start is called before the first frame update
    private void Awake()
    {
        if (sr != null)
        {
            //print(sr.sprite.border);
              // print(sr.bounds.size + gameObject.name + "-------------------------------");
            quadrado.x = sr.bounds.size.x;
            quadrado.y = sr.bounds.size.y;
        }
        // print(sr.bounds.size);
        Contar();
    }
    void OnDrawGizmosSelected() //esse é pra fazer um desenho do circulo do que vai servir de colisao na tela da cena
    {
        Gizmos.DrawCube(this.gameObject.transform.position, new Vector3(quadrado.x,quadrado.y));
        // Gizmos.DrawWireSphere(this.gameObject.transform.position, raio); // desenha uma esfera na posicao do temporario e com o raio do raio de ataque
    }
    void Start()
    {

       // AstarPath.active.Scan();
        //AstarPath.active.ScanAsync();
      
        //print("Primeiro ---------------");
        //chamar isso depois de colocar os objetos na tela

        //print(AstarPath.active.isScanning);
    }

    // Update is called once per frame


    private void Contar()
    {
        for (int i = 0; i < objetosNesseCenario; i++)
        {
            //Spawnar();
            SpawnAlgo();
            
        }
        

    }

    private void Spawnar()
    {
        GameObject objeto;
      
        if (usarRotacao)
        {
            int temp = Random.Range(0, 361);
            if (temp < 45)
            {
                temp = 0;
            }
            else if (temp < 90)
            {
                temp = 45;
            }
            else if (temp < 135)
            {
                temp = 90;
            }
            else if (temp < 180)
            {
                temp = 135;
            }
            else if (temp < 225)
            {
                temp = 180;
            }
            else if (temp < 270)
            {
                temp = 225;
            }
            else if (temp < 315)
            {
                temp = 270;
            }
            else
            {
                temp = 315;
            }
             objeto = Instantiate(EscolhaInimigo(), NovaPosicao(), Quaternion.Euler(0, 0, temp));
        }
        else
        {
             objeto = Instantiate(EscolhaInimigo(), NovaPosicao(), Quaternion.identity);
            objeto.transform.name = "Objeto";
        }

        objetosAdicionadosLista.Add(objeto);


        objeto.transform.SetParent(this.transform);
    }
    public Vector3 NovaPosicao()
    {
        // angulo = Random.Range(0, 361);
        //Vector2 tempCentro = sr.bounds.center;
        float tempX = quadrado.x / 2;
        float tempY = 0 - (quadrado.y / 2);
        //Vector2 posFinal = new Vector2(Random.Range(tempX + this.transform.position.x, quadrado.x / 2 + this.transform.position.x), Random.Range(tempY + this.transform.position.y, quadrado.y / 2 + this.transform.position.y));
        Vector3 posFinal = new Vector3(Random.Range(-quadrado.x/2 + this.transform.position.x, quadrado.x/2 + this.transform.position.x),
            Random.Range(-quadrado.y/2 + this.transform.position.y, quadrado.y/2 + this.transform.position.y), -1);
        
        // Vector2 pos = new Vector2(Random.Range(0,raio) * Mathf.Cos(angulo) , Random.Range(0, raio) * Mathf.Sin(angulo) );
        return posFinal;
    }

    private GameObject EscolhaInimigo()
    {
        int temp = Random.Range(0, ObjetosLista.Capacity);

        return ObjetosLista[temp];

    }

    private void SpawnAlgo()
    {
        Vector2 pos = Vector2.one;
        bool SpawnarAqui = false;
        int Seguranca = 0;
        GameObject obj;

        if (usarRotacao)
        {
            int temp = Random.Range(0, 361);
            if (temp < 45)
            {
                temp = 0;
            }
            else if (temp < 90)
            {
                temp = 45;
            }
            else if (temp < 135)
            {
                temp = 90;
            }
            else if (temp < 180)
            {
                temp = 135;
            }
            else if (temp < 225)
            {
                temp = 180;
            }
            else if (temp < 270)
            {
                temp = 225;
            }
            else if (temp < 315)
            {
                temp = 270;
            }
            else
            {
                temp = 315;
            }
            obj = Instantiate(EscolhaInimigo(), NovaPosicao(), Quaternion.Euler(0, 0, temp));
        }
        else
        {
            obj = Instantiate(EscolhaInimigo(), Vector2.zero, Quaternion.identity);
            obj.transform.name = "Objeto";
        }

        while (!SpawnarAqui)
        {
            pos = NovaPosicao();
            SpawnarAqui = previnirOverlap(pos);
            obj.transform.position = pos;
            contatos = obj.GetComponent<PolygonCollider2D>().OverlapCollider(contactFilter2, collider2Ds);
            if (contatos > 0)
            {
                SpawnarAqui = false;
            }
            else if (contatos == 0 && SpawnarAqui == true)
            {
                obj.transform.position = pos;
                objetosAdicionadosLista.Add(obj);
                obj.transform.SetParent(this.transform);
                break;
            }

            Seguranca++;
            if (Seguranca > 150)
            {
                print("muitos testes ------- " + gameObject.name );
                Destroy(obj);
                break;
            }
        }


        obj.name = "obj " + collider2Ds.Length;
        obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, -5);
      //  print("segundo -----------------");


    }

    bool previnirOverlap(Vector2 pos)
    {
        collider2Ds = Physics2D.OverlapCircleAll(pos, radios);
        for (int i = 0; i < collider2Ds.Length; i++)
        {
            Vector2 centro = collider2Ds[i].bounds.center;
            float width = collider2Ds[i].bounds.extents.x;
            float height = collider2Ds[i].bounds.extents.y;
            //print(centro + " Centro\n" + width + " width\n" + height + " height");
            float LeftExtends = centro.x - width;
            float RightExtends = centro.x + width;
            float UpExtends = centro.y + height;
            float DownExtends = centro.y - height;

            if (pos.x >= LeftExtends && pos.x <= RightExtends)
                if (pos.y >= DownExtends && pos.y <= UpExtends)
                {
                    return false;
                }
        }
        return true;
    }
}
