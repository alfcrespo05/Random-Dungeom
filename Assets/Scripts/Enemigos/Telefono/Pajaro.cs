using UnityEngine;

public enum TipoObjetivo { Jugador, Enemigo }

public class Pajaro : MonoBehaviour
{
    [SerializeField] private float velocidad = 5f;
    private TipoObjetivo tipoObjetivo;
    private Transform objetivo;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        BuscarObjetivo();
    }

    public void SetObjetivo(TipoObjetivo tipo)
    {
        tipoObjetivo = tipo;
        BuscarObjetivo();
    }

    void BuscarObjetivo()
    {
        if (tipoObjetivo == TipoObjetivo.Jugador)
        {
            PlayerController jugador = FindFirstObjectByType<PlayerController>();
            if (jugador != null) objetivo = jugador.transform;
        }
        else
        {
            // Busca un enemigo aleatorio (slime o sierra)
            MonoBehaviour[] enemigos = new MonoBehaviour[0];
            SlimeController[] slimes = FindObjectsByType<SlimeController>(FindObjectsSortMode.None);
            SierraController[] sierras = FindObjectsByType<SierraController>(FindObjectsSortMode.None);

            int total = slimes.Length + sierras.Length;
            if (total == 0) return;

            int indice = UnityEngine.Random.Range(0, total);
            if (indice < slimes.Length)
                objetivo = slimes[indice].transform;
            else
                objetivo = sierras[indice - slimes.Length].transform;
        }
    }

    void Update()
    {
        if (objetivo == null)
        {
            BuscarObjetivo(); // si el objetivo murió busca otro
            return;
        }

        // Mueve hacia el objetivo
        Vector2 dir = (objetivo.position - transform.position).normalized;
        rb.linearVelocity = dir * velocidad;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Telefono>() != null) return;
        if (other.GetComponent<Pajaro>() != null) return;

        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc != null)
        {
            if (!pc.esInvulnerable)
            {
                AudioManager.Instance.PlaySound(AudioManager.Instance.dañoSound);
                GameManager.Instance.vidas -= 2;
                pc.StartCoroutine(pc.Invulnerabilidad());
            }
            pc.Muerte();
            return;
        }

        SlimeController slc = other.GetComponent<SlimeController>();
        if (slc != null)
        {
            slc.Muerte();
            Destroy(gameObject);
            return;
        }

        SierraController sc = other.GetComponent<SierraController>();
        if (sc != null)
        {
            sc.Muerte();
            Destroy(gameObject);
            return;
        }
    }

    public void Muerte()
    {
        AudioManager.Instance.PlaySound(AudioManager.Instance.muerteEnemigo);
        Destroy(gameObject);
    }
}