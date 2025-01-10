using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicSource : MonoBehaviour, IMusicChanger
{
    public AudioSource Source;

    private EventBus _eventBus;

    public void Init(EventBus eventBus)
    {
        _eventBus = eventBus;
        _eventBus.Subscribe(this);
    }
    private void OnDisable()
    {
        _eventBus.Unsubscribe(this);
    }

    public void ChangeSoundVolume(float volume)
    {
        Source.volume = volume;
    }
}
