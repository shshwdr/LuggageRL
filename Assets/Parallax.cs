using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float parallaxSpeed = 0.02f; // Adjust this value to change the parallax speed
    private Vector3 previousStagePosition;
    private Stage stage;

    private void Start()
    {
        stage = FindObjectOfType<Stage>();
        previousStagePosition = stage.transform.position;
    }

    private void Update()
    {
        Vector3 currentStagePosition = stage.transform.position;
        Vector3 delta = currentStagePosition - previousStagePosition;
        transform.position += delta * parallaxSpeed;
        previousStagePosition = currentStagePosition;
    }
}