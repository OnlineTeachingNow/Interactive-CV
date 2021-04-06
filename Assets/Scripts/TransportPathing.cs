using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportPathing : MonoBehaviour
{
    [Header ("References to Other Scripts") ]
    TripConfig _tripConfig;
    TripSpawner _tripSpawner;
    FauxGravityAttractor _earth;

    [Header ("Waypoints")]
    List<Transform> _wayPoints;
    private int _wayPointsIndex = 0;

    [Header("Transportation Position and Reference")]
    private Transform _tr;
    private Vector3 _original;
    Vector3 _currentPosition;

    [Header("Transportation Rotation")]
    //Quaternion _bodyRotation;
    float _waypointRotationSmoothing = 0.1f;

    void Start()
    {
        _tr = GetComponent<Transform>();
        _tripSpawner = FindObjectOfType<TripSpawner>();
        _earth = FindObjectOfType<FauxGravityAttractor>();
        GetWayPointsSetOrigin();
        _currentPosition = _tr.position;
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

            if (Vector3.Distance(_currentPosition, _targetPosition) < 0.08)
            {
                _wayPointsIndex++;
            }
        }
        else
        {
            _tripSpawner.ManageLastWayPoint(true);
            Destroy(this.gameObject);
        }
    }

    private void MoveTransportation(Vector3 _targetPosition, Vector3 _currentPosition)
    {
        float _moveSpeed = 3f; //Trying to use the number of photos in order to determine the speed of the bicycle. I don't think it's working too well. Need to find a way to get the distance between each of the waypoints better. 
        var _movementThisFrame = _moveSpeed * Time.deltaTime;
        _tr.position = Vector3.MoveTowards(_currentPosition, _targetPosition, _movementThisFrame);
    }

    private void RotateTransportation(Vector3 _targetPosition, Vector3 _currentPosition)
    {
        //_bodyRotation = _earth.ReturnCurrentRotation();
        Vector3 _distanceToNextWaypoint = _targetPosition - _currentPosition;
        float _distanceToPlane = Vector3.Dot(_tr.up, _distanceToNextWaypoint);
        Vector3 _pointOnPlane = _targetPosition - (_tr.up * _distanceToPlane);
        Quaternion q = Quaternion.LookRotation(_pointOnPlane - _tr.position, _tr.up);
        _tr.localRotation = Quaternion.Slerp(_tr.localRotation, q, Time.deltaTime); //Recently added time.deltaTime, hoping it would cause a smoother transition. It's not looking like it.
        Debug.DrawRay(_tr.position, _distanceToNextWaypoint, Color.red);
    }

    private float GetMoveSpeed()
    {
        float _totalTripTime = FindObjectOfType<PhotoDisplay>().ReturnTotalTripTime();
        var _secondsPerWayPoint = _totalTripTime / (_wayPoints.Count - 1);
        float _distanceBetweenWayPoints = Vector3.Distance(_currentPosition, _wayPoints[_wayPointsIndex].position);
        float _moveSpeed = _secondsPerWayPoint / _distanceBetweenWayPoints;
        return _moveSpeed;
    }

    private void GetWayPointsSetOrigin()
    {
        //Setting original position
        _wayPoints = _tripConfig.GetWayPoints();
        _original = _wayPoints[_wayPointsIndex].transform.position;
        _tr.position = _original;
    }

    public void SetTripConfig(TripConfig tripConfigToSet)
    {
        this._tripConfig = tripConfigToSet;
    }
}
