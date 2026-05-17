using UnityEngine;

public class Aire : MonoBehaviour
{
    private bool arrastrando = false;
    private Camera cam;
    [SerializeField] private LayerMask fondoLayer;
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
        
        SlimeController[] slimes = FindObjectsByType<SlimeController>(FindObjectsSortMode.None);
        foreach (SlimeController slime in slimes) slime.Subrayar();

        FondoController fc = FindFirstObjectByType<FondoController>(); fc.Subrayar();

        arrastrando = true;
        rb.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Collider2D>().enabled = false; // desactiva colisiones al arrastrar
    }

    void OnMouseUp()
    {
        SlimeController[] slimes = FindObjectsByType<SlimeController>(FindObjectsSortMode.None);
        foreach (SlimeController slime in slimes) slime.NoSubrayar();
        FondoController fc = FindFirstObjectByType<FondoController>(); fc.NoSubrayar();
        if (!arrastrando) return;
        arrastrando = false;
        if (costeEstamina <= GameManager.Instance.estamina)
        {
            // Primero comprueba el slime
            Collider2D slimeh = Physics2D.OverlapPoint(transform.position, enemigoLayer);
            if (slimeh != null)
            {
                SlimeController slime = slimeh.GetComponent<SlimeController>();
                if (slime != null)
                {
                    GameManager.Instance.estamina -= costeEstamina;
                    slime.CambiarAAire();
                    FindFirstObjectByType<MazoManager>().CartaUsada();
                    Destroy(gameObject);
                    return;
                }
            }

            // Luego comprueba el fondo
            Collider2D fondo = Physics2D.OverlapPoint(transform.position, fondoLayer);
            if (fondo != null)
            {
                FondoController fo = fondo.GetComponent<FondoController>();
                if (fo != null)
                {
                    GameManager.Instance.estamina -= costeEstamina;
                    fo.CambiarAAire();
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
                    GameManager.Instance.estamina -= (costeEstamina / 2);
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
