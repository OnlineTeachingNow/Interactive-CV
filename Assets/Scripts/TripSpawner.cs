using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TripSpawner : MonoBehaviour
{
    [SerializeField] private List<TripConfig> _trips;
    [SerializeField] Button _bicycleButton;
    [SerializeField] PhotoDisplay _photoDisplay; 
    TransportPathing _transportPathing;
    Camera _mainCamera;
    private Transform _originTransform;
    private GameObject _transportPrefab;
    TransportPathing _newTransportation;
    bool _hasReachedLastTripWaypoint = false;

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
        _newTransportation = Instantiate(trip.GetTransportationPrefab(), trip.GetWayPoints()[0].transform.position, Quaternion.identity);
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
