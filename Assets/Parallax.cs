using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float parallaxSpeed = 0.02f; // Adjust this value to change the parallax speed
    private Vector3 previousCameraPosition;

    private void Start()
    {
        previousCameraPosition = Camera.main.transform.position;
    }

    private void Update()
    {
        Vector3 delta = Camera.main.transform.position - previousCameraPosition;
        transform.position += delta * parallaxSpeed;
        previousCameraPosition = Camera.main.transform.position;
    }
}