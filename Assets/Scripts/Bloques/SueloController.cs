using UnityEngine;

public class SueloController : MonoBehaviour
{
    [Header("Materiales y colores")]
    [SerializeField] private PhysicsMaterial2D materialNormal;
    [SerializeField] private PhysicsMaterial2D materialHielo;
    [SerializeField] private PhysicsMaterial2D materialPiedra;
    [SerializeField] private Sprite spriteTierra;
    [SerializeField] private Sprite spriteRoca;
    [SerializeField] private Sprite spriteMadera;
    private bool esHielo = false;
    [SerializeField] private Color colorHielo = new Color(0.5f, 0.8f, 1f);
    [SerializeField] private Color colorPiedra = new Color(0.31f, 0.27f, 0.25f);
    [SerializeField] private Color colorFuego = new Color(1f, 0.14f, 0f);

    [Header("Clase")]
    public bool esTierra = false;
    public bool esRoca = false;
    public bool esMadera = false;

    [Header("Pinchos")]
    [SerializeField] private GameObject spritePinchos; // hijo desactivado
    [SerializeField] private Vector2 pinchosCheckSize = new Vector2(0.9f, 0.2f);
    [SerializeField] private Transform pinchosCheck;
    [SerializeField] private bool esHorizontal = false;
    [SerializeField] private float tiempoDestruccion = 2f;
    [SerializeField] private LayerMask objetivoLayer;

    [Header("Llama")]
    [SerializeField] private float tiempoDestruccionFuego = 2f;
    [SerializeField] private GameObject spriteFuego;
    [SerializeField] private Vector2 fuegoCheckSize = new Vector2(0.9f, 0.2f);
    [SerializeField] private Transform fuegoCheck;
    [SerializeField] private LayerMask objetivoLayerFuego;
    private bool esFuego = false;
    private bool fuegoActivos = false;
    private bool fragilFuego = false;
    private float timerFragilFuego = 0f;
    [SerializeField] private float tiempoFragilFuego = 2f;
    private bool contandoFragilFuego = false;
    public Vector3   posicionOriginal;
    [SerializeField] private GameObject prefabLlamaAnimal;
    [SerializeField] private float alturaSueloLlama = 1.5f;
    [SerializeField] private GameObject prefabTelefono;
    [SerializeField] private float alturaSueloTelefono = 1.5f;
    private AudioSource audioFuego;
    private AudioSource audioMadera;

    [Header("Banco")]
    [SerializeField] private GameObject prefabPeces;
    [SerializeField] private float alturaSueloPeces = 1f;
    [SerializeField] private GameObject prefabBancoEdificio;
    [SerializeField] private float alturaSueloBancoEdificio = 0.25f;
    [SerializeField] private GameObject prefabBancoSilla;
    [SerializeField] private float alturaSueloBancoSilla = 0.5f;


    private Collider2D col;
    private SpriteRenderer sr;
    private bool esPiedra = false;
    private bool pinchosActivos = false;
    private bool fragil = false;
    private float timerPinchos = 0f;
    private float timerFragil = 0f;
    [SerializeField] private float tiempoFragil = 2f;
    private bool contandoFragil = false;

    void Start()
    {
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        posicionOriginal = transform.position;
        if (esTierra) sr.sprite = spriteTierra;
        if (esRoca) sr.sprite = spriteRoca;
        if (esMadera) sr.sprite = spriteMadera;
        audioFuego = gameObject.AddComponent<AudioSource>();
        audioFuego.clip = AudioManager.Instance.Fuego;
        audioFuego.loop = true;
        audioFuego.volume = 0.3f;
        audioMadera = gameObject.AddComponent<AudioSource>();
        audioMadera.clip = AudioManager.Instance.Madera;
        audioMadera.loop = false;
        audioMadera.volume = 0.3f;
    }

    void Update()
    {
        if (audioFuego != null)
            audioFuego.volume = AudioManager.Instance.sonidoSource.volume;
        // PINCHOS
        if (esPiedra)
        {
            Collider2D objetivo = Physics2D.OverlapBox(pinchosCheck.position, pinchosCheckSize, 0f, objetivoLayer);

            if (pinchosActivos)
            {
                if (objetivo != null)
                {
                    PlayerController pc = objetivo.GetComponent<PlayerController>();
                    if (pc != null && !pc.esInvulnerable == true)
                    {
                        AudioManager.Instance.PlaySound(AudioManager.Instance.dañoSound);
                        pc.StartCoroutine(pc.Invulnerabilidad());
                        GameManager.Instance.vidas -= 2;
                    }
                    SlimeController slime = objetivo.GetComponent<SlimeController>();
                    if (slime != null) slime.Muerte();
                    SierraController sierra = objetivo.GetComponent<SierraController>();
                    if (sierra != null) sierra.Muerte();
                    LlamaAnimal llama = objetivo.GetComponent<LlamaAnimal>();
                    if (llama != null) llama.Muerte();
                    Telefono te = objetivo.GetComponent<Telefono>();
                    if (te != null) te.Muerte();
                    BancoEdificio be = objetivo.GetComponent<BancoEdificio>();
                    if (be != null) be.Muerte();
                    BancoSilla bs = objetivo.GetComponent<BancoSilla>();
                    if (bs != null) bs.Muerte();
                }

                timerPinchos += Time.deltaTime;
                if (timerPinchos >= tiempoDestruccion)
                {
                    pinchosActivos = false;
                    if (spritePinchos != null)
                        spritePinchos.SetActive(false);
                    if (esHorizontal)
                        fragil = true;
                }
            }

            if (fragil == true)
            {
                if (objetivo != null && !contandoFragil)
                    contandoFragil = true;
                if (contandoFragil)
                {
                    timerFragil += Time.deltaTime;
                    if (timerFragil >= tiempoFragil)
                        Destroy(gameObject);
                }
            }
        }

        // FUEGO
        if (esFuego)
        {
            Collider2D objetivoFuego = Physics2D.OverlapBox(fuegoCheck.position, fuegoCheckSize, 0f, objetivoLayerFuego);
            if (Time.timeScale == 1 && !audioFuego.isPlaying) audioFuego.Play();
            if (Time.timeScale == 0 && audioFuego.isPlaying) audioFuego.Pause();
            if (fuegoActivos)
            {
                if (objetivoFuego != null)
                {
                    PlayerController pc = objetivoFuego.GetComponent<PlayerController>();
                    if (pc != null && !pc.esInvulnerable)
                    {
                        AudioManager.Instance.PlaySound(AudioManager.Instance.dañoSound);
                        pc.StartCoroutine(pc.Invulnerabilidad());
                        GameManager.Instance.vidas -= 2;
                    }
                    SlimeController slime = objetivoFuego.GetComponent<SlimeController>();
                    if (slime != null) slime.Muerte();
                    SierraController sierra = objetivoFuego.GetComponent<SierraController>();
                    if (sierra != null) sierra.Muerte();
                    BancoEdificio be = objetivoFuego.GetComponent<BancoEdificio>();
                    if (be != null) be.Muerte();
                    BancoSilla bs = objetivoFuego.GetComponent<BancoSilla>();
                    if (bs != null) bs.Muerte();
                }
            }

            if (fragilFuego)
            {
                if (esMadera || objetivoFuego != null)
                    contandoFragilFuego = true;

                if (contandoFragilFuego)
                {
                    timerFragilFuego += Time.deltaTime; 

                    if (timerFragilFuego >= tiempoFragilFuego)
                    {
                        AudioSource.PlayClipAtPoint(AudioManager.Instance.Madera, transform.position,
                            AudioManager.Instance.sonidoSource.volume);
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
   
    void OnDrawGizmos()
    {
        if (pinchosCheck != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(pinchosCheck.position, pinchosCheckSize);
        }
    }
    public bool EsMismoTipo(SueloController otro)
    {
        if (esTierra && otro.esTierra) return true;
        if (esRoca && otro.esRoca) return true;
        if (esMadera && otro.esMadera) return true;
        return false;
    }
    public void CambiarANormal()
    {
        col.sharedMaterial = materialNormal;
        if (esTierra) sr.sprite = spriteTierra;
        if (esRoca) sr.sprite = spriteRoca;
        if (esMadera) sr.sprite = spriteMadera;
        esPiedra = false;

        if (spritePinchos != null)
            spritePinchos.SetActive(false);
    }

    public void CambiarAHielo()
    {
        col.sharedMaterial = materialHielo;
        sr.color = colorHielo;
        esHielo = true;
    }

    public void CambiarAPiedra()
    {
        col.sharedMaterial = materialPiedra;
        sr.color = colorPiedra;
        esPiedra = true;
        pinchosActivos = true;
        timerPinchos = 0f;

        if (spritePinchos != null)
            spritePinchos.SetActive(true);
    }
    public void Llama()
    {
        int prob = UnityEngine.Random.Range(1, 4);
        switch (prob)
        {
            case 1:
                col.sharedMaterial = materialNormal;
                sr.color = colorFuego;
                esFuego = true;
                fuegoActivos = true;
                if (esMadera) fragilFuego = true;
                if (spriteFuego != null)
                    spriteFuego.SetActive(true);
                if (Time.timeScale == 1) audioFuego.Play();
                break;
               
            case 2:

                SueloController[] todosLosBloques = FindObjectsByType<SueloController>(FindObjectsSortMode.None);
                SueloController bloqueHorizontalMasCercano = null;
                float distanciaMinima = float.MaxValue;

                foreach (SueloController bloque in todosLosBloques)
                {
                    if (!bloque.esHorizontal) continue; // solo bloques horizontales

                    float distancia = Vector3.Distance(transform.position, bloque.transform.position);
                    if (distancia < distanciaMinima)
                    {
                        distanciaMinima = distancia;
                        bloqueHorizontalMasCercano = bloque;
                    }
                }

                if (bloqueHorizontalMasCercano != null)
                {
                    Vector3 spawnPos = bloqueHorizontalMasCercano.transform.position + Vector3.up * alturaSueloLlama;
                    Instantiate(prefabLlamaAnimal, spawnPos, Quaternion.identity);
                }
                break;
            case 3:
                SueloController[] todosLosBloques2 = FindObjectsByType<SueloController>(FindObjectsSortMode.None);
                SueloController bloqueHorizontalMasCercano2 = null;
                float distanciaMinima2 = float.MaxValue;

                foreach (SueloController bloque in todosLosBloques2)
                {
                    if (!bloque.esHorizontal) continue; // solo bloques horizontales

                    float distancia = Vector3.Distance(transform.position, bloque.transform.position);
                    if (distancia < distanciaMinima2)
                    {
                        distanciaMinima2 = distancia;
                        bloqueHorizontalMasCercano2 = bloque;
                    }
                }

                if (bloqueHorizontalMasCercano2 != null)
                {
                    Vector3 spawnPos = bloqueHorizontalMasCercano2.transform.position + Vector3.up * alturaSueloTelefono;
                    Instantiate(prefabTelefono, spawnPos, Quaternion.identity);
                }
                break;
        }
    }
    public void Banco()
    {
        int prob = UnityEngine.Random.Range(1, 4);
        switch (prob)
        {
            case 1:
                SueloController[] todosLosBloques = FindObjectsByType<SueloController>(FindObjectsSortMode.None);
                SueloController bloqueHorizontalMasCercano = null;
                float distanciaMinima = float.MaxValue;

                foreach (SueloController bloque in todosLosBloques)
                {
                    if (!bloque.esHorizontal) continue; // solo bloques horizontales

                    float distancia = Vector3.Distance(transform.position, bloque.transform.position);
                    if (distancia < distanciaMinima)
                    {
                        distanciaMinima = distancia;
                        bloqueHorizontalMasCercano = bloque;
                    }
                }

                if (bloqueHorizontalMasCercano != null)
                {
                    Vector3 spawnPos = bloqueHorizontalMasCercano.transform.position + Vector3.up * alturaSueloPeces;
                    Instantiate(prefabPeces, spawnPos, Quaternion.identity);
                }
                break;
            case 2:
                SueloController[] todosLosBloques2 = FindObjectsByType<SueloController>(FindObjectsSortMode.None);
                SueloController bloqueHorizontalMasCercano2 = null;
                float distanciaMinima2 = float.MaxValue;

                foreach (SueloController bloque in todosLosBloques2)
                {
                    if (!bloque.esHorizontal) continue; // solo bloques horizontales

                    float distancia = Vector3.Distance(transform.position, bloque.transform.position);
                    if (distancia < distanciaMinima2)
                    {
                        distanciaMinima2 = distancia;
                        bloqueHorizontalMasCercano2 = bloque;
                    }
                }

                if (bloqueHorizontalMasCercano2 != null)
                {
                    Vector3 spawnPos = bloqueHorizontalMasCercano2.transform.position + Vector3.up * alturaSueloBancoEdificio;
                    Instantiate(prefabBancoEdificio, spawnPos, Quaternion.identity);
                }
             
                break;
            case 3:
                SueloController[] todosLosBloques3 = FindObjectsByType<SueloController>(FindObjectsSortMode.None);
                SueloController bloqueHorizontalMasCercano3 = null;
                float distanciaMinima3 = float.MaxValue;
                foreach (SueloController bloque in todosLosBloques3)
                {
                    if (!bloque.esHorizontal) continue;
                    float distancia = Vector3.Distance(transform.position, bloque.transform.position);
                    if (distancia < distanciaMinima3) 
                    {
                        distanciaMinima3 = distancia; 
                        bloqueHorizontalMasCercano3 = bloque;
                    }
                }
                if (bloqueHorizontalMasCercano3 != null)
                {
                    Vector3 spawnPos = bloqueHorizontalMasCercano3.transform.position + Vector3.up * alturaSueloBancoSilla;
                    Instantiate(prefabBancoSilla, spawnPos, Quaternion.identity);
                }
                break;
        }
    }
    public void Subrayar()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = new Color(1f, 1f, 0f, 0.7f);
    }
    public void NoSubrayar()
    {
        col.sharedMaterial = materialNormal;
        if (esPiedra == true) sr.color = colorPiedra;
        if (esHielo == true) sr.color = colorHielo;
        if(esFuego == true) sr.color = colorFuego;
        else sr.color = Color.white;
        if (esTierra == true) sr.sprite = spriteTierra;
        if (esRoca == true) sr.sprite = spriteRoca;
        if (esMadera == true) sr.sprite = spriteMadera;
    }

}