using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FauxGravityAttractor : MonoBehaviour
{
    [SerializeField] float _gravity = -20;
    Vector3 _gravityUp;
    Vector3 _bodyUp;
    Vector3 _bodyForward;
    Quaternion _currentObjectRotation;
    public void Attract(Transform body)
    {
        _gravityUp = (body.position - this.transform.position).normalized;
        _bodyUp = body.up;
        _bodyForward = body.forward; //getting the local z axis position. 

        body.GetComponent<Rigidbody>().AddForce(_gravityUp * _gravity);
        Quaternion _targetRotation = Quaternion.FromToRotation(_bodyUp, _gravityUp) * body.rotation;
        body.rotation = Quaternion.Slerp(body.rotation, _targetRotation, 50 * Time.deltaTime);
        _currentObjectRotation = body.rotation;
    }

    public Quaternion ReturnCurrentRotation()
    {
        return _currentObjectRotation;
    }
}
