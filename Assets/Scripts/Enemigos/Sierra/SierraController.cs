using UnityEngine;

public class SierraController : MonoBehaviour
{
    [SerializeField] private float hitCheckRadius = 0.2f;
    [SerializeField] private Transform hitCheck;
    [SerializeField] private LayerMask playerLayer;
    private SpriteRenderer sr;

    [Header("Hielo")]
    [SerializeField] private Color colorHielo = new Color(0.5f, 0.8f, 1f);
    [SerializeField] private Color colorNormal = Color.gray;
    [SerializeField] private PhysicsMaterial2D materialNormal;
    [SerializeField] private PhysicsMaterial2D materialHielo;

    [Header("Patrulla")]
    private bool patrullando = false;
    [SerializeField] private float velocidadPatrulla = 2f;
    [SerializeField] private float distanciaBloque = 2f; // mitad del ancho del bloque
    private Vector3 puntoOrigen; // centro del bloque donde nació
    private float direccion = 1f;

    private Collider2D col;
    private bool helado = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        puntoOrigen = transform.position;
        CambiarANormal();
    }

    void Update()
    {
        if (helado) return;

        if (patrullando)
            VelocidadBuff();

        Collider2D jugador = Physics2D.OverlapCircle(hitCheck.position, hitCheckRadius, playerLayer);
        if (jugador != null)
        {
            jugador.GetComponent<PlayerController>().Muerte();
            GameManager manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            manager.vidas -= 2;
            manager.SpawnJugador();
        }
    }

    void OnDrawGizmos()
    {
        if (hitCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hitCheck.position, hitCheckRadius);
        }
        Gizmos.DrawWireSphere(new Vector3(transform.position.x + distanciaBloque, transform.position.y, 0), 0.1f);
        Gizmos.DrawWireSphere(new Vector3(transform.position.x - distanciaBloque, transform.position.y, 0), 0.1f);
    }

    public void CambiarAHielo()
    {
        helado = true;
        col.sharedMaterial = materialHielo;
        sr.color = colorHielo;
    }

    public void CambiarANormal()
    {
        helado = false;
        col.sharedMaterial = materialNormal;
        sr.color = colorNormal;
    }
    public void IniciarPatrulla()
    {
        patrullando = true;
    }
    public void VelocidadBuff()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        if (transform.position.x >= puntoOrigen.x + distanciaBloque)
            direccion = -1f;
        else if (transform.position.x <= puntoOrigen.x - distanciaBloque)
            direccion = 1f;

        rb.linearVelocity = new Vector2(direccion * velocidadPatrulla, rb.linearVelocity.y);
    }
    public void InmovilidadBuff()
    {

    }
    public void Muerte()
    {
        Destroy(gameObject);
    }
}