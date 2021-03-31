using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FauxGravityAttractor : MonoBehaviour
{
    [SerializeField] float _gravity = -20;
    public void Attract(Transform body)
    {
        Vector3 _gravityUp = (body.position - this.transform.position).normalized;
        Vector3 _bodyUp = body.up;

        body.GetComponent<Rigidbody>().AddForce(_gravityUp * _gravity);
        Quaternion _targetRotation = Quaternion.FromToRotation(_bodyUp, _gravityUp) * body.rotation;
        body.rotation = Quaternion.Slerp(body.rotation, _targetRotation, 50 * Time.deltaTime);
    }
}
