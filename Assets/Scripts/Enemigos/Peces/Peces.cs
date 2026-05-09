using UnityEngine;

public class Peces : MonoBehaviour
{
    [SerializeField] private float velocidad = 5f;
    private Rigidbody2D rb;
    private Vector2 posicionObjetivo;
    private Vector2 direccionFija;
    private bool llegando = false;
    private bool iniciado = false;
    private float timerEspera = 0f;
    [SerializeField] private float tiempoEspera = 1f;
    //[SerializeField] private float timerVida = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        FijarObjetivo();
    }

    void FijarObjetivo()
    {
        PlayerController jugador = FindFirstObjectByType<PlayerController>();
        if (jugador != null)
        {
            posicionObjetivo = jugador.transform.position;
            direccionFija = (posicionObjetivo - (Vector2)transform.position).normalized;
            llegando = false;
        }
    }

    void Update()
    {
        if (!iniciado)
        {
            FijarObjetivo();
            iniciado = true;
            return;
        }

        //timerVida -= Time.deltaTime;
        //if (timerVida <= 0f) { Destroy(gameObject); return; }

        if (!llegando)
        {
            rb.linearVelocity = direccionFija * velocidad;

            if (Vector2.Distance(transform.position, posicionObjetivo) < 0.5f)
            {
                llegando = true;
                timerEspera = tiempoEspera;
            }
        }
        else
        {
            rb.linearVelocity = direccionFija * velocidad;

            timerEspera -= Time.deltaTime;
            if (timerEspera <= 0f)
                FijarObjetivo();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Peces>() != null) return;

        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc != null)
        {
            if (!pc.esInvulnerable)
            {
                GameManager.Instance.vidas -= 1;
                pc.StartCoroutine(pc.Invulnerabilidad());
            }
            return;
        }
    }

    public void Muerte()
    {
        Destroy(gameObject);
    }
}