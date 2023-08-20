using System.Collections;
using UnityEngine;

public enum TileKind { None, Dirt, Grass, Stone, Path, Wood}
public class StepSounds : MonoBehaviour
{
    public AudioClip[] dirtSounds;
    public AudioClip[] grassSounds;
    public AudioClip[] stoneSounds;
    public AudioClip[] pathSounds;
    public AudioClip[] woodSounds;

    private AudioSource audioSource;
    public PlayerMovement movement;

    public TileKind kind;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public IEnumerator PlayRandomSound()
    {
        yield return new WaitWhile(() => audioSource.isPlaying);

        switch (kind)
        {
            case TileKind.Grass:
                audioSource.clip = grassSounds[Random.Range(0, grassSounds.Length)];                
                break;
            case TileKind.Dirt:
                audioSource.clip = dirtSounds[Random.Range(0, dirtSounds.Length)];
                break;
            case TileKind.Stone:
                audioSource.clip = stoneSounds[Random.Range(0, stoneSounds.Length)];
                break;
            case TileKind.Path:
                audioSource.clip = pathSounds[Random.Range(0, pathSounds.Length)];
                break;
            case TileKind.Wood:
                audioSource.clip = woodSounds[Random.Range(0, woodSounds.Length)];
                break;
        };
        audioSource.Play();
    }

    public void Update()
    {
        if ((movement.m_PlayerMovement.x != 0 || movement.m_PlayerMovement.y != 0) && !movement.forbidToWalk)
        {
            StartCoroutine(PlayRandomSound());
        }
        else
        {
            StopCoroutine(PlayRandomSound());
            audioSource.Stop();
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "Dirt")
        {
            kind = TileKind.Dirt;
        }
        
        if (collision.name == "Gras")
        {
            kind = TileKind.Grass;
        }
        
        if (collision.name == "Stone")
        {
            kind = TileKind.Stone;
        }
        
        if (collision.name == "Path")
        {
            kind = TileKind.Path;
        }

        if (collision.name == "Wood")
        {
            kind = TileKind.Wood;
        }
    }
}
