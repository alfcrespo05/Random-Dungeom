using UnityEngine;

public class SueloController : MonoBehaviour
{
    [Header("Materiales y colores")]
    [SerializeField] private PhysicsMaterial2D materialNormal;
    [SerializeField] private PhysicsMaterial2D materialHielo;
    [SerializeField] private PhysicsMaterial2D materialPiedra;
    [SerializeField] private Color colorNormalTierra = Color.brown;
    [SerializeField] private Color colorNormalRoca = Color.gray2;
    [SerializeField] private Color colorNormalMadera = Color.saddleBrown;
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

    [Header("Banco")]
    [SerializeField] private GameObject prefabPeces;
    [SerializeField] private float alturaSueloPeces = 1f;
    [SerializeField] private GameObject prefabBancoEdificio;
    [SerializeField] private float alturaSueloBancoEdificio = 0.25f;
    //[SerializeField] private GameObject prefabBancoSilla;
    //[SerializeField] private float alturaSueloBancoSilla = 0.5f;


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
        if(esTierra == true) sr.color = colorNormalTierra;
        if (esRoca == true) sr.color = colorNormalRoca;
        if (esMadera == true) sr.color = colorNormalMadera;
    }

    void Update()
    {
        // PINCHOS
        if (esPiedra)
        {
            Collider2D objetivo = Physics2D.OverlapBox(pinchosCheck.position, pinchosCheckSize, 0f, objetivoLayer);

            if (pinchosActivos)
            {
                if (objetivo != null)
                {
                    PlayerController pc = objetivo.GetComponent<PlayerController>();
                    if (pc != null && !pc.esInvulnerable)
                    {
                        pc.StartCoroutine(pc.Invulnerabilidad());
                        GameManager.Instance.vidas -= 2;
                    }
                    SlimeController slime = objetivo.GetComponent<SlimeController>();
                    if (slime != null) slime.Muerte();
                    SierraController sierra = objetivo.GetComponent<SierraController>();
                    if (sierra != null) sierra.Muerte();
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

            if (fragil)
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

            if (fuegoActivos)
            {
                if (objetivoFuego != null)
                {
                    PlayerController pc = objetivoFuego.GetComponent<PlayerController>();
                    if (pc != null && !pc.esInvulnerable)
                    {
                        pc.StartCoroutine(pc.Invulnerabilidad());
                        GameManager.Instance.vidas -= 2;
                    }
                    SlimeController slime = objetivoFuego.GetComponent<SlimeController>();
                    if (slime != null) slime.Muerte();
                    SierraController sierra = objetivoFuego.GetComponent<SierraController>();
                    if (sierra != null) sierra.Muerte();
                }
            }

            if (fragilFuego)
            {
                if (objetivoFuego != null && !contandoFragilFuego)
                    contandoFragilFuego = true;
                if (contandoFragilFuego)
                {
                    timerFragilFuego += Time.deltaTime;
                    if (timerFragilFuego >= tiempoFragilFuego)
                        Destroy(gameObject);
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
        if (esTierra == true) sr.color = colorNormalTierra;
        if (esRoca == true) sr.color = colorNormalRoca;
        if (esMadera == true) sr.color = colorNormalMadera;
        esPiedra = false;

        if (spritePinchos != null)
            spritePinchos.SetActive(false);
    }

    public void CambiarAHielo()
    {
        col.sharedMaterial = materialHielo;
        sr.color = colorHielo;
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
                if (esMadera) fragilFuego = true; // madera empieza a contar inmediatamente
                if (spriteFuego != null)
                    spriteFuego.SetActive(true);
                break;
            case 2:
                posicionOriginal += Vector3.up * alturaSueloLlama;
                GameObject LlamaAnimal = Instantiate(prefabLlamaAnimal, posicionOriginal, Quaternion.identity);
                break;
            case 3:
                posicionOriginal += Vector3.up * alturaSueloTelefono;
                GameObject Telefono = Instantiate(prefabTelefono, posicionOriginal, Quaternion.identity);
                break;
        }
    }
    public void Banco()
    {
        int prob = UnityEngine.Random.Range(1, 2);
        switch (prob)
        {
            case 1:
                posicionOriginal += Vector3.up * alturaSueloPeces;
                GameObject Peces = Instantiate(prefabPeces, posicionOriginal, Quaternion.identity);
                break;
            case 2:
                posicionOriginal += Vector3.up * alturaSueloBancoEdificio;
                GameObject BancoEdificio = Instantiate(prefabBancoEdificio, posicionOriginal, Quaternion.identity);
                break;
            case 3:
                //posicionOriginal += Vector3.up * alturaSueloBancoSilla;
                //GameObject BancoSilla = Instantiate(prefabBancoSilla, posicionOriginal, Quaternion.identity);
                break;
        }
    }
    public void Subrayar()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = new Color(1f, 0.84f, 0f, 0.49f);
    }
    public void NoSubrayar()
    {
        col.sharedMaterial = materialNormal;
        if (esTierra == true) sr.color = colorNormalTierra;
        if (esRoca == true) sr.color = colorNormalRoca;
        if (esMadera == true) sr.color = colorNormalMadera;
    }

}