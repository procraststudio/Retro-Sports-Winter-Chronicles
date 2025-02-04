using UnityEngine;

public static class SoundManager
{

    public enum Sound
    {
        Start,
        Disqualified,
        OutOf15,
        CrowdHappy,
    }

    public static void PlaySound(AudioClip clip)
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        //audioSource.PlayOneShot(soundClips[numberOfSound]);
        audioSource.PlayOneShot(clip);
        Object.Destroy(soundGameObject, clip.length);

    }

    public static void PlaySoundOnHover(AudioClip clip)
    {
        //
    }

    //public static void PlayRandomSound(AudioClip[] sounds)
    //{
    //    GameObject soundGameObject = new GameObject("Sound");
    //    AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
    //    int randomIndex = Random.Range(0, sounds.Length);
    //    audioSource.clip = sounds[randomIndex];
    //    //audioSource.PlayOneShot(soundClips[numberOfSound]);
    //     audioSource.PlayOneShot(sounds[randomIndex]);
    //    //audioSource.clip = sounds[randomIndex];
    //    //audioSource.Play();
    //}

    public static void PlayOneSound(string soundName)
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/" + soundName);
        if (clip != null)
        {
            GameObject soundObject = new GameObject("Sound");
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.Play();
            Object.Destroy(soundObject, clip.length);
        }
        else
        {
            Debug.LogWarning("Sound not found: " + soundName);
        }
    }

    public static void PlayRandomSound(string[] soundNames)
    {
        int randomIndex = Random.Range(0, soundNames.Length);
        AudioClip clip = Resources.Load<AudioClip>("Sounds/" + soundNames[randomIndex]);
        if (clip != null)
        {
            GameObject soundObject = new GameObject("Sound");
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.Play();
            Object.Destroy(soundObject, clip.length);
        }
        else
        {
            Debug.LogWarning("Sound not found: " + clip.name);
        }
    }
}


