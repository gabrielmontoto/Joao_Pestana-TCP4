using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vida_TransicaoTela : MonoBehaviour
{
    public int vida;
    LevelLoader lvlloader;
    // Start is called before the first frame update
    void Start()
    {
        lvlloader = FindObjectOfType<LevelLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            vida -= 10;
        }
        if(vida<= 0)
        {
            lvlloader.LoadNexLevel();
        }
    }
}
