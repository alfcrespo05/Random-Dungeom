using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public int vidas = 6;
    [SerializeField] public int estamina = 0;
    public TMP_Text vidasLeftText;

    [Header("Corazones")]
    [SerializeField] private GameObject vida1;
    [SerializeField] private GameObject vida2;
    [SerializeField] private GameObject vida3;
    [SerializeField] private GameObject vidaPiedra1;
    [SerializeField] private GameObject vidaPiedra2;
    [Header("Mitades Corazones")]
    [SerializeField] private GameObject vidam1;
    [SerializeField] private GameObject vidam2;
    [SerializeField] private GameObject vidam3;
    [SerializeField] private GameObject vidaPiedram1;
    [SerializeField] private GameObject vidaPiedram2;

    public TMP_Text estaminaLeftText;

    [SerializeField] private GameObject prefabJugador;

    public static GameManager Instance;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        //vidasLeftText.text = $"<3 <3 <3";
        vida1.SetActive(true);
        vida2.SetActive(true);
        vida3.SetActive(true);
        vidaPiedra1.SetActive(false);
        vidaPiedra2.SetActive(false);

        vidam1.SetActive(false);
        vidam2.SetActive(false);
        vidam3.SetActive(false);
        vidaPiedram1.SetActive(false);
        vidaPiedram2.SetActive(false);

        EstaminaRestante();
        estamina = UnityEngine.Random.Range(15, 101); //15, 101
        SpawnJugador(); // spawna el jugador al inicio
    }

    // Update is called once per frame
    void Update()
    {
        EstaminaRestante();

        Hielo[] cartasHielo = FindObjectsByType<Hielo>(FindObjectsSortMode.None);
        Piedra[] cartasPiedra = FindObjectsByType<Piedra>(FindObjectsSortMode.None);
        Aire[] cartasAire = FindObjectsByType<Aire>(FindObjectsSortMode.None);
        Velocidad[] cartasVelocidad = FindObjectsByType<Velocidad>(FindObjectsSortMode.None);
        Inmovilidad[] cartasInmovilidad = FindObjectsByType<Inmovilidad>(FindObjectsSortMode.None);
        DobleSalto[] cartasDobleSalto = FindObjectsByType<DobleSalto>(FindObjectsSortMode.None);

        bool puedePagarAlguna = false;

        foreach (Hielo carta in cartasHielo)
            if (carta.costeEstamina <= estamina) { puedePagarAlguna = true; break; }
        foreach (Piedra carta in cartasPiedra)
            if (carta.costeEstamina <= estamina) { puedePagarAlguna = true; break; }
        foreach (Aire carta in cartasAire)
            if (carta.costeEstamina <= estamina) { puedePagarAlguna = true; break; }
        foreach (Velocidad carta in cartasVelocidad)
            if (carta.costeEstamina <= estamina) { puedePagarAlguna = true; break; }
        foreach (Inmovilidad carta in cartasInmovilidad)
            if (carta.costeEstamina <= estamina) { puedePagarAlguna = true; break; }
        foreach (DobleSalto carta in cartasDobleSalto)
            if (carta.costeEstamina <= estamina) { puedePagarAlguna = true; break; }

        estaminaLeftText.color = puedePagarAlguna ? Color.white : Color.red;
    }
    private void FixedUpdate()
    {
        ContarVidas();
        
    }
    public void EstaminaRestante()
    {
        estaminaLeftText.text = $"Estamina: " + estamina;
    }
    public void GameOver()
    {

    }
    public void ContarVidas()
    {
        if (vidas == 10)
        {
            //vidasLeftText.text = $"<3 <3 <3 <3 <3";
            //vidasLeftText.color = new Color(0.31f, 0.27f, 0.25f);
            
            vida1.SetActive(true);
            vida2.SetActive(true);
            vida3.SetActive(true);
            vidaPiedra1.SetActive(true);
            vidaPiedra2.SetActive(true);

            vidam1.SetActive(false);
            vidam2.SetActive(false);
            vidam3.SetActive(false);
            vidaPiedram1.SetActive(false);
            vidaPiedram2.SetActive(false);
        }
        if (vidas == 9)
        {
            //vidasLeftText.text = $"<3 <3 <3 <3 <";
            vida1.SetActive(true);
            vida2.SetActive(true);
            vida3.SetActive(true);
            vidaPiedra1.SetActive(true);
            vidaPiedra2.SetActive(false);

            vidam1.SetActive(false);
            vidam2.SetActive(false);
            vidam3.SetActive(false);
            vidaPiedram1.SetActive(false);
            vidaPiedram2.SetActive(true);

        }
        if (vidas == 8)
        {
            //vidasLeftText.text = $"<3 <3 <3 <3";
            vida1.SetActive(true);
            vida2.SetActive(true);
            vida3.SetActive(true);
            vidaPiedra1.SetActive(true);
            vidaPiedra2.SetActive(false);

            vidam1.SetActive(false);
            vidam2.SetActive(false);
            vidam3.SetActive(false);
            vidaPiedram1.SetActive(false);
            vidaPiedram2.SetActive(false);

        }
        if (vidas == 7)
        {
            //vidasLeftText.text = $"<3 <3 <3 <";
            vida1.SetActive(true);
            vida2.SetActive(true);
            vida3.SetActive(true);
            vidaPiedra1.SetActive(false);
            vidaPiedra2.SetActive(false);

            vidam1.SetActive(false);
            vidam2.SetActive(false);
            vidam3.SetActive(false);
            vidaPiedram1.SetActive(true);
            vidaPiedram2.SetActive(false);

        }
        if (vidas == 6)
        {
            //vidasLeftText.text = $"<3 <3 <3";
            //vidasLeftText.color = new Color(1f, 1f, 1f);
            vida1.SetActive(true);
            vida2.SetActive(true);
            vida3.SetActive(true);
            vidaPiedra1.SetActive(false);
            vidaPiedra2.SetActive(false);

            vidam1.SetActive(false);
            vidam2.SetActive(false);
            vidam3.SetActive(false);
            vidaPiedram1.SetActive(false);
            vidaPiedram2.SetActive(false);
        }
        if (vidas == 5)
        {
            //vidasLeftText.text = $"<3 <3 <";
            vida1.SetActive(true);
            vida2.SetActive(true);
            vida3.SetActive(false);
            vidaPiedra1.SetActive(false);
            vidaPiedra2.SetActive(false);

            vidam1.SetActive(false);
            vidam2.SetActive(false);
            vidam3.SetActive(true);
            vidaPiedram1.SetActive(false);
            vidaPiedram2.SetActive(false);
        }
        if (vidas == 4)
        {
            //vidasLeftText.text = $"<3 <3";
            vida1.SetActive(true);
            vida2.SetActive(true);
            vida3.SetActive(false);
            vidaPiedra1.SetActive(false);
            vidaPiedra2.SetActive(false);

            vidam1.SetActive(false);
            vidam2.SetActive(false);
            vidam3.SetActive(false);
            vidaPiedram1.SetActive(false);
            vidaPiedram2.SetActive(false);
        }
        if (vidas == 3)
        {
            //vidasLeftText.text = $"<3 <";
            vida1.SetActive(true);
            vida2.SetActive(false);
            vida3.SetActive(false);
            vidaPiedra1.SetActive(false);
            vidaPiedra2.SetActive(false);

            vidam1.SetActive(false);
            vidam2.SetActive(true);
            vidam3.SetActive(false);
            vidaPiedram1.SetActive(false);
            vidaPiedram2.SetActive(false);
        }
        if (vidas == 2)
        {
            //vidasLeftText.text = $"<3";
            vida1.SetActive(true);
            vida2.SetActive(false);
            vida3.SetActive(false);
            vidaPiedra1.SetActive(false);
            vidaPiedra2.SetActive(false);

            vidam1.SetActive(false);
            vidam2.SetActive(false);
            vidam3.SetActive(false);
            vidaPiedram1.SetActive(false);
            vidaPiedram2.SetActive(false);
        }
        if (vidas == 1)
        {
            //vidasLeftText.text = $"<";
            vida1.SetActive(false);
            vida2.SetActive(false);
            vida3.SetActive(false);
            vidaPiedra1.SetActive(false);
            vidaPiedra2.SetActive(false);

            vidam1.SetActive(true);
            vidam2.SetActive(false);
            vidam3.SetActive(false);
            vidaPiedram1.SetActive(false);
            vidaPiedram2.SetActive(false);
        }
        if (vidas == 0)
        {
            //vidasLeftText.text = $"";
            vida1.SetActive(false);
            vida2.SetActive(false);
            vida3.SetActive(false);
            vidaPiedra1.SetActive(false);
            vidaPiedra2.SetActive(false);

            vidam1.SetActive(false);
            vidam2.SetActive(false);
            vidam3.SetActive(false);
            vidaPiedram1.SetActive(false);
            vidaPiedram2.SetActive(false);
        }
    }
    //JUGADOR
    public void SpawnJugador()
    {
        if (vidas != 0)
        {
            GameObject nuevoJugador = Instantiate(prefabJugador, new Vector3(-7.5f, -2.86f, 0), Quaternion.identity);
           
        }
        else GameOver();
    }
    //PIEDRA
    public void AñadirPiedra()
    {
        vidas += 4;
       

    }
    
}
