using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportPathing : MonoBehaviour
{
    TripConfig _tripConfig;
    TripSpawner _tripSpawner;
    List<Transform> _wayPoints;
    private Vector3 _original;
    private int _wayPointsIndex = 0;

    void Start()
    {
        _tripSpawner = FindObjectOfType<TripSpawner>();
        _wayPoints = _tripConfig.GetWayPoints();
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
}
