using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acelerometro : MonoBehaviour
{
    Vector3 aceleration;
    Jogador jogador;
    // Start is called before the first frame update
    void Start()
    {
        jogador = GetComponent<Jogador>();
    }

    // Update is called once per frame
    void Update()
    {
        aceleration = Input.acceleration;
        if(aceleration.magnitude >= 3)
        {
            jogador.Especial();
        }
    }
}
