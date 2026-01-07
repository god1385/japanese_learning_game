using UnityEngine;

public class AudioSourceHandler : MonoBehaviour
{
    public static AudioSourceHandler Instance { get; private set; }
    private AudioSource _audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
            return;
        }    
    }

    public void PlayAudio(AudioClip clip)
    {
        Debug.Log(clip.ToString());
        _audioSource.Stop();
        _audioSource.PlayOneShot(clip);
    }

    public void StopAudio() => _audioSource.Stop();
}
