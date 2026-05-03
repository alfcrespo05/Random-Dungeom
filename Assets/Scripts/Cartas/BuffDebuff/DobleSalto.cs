using UnityEngine;

public class DobleSalto : MonoBehaviour
{
    private bool arrastrando = false;
    private Camera cam;
    [SerializeField] private LayerMask jugadorLayer;
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

            Collider2D jugador = Physics2D.OverlapPoint(transform.position, jugadorLayer);
            if (jugador != null)
            {
                PlayerController pc = jugador.GetComponent<PlayerController>();
                if (pc != null)
                {
                    GameManager.Instance.estamina -= costeEstamina;
                    pc.DobleSaltoBuff();
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
