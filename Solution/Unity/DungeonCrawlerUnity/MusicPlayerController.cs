using UnityEngine;

namespace CaptainCoder.DungeonCrawler.Unity;

public class MusicPlayerController : MonoBehaviour
{
    public AudioSource AudioSource = default!;

    public void OnVolumeChange(float value) => AudioSource.volume = value;
}