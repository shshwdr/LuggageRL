using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int canKeepItemsCount = 0;
    public  List<ItemType> preselectedItems = new List<ItemType>();
    //int finishedMoveItemHint;
    
    // Start is called before the first frame update
    void Start()
    {

        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
