using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChangeTrigger : MonoBehaviour
{
    [SerializeField] private BiomeArea area;
    [SerializeField] bool hasTriggered = false;

    private void Start()
    {
        hasTriggered = false;
    }
    private void Update()
    {
        if (!hasTriggered && transform.position.x <= 0)
        {
            AudioManager.Instance.SetMusicArea(area);
            hasTriggered = true;
        }
    }
}
