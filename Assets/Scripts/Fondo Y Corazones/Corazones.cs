using UnityEngine;

public class Corazones : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private Color colorNormal = new Color(0f,0f,0f,0f);
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Subrayar()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = new Color(1f, 0.84f, 0f, 0.49f);
    }
    public void NoSubrayar()
    {
     
        sr.color = colorNormal;
    }
}
