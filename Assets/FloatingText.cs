using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    public Transform go;
    public Vector3 endValue;
    public int jumpPower = 10;
    public float animTime = 1;
    public float moveUp = 1;
    public void init(string text,Vector3 pos,Color color)
    {
        gameObject.SetActive(true);
        GetComponentInChildren<Text>().text = text;
        GetComponentInChildren<Text>().color = color;
        transform.position = pos;
        Destroy(gameObject, 1);
    }
    // Start is called before the first frame update
    void Start()
    {
        go.DOPunchScale(endValue,animTime, jumpPower);
        go.DOLocalMoveY(moveUp, animTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
