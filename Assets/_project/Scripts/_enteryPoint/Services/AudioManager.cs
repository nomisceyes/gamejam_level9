using UnityEngine;

public class AudioManager : MonoBehaviour, IService
{
    private AudioSource _musicSource;
    private AudioSource _soundSource;

    public float MusicVolume { get; private set; } = 0.4f;
    public float SoundVolume { get; private set; } = 0.75f;

    private float _savedMusicTime = 0f;

    public void Init()
    {
        GameObject mSource = new GameObject("MusicSource") { transform = { parent = transform } };
        _musicSource = mSource.AddComponent<AudioSource>();
        _musicSource.loop = true;

        GameObject sSource = new GameObject("SoundSource") { transform = { parent = transform } };
        _soundSource = sSource.AddComponent<AudioSource>();
        _soundSource.loop = false;

        SetMusicVolume(MusicVolume);
        SetSoundVolume(SoundVolume);
    }

    public void SetMusicVolume(float value)
    {
        MusicVolume = value;
        _musicSource.volume = MusicVolume;
    }

    public void SetSoundVolume(float value)
    {
        SoundVolume = value;
        _soundSource.volume = SoundVolume;
    }

    public void PlayMusic(AudioClip clip)
    {
        if (_musicSource.clip == clip && _musicSource.isPlaying) return;

        _musicSource.clip = clip;
        _musicSource.Play();
    }

    public void ResumeMusic()
    {
        if (_musicSource.clip != null && _musicSource.isPlaying == false)
        {
            _musicSource.time = _savedMusicTime;
            _musicSource.Play();
        }
    }

    public void PauseMusic()
    {
        if (_musicSource.clip == null) return;

        _savedMusicTime = _musicSource.time;
        _musicSource.Stop();
    }

    public void PlaySound(AudioClip clip, float addedPitch = 1f)
    {
        if (clip == null) return;

        GameObject tempAudioObject = new GameObject("TempAudio: " + clip.name);
        DontDestroyOnLoad(tempAudioObject);
        AudioSource audioSource = tempAudioObject.AddComponent<AudioSource>();

        audioSource.clip = clip;
        audioSource.volume = SoundVolume;
        audioSource.Play();

        Destroy(tempAudioObject, (clip.length / audioSource.pitch) + 0.1f);
    }
}