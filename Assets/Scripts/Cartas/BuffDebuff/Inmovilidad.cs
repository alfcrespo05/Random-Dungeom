using UnityEngine;

public class Inmovilidad : MonoBehaviour
{
    private bool arrastrando = false;
    private Camera cam;
    [SerializeField] private LayerMask jugadorLayer;
    [SerializeField] private LayerMask enemigoLayer;
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
        arrastrando = true;
        rb.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Collider2D>().enabled = false; // desactiva colisiones al arrastrar
    }

    void OnMouseUp()
    {
        arrastrando = false;
        if (costeEstamina <= GameManager.Instance.estamina)
        {
            // Comprueba enemigos
            Collider2D enemigo = Physics2D.OverlapPoint(transform.position, enemigoLayer);
            if (enemigo != null)
            {
                SierraController sic = enemigo.GetComponent<SierraController>();
                SlimeController slc = enemigo.GetComponent<SlimeController>();

                if (sic != null || slc != null)
                {
                    GameManager.Instance.estamina -= costeEstamina;

                    if (sic != null) sic.InmovilidadBuff();
                    if (slc != null) slc.InmovilidadBuff();

                    // Afecta a todos los slimes de la escena
                    SlimeController[] slimes = FindObjectsByType<SlimeController>(FindObjectsSortMode.None);
                    foreach (SlimeController sl in slimes)
                        sl.InmovilidadBuff();

                    // Afecta a todas las sierras de la escena
                    SierraController[] sierras = FindObjectsByType<SierraController>(FindObjectsSortMode.None);
                    foreach (SierraController sc in sierras)
                        sc.InmovilidadBuff();

                    // Añade al jugador
                    PlayerController pc = FindFirstObjectByType<PlayerController>();
                    if (pc != null) pc.InmovilidadBuff();

                    Destroy(gameObject);
                    return;
                }
            }

            // Comprueba jugador
            Collider2D jugador = Physics2D.OverlapPoint(transform.position, jugadorLayer);
            if (jugador != null)
            {
                PlayerController pc = jugador.GetComponent<PlayerController>();
                if (pc != null)
                {
                    GameManager.Instance.estamina -= costeEstamina;
                    pc.InmovilidadBuff();

                    SierraController[] sierras = FindObjectsByType<SierraController>(FindObjectsSortMode.None);
                    foreach (SierraController sc in sierras)
                        sc.InmovilidadBuff();

                    SlimeController[] slimes = FindObjectsByType<SlimeController>(FindObjectsSortMode.None);
                    foreach (SlimeController sl in slimes)
                        sl.InmovilidadBuff();

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
