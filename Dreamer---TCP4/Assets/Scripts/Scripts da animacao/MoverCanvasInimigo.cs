using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverCanvasInimigo : MonoBehaviour
{
    public Transform transformInimigo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = transformInimigo.position;
    }
}
