using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Camera cameraToFace;

    void Start() {
        cameraToFace = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + cameraToFace.transform.rotation * Vector3.back, cameraToFace.transform.rotation * Vector3.up);
    }
}
