using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportPathing : MonoBehaviour
{
    [Header ("References to Other gameObjects") ]
    TripConfig _tripConfig;
    TripSpawner _tripSpawner;

    [Header ("Waypoints")]
    List<Transform> _wayPoints;
    private int _wayPointsIndex = 0;

    [Header("Transportation Position and Reference")]
    private Transform _tr;
    Vector3 _currentPosition;
    float _moveSpeed;

    void Start()
    {
        _tr = GetComponent<Transform>();
        _tripSpawner = FindObjectOfType<TripSpawner>();
        _wayPoints = _tripConfig.GetWayPoints();
    }
    void Update()
    {
        FollowWaypoints();
    }
    private void FollowWaypoints()
    {
        if (_wayPointsIndex < _wayPoints.Count)
        {
            var _targetPosition = _wayPoints[_wayPointsIndex].transform.position;
            _currentPosition = _tr.position;

            RotateTransportation(_targetPosition, _currentPosition);
            MoveTransportation(_targetPosition, _currentPosition);

            if (Vector3.Distance(_currentPosition, _targetPosition) < 0.1f)
            {
                _wayPointsIndex++;
            }
        }
        else
        {
            _tripSpawner.ManageLastWayPoint(true); //consider replacing this with a delegate to avoid having to reference the trip spawner.
            Destroy(this.gameObject);
        }
    }

    private void RotateTransportation(Vector3 _targetPosition, Vector3 _currentPosition)
    {
        Vector3 _distanceToNextWaypoint = _targetPosition - _currentPosition;
        float _distanceToPlane = Vector3.Dot(_tr.up, _distanceToNextWaypoint);
        Vector3 _pointOnPlane = _targetPosition - (_tr.up * _distanceToPlane);
        Quaternion q = Quaternion.LookRotation(_pointOnPlane - _tr.position, _tr.up);
        _tr.localRotation = Quaternion.Slerp(_tr.localRotation, q, Time.deltaTime); //Time.deltaTime is just fine at the moment but may need to adjust this with another variable in the future.
    }
    private void MoveTransportation(Vector3 _targetPosition, Vector3 _currentPosition)
    {
        //float _moveSpeed = 3f; //Trying to use the number of photos in order to determine the speed of the bicycle. I don't think it's working too well. Need to find a way to get the distance between each of the waypoints better. 
        var _movementThisFrame = _moveSpeed * Time.deltaTime;
        _tr.position = Vector3.MoveTowards(_currentPosition, _targetPosition, _movementThisFrame);
    }

    public void SetTransportMoveSpeed(float transportMoveSpeed)
    {
        _moveSpeed = transportMoveSpeed;
        Debug.Log("Move speed is: " + _moveSpeed);
    }

    public void SetTripConfig(TripConfig tripConfigToSet)
    {
        _tripConfig = tripConfigToSet;
    }
}
