using System;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float rotationSpeed = 1f;

    private void Update()
    {
        Vector3 direction = target.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 newRotationVector = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
        transform.rotation = Quaternion.Euler(0, newRotationVector.y, 0);
    }
}
