using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class findujeu : MonoBehaviour
{
    public GameObject ancienUI;
    public GameObject disable;
    public GameObject fade;
    void Start()
    {
        
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine("oui");
        }
    }
    
    public void StartFin()
    {
        StartCoroutine("termine");
    }

    IEnumerator termine()
    {        
        yield return new WaitForSeconds(10);
        disable.SetActive(false);
        ancienUI.SetActive(true);
        
    }

    IEnumerator oui()
    {
        
        Color c = fade.GetComponent<Image>().color;
        while(c.a < 1)
        {
            c.a += Time.deltaTime;
            fade.GetComponent<Image>().color = c;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
