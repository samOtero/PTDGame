using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Camera cameraToFace;
    public BasicEvent DoRun;

    void Start() {
        cameraToFace = Camera.main;
        DoRun.RegisterListener(onDoRun);
    }

    // Update is called once per frame
    public int onDoRun()
    {
        transform.LookAt(transform.position + cameraToFace.transform.rotation * Vector3.back, cameraToFace.transform.rotation * Vector3.up);
        return 1;
    }
}
