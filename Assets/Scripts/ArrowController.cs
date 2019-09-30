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
        transform.rotation = Quaternion.Euler(newRotationVector);
        
        Debug.DrawRay(transform.position, transform.up, Color.green);
        Debug.DrawRay(transform.position, transform.forward, Color.blue);
        Debug.DrawRay(transform.position, transform.right, Color.red);
    }
}
