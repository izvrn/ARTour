using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    [SerializeField] private Transform _waypoint;

    private Vector3 _awakePosition;

    private Vector3 _targetPositon;

    private Queue<Vector3> _targets;

    private float _timer = 1.5f;
    // Start is called before the first frame update
    void Awake()
    {
        _awakePosition = transform.localPosition;
        _targets = new Queue<Vector3>();
        _targets.Enqueue(_waypoint.localPosition);
        _targets.Enqueue(_awakePosition);
        _targetPositon = _awakePosition;      
    }

    private void SwitchWaypoint()
    {
        _targetPositon = _targets.Dequeue();
        _targets.Enqueue(_targetPositon);
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.touchCount > 0 || Input.GetKeyDown(KeyCode.Mouse0)) && _timer > 1f)
        {
            _timer = 0;
            SwitchWaypoint();
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, _targetPositon, Time.deltaTime * 2f);
        _timer += Time.deltaTime;
    }
}
