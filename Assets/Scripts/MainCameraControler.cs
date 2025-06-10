using System;
using UnityEngine;

public class MainCameraControler : MonoBehaviour
{
    public Transform target; // el jugador
    public float smoothSpeed = 0.125f;
    public Vector3 offset; // opcional: para separar la cámara del centro exacto
    public float cameraDistance=1;

    private void Awake()
    {
        Camera.main.orthographicSize =  cameraDistance* 5f;
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z); // mantener Z fijo
    }
}
