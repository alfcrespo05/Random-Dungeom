using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public int vidas = 6;
    [SerializeField] public int estamina = 0;
    public TMP_Text vidasLeftText;
    public TMP_Text estaminaLeftText;

    [SerializeField] private GameObject prefabJugador;

    void Start()
    {
        vidasLeftText.text = $"<3 <3 <3";
        EstaminaRestante();
        estamina = UnityEngine.Random.Range(15, 101);
        SpawnJugador(); // spawna el jugador al inicio
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
        if (vidas == 6)
        {
            vidasLeftText.text = $"<3 <3 <3";
        }
        if (vidas == 5)
        {
            vidasLeftText.text = $"<3 <3 <";
        }
        if (vidas == 4)
        {
            vidasLeftText.text = $"<3 <3";
        }
        if (vidas == 3)
        {
            vidasLeftText.text = $"<3 <";
        }
        if (vidas == 2)
        {
            vidasLeftText.text = $"<3";
        }
        if (vidas == 1)
        {
            vidasLeftText.text = $"<";
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
            GameObject nuevoJugador = Instantiate(prefabJugador, new Vector3(-7.5f, -2.86f, 0), Quaternion.identity);
           
        }
        else GameOver();
    }
    public void GameOver()
    {

    }
}
