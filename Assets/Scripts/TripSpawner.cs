using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripSpawner : MonoBehaviour
{
    [SerializeField] private List<TripConfig> _trips;
    TransportPathing _transportPathing;
    private Transform _originTransform;
    private GameObject _transportPrefab;
    TransportPathing _newTransportation;
    bool _hasReachedLastTripWaypoint = false;
    //bool _looping = false;

    private void Start()
    {

    }
    public void StartTrip()
    {
        StartCoroutine(SpawnTrips());
    }

    /*
     *The following method doesn't seem to do much when passed over, so I decided to pass it over. 
     * 
    private IEnumerator StartTripSpawning()
    {
        do
        {
            yield return StartCoroutine(SpawnTrips());
        }
        while (_looping == true);
    }
    */

    private IEnumerator SpawnTrips()
    {
        for (int tripIndex = 0; tripIndex <= _trips.Count; tripIndex++)
        {
            Debug.Log("Trip Count: " + _trips.Count);
            Debug.Log("Trip Index: " + tripIndex);
            _hasReachedLastTripWaypoint = false;
            yield return (StartCoroutine(InstantiateTransportation(_trips[tripIndex])));
        }
    }

    private IEnumerator InstantiateTransportation(TripConfig trip)
    {
        _newTransportation = Instantiate(trip.GetTransportationPrefab(), trip.GetWayPoints()[0].transform.position, Quaternion.identity);
        _newTransportation.SetTripConfig(trip);
      //  _newTransportation.GetComponent<TransportPathing>().SetTripConfig(trip); ; 
       /*
        if (_transportPathing != null)
        {
       */
            yield return new WaitUntil(() => _hasReachedLastTripWaypoint == true); //Doing some crazy casting from a system.fun<bool> to a regular bool. Need to look into this more.
            Debug.Log("_transportPathing.hasReachedLastWayPoint");
        /*}
        else
        {
            yield return new WaitForSeconds(5f);
            Debug.LogError("Transport pathing is NULL.");
        } */ 
    }

    public bool ManageLastWayPoint(bool trueorfalse)
    {
        _hasReachedLastTripWaypoint = trueorfalse;
        return _hasReachedLastTripWaypoint;
    }

}
