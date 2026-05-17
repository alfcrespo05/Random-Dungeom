using UnityEngine;

public class SlimeController : MonoBehaviour
{
    [Header("Patrulla")]
    [SerializeField] private float velocidadPatrulla = 2f;
    [SerializeField] private float distanciaBloque = 2f; // mitad del ancho del bloque

    [Header("Límites de Persecución")]
    [SerializeField] private Transform bordeIzquierdo;
    [SerializeField] private Transform bordeDerecho;
    [SerializeField] private LayerMask sueloLayer;
    private float timerInicio = 0.1f;
    private bool listo = false;

    [Header("Persecución")]
    [SerializeField] private float velocidadPersecucion = 4f;
    [SerializeField] private Vector2 rangoDeteccion = new Vector2(5f, 2f);

    [Header("Hit Check")]
    [SerializeField] private Vector2 hitCheckSize = new Vector2(1f, 0.2f);
    [SerializeField] private Transform hitCheck;
    [SerializeField] private LayerMask playerLayer;

    [Header("Hit Check Lateral")]
    [SerializeField] private Vector2 hitCheckLateralSize = new Vector2(1f, 0.8f);

    [Header("Aire")]
    [SerializeField] private Sprite Slime;
    [SerializeField] private Sprite slimeAire;
    [SerializeField] private Color colorNormal = new Color(0.83f, 0.99f, 0.27f);
    [SerializeField] private PhysicsMaterial2D materialNormal;
    [SerializeField] private PhysicsMaterial2D materialAire;

    [Header("Modo Torreta")]
    [SerializeField] private GameObject prefabProyectil;
    [SerializeField] private float intervaloDisparo = 2f;
    [SerializeField] private float alturaElevacion = 1.5f;
    private float timerDisparo = 0f;

    [Header("Charco")]
    [SerializeField] private Sprite slimeCharco;
    //[SerializeField] private Vector2 tamañoCharco = new Vector2(2f, 0.3f);
    private BoxCollider2D boxCollider;
    [SerializeField] private Vector2 ColliderCharco = new Vector2(2f, 0.3f);
    [SerializeField] private Vector2 ColliderCharcoPosicion = new Vector2(2f, 0.3f);
    [SerializeField] private Vector2 hitCheckLateralCharco = new Vector2(2f, 0.3f);
    [SerializeField] private float alturaCharco = -0.5f;
    [SerializeField] private PhysicsMaterial2D materialCharco;
    private bool inmovilizado = false;

    private Collider2D col;
    private bool aire = false;
    private SpriteRenderer sr;

    private Rigidbody2D rb;
    private Vector3 puntoOrigen; // centro del bloque donde nació
    private float direccion = 1f;
    private bool persiguiendo = false;
    private Transform jugador;

    private Animator anim;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>(); 
        sr = GetComponent<SpriteRenderer>(); 
        puntoOrigen = transform.position;
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
      
    }

    void Update()
    {
        if (!listo)
        {
            timerInicio -= Time.deltaTime;
            if (timerInicio <= 0f) listo = true;
            return;
        }
        if (inmovilizado)
        {
            ComprobarHitLateral(); // el charco sigue haciendo daño
            return;
        } 
        if (aire)
        {
            ModoTorreta();
            return;
        }

        BuscarJugador();

        if (persiguiendo && jugador != null)
            Perseguir();
        else
            Patrullar();

        ComprobarHit();
        ComprobarHitLateral();
    }

    void BuscarJugador()
    {
        Collider2D detectado = Physics2D.OverlapBox(transform.position, rangoDeteccion, 0f, playerLayer);

        if (detectado != null)
        {
            jugador = detectado.transform;
            persiguiendo = true;
        }
        else
        {
            jugador = null;
            persiguiendo = false;
        }
    }

    void Patrullar()
    {
      
        // Cambia de dirección al llegar al borde del bloque
        if (transform.position.x >= puntoOrigen.x + distanciaBloque)
            direccion = -1f;
        else if (transform.position.x <= puntoOrigen.x - distanciaBloque)
            direccion = 1;
        transform.localScale = new Vector3(-direccion * 0.5f, 0.5f, 1);
        anim.SetBool("Caminar", true);
        rb.linearVelocity = new Vector2(direccion * velocidadPatrulla, rb.linearVelocity.y);
        //Debug.Log("Patrullando Slime");
    }

    void Perseguir()
    {
        bool sueloIzquierda = Physics2D.Raycast(bordeIzquierdo.position, Vector2.down, 0.3f, sueloLayer);
        bool sueloDerecha = Physics2D.Raycast(bordeDerecho.position, Vector2.down, 0.3f, sueloLayer);

        float dirX = jugador.position.x > transform.position.x ? 1f : -1f;

        // Si no hay suelo en esa dirección, no avanza
        if (dirX == 1f && !sueloDerecha) dirX = 0f;
        if (dirX == -1f && !sueloIzquierda) dirX = 0f;

        rb.linearVelocity = new Vector2(dirX * velocidadPersecucion, rb.linearVelocity.y);

        anim.SetBool("Caminar", dirX != 0);
        if (dirX != 0)
            transform.localScale = new Vector3(-dirX * 0.5f, 0.5f, 1);
    }

    void ComprobarHit()
    {
        if (hitCheck == null) return;
        Collider2D jugadorArriba = Physics2D.OverlapBox(hitCheck.position, hitCheckSize, 0f, playerLayer);
        if (jugadorArriba != null)
        {
            // El jugador pisó al slime muere el slime
            Muerte();
        }
    }
    void ComprobarHitLateral()
    {
        Vector2 tamañoActual = inmovilizado ? hitCheckLateralCharco : hitCheckLateralSize;
        Collider2D jugadorLateral = Physics2D.OverlapBox(transform.position, tamañoActual, 0f, playerLayer);
        if (jugadorLateral != null)
        {
            if (inmovilizado || jugadorLateral.transform.position.y < transform.position.y + 0.3f)
            {
                PlayerController pc = jugadorLateral.GetComponent<PlayerController>();
                if (pc != null && !pc.esInvulnerable) // comprueba primero
                {
                    AudioManager.Instance.PlaySound(AudioManager.Instance.dañoSound);
                    GameManager.Instance.vidas -= 1; // luego resta vidas
                    pc.StartCoroutine(pc.Invulnerabilidad());
                }
            }
        }
    }


    void OnDrawGizmos()
    {
        // Rango de detección
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, rangoDeteccion);

        // Hit check arriba (mata al slime)
        if (hitCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(hitCheck.position, hitCheckSize);
        }

        // Hit check lateral (mata al jugador)
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, hitCheckLateralSize);

        // Tamaño del charco
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, hitCheckLateralCharco);

        // Bordes para no caerse
        if (bordeIzquierdo != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(bordeIzquierdo.position, bordeIzquierdo.position + Vector3.down * 0.3f);
        }
        if (bordeDerecho != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(bordeDerecho.position, bordeDerecho.position + Vector3.down * 0.3f);
        }

        // Límites de patrulla
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x + distanciaBloque, transform.position.y, 0), 0.1f);
        Gizmos.DrawWireSphere(new Vector3(transform.position.x - distanciaBloque, transform.position.y, 0), 0.1f);
    }
    public void CambiarAAire()
    {
        anim.SetBool("Caminar", false);
        aire = true;
        col.sharedMaterial = materialAire;
        sr.sprite = slimeAire;
        anim.SetBool("Aire", true);     
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        transform.position += Vector3.up * alturaElevacion;
    }
    void ModoTorreta()
    {
        // Sigue comprobando si el jugador lo pisa
        ComprobarHit();

        timerDisparo += Time.deltaTime;
        if (timerDisparo >= intervaloDisparo)
        {
            timerDisparo = 0f;
            Disparar();
        }
    }

    void Disparar()
    {
        PlayerController jugador = FindFirstObjectByType<PlayerController>();
        if (jugador == null || prefabProyectil == null) return;

        Vector2 dir = (jugador.transform.position - transform.position).normalized;
        AudioManager.Instance.PlaySound(AudioManager.Instance.enemigoDisparo);
        GameObject proyectil = Instantiate(prefabProyectil, transform.position, Quaternion.identity);
        proyectil.GetComponent<Proyectil>().SetDireccion(dir);
    }
    public void CambiarANormal()
    {
        anim.SetBool("Aire", false);
        aire = false;
        inmovilizado = false;
        col.sharedMaterial = materialNormal;
        sr.sprite = Slime;
        sr.color = Color.white;
    }
    public void InmovilidadBuff()
    {
        if (aire == true) { Destroy(gameObject); return; }
        anim.enabled = false;
        boxCollider.size = ColliderCharco;
        // Para el movimiento
        inmovilizado = true;
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        transform.position += Vector3.up * alturaCharco;
        col.sharedMaterial = materialCharco;
        sr.sprite = slimeCharco;
        // Desactiva el hitCheck de arriba (el que lo mata)
        if (hitCheck != null)
            hitCheck.gameObject.SetActive(false);
        boxCollider.size = ColliderCharco;
        boxCollider.offset = ColliderCharcoPosicion;
        gameObject.layer = LayerMask.NameToLayer("Suelo");

    }
    public void Subrayar()
    {
        if (sr != null) sr.color = new Color(1f, 0.84f, 0f, 0.49f);
    }
    public void NoSubrayar()
    {
        if(aire == true) sr.sprite = slimeAire;
        if (inmovilizado == true) sr.sprite = slimeCharco;
        else sr.sprite = Slime;
        sr.color = Color.white;
    }
    public void Muerte()
    {
        AudioManager.Instance.PlaySound(AudioManager.Instance.muerteEnemigo);
        Destroy(gameObject);
    }
}