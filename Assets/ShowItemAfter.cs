using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowItemAfter : MonoBehaviour
{
    public GameObject[] test;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("show", 0.1f);
    }
    void show()
    {
        foreach(var t in test)
        {

            t.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
