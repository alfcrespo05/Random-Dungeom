using UnityEngine;

public class Estrella : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc != null)
        {
            AudioManager.Instance.PlaySound(AudioManager.Instance.estrella);
            GameManager.Instance.estrella += 1;
            Destroy(gameObject);

        }
    }
   
}
