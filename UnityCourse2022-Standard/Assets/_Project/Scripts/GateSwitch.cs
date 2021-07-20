using UnityEngine;
using UnityEngine.Playables;

public class GateSwitch : MonoBehaviour
{
    public PlayableDirector timeline;
    public AudioSource audioSource;

    void Start()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        timeline.Play();
        audioSource.Play();
        gameObject.SetActive(false);
    }
}
