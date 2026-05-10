using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    [SerializeField] GameObject menuAjustes;
    [SerializeField] GameObject menuCreditos;
    [SerializeField] GameObject menuSonido;
    [SerializeField] GameObject menuControles;
    void Start()
    {
        menuAjustes.SetActive(false);
        menuCreditos.SetActive(false);
        menuSonido.SetActive(false);
        menuControles.SetActive(false); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Jugar()
    {
        SceneManager.LoadScene(1);
    }
    public void Ajustes()
    {
        menuAjustes.SetActive(true);
    }
    public void Creditos()
    {
        menuCreditos.SetActive(true);
    }
    public void Salir()
    {
        Application.Quit();
        Debug.Log("Saliendo...");
    }
    public void VolverCreditos()
    {
        menuCreditos.SetActive(false);
    }
    public void VolverAjustes()
    {
        menuAjustes.SetActive(false);
    }
    public void Sonido()
    {
        menuSonido.SetActive(true);
    }
    public void Controles()
    {
        menuControles.SetActive(true);
    }
    public void VolverSonido()
    {
        menuSonido.SetActive(false);
    }
    public void VolverControles()
    {
        menuControles.SetActive(false);
    }
}
