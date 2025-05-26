using Cube;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource MusicSource;
    [SerializeField] private AudioClip[] Soundtracks;

    [SerializeField] private AudioSource FXSource;
    [SerializeField] private AudioClip RollSound;
    [SerializeField] private AudioClip DeathSound;

    [SerializeField] private Cube.Cube Cube;
    [SerializeField] private CubeManager CubeManager;

    private void Awake()
    {
        Cube.OnMoved += _ => PlayRollSound();
        CubeManager.OnCubeDestroyed += PlayDeathSound;
    }

    public void ToggleSound() => MusicSource.mute = !MusicSource.mute;

    public void Update()
    {
        if (MusicSource.isPlaying) return;
        
        MusicSource.clip = Soundtracks[Random.Range(0, Soundtracks.Length)];
        MusicSource.Play();
    }

    private void PlayRollSound()
    {
        FXSource.pitch = Random.Range(0.75f, 1.25f);
        FXSource.clip = RollSound;
        FXSource.Play();
    }
    
    private void PlayDeathSound()
    {
        FXSource.clip = DeathSound;
        FXSource.Play();
    }
}