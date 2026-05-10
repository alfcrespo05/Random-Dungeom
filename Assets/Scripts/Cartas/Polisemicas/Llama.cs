using UnityEngine;

public class Llama : MonoBehaviour
{
    private bool arrastrando = false;
    private Camera cam;
    private int sueloLayer;
    private Rigidbody2D rb;
    public int costeEstamina = 15;
    public Vector2 posicionOriginal;
    [SerializeField] private LayerMask papeleraLayer;
    void Start()
    {
        cam = Camera.main;
        sueloLayer = LayerMask.GetMask("Suelo");
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
        SueloController sc = FindFirstObjectByType<SueloController>(); sc.Subrayar();
        arrastrando = true;
        rb.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Collider2D>().enabled = false; // desactiva colisiones al arrastrar
    }

    void OnMouseUp()
    {
        SueloController sc = FindFirstObjectByType<SueloController>(); sc.NoSubrayar();
        if (!arrastrando) return;
        arrastrando = false;
        if (costeEstamina <= GameManager.Instance.estamina)
        {
            Collider2D bloque = Physics2D.OverlapPoint(transform.position, sueloLayer);
            if (bloque != null)
            {
                SueloController suelo = bloque.GetComponent<SueloController>();
                if (suelo != null)
                {
                    GameManager.Instance.estamina -= costeEstamina;
                    suelo.Llama();
                    FindFirstObjectByType<MazoManager>().CartaUsada();
                    Destroy(gameObject);
                    return;
                }
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

        transform.position = posicionOriginal;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Collider2D>().enabled = true;
    }
}
