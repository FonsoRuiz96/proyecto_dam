using UnityEngine;
using UnityEngine.UI;
using TMPro; // O usar UnityEngine.UI.Text si no usas TextMeshPro

public class MenuHabilidadesUI : MonoBehaviour
{
    public GameObject contenedorHabilidades; 
    public GameObject prefabBotonHabilidad;
    public Canvas contenedorFrameHabilidades;
    public GameObject prefabFrameHabilidades;
    
    public GameObject contenedorConsumibles; 
    public GameObject prefabBotonConsumible;
    public Canvas contenedorFrameConsumibles;
    public GameObject prefabFrameConsumibles;
    
    private GameObject nuevoFrame;
    
    public CombatCtrl combatCtrl;
    


    public void MostrarHabilidades(int pjId)
    {
        // Limpiar botones anteriores
        OcultarHabilidades();
        nuevoFrame = Instantiate(prefabFrameHabilidades, contenedorFrameHabilidades.transform);
        nuevoFrame.transform.SetSiblingIndex(0);
        

        // Crear un botón por cada habilidad
        foreach (HabilidadData hab in DatabaseLoader.ListaHabilidades(pjId))
        {
            GameObject nuevoBoton = Instantiate(prefabBotonHabilidad, contenedorHabilidades.transform);
            nuevoBoton.name = hab.nombre;

            // Configurar el texto del botón
            var texto = nuevoBoton.GetComponentInChildren<TMP_Text>();
            texto.text = hab.nombre;
            
        }
    }

    public void OcultarHabilidades()
    {
        foreach (Transform child in contenedorHabilidades.transform)
        {
            Destroy(child.gameObject);
        }
        
        Destroy(nuevoFrame);
    }
    
    public void MostrarConsumibles()
    {
        // Limpiar botones anteriores
        OcultarConsumibles();
        nuevoFrame = Instantiate(prefabFrameConsumibles, contenedorFrameConsumibles.transform);
        nuevoFrame.transform.SetSiblingIndex(0);
        
        
        foreach (ConsumibleData consumible in DatabaseLoader.ListaConsumibles(true))
        {
            GameObject nuevoBoton = Instantiate(prefabBotonConsumible, contenedorConsumibles.transform);
            nuevoBoton.name = consumible.nombre;
            
            // Configurar el texto del botón
            var texto = nuevoBoton.GetComponentInChildren<TMP_Text>();
            texto.text = consumible.nombre + " x" + consumible.cantidad;
            
        }
    }

    public void OcultarConsumibles()
    {
        foreach (Transform child in contenedorConsumibles.transform)
        {
            Destroy(child.gameObject);
        }
        
        Destroy(nuevoFrame);
    }
}