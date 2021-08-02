using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ImageTracking : MonoBehaviour
{
    [System.Serializable]
    public class ARObject
    {
        public string name;
        public GameObject prefab;
    }

    public ARObject[] settings;
    public ARTrackedImageManager imageTrcker;
    private Dictionary<string, GameObject> arItems = new Dictionary<string, GameObject>();

    void Start()
    {
        foreach (var item in settings)
        {
            GameObject newPrefab = Instantiate(item.prefab);
            newPrefab.name = item.name;
            newPrefab.SetActive(false);
            arItems.Add(item.name, newPrefab);
        }

        imageTrcker.trackedImagesChanged += onImageChanged;
    }

    void onImageChanged(ARTrackedImagesChangedEventArgs markers)
    {
        foreach (ARTrackedImage marker in markers.added)
        {
            GameObject item = arItems[marker.referenceImage.name];
            item.SetActive(true);
            item.transform.position = marker.transform.position;
            item.transform.rotation = Quaternion.identity;
        }
    }
}
