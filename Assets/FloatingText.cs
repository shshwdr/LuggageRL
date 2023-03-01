using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{


    public void init(string text,Vector3 pos)
    {
        gameObject.SetActive(true);
        GetComponentInChildren<Text>().text = text;
        transform.position = pos;
        Destroy(gameObject, 1);
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
