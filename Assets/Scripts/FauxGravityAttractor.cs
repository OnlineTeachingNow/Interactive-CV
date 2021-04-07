using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FauxGravityAttractor : MonoBehaviour
{
    [SerializeField] float _gravity = -20;
    float _xzRotationFactor = 50f;
    Vector3 _gravityUp;
    Vector3 _bodyUp;
    public void Attract(Transform body)
    {
        _gravityUp = (body.position - this.transform.position).normalized;
        _bodyUp = body.up;

        //Adding Gravity Force
        body.GetComponent<Rigidbody>().AddForce(_gravityUp * _gravity);

        //X-Z Rotation
        Quaternion _targetRotation = Quaternion.FromToRotation(_bodyUp, _gravityUp) * body.rotation;
        body.rotation = Quaternion.Slerp(body.rotation, _targetRotation, _xzRotationFactor * Time.deltaTime);
    }

    public Vector3 ReturnBodyUp()
    {
        return _bodyUp;
    }
}
