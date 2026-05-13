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
    void OnCollisionEnter2D(Collision2D other)
    {
        SlimeController slc = other.gameObject.GetComponent<SlimeController>();
        if (slc != null)
        {
            slc.Muerte();
            return;
        }
        LlamaAnimal La = other.gameObject.GetComponent<LlamaAnimal>();
        if (La != null)
        {
            La.Muerte();
            return;
        }
        SierraController sic = other.gameObject.GetComponent<SierraController>();
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
