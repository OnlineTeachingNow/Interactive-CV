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
    private float _moveSpeed;
    [SerializeField] private Texture2D[] _tripPhotos;
        //Probably going to need to put the photo references in here. 
        //probably going to put the instantiate code in here.

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
            //Check to make sure that the parent transform is not added. Then, I will have to delete the code here. 
        }

        public float GetMoveSpeed()
        {
            return _moveSpeed;
        }

        public int GetNumberOfDays()
        {
            return _numDays;
        }

        public void SetTripSpeed(float speed)
        {
            _moveSpeed = speed;
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
