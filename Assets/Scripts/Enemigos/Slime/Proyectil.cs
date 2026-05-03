using UnityEngine;

public class Proyectil : MonoBehaviour
{
    [SerializeField] private float velocidad = 8f;
    [SerializeField] private float empuje = 10f;
    [SerializeField] private float radioEmpuje = 1.5f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask sueloLayer;
    [SerializeField] private LayerMask enemigoLayer;

    private Vector2 direccion;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direccion * velocidad;
    }

    public void SetDireccion(Vector2 dir)
    {
        direccion = dir.normalized;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<SlimeController>() != null) return;

        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc != null)
        {
            // Empuje siempre, daño solo si no es invulnerable
            Rigidbody2D rbJugador = other.GetComponent<Rigidbody2D>();
            if (rbJugador != null)
                rbJugador.AddForce(direccion * empuje, ForceMode2D.Impulse);

            if (!pc.esInvulnerable)
            {
                GameManager.Instance.vidas -= 1;
                pc.StartCoroutine(pc.Invulnerabilidad());
            }

            Destroy(gameObject);
            return;
        }

        // Golpea bloque o enemigo
        if (((1 << other.gameObject.layer) & (sueloLayer | enemigoLayer)) != 0)
        {
            EmpujarJugadorCercano();
            Destroy(gameObject);
            return;
        }
    }

    void EmpujarJugadorCercano()
    {
        Collider2D jugador = Physics2D.OverlapCircle(transform.position, radioEmpuje, playerLayer);
        if (jugador != null)
        {
            Rigidbody2D rbJugador = jugador.GetComponent<Rigidbody2D>();
            if (rbJugador != null)
                rbJugador.AddForce(direccion * empuje, ForceMode2D.Impulse);
        }
    }

    void Update()
    {
        // Destruye si sale de la pantalla
        if (Mathf.Abs(transform.position.x) > 20f || Mathf.Abs(transform.position.y) > 20f)
            Destroy(gameObject);
    }
}