using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public int vidas = 6;
    [SerializeField] public int estamina = 0;
    [SerializeField] public float tiempo = 0;
    public TMP_Text vidasLeftText;
    public TMP_Text estaminaLeftText;
    public TextMeshProUGUI timeLeftText;

    [Header("UI")]
    [SerializeField] private GameObject botonPausa;
    [SerializeField] private GameObject botonPlay;
    [SerializeField] private GameObject menuPausa;
    [SerializeField] private GameObject menuAjustes;
    bool play = true;
    public bool MoverCartas = true;

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

    private bool gameOver = false;

    [SerializeField] private GameObject prefabJugador;

    public static GameManager Instance;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        Time.timeScale = 0;
        botonPausa.SetActive(true);
        botonPlay.SetActive(true);
        menuPausa.SetActive(false);
        menuAjustes.SetActive(false);
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
        tiempo += Time.deltaTime;
        int minutes = Mathf.FloorToInt(tiempo / 60f);
        int seconds = Mathf.FloorToInt(tiempo % 60f);

        timeLeftText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        EstaminaRestante();

        Hielo[] cartasHielo = FindObjectsByType<Hielo>(FindObjectsSortMode.None);
        Piedra[] cartasPiedra = FindObjectsByType<Piedra>(FindObjectsSortMode.None);
        Aire[] cartasAire = FindObjectsByType<Aire>(FindObjectsSortMode.None);
        Velocidad[] cartasVelocidad = FindObjectsByType<Velocidad>(FindObjectsSortMode.None);
        Inmovilidad[] cartasInmovilidad = FindObjectsByType<Inmovilidad>(FindObjectsSortMode.None);
        DobleSalto[] cartasDobleSalto = FindObjectsByType<DobleSalto>(FindObjectsSortMode.None);
        Banco[] cartasBanco = FindObjectsByType<Banco>(FindObjectsSortMode.None);
        Llama[] cartasLlama = FindObjectsByType<Llama>(FindObjectsSortMode.None);

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
        foreach (Banco carta in cartasBanco)
            if (carta.costeEstamina <= estamina) { puedePagarAlguna = true; break; }
        foreach (Llama carta in cartasLlama)
            if (carta.costeEstamina <= estamina) { puedePagarAlguna = true; break; }

        estaminaLeftText.color = puedePagarAlguna ? Color.black : Color.red;
    }
    private void FixedUpdate()
    {
        ContarVidas();
        GameOver();
    }
    public void EstaminaRestante()
    {
        estaminaLeftText.text = $"Estamina: " + estamina;
    }
    public void GameOver()
    {
        if (vidas <= 0 && !gameOver)
        {
            gameOver = true;
            muerteJugador();
        }
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
        if (vidas <= 0)
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
            GameObject nuevoJugador = Instantiate(prefabJugador, new Vector3(-7.4f, -4.37f, 0), Quaternion.identity);

        }
        else GameOver();
    }
    public void muerteJugador()
    {
        PlayerController pc = FindFirstObjectByType<PlayerController>();
        if (pc != null)
            pc.Muerte();
    }
    //PIEDRA
    public void AñadirPiedra()
    {
        vidas += 4;
       

    }
    public void Pausa()
    {
        Time.timeScale = 0;
        botonPausa.SetActive(false);
        botonPlay.SetActive(false);
        menuPausa.SetActive(true);
        menuAjustes.SetActive(false);
        MoverCartas = false;

    }
    public void Play()
    {
        Time.timeScale = 1;
        play = false;
        MoverCartas = false;
        botonPausa.SetActive(true);
        botonPlay.SetActive(false);
        menuPausa.SetActive(false);
        menuAjustes.SetActive(false);
    }
    public void Reanudar()
    {
        if (play == true) { Time.timeScale = 0; MoverCartas = true; }
        else Time.timeScale = 1;
        if (play == true) botonPlay.SetActive(true);
        else botonPlay.SetActive(false);
        botonPausa.SetActive(true);
        menuPausa.SetActive(false);
        menuAjustes.SetActive(false);
    }

}
