using UnityEngine;

public class BancoSilla : MonoBehaviour
{
    [SerializeField] private PhysicsMaterial2D materialNormal;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        SlimeController slc = other.GetComponent<SlimeController>();
        if (slc != null)
        {
            slc.Muerte();
            return;
        }
        LlamaAnimal La = other.GetComponent<LlamaAnimal>();
        if (La != null)
        {
            La.Muerte();
            return;
        }
        SierraController sic = other.GetComponent<SierraController>();
        if (sic != null && sic.patrullando == true)
        {
            Muerte();
            return;
        }
    }
    public void Muerte()
    {
        Destroy(gameObject);
    }
}
