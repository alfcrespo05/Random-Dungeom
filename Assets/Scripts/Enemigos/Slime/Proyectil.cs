using UnityEngine;

public class Proyectil : MonoBehaviour
{
    [SerializeField] private float velocidad = 8f;
    [SerializeField] private float empuje = 5f;
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
        // Ignora al propio slime que lo disparó
        if (other.GetComponent<SlimeController>() != null) return;

        // Golpea al jugador
        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.Muerte();
            GameManager manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            manager.vidas -= 1;
            manager.SpawnJugador();
            Destroy(gameObject);
            return;
        }

        // Golpea bloque o enemigo, empuja al jugador si está cerca
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