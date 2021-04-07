using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoDisplay : MonoBehaviour
{
    RawImage _thisRawImage;
    private Texture2D[] _tripPhotos;
    [SerializeField] float _timeBetweenPhotos;
    [SerializeField] int _photoSizeScale = 1;


    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    public float StartPhotoDisplay(TripConfig thisTrip)
    {
        this.gameObject.SetActive(true);
        _thisRawImage = GetComponent<RawImage>(); //Gets the Raw Image UI Component
        _tripPhotos = thisTrip.GetPhotos();
        StartCoroutine(DisplayPhotos());
        Debug.Log("Time between photos: " + _timeBetweenPhotos);
        return ReturnTotalTripTime(thisTrip);
    }

    private IEnumerator DisplayPhotos()
    {
        foreach (var photo in _tripPhotos)
        {
            var _photoHeight = photo.height;
            var _photoWidth = photo.width;
            GetComponent<RectTransform>().sizeDelta = new Vector2(_photoWidth, _photoHeight)/_photoSizeScale;
            _thisRawImage.texture = photo;
            yield return new WaitForSeconds(_timeBetweenPhotos);
        }
    }
    private float ReturnTotalTripTime(TripConfig trip)
    {
        var _numPhotos = _tripPhotos.Length;
        return (_numPhotos * _timeBetweenPhotos);
    }
}
