using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenarioManager : MonoBehaviour
{
    [SerializeField] GameObject[] anexos;
    //[SerializeField] [Range(0, 360)] int angulo;
    [SerializeField] Vector3[] Posicoes; //0 -> 0 ; 1-> 90 ; 2-> 180; 3 -> 270
    // Start is called before the first frame update
    private void Awake()
    {
        LugaresAnexos();
    }

    private void LugaresAnexos()
    {
        for (int i = 0; i < Posicoes.Length; i++)
        {
            anexos[i].transform.rotation = Quaternion.Euler(0, 0, 90*i);
            anexos[i].transform.position = Posicoes[i];
        }
        
    }

}
