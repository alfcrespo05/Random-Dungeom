using UnityEngine;
using UnityEngine.UI;

public class BotonMute : MonoBehaviour
{
    [SerializeField] private Slider musicaSlider;
    [SerializeField] private Slider sonidoSlider;
    [SerializeField] private GameObject Mute;
    [SerializeField] private GameObject Unmute;
    private float volumenMusicaAntes = 1f;
    private float volumenSonidoAntes = 1f;
    private bool esMute = false;

    void Start()
    {
        // Sincroniza sliders al iniciar
        if (musicaSlider != null)
            musicaSlider.value = PlayerPrefs.GetFloat("volumenMusica", 1f);
        if (sonidoSlider != null)
            sonidoSlider.value = PlayerPrefs.GetFloat("volumenSonido", 1f);
    }

    public void MuteUnmute()
    {
        if (!esMute)
        {
            volumenMusicaAntes = AudioManager.Instance.musicaSource.volume;
            volumenSonidoAntes = AudioManager.Instance.sonidoSource.volume;
            AudioManager.Instance.SetVolumenMusica(0f);
            AudioManager.Instance.SetVolumenSonido(0f);
            if (musicaSlider != null) musicaSlider.value = 0f;
            if (sonidoSlider != null) sonidoSlider.value = 0f;
            Mute.SetActive(true);
            Unmute.SetActive(false);
        }
        else
        {
            AudioManager.Instance.SetVolumenMusica(volumenMusicaAntes);
            AudioManager.Instance.SetVolumenSonido(volumenSonidoAntes);
            if (musicaSlider != null) musicaSlider.value = volumenMusicaAntes;
            if (sonidoSlider != null) sonidoSlider.value = volumenSonidoAntes;
            Mute.SetActive(false);
            Unmute.SetActive(true);
        }
        esMute = !esMute;
    }
}