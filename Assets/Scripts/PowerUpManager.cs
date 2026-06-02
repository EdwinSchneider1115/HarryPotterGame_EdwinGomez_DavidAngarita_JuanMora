using UnityEngine;
using System.Collections;

public class PowerUpManager : MonoBehaviour
{
    [Header("Velocidad")]
    public float multiplicadorVelocidad = 2f;
    public float duracionVelocidad = 5f;

    [Header("Invisibilidad")]
    public float duracionInvisibilidad = 5f;

    private Movimiento movimiento;
    private Renderer[] renderers;

    // Para evitar que se sobrepongan efectos
    private Coroutine coroutineVelocidad;
    private Coroutine coroutineInvisibilidad;

    void Start()
    {
        movimiento = GetComponent<Movimiento>();
        // Guarda todos los renderers del jugador para hacerlo invisible
        renderers = GetComponentsInChildren<Renderer>();
    }

    // ───── VELOCIDAD ─────

    public void ActivarVelocidad()
    {
        if (coroutineVelocidad != null) StopCoroutine(coroutineVelocidad);
        coroutineVelocidad = StartCoroutine(EfectoVelocidad());
    }

    private IEnumerator EfectoVelocidad()
    {
        float velocidadOriginal = movimiento.velocidad;
        float velocidadCorrerOriginal = movimiento.velocidadCorrer;

        movimiento.velocidad *= multiplicadorVelocidad;
        movimiento.velocidadCorrer *= multiplicadorVelocidad;
        Debug.Log("Velocidad activada");

        yield return new WaitForSeconds(duracionVelocidad);

        movimiento.velocidad = velocidadOriginal;
        movimiento.velocidadCorrer = velocidadCorrerOriginal;
        Debug.Log("Velocidad terminada");
        coroutineVelocidad = null;
    }

    // ───── INVISIBILIDAD ─────

    public void ActivarInvisibilidad()
    {
        if (coroutineInvisibilidad != null) StopCoroutine(coroutineInvisibilidad);
        coroutineInvisibilidad = StartCoroutine(EfectoInvisibilidad());
    }

    private IEnumerator EfectoInvisibilidad()
    {
        // Ocultar renderers del jugador
        foreach (Renderer r in renderers) r.enabled = false;
        Debug.Log("Invisibilidad activada");

        // Desactivar detección de enemigos
        EnemyAI[] enemigos = FindObjectsByType<EnemyAI>(FindObjectsSortMode.None);
        foreach (EnemyAI e in enemigos) e.enabled = false;

        yield return new WaitForSeconds(duracionInvisibilidad);

        // Restaurar
        foreach (Renderer r in renderers) r.enabled = true;
        foreach (EnemyAI e in enemigos) e.enabled = true;
        Debug.Log("Invisibilidad terminada");
        coroutineInvisibilidad = null;
    }
}
