using UnityEngine;

public class SierraController : MonoBehaviour
{
    [SerializeField] private float hitCheckRadius = 0.2f;
    [SerializeField] private int Daño = 2;
    [SerializeField] private Transform hitCheck;
    [SerializeField] private LayerMask playerLayer;
    private SpriteRenderer sr;


    [Header("Hielo")]
    [SerializeField] private Sprite sierraHielo;
    [SerializeField] private Sprite Sierra;
    [SerializeField] private PhysicsMaterial2D materialNormal;
    [SerializeField] private PhysicsMaterial2D materialHielo;

    [Header("Patrulla")]
    public bool patrullando = false;
    [SerializeField] private float velocidadPatrulla = 2f;
    [SerializeField] private float distanciaBloque = 2f; // mitad del ancho del bloque
    private Vector3 puntoOrigen; // centro del bloque donde nació
    private float direccion = 1f;

    [Header("Inmovil")]
    [SerializeField] private Sprite sierraQuieta;
    [SerializeField] private float empuje = 10f;
    [SerializeField] private float radioEmpuje = 1.5f;
    private bool inmovilizado = false;

    private Collider2D col;
    private bool helado = false;
    private Rigidbody2D rb;

    private Animator anim;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
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
                AudioManager.Instance.PlaySound(AudioManager.Instance.dañoSound);
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
    void OnCollisionEnter2D(Collision2D other)
    {
        SlimeController slc = other.gameObject.GetComponent<SlimeController>();
        if (slc != null)
        {
            slc.Muerte();
            return;
        }
        LlamaAnimal La = other.gameObject.GetComponent<LlamaAnimal>();
        if (La != null)
        {
            La.Muerte();
            return;
        }
        BancoEdificio be = other.gameObject.GetComponent<BancoEdificio>();
        if (be != null)
        {
            Muerte();
            return;
        }
        Telefono te = other.gameObject.GetComponent<Telefono>();
        if (te != null)
        {
            te.Muerte();
            return;
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
        anim.enabled = false;
        col.sharedMaterial = materialHielo;
        sr.sprite = sierraHielo;
        gameObject.layer = LayerMask.NameToLayer("Suelo");

    }

    public void CambiarANormal()
    {
        helado = false;
        anim.enabled = true;
        col.sharedMaterial = materialNormal;
        sr.sprite = Sierra;
        gameObject.layer = LayerMask.NameToLayer("Enemigo");
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
        gameObject.layer = LayerMask.NameToLayer("Enemigo");
        inmovilizado = true;
        anim.enabled = false;
        sr.sprite = sierraQuieta;
        Daño = 1;
    }
    public void Subrayar()
    {
       SpriteRenderer sr = GetComponent<SpriteRenderer>();
       if (sr != null) sr.color = new Color(1f, 0.84f, 0f, 0.49f);
    }
    public void NoSubrayar()
    {
        
        sr.color = Color.white;
        if (helado)
        {
            sr.sprite = sierraHielo; 
            col.sharedMaterial = materialHielo;
        }  
        else if (inmovilizado)
        {
            sr.sprite = sierraQuieta; 
            col.sharedMaterial = materialNormal;
        }
        else sr.sprite = Sierra; col.sharedMaterial = materialNormal;
    }
    public void Muerte()
    {
        AudioManager.Instance.PlaySound(AudioManager.Instance.muerteEnemigo);
        Destroy(gameObject);
    }
}