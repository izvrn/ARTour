using UnityEngine;

public class RotationScript : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.RotateAroundLocal(new Vector3(0, 0, 1), 2f);
    }
}
