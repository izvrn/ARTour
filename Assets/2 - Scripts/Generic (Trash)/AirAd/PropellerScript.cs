using UnityEngine;

public class PropellerScript : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 8f;
    void Update()
    {
        transform.RotateAround(new Vector3(0, 1f, 0), Time.deltaTime * rotateSpeed);
    }
}
