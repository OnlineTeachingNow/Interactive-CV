using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waypoint : MonoBehaviour
{
    TripSpawner _tripSpawner;
    private void OnTriggerEnter(Collider other)
    {
        _tripSpawner = FindObjectOfType<TripSpawner>();
        if (other.tag == "bicycle")
        {
            if (_tripSpawner != null)
            {
                _tripSpawner.ManageLastWayPoint(true);
            //    Debug.Log("collision");
            }
            else
            {
                Debug.LogError("TripSpawner is NULL.");
            }
        }
    }
}
