using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportPathing : MonoBehaviour
{
    TripConfig _tripConfig;
    List<Transform> _wayPoints;
    private Vector3 _original;
    private int _wayPointsIndex = 0;

    void Start()
    {
        _wayPoints = _tripConfig.GetWayPoints();
        //Debug.Log("Getwaypoints: " + _wayPoints[0] + _wayPoints[1]);
        _original = _wayPoints[_wayPointsIndex].transform.position;
        this.transform.position = _original;
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
            var _targetPosition = _wayPoints[_wayPointsIndex].transform.position;
            var _currentPosition = this.transform.position;
            var _movementThisFrame = _tripConfig.GetMoveSpeed() * Time.deltaTime;

            this.transform.position = Vector3.MoveTowards(_currentPosition, _targetPosition, _movementThisFrame);
        //  Debug.Log("this transform.position: " + this.transform.position);
         // Debug.Log("distance: " + Vector3.Distance(transform.position, _targetPosition));
            if (Vector3.Distance(_currentPosition, _targetPosition) < 0.08) //changed this from the trip over value to this, because it wasn't incrementing because the target waypoint was set to 
                //North carolina and there was no way to increment the thing because it only incremented on the last trip waypoint. With the following code, it increments and then the new target
                //waypoint becomes Colorado.
            {
                _wayPointsIndex++;
            //    Debug.Log("Are waypoints being indexed?" + _wayPointsIndex);
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
