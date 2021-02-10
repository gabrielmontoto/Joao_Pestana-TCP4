using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabelaPontuacao : MonoBehaviour
{
    [SerializeField]
    private Transform espaco;
    [SerializeField]
    private Transform template;
    private void Awake()
    {
        espaco = transform.Find("Espaco");
        template = espaco.Find("Template");

        template.gameObject.SetActive(false);

        float alturaTemplate = 20;
        for(int i = 0; i<10;i++)
        {
            Transform posicao = Instantiate(template, espaco);
            RectTransform rectTransform = posicao.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, (-alturaTemplate * i)+40);
            posicao.gameObject.SetActive(true);
        }
         
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
