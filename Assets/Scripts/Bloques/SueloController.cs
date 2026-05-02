using UnityEngine;

public class SueloController : MonoBehaviour
{
    [SerializeField] private PhysicsMaterial2D materialNormal;
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

    public void CambiarAHielo()
    {
        col.sharedMaterial = materialHielo;
        sr.color = colorHielo;
    }

    public void CambiarANormal()
    {
        col.sharedMaterial = materialNormal;
        sr.color = colorNormal;
    }
}