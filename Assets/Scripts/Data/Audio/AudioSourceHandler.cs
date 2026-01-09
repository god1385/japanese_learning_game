using UnityEngine;

public class AudioSourceHandler : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.loop = false;
        _audioSource.spatialBlend = 0f;
        _audioSource.volume = 1f;
    }

    public void PlayAudio(AudioClip clip)
    {
        Debug.Log(clip.ToString());
        _audioSource.Stop();
        _audioSource.PlayOneShot(clip);
    }

    public void StopAudio() => _audioSource.Stop();
}
