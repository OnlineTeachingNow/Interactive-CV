using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoDisplay : MonoBehaviour
{
    RawImage _thisRawImage;
    private Texture2D[] _tripPhotos;
    float _thisWidth;
    float _thisHeight;
    [SerializeField] float _timeBetweenPhotos = 1.0f;
    [SerializeField] int _photoSizeScale = 1;
    float _totalTimeForAllPhotos;

    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void StartPhotoDisplay(TripConfig thisTrip)
    {
        this.gameObject.SetActive(true);
        _thisRawImage = GetComponent<RawImage>();
        _tripPhotos = thisTrip.GetPhotos();
        SetTripSpeed(thisTrip);
        StartCoroutine(DisplayPhotos());
    }

    private IEnumerator DisplayPhotos()
    {
        foreach (var photo in _tripPhotos)
        {
            var _photoHeight = photo.height;
            var _photoWidth = photo.width;
            Debug.Log("Width: " + _photoWidth + " and height: " + _photoHeight);
            GetComponent<RectTransform>().sizeDelta = new Vector2(_photoWidth, _photoHeight)/_photoSizeScale;
            //Debug.Log("raw image size delta: " + _thisRawImage.rectTransform.sizeDelta);
            _thisRawImage.texture = photo;
            yield return new WaitForSeconds(_timeBetweenPhotos);
        }
    }
    private void SetTripSpeed(TripConfig trip)
    {
        var _numPhotos = _tripPhotos.Length;
        _totalTimeForAllPhotos = _numPhotos * _timeBetweenPhotos;
        //number of waypoints
        //dividing the photo count by number of waypoints 
        //
    }

    public float ReturnTotalTripTime()
    {
        return _totalTimeForAllPhotos;
    }

}
