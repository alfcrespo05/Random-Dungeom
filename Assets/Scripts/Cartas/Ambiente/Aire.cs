using UnityEngine;

public class Aire : MonoBehaviour
{
    private bool arrastrando = false;
    private Camera cam;
    [SerializeField] private LayerMask fondoLayer;
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
            // Primero comprueba el slime
            Collider2D slimeh = Physics2D.OverlapPoint(transform.position, enemigoLayer);
            if (slimeh != null)
            {
                SlimeController slime = slimeh.GetComponent<SlimeController>();
                if (slime != null)
                {
                    GameManager.Instance.estamina -= costeEstamina;
                    slime.CambiarAAire();
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
