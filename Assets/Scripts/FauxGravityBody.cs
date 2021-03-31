using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FauxGravityBody : MonoBehaviour
{

    [SerializeField] FauxGravityAttractor _attractor;
    private Transform _myTransform;
    private Rigidbody _thisRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        _thisRigidbody = GetComponent<Rigidbody>();
        _thisRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        _thisRigidbody.useGravity = false;
        _myTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        _attractor.Attract(_myTransform);
    }
}
