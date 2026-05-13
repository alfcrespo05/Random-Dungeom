using UnityEngine;

public class Piedra : MonoBehaviour
{
    private bool arrastrando = false;
    private Camera cam;
    private int sueloLayer;
    [SerializeField] private LayerMask UILayer;
    [SerializeField] private LayerMask papeleraLayer;
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
        if (!GameManager.Instance.MoverCartas || Time.timeScale == 1) return;

        SueloController[] suelos = FindObjectsByType<SueloController>(FindObjectsSortMode.None);
        foreach (SueloController suelo in suelos)
            suelo.Subrayar();
        Corazones cc = FindFirstObjectByType<Corazones>(); cc.Subrayar();

        arrastrando = true;
        rb.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Collider2D>().enabled = false; // desactiva colisiones al arrastrar
    }

    void OnMouseUp()
    {
        SueloController[] suelos = FindObjectsByType<SueloController>(FindObjectsSortMode.None);
        foreach (SueloController suelo in suelos)
            suelo.NoSubrayar();
        Corazones cc = FindFirstObjectByType<Corazones>(); cc.NoSubrayar();
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

                    // Aplica hielo a todos los bloques del mismo tipo
                    SueloController[] todosLosBloques = FindObjectsByType<SueloController>(FindObjectsSortMode.None);
                    foreach (SueloController b in todosLosBloques)
                    {
                        if (b.EsMismoTipo(suelo))
                            b.CambiarAPiedra();
                    }
                    FindFirstObjectByType<MazoManager>().CartaUsada();
                    Destroy(gameObject);
                    return;
                }
            }
            Collider2D corazones = Physics2D.OverlapPoint(transform.position, UILayer);
            if (corazones != null)
            {
              
                GameManager.Instance.estamina -= costeEstamina;
                GameManager.Instance.AñadirPiedra();
                FindFirstObjectByType<MazoManager>().CartaUsada();
                Destroy(gameObject);
                return;
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

        // Si no se usó la carta por cualquier motivo, vuelve a la posición original
        transform.position = posicionOriginal;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Collider2D>().enabled = true;
    }
}
