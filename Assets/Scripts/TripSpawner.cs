using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TripSpawner : MonoBehaviour
{
    [SerializeField] private List<TripConfig> _trips;

    [Header("References to other gameObjects")]
    [SerializeField] Button _bicycleButton;
    [SerializeField] PhotoDisplay _photoDisplay;
    [SerializeField] FauxGravityAttractor _earth;
    [SerializeField] Camera _mainCamera;

    //Transport Object Instantiated by this Script
    TransportPathing _newTransportation;
    List<Transform> _waypoints;
    bool _hasReachedLastTripWaypoint = false;
    float _totalTripDistance;

    // This was used to troubleshoot the waypoints. It is not necesssary at this point. However, I will hold off on deleting it for now.
    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            {
            Time.timeScale = 1;
            }
    }
    */

    public void StartTrip()
    {
        ToggleBicycleButton(false);
        StartCoroutine(SpawnTrips());
    }

    private IEnumerator SpawnTrips()
    {
        for (int tripIndex = 0; tripIndex < _trips.Count; tripIndex++)
        {
            ManageLastWayPoint(false);
            float _totalTripDuration = _photoDisplay.StartPhotoDisplay(_trips[tripIndex]);
            float _totalTripDistance = CalculateTripDistance(_trips[tripIndex]);
            float _transportSpeed = CalculateTransportMoveSpeed(_totalTripDuration, _totalTripDistance);
            yield return (StartCoroutine(InstantiateTransportation(_trips[tripIndex], _transportSpeed)));
        }
        ToggleMainCamera(true);
        ToggleBicycleButton(true);
    }
    private IEnumerator InstantiateTransportation(TripConfig trip, float transportSpeed)
    {
        //Determining direction to face
        Vector3 _directionToFace = trip.GetWayPoints()[1].transform.position - trip.GetWayPoints()[0].transform.position;
        //Instantiate Object at appropriate rotation
        _newTransportation = Instantiate(trip.GetTransportationPrefab(), trip.GetWayPoints()[0].transform.position, Quaternion.LookRotation(_directionToFace, _earth.ReturnBodyUp()));
        //SettingTransportationMoveSpeed
        _newTransportation.SetTransportMoveSpeed(transportSpeed);
        //Troubleshooting waypoint directions. Leaving here now, will delete later
        //Debug.DrawRay(_newTransportation.transform.position, _directionToFace, Color.yellow);
        //Time.timeScale = 0;
        ToggleMainCamera(false); //disabling main camera such that it defaults to the local camera above the transportation object
        _newTransportation.SetTripConfig(trip); //Passing in all the details of the trip from TripConfig scriptable object to the newly instantiated transport object.
        yield return new WaitUntil(() => _hasReachedLastTripWaypoint == true); //Waits until the last waypoint is reached before returning and ultimately indexing to the next trip 
        //in the functions above. Doing some crazy casting from a system.fun<bool> to a regular bool. Need to learn more about this.
    }

    public float CalculateTripDistance(TripConfig trip)
    {
        //This code is a bit of a mess because I am not satisfied with it and would like to find a better way to use the old solution and iterate through all the waypoints.
        //The problem is that Vector3.Distance is taking into account the distance between waypoints irrespective of the earth's curvature. I.e. it's not an accurate reflection of the actual
        //bicycle travel distance. Therefore... An estimation that seems to work is just substracting the last waypoint from the first and using that distance. Not a very elegant solution, 
        //but at least it seems to work for the three trips I have thus far.
        _totalTripDistance = 0; //Resetting trip distance from last trip. Otherwise, trip distance continues to accumulate between trips.
        _waypoints = trip.GetWayPoints();
        int _waypointIndex = 0;
        Transform _initialtWaypoint = _waypoints[_waypointIndex];

        for (int waypoint = 1; waypoint < _waypoints.Count; waypoint++)
        {
            float _tripLegDistance = Vector3.Distance(_waypoints[_waypoints.Count -1].position, _initialtWaypoint.position);
            _totalTripDistance = _tripLegDistance;
          //  _waypointIndex++;
        }
        Debug.Log("TotalTripDistance: " + _totalTripDistance);
        return _totalTripDistance;
    }

    public float CalculateTransportMoveSpeed(float tripDuration, float tripDistance)
    {
        var _tripSpeed = tripDistance / tripDuration;
        return _tripSpeed;
    }    

    public void ManageLastWayPoint(bool hasReachedLastWaypoint)
    {
        _hasReachedLastTripWaypoint = hasReachedLastWaypoint;
    }

    private void ToggleBicycleButton(bool isBicycleButtonActive)
    {
        _bicycleButton.gameObject.SetActive(isBicycleButtonActive);
    }

    private void ToggleMainCamera(bool isMainCameraActive)
    {
        _mainCamera.gameObject.SetActive(isMainCameraActive);
    }

}
