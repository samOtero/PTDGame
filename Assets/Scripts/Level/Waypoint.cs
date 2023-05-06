using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] private bool destroyRenderer = false;

    public Waypoint prev;
    public Waypoint next;
    public Direction forward;
    public Direction backward;

    private void Start()
    {
        if (destroyRenderer)
            Destroy(GetComponent<Renderer>()); // Destroys the renderer if the destroyRenderer variable is true.
        else
            GetComponent<Renderer>().enabled = false; // If the variable is not true, the renderer will be disabled instead.
    }
}
