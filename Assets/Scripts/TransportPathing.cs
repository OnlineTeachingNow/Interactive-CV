using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportPathing : MonoBehaviour
{
    TripConfig _tripConfig;
    TripSpawner _tripSpawner;
    List<Transform> _wayPoints;
    FauxGravityAttractor _earth;
    private Vector3 _original;
    private int _wayPointsIndex = 0;
    Vector3 _direction;
    Vector3 _thisLocation;

    void Start()
    {
        _tripSpawner = FindObjectOfType<TripSpawner>();
        _wayPoints = _tripConfig.GetWayPoints();
        _original = _wayPoints[_wayPointsIndex].transform.position;
        _earth = FindObjectOfType<FauxGravityAttractor>();
        this.transform.position = _original;
        _direction = (_wayPoints[_wayPointsIndex].transform.position - this.transform.position).normalized;
        _thisLocation = this.transform.position;
    }

    void Update()
    {
        MoveTransport();
    }

    public void SetTripConfig(TripConfig tripConfigToSet)
    {
        this._tripConfig = tripConfigToSet;
    }

    private void MoveTransport()
    {
        if (_wayPointsIndex < _wayPoints.Count)
        {
            float _moveSpeed = GetMoveSpeed(); //Trying to use the number of photos in order to determine the speed of the bicycle. I don't think it's working too well. Need to find a way to get the distance between each of the waypoints better. 
            var _targetPosition = _wayPoints[_wayPointsIndex].transform.position;
            var _currentPosition = this.transform.position;
            var _movementThisFrame = _moveSpeed * Time.deltaTime;
            Vector3 _relativePos = _targetPosition - _currentPosition;
             

            this.transform.position = Vector3.MoveTowards(_currentPosition, _targetPosition, _movementThisFrame);
            //this.transform.rotation = Quaternion.LookRotation(_direction, transform.up) * _earth.ReturnCurrentRotation();
            //The program loses its mind and jumps around to all sorts of crazy places with this function.
            //this.transform.rotation = Quaternion.LookRotation(_direction); //This is the closest I have to it working. However, the bicycle leans hard towards the earth and I want it to maintain the upright position.
            //transform.LookAt(_targetPosition, _earth.ReturnBodyUp()); //earth.ReturnBodyUp returns the local y axis of the game object, i.e. Perpendicular to the surface of the earth. With this function, the bike doesn't move at all.
            //this.transform.position = Vector3.RotateTowards(_currentPosition, _targetPosition, 0.01f, 0.0f); //The bike doesn't move at all because (I think), it's trying to rotate into the earth's surface and getting stopped by the collider.
            
            if (Vector3.Distance(_currentPosition, _targetPosition) < 0.08)
            {
                _thisLocation = _wayPoints[_wayPointsIndex].position;
                _wayPointsIndex++;
                //_direction = (_wayPoints[_wayPointsIndex].transform.position - this.transform.position).normalized;
            }
        }
        else
        {
            _tripSpawner.ManageLastWayPoint(true);
            Destroy(this.gameObject);
        }
    }

    private float GetMoveSpeed()
    {
        float _totalTripTime = FindObjectOfType<PhotoDisplay>().ReturnTotalTripTime();
        var _secondsPerWayPoint = _totalTripTime / (_wayPoints.Count - 1);
        float _distanceBetweenWayPoints = Vector3.Distance(_thisLocation, _wayPoints[_wayPointsIndex].position);
        float _moveSpeed = _secondsPerWayPoint / _distanceBetweenWayPoints;
        return _moveSpeed;
    }
}
