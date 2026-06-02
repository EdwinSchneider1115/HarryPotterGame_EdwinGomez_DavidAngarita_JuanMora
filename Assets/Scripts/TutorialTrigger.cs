using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [Header("Mensaje")]
    public string titulo = "Tutorial";

    [TextArea(3, 6)]
    public string contenido = "Escribe aqui el mensaje...";

    [Header("Comportamiento")]
    [Tooltip("Si esta activo solo se activa una vez")]
    public bool unaVez = true;

    [Tooltip("Si esta activo oculta el mensaje al salir de la zona")]
    public bool ocultarAlSalir = false;

    private bool activado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Jugador")) return;
        if (unaVez && activado) return;

        activado = true;
        TutorialManager.instancia.MostrarMensaje(titulo, contenido);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Jugador")) return;
        if (!ocultarAlSalir) return;

        TutorialManager.instancia.OcultarMensaje();
    }
}
