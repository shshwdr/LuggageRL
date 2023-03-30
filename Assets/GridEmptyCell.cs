using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridEmptyCell : MonoBehaviour
{
    public Vector2Int index;
    public MMF_Player CellAttackedAnimation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void PlayCellAttackedAnimation()
    {
        CellAttackedAnimation.Initialization();
        CellAttackedAnimation.PlayFeedbacks();
    }
}
