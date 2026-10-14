using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class AudioSystem : StaticInstance<AudioSystem>
{
    [SerializeField]
    private bool mayaSTFU;
    public Sound[] Sounds2D;
    public Sound[] Sounds3D;
    public Sound[] Footsteps;
    public Sound[] Dialogues;
    public AudioMixerGroup AudioMixer2D;
    public AudioMixerGroup AudioMixer3D;
    public AudioMixerGroup AudioMixerFootsteps;
    public AudioMixerGroup AudioMixerDialogues;

    protected override void Awake()
    {
        base.Awake();

        foreach (Sound s in Sounds2D)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = false;
            s.source.outputAudioMixerGroup = AudioMixer2D;
        }
        foreach (Sound s in Dialogues)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            if (mayaSTFU)
                s.source.volume = 0f;
            else
                s.source.volume = s.volume;

            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = false;
            s.source.outputAudioMixerGroup = AudioMixerDialogues;
        }
    }

    private void Start()
    {
        Debug.Log("Playing white noise");
        Play2DSound("white noise");
    }

    public void Play2DSound(string name)
    {
        Sound s = Array.Find(Sounds2D, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Son: " + name + " introuvable !!!");
            return;
        }
        
        s.source.Play();
    }

    public void Play2DSoundRdmPitchVol(string name)
    {
        Sound s = Array.Find(Sounds2D, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Son: " + name + " introuvable !!!");
            return;
        }

        s.source.outputAudioMixerGroup = AudioMixer2D;
        float pitch = s.source.pitch;
        float volume = s.source.volume;
        s.source.pitch += UnityEngine.Random.Range(-0.05f, 0.05f);
        s.source.volume += UnityEngine.Random.Range(-0.05f, 0.05f);
        s.source.Play();
        StartCoroutine(WaitForAudio(s.source, volume, pitch));
    }

    public void PlayFootstep(Vector3 position)
    {
        Sound s = Footsteps[UnityEngine.Random.Range(0, Footsteps.Length)];

        // Copi� de la fonction de unity  de base
        GameObject gameObject = new GameObject("One shot audio");
        gameObject.transform.position = position;
        AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        audioSource.outputAudioMixerGroup = AudioMixerFootsteps;
        audioSource.clip = s.clip;
        audioSource.spatialBlend = 1f;
        audioSource.volume = s.volume + UnityEngine.Random.Range(-0.05f, 0.05f);
        audioSource.pitch = s.pitch + UnityEngine.Random.Range(-0.05f, 0.05f);
        audioSource.Play();
        UnityEngine.Object.Destroy(gameObject, s.clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
    }

    public void PlayDialogue(string name)
    {
        Sound s = Array.Find(Dialogues, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Son: " + name + " introuvable !!!");
            return;
        }


        // Check si il y a un autre dialogue en cours
        Sound soundCurrentlyPlaying = Array.Find(Dialogues, sound => sound.source.isPlaying);
        if (soundCurrentlyPlaying != null)
        {
            StartCoroutine(PlayDialogueWhenRdy(s, soundCurrentlyPlaying));
        }
        else
        {
            s.source.Play();
        }
    }

    IEnumerator PlayDialogueWhenRdy(Sound soundToPlay, Sound soundCurrentlyPlaying)
    {
        yield return new WaitWhile(() => soundCurrentlyPlaying.source.isPlaying);

        soundToPlay.source.Play();
    }

    public void Play3DSoundRdmPitchVol(string name, Vector3 position)
    {
        Sound s = Array.Find(Sounds3D, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Son: " + name + " introuvable !!!");
            return;
        }

        // Copi� de la fonction de unity  de base
        GameObject gameObject = new GameObject("One shot audio");
        gameObject.transform.position = position;
        AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        audioSource.outputAudioMixerGroup = AudioMixer3D;
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        audioSource.minDistance = 20f;
        audioSource.maxDistance = 50f;
        audioSource.clip = s.clip;
        audioSource.spatialBlend = 1f;
        audioSource.volume = s.volume + UnityEngine.Random.Range(-s.volume / 8, s.volume / 8);
        audioSource.pitch = s.pitch + UnityEngine.Random.Range(-0.05f, 0.05f);
        audioSource.Play();
        UnityEngine.Object.Destroy(gameObject, s.clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
    }

    public void Play3DSoundRdmPitchVol(string name, Vector3 position, float randomVolume, float randomPitch)
    {
        Sound s = Array.Find(Sounds3D, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Son: " + name + " introuvable !!!");
            return;
        }

        // Copi� de la fonction de unity  de base
        GameObject gameObject = new GameObject("One shot audio");
        gameObject.transform.position = position;
        AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        audioSource.outputAudioMixerGroup = AudioMixer3D;
        audioSource.clip = s.clip;
        audioSource.spatialBlend = 1f;
        audioSource.volume = s.volume + randomVolume;
        audioSource.pitch = s.pitch + randomPitch;
        audioSource.Play();
        UnityEngine.Object.Destroy(gameObject, s.clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
    }

    public void Play3DSoundRdmPitch(string name, Vector3 position, float playVolume)
    {
        Sound s = Array.Find(Sounds3D, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Son: " + name + " introuvable !!!");
            return;
        }

        // Copi� de la fonction de unity  de base
        GameObject gameObject = new GameObject("One shot audio");
        gameObject.transform.position = position;
        AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        audioSource.outputAudioMixerGroup = AudioMixer3D;
        audioSource.clip = s.clip;
        audioSource.spatialBlend = 1f;
        audioSource.volume = playVolume;
        audioSource.pitch = s.pitch + UnityEngine.Random.Range(-0.05f, 0.05f);
        audioSource.Play();
        UnityEngine.Object.Destroy(gameObject, s.clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
    }

    IEnumerator WaitForAudio(AudioSource audioSource, float oldVolume, float oldPitch)
    {
        yield return new WaitWhile(() => audioSource.isPlaying);

        audioSource.volume = oldVolume;
        audioSource.pitch = oldPitch;
    }
}