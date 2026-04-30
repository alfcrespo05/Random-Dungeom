using UnityEngine;

public class SueloController : MonoBehaviour
{
    [SerializeField] private PhysicsMaterial2D materialBasico;
    [SerializeField] private PhysicsMaterial2D materialHielo;
    [SerializeField] private Color colorNormal = Color.white;
    [SerializeField] private Color colorHielo = new Color(0.5f, 0.8f, 1f);

    private Collider2D col;
    private SpriteRenderer sr;

    void Start()
    {
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("CartaHielo"))
        {
            col.sharedMaterial = materialHielo;
            sr.color = colorHielo;
            Destroy(collision.gameObject); // destruye la carta al colisionar
        }
    }

    public void CambiarANormal()
    {
        col.sharedMaterial = materialBasico;
        sr.color = colorNormal;
    }
}