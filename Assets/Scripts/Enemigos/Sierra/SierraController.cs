using UnityEngine;

public class SierraController : MonoBehaviour
{
    [SerializeField] private float hitCheckRadius = 0.2f;
    [SerializeField] private int Daño = 2;
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

    [Header("Inmovil")]
    [SerializeField] private float empuje = 10f;
    [SerializeField] private float radioEmpuje = 1.5f;
    private bool inmovilizado = false;

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
            PlayerController pc = jugador.GetComponent<PlayerController>();
            if (pc != null && !pc.esInvulnerable)
            {
                GameManager.Instance.vidas -= Daño;
                pc.StartCoroutine(pc.Invulnerabilidad());

                // Empuje solo si está inmovilizada
                if (inmovilizado)
                {
                    Rigidbody2D rbJugador = jugador.GetComponent<Rigidbody2D>();
                    if (rbJugador != null)
                    {
                        Vector2 dir = (jugador.transform.position - transform.position).normalized;
                        rbJugador.AddForce(dir * empuje, ForceMode2D.Impulse);
                    }
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        if (hitCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hitCheck.position, hitCheckRadius);
        }

        // Límites de patrulla
        Gizmos.DrawWireSphere(new Vector3(transform.position.x + distanciaBloque, transform.position.y, 0), 0.1f);
        Gizmos.DrawWireSphere(new Vector3(transform.position.x - distanciaBloque, transform.position.y, 0), 0.1f);

        // Radio de empuje
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioEmpuje);
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
        Daño = 1;
        inmovilizado = true;
    }
    public void Subrayar()
    {
       SpriteRenderer sr = GetComponent<SpriteRenderer>();
       if (sr != null) sr.color = new Color(1f, 0.84f, 0f, 0.49f);
    }
    public void NoSubrayar()
    {
        col.sharedMaterial = materialNormal;
        sr.color = colorNormal;
    }
    public void Muerte()
    {
        Destroy(gameObject);
    }
}