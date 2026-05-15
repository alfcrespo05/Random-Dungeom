using UnityEngine;
using UnityEngine.UI;

public class SliderAudio : MonoBehaviour
{
    [SerializeField] private bool esMusica = true;
    private Slider slider;
    private bool inicializando = false;

    void Start()
    {
        slider = GetComponent<Slider>();
        inicializando = true;

        if (esMusica)
            slider.value = PlayerPrefs.GetFloat("volumenMusica", 1f);
        else
            slider.value = PlayerPrefs.GetFloat("volumenSonido", 1f);

        inicializando = false;
        slider.onValueChanged.AddListener(CambiarVolumen);
    }

    void CambiarVolumen(float valor)
    {
        if (inicializando) return;
        if (AudioManager.Instance == null) return;

        if (esMusica)
            AudioManager.Instance.SetVolumenMusica(valor);
        else
            AudioManager.Instance.SetVolumenSonido(valor);
    }
}