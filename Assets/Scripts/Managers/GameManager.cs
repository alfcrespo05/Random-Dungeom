using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public int vidas = 3;
    [SerializeField] public int estamina = 0;
    public TMP_Text vidasLeftText;
    public TMP_Text estaminaLeftText;

    [SerializeField] private GameObject prefabJugador;
    void Start()
    {
        vidasLeftText.text = $"<3 <3 <3";
        SpawnJugador();
        EstaminaRestante();
        estamina = UnityEngine.Random.Range(15, 101);
    }

    // Update is called once per frame
    void Update()
    {
        EstaminaRestante();
    }
    private void FixedUpdate()
    {
        ContarVidas();
        
    }
    public void EstaminaRestante()
    {
        estaminaLeftText.text = $"Estamina: " + estamina;
    }
    public void ContarVidas()
    {
        if (vidas == 3)
        {
            vidasLeftText.text = $"<3 <3 <3";
        }
        if (vidas == 2)
        {
            vidasLeftText.text = $"<3 <3";
        }
        if (vidas == 1)
        {
            vidasLeftText.text = $"<3";
        }
        if (vidas == 0)
        {
            vidasLeftText.text = $"";
        }
    }
    public void SpawnJugador()
    {
        if (vidas != 0)
        {
            Instantiate(prefabJugador, new Vector3(-7.5f, -2.86f, 0), Quaternion.Euler(0, 0, 0));
        }
        else GameOver();
    }
    public void GameOver()
    {

    }
}
