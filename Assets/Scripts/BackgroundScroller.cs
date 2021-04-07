using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] float _backgroundScrollSpeed = 0.5f;
    Material _myMaterial;
    Vector2 _offset;
    void Start()
    {
        _myMaterial = GetComponent<Renderer>().material;
        _offset = new Vector2(_backgroundScrollSpeed, _backgroundScrollSpeed);
    }
    void Update()
    {
        _myMaterial.mainTextureOffset += _offset * Time.deltaTime;
    }
}
