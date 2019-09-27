using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public Transform lookAtObject;

    private void Start()
    {
        transform.LookAt(lookAtObject);
    }
}
