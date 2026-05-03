using UnityEngine;

public class SueloController : MonoBehaviour
{
    [Header("Materiales y colores")]
    [SerializeField] private PhysicsMaterial2D materialNormal;
    [SerializeField] private PhysicsMaterial2D materialHielo;
    [SerializeField] private PhysicsMaterial2D materialPiedra;
    [SerializeField] private Color colorNormal = Color.white;
    [SerializeField] private Color colorHielo = new Color(0.5f, 0.8f, 1f);
    [SerializeField] private Color colorPiedra = new Color(0.31f, 0.27f, 0.25f);

    [Header("Pinchos")]
    [SerializeField] private GameObject spritePinchos; // hijo desactivado
    [SerializeField] private Vector2 pinchosCheckSize = new Vector2(0.9f, 0.2f);
    [SerializeField] private Transform pinchosCheck;
    [SerializeField] private bool esHorizontal = false;
    [SerializeField] private float tiempoDestruccion = 2f;
    [SerializeField] private LayerMask objetivoLayer;

    [Header("Clase")]
    public bool esTierra = false;
    public bool esRoca = false;
    public bool esMadera = false;


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
    }

    void Update()
    {
        if (!esPiedra) return;

        Collider2D objetivo = Physics2D.OverlapBox(pinchosCheck.position, pinchosCheckSize, 0f, objetivoLayer);

        if (pinchosActivos)
        {
            // Hace daño mientras los pinchos están activos
            if (objetivo != null)
            {
                PlayerController pc = objetivo.GetComponent<PlayerController>();
                if (pc != null) 
                {
                    pc.Muerte();
                    GameManager manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
                    manager.vidas -= 2;
                    manager.SpawnJugador();
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

                // Solo queda frágil si es horizontal
                if (esHorizontal)
                    fragil = true;
            }
        }
        if (fragil)
        {
            if (objetivo != null && !contandoFragil)
                contandoFragil = true; // empieza la cuenta al primer contacto

            if (contandoFragil)
            {
                timerFragil += Time.deltaTime;
                if (timerFragil >= tiempoFragil)
                    Destroy(gameObject);
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
        sr.color = colorNormal;
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


}