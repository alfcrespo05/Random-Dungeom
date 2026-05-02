using UnityEngine;

public class SlimeController : MonoBehaviour
{
    [Header("Patrulla")]
    [SerializeField] private float velocidadPatrulla = 2f;
    [SerializeField] private float distanciaBloque = 2f; // mitad del ancho del bloque

    [Header("Persecución")]
    [SerializeField] private float velocidadPersecucion = 4f;
    [SerializeField] private Vector2 rangoDeteccion = new Vector2(5f, 2f);

    [Header("Hit Check")]
    [SerializeField] private Vector2 hitCheckSize = new Vector2(1f, 0.2f);
    [SerializeField] private Transform hitCheck;
    [SerializeField] private LayerMask playerLayer;

    private Rigidbody2D rb;
    private Vector3 puntoOrigen; // centro del bloque donde nació
    private float direccion = 1f;
    private bool persiguiendo = false;
    private Transform jugador;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        puntoOrigen = transform.position; // guarda la posición inicial
    }

    void Update()
    {
        BuscarJugador();

        if (persiguiendo && jugador != null)
            Perseguir();
        else
            Patrullar();

        ComprobarHit();
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
            direccion = 1f;

        rb.linearVelocity = new Vector2(direccion * velocidadPatrulla, rb.linearVelocity.y);
    }

    void Perseguir()
    {
        float dirX = jugador.position.x > transform.position.x ? 1f : -1f;
        rb.linearVelocity = new Vector2(dirX * velocidadPersecucion, rb.linearVelocity.y);
    }

    void ComprobarHit()
    {
        if (hitCheck == null) return;
        Collider2D jugadorArriba = Physics2D.OverlapBox(hitCheck.position, hitCheckSize, 0f, playerLayer);
        if (jugadorArriba != null)
        {
            PlayerController pc = jugadorArriba.GetComponent<PlayerController>();
            if (pc != null) pc.Muerte();
        }
    }

    void OnDrawGizmos()
    {
        // Rango de detección
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, rangoDeteccion);

        // Hit check
        if (hitCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(hitCheck.position, hitCheckSize);
        }

        // Límites de patrulla
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x + distanciaBloque, transform.position.y, 0), 0.1f);
        Gizmos.DrawWireSphere(new Vector3(transform.position.x - distanciaBloque, transform.position.y, 0), 0.1f);
    }
}