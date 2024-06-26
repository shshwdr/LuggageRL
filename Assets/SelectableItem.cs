using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectableItem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (ItemManager.Instance.isInControl)
        {
            if (!GridManager.Instance.GridItemDict.Values.ToList().Contains(GetComponent<GridItem>()))
            {

                ItemManager.Instance.select(this);
            }
        }else if (ItemRemoveManager.Instance.isInControl)
        {

            ItemRemoveManager.Instance.select(this);
        }
        else if (GameOver.Instance.isGameOver)
        {
            GameOver.Instance.select(this);
        }

    }
}
