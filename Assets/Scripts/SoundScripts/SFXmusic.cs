using UnityEngine;

public class SFXmusic : MonoBehaviour
{
    public AudioSource SFXMusic;

    public void PlaySoundEffects(AudioClip clip)
    {
        SFXMusic.PlayOneShot(clip);

    }
}
