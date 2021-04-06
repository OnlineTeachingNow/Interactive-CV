using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TripSpawner : MonoBehaviour
{
    [SerializeField] private List<TripConfig> _trips;
    [SerializeField] Button _bicycleButton;
    [SerializeField] PhotoDisplay _photoDisplay;
    [SerializeField] FauxGravityAttractor _earth;
    TransportPathing _transportPathing;
    Camera _mainCamera;
    private Transform _originTransform;
    private GameObject _transportPrefab;
    TransportPathing _newTransportation;
    bool _hasReachedLastTripWaypoint = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            {
            Time.timeScale = 1;
            }
    }
    private void Start()
    {
        _mainCamera = FindObjectOfType<Camera>();
    }
    public void StartTrip()
    {
        _bicycleButton.gameObject.SetActive(false);
        StartCoroutine(SpawnTrips());
    }
    private IEnumerator SpawnTrips()
    {
        for (int tripIndex = 0; tripIndex < _trips.Count; tripIndex++)
        {
            _hasReachedLastTripWaypoint = false;
            _photoDisplay.StartPhotoDisplay(_trips[tripIndex]);
            yield return (StartCoroutine(InstantiateTransportation(_trips[tripIndex])));
        }
        _bicycleButton.gameObject.SetActive(true);
    }
    private IEnumerator InstantiateTransportation(TripConfig trip)
    {
        Vector3 _directionToFace = trip.GetWayPoints()[1].transform.position - trip.GetWayPoints()[0].transform.position;
        //Instantiate Object
        _newTransportation = Instantiate(trip.GetTransportationPrefab(), trip.GetWayPoints()[0].transform.position, Quaternion.LookRotation(_directionToFace, _earth.ReturnBodyUp()));
        Debug.DrawRay(_newTransportation.transform.position, _directionToFace, Color.yellow);
        Time.timeScale = 0;
        //Change Object Rotation The following script made everything much worse. 
        /* Vector3 _distanceToNextWaypoint = trip.GetWayPoints()[1].transform.position - _newTransportation.transform.position;
        float _distanceToPlane = Vector3.Dot(_newTransportation.transform.up, _distanceToNextWaypoint);
        Vector3 _pointOnPlane = trip.GetWayPoints()[1].transform.position - (_newTransportation.transform.up * _distanceToPlane);
        _newTransportation.transform.localRotation = Quaternion.LookRotation(_pointOnPlane - _newTransportation.transform.position, _newTransportation.transform.up);
        */

        //Debug.Log("target position: " + trip.GetWayPoints()[1].transform.position);
        //Debug.Log("instantiate position: " + trip.GetWayPoints()[0].transform.position);
        //Quaternion.LookRotation(trip.GetWayPoints()[1].transform.position - trip.GetWayPoints()[0].transform.position, _earth.ReturnBodyUp()));
        //Quaternion.Euler(0f, 180f, 0f))
        _mainCamera.gameObject.SetActive(false);
        _newTransportation.SetTripConfig(trip);
        yield return new WaitUntil(() => _hasReachedLastTripWaypoint == true); //Doing some crazy casting from a system.fun<bool> to a regular bool. Need to look into this more. 
    }

    public bool ManageLastWayPoint(bool trueorfalse)
    {
        _hasReachedLastTripWaypoint = trueorfalse;
        return _hasReachedLastTripWaypoint;
    }

}
