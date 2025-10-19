using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioSource audioSource;

    public bool playOneTime;

    private void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (playOneTime)
        {
            if (!audioSource.isPlaying)
                audioSource.PlayOneShot(clip);
        }
        else
            audioSource.PlayOneShot(clip);
    }
}
