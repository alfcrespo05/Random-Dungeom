using UnityEngine;

public class Velocidad : MonoBehaviour
{
    private bool arrastrando = false;
    private Camera cam;
    [SerializeField] private LayerMask jugadorLayer;
    [SerializeField] private LayerMask enemigoLayer;
    [SerializeField] private LayerMask papeleraLayer;
    private Rigidbody2D rb;
    public int costeEstamina = 10;
    public Vector2 posicionOriginal;
    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        posicionOriginal = transform.position; // se guarda sola
    }


    void Update()
    {
        if (arrastrando)
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            transform.position = mousePos;
        }
    }
    void OnMouseDown()
    {
        if (!GameManager.Instance.MoverCartas || Time.timeScale == 1) return;
        arrastrando = true;
        rb.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Collider2D>().enabled = false; // desactiva colisiones al arrastrar
    }

    void OnMouseUp()
    {
        if (!arrastrando) return;
        arrastrando = false;
        if (costeEstamina <= GameManager.Instance.estamina)
        {
            Collider2D sierra = Physics2D.OverlapPoint(transform.position, enemigoLayer);
            if (sierra != null)
            {
                SierraController sc = sierra.GetComponent<SierraController>();
                if (sc != null)
                {
                    GameManager.Instance.estamina -= costeEstamina;
                    sc.IniciarPatrulla();
                    sc.VelocidadBuff();
                    // Aplica buff a todos los jugadores
                    PlayerController pc = FindFirstObjectByType<PlayerController>();
                    if (pc != null) pc.VelocidadBuff();
                    FindFirstObjectByType<MazoManager>().CartaUsada();
                    Destroy(gameObject);
                    return;
                }
            }

            Collider2D jugador = Physics2D.OverlapPoint(transform.position, jugadorLayer);
            if (jugador != null)
            {
                PlayerController pc = jugador.GetComponent<PlayerController>();
                if (pc != null)
                {
                    GameManager.Instance.estamina -= costeEstamina;
                    pc.VelocidadBuff();
                    // Aplica buff a todas las sierras
                    SierraController[] sierras = FindObjectsByType<SierraController>(FindObjectsSortMode.None);
                    foreach (SierraController sc in sierras)
                    {
                        sc.IniciarPatrulla();
                        sc.VelocidadBuff();
                    }
                    FindFirstObjectByType<MazoManager>().CartaUsada();
                    Destroy(gameObject);
                    return;
                }
            }
            Collider2D papelera = Physics2D.OverlapPoint(transform.position, papeleraLayer);
            if (papelera != null)
            {
                Papelera papelerac = papelera.GetComponent<Papelera>();
                if (papelera != null)
                {
                    GameManager.Instance.estamina += (costeEstamina / 2);
                    FindFirstObjectByType<MazoManager>().CartaUsada();
                    Destroy(gameObject);
                    return;
                }
            }
        }

        transform.position = posicionOriginal;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Collider2D>().enabled = true;
    }
}
