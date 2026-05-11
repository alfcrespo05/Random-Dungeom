using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource musicaSource;
    public AudioSource sonidoSource;

    [Header("UI Sounds")]
    public AudioClip clickSound;

    [Header("Gameplay Sounds")]
    public AudioClip muerteSound;
    public AudioClip dañoSound;
    public AudioClip estrella;
    public AudioClip muerteEnemigo;
    public AudioClip enemigoDisparo;
    public AudioClip balaColision;
    public AudioClip Salto;

    [Header("Musica")]
    public AudioClip musicaMenu;
    public AudioClip musicaRandomDungeom;
    public AudioClip musicaVictoria;
    public AudioClip musicaDerrota;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            musicaSource.volume = PlayerPrefs.GetFloat("volumenMusica", 0.1f);
            sonidoSource.volume = PlayerPrefs.GetFloat("volumenSonido", 20f);

            // Suscribirse al evento de cambio de escena
            SceneManager.sceneLoaded += OnScenaCargada;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnScenaCargada;
    }

    void OnScenaCargada(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
            PlayMusica(musicaMenu);
        else
            PlayMusica(musicaRandomDungeom);
    }

    public void PlayMusica(AudioClip clip)
    {
        if (musicaSource.clip == clip) return;
        musicaSource.clip = clip;
        musicaSource.loop = true;
        musicaSource.Play();
    }

    public void StopMusica()
    {
        musicaSource.Stop();
    }

    public void SetVolumenMusica(float valor)
    {
        musicaSource.volume = valor;
        PlayerPrefs.SetFloat("volumenMusica", valor);
        PlayerPrefs.Save();
    }

    public void SetVolumenSonido(float valor)
    {
        sonidoSource.volume = valor;
        PlayerPrefs.SetFloat("volumenSonido", valor);
        PlayerPrefs.Save();
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
            sonidoSource.PlayOneShot(clip);
    }

    public void PlayClick()
    {
        PlaySound(clickSound);
    }
}

