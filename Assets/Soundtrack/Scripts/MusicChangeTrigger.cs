using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChangeTrigger : MonoBehaviour
{
    [Header("Area")]
    [SerializeField] private MusicArea area;
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 1);
    }
}
