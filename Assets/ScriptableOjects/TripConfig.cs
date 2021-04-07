using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Trips.asset", menuName = "Trips/Trip")]
public class TripConfig : ScriptableObject
{
    [SerializeField] private string _tripName;
    [SerializeField] private TransportPathing _transportPrefab;
    [SerializeField] private int _numDays;
    [SerializeField] private GameObject _pathPrefab;
    [SerializeField] private Texture2D[] _tripPhotos;
    public TransportPathing GetTransportationPrefab()
    {
        return _transportPrefab;
    }
    public List<Transform> GetWayPoints()
    {
        List<Transform> _tripWayPoints = new List<Transform>();
        foreach (Transform child in _pathPrefab.transform)
        {
           _tripWayPoints.Add(child);
        }
        return _tripWayPoints; 
    }
    public int GetNumberOfDays()
    {
        return _numDays;
    }
    public string GetTripName()
    {
        return _tripName;
    }
    public Texture2D[] GetPhotos()
    {
        return _tripPhotos;
    }
}
