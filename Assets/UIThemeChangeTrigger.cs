using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class UIThemeChangeTrigger : MonoBehaviour
{
    [SerializeField] private BiomeArea area;
    [SerializeField] bool hasFinishedTransitioning = false;

    private GameObject airportFrameUI;
    private GameObject outsideFrameUI;

    [SerializeField] private float transitionDistance = 8;

    private void Start()
    {
        foreach(ThemedUIContainer container in FindObjectsOfType<ThemedUIContainer>())
        {
            switch (container.theme)
            {
                case BiomeArea.AIRPORT:
                case BiomeArea.BOSS:
                    airportFrameUI = container.gameObject;

                    break;
                case BiomeArea.OUTSIDE:
                    outsideFrameUI = container.gameObject;
                    break;
                default:
                    Debug.LogError("Alert: Anomaly biome.");
                    break;
            }
        }
    }
    private void Update()
    {
        if (hasFinishedTransitioning)
        {
            return;
        }
        if (transform.position.x <= 0)
        {
            float travelledDistance = -transform.position.x;
            if (travelledDistance >= transitionDistance)
            {
                hasFinishedTransitioning = true;
            }

            Color fadeInAlpha = new Color(1.0f, 1.0f, 1.0f, Mathf.Lerp(0f, 1f, travelledDistance / transitionDistance));
            Color fadeOutAlpha = new Color(1.0f, 1.0f, 1.0f, Mathf.Lerp(1f, 0f, travelledDistance / transitionDistance));
            switch(area)
            {
                case BiomeArea.AIRPORT:
                case BiomeArea.BOSS:
                    foreach(SpriteRenderer ren in airportFrameUI.GetComponentsInChildren<SpriteRenderer>())
                    {
                        ren.material.color = fadeInAlpha;
                    }
                    foreach (SpriteRenderer ren in outsideFrameUI.GetComponentsInChildren<SpriteRenderer>())
                    {
                        ren.material.color = fadeOutAlpha;
                    }
                    break;
                case BiomeArea.OUTSIDE:
                    foreach (SpriteRenderer ren in airportFrameUI.GetComponentsInChildren<SpriteRenderer>())
                    {
                        ren.material.color = fadeOutAlpha;
                    }
                    foreach (SpriteRenderer ren in outsideFrameUI.GetComponentsInChildren<SpriteRenderer>())
                    {
                        ren.material.color = fadeInAlpha;
                    }
                    break;
                default:
                    Debug.LogError("Alert: Anomaly biome.");
                    break;
            }

        }
    }
}
