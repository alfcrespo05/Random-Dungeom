using UnityEngine;

public class Hielo : MonoBehaviour
{
    private bool arrastrando = false;
    private Camera cam;
    private int sueloLayer;
    [SerializeField] private LayerMask enemigoLayer;
    private Rigidbody2D rb;
    public int costeEstamina = 10;
    public Vector2 posicionOriginal;

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
        arrastrando = true;
        rb.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Collider2D>().enabled = false; // desactiva colisiones al arrastrar
    }

    void OnMouseUp()
    {
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
                    suelo.CambiarAHielo();
                    Destroy(gameObject);
                    return; // sale del método, no ejecuta el resto
                }
            }
            Collider2D sierrah = Physics2D.OverlapPoint(transform.position, enemigoLayer);
            if (sierrah != null)
            {
                SierraController sierra = sierrah.GetComponent<SierraController>();
                if (sierra != null)
                {
                    GameManager.Instance.estamina -= costeEstamina;
                    sierra.CambiarAHielo();
                    Destroy(gameObject);
                    return; // sale del método, no ejecuta el resto
                }
            }
        }

        // Si no se usó la carta por cualquier motivo, vuelve a la posición original
        transform.position = posicionOriginal;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Collider2D>().enabled = true;
    }


}