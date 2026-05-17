using UnityEngine;

public class Lava : MonoBehaviour
{
    private AudioSource audioLava;
    void Start()
    {
        audioLava = gameObject.AddComponent<AudioSource>();
        audioLava.clip = AudioManager.Instance.Lava;
        audioLava.loop = true;
        audioLava.volume = 0.3f;
        if (Time.timeScale == 1) audioLava.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 1 && !audioLava.isPlaying) audioLava.Play();
        if (Time.timeScale == 0 && audioLava.isPlaying) audioLava.Pause();
    }
    void OnTriggerEnter2D(Collider2D other)
    {

        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc != null)
        {
            Rigidbody2D rbJugador = other.GetComponent<Rigidbody2D>();
            AudioManager.Instance.PlaySound(AudioManager.Instance.dañoSound);
            pc.Muerte();
            
            return;
        }
    }
}
