using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{

    public Vector3 endValue;
    public float jumpPower = 10;
    public float animTime = 1;
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
        transform.DOPunchScale(endValue,animTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
