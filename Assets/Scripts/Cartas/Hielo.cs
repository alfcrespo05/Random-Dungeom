using UnityEngine;

public class Hielo : MonoBehaviour
{
    private bool arrastrando = false;
    private Camera cam;
    private int sueloLayer;
    private Rigidbody2D rb;

    void Start()
    {
        cam = Camera.main;
        sueloLayer = LayerMask.GetMask("Suelo");
        rb = GetComponent<Rigidbody2D>();
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
        rb.bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Collider2D>().enabled = true; // reactiva al soltar

        Collider2D bloque = Physics2D.OverlapPoint(transform.position, sueloLayer);
        if (bloque != null)
        {
            SueloController suelo = bloque.GetComponent<SueloController>();
            if (suelo != null)
            {
                GameManager manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
                manager.estamina -= 10;
                suelo.CambiarAHielo();
                Destroy(gameObject);
            }
        }
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
}