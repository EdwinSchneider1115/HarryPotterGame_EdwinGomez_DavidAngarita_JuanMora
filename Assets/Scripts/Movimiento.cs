using UnityEngine;

public class Movimiento : MonoBehaviour
{
    public float velocidad = 5f;
    public float velocidadCorrer = 10f;
    public float sensibilidadMouse = 200f;
    public float fuerzaSalto = 10f;

    private Rigidbody rb;
    private bool enSuelo = false;

    private Vector3 normalPared = Vector3.zero;
    private bool enPared = false;

    private Vector3 checkpointPosition;

    private Animator animator;

    private bool isCrouching = false;

    private CapsuleCollider capsuleCollider;
    private float alturaOriginal;
    private Vector3 centroOriginal;

    void piso() { }


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        checkpointPosition = transform.position;

        alturaOriginal = capsuleCollider.height;
        centroOriginal = capsuleCollider.center;
    }

    void Update()
    { 
        animator.SetBool("aire", !enSuelo);
        animator.SetFloat("velocidadVertical", rb.linearVelocity.y);
    
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadMouse * Time.deltaTime;
        transform.Rotate(0f, mouseX, 0f);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");


        Vector3 direccion = transform.forward * z + transform.right * x;
        direccion.Normalize();

        if (enPared)
        {
            direccion = Vector3.ProjectOnPlane(direccion, normalPared);
        }

        float velocidadActual = velocidad;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            velocidadActual = velocidadCorrer;
        }

        rb.linearVelocity = new Vector3(direccion.x * velocidadActual, rb.linearVelocity.y, direccion.z * velocidadActual);

        // caminar
        if (direccion.magnitude > 0)
        {
            animator.SetBool("walk", true);
        }
        else
        {
            animator.SetBool("walk", false);
        }

        // correr
        if (Input.GetKey(KeyCode.LeftShift) && direccion.magnitude > 0 && !isCrouching)
        {
            animator.SetBool("run", true);
        }
        else
        {
            animator.SetBool("run", false);
        }

        animator.SetBool("aire", !enSuelo);

        // agacharse
        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching;
            animator.SetBool("crouch", isCrouching);

            if (isCrouching)
            {
                capsuleCollider.height = alturaOriginal / 2f;
                capsuleCollider.center = new Vector3(centroOriginal.x, centroOriginal.y / 2f, centroOriginal.z);
            }
            else
            {
                capsuleCollider.height = alturaOriginal;
                capsuleCollider.center = centroOriginal;
            }
        }

        // saltar
        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
            enSuelo = false;

            animator.SetTrigger("jump");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            enSuelo = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contacto in collision.contacts)
        {
            if (Vector3.Dot(contacto.normal, Vector3.up) < 0.5f)
            {
                enPared = true;
                normalPared = contacto.normal;
                return;
            }
        }

        enPared = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        enPared = false;
    }

    public void SetCheckpoint(Vector3 nuevaPosicion)
    {
        checkpointPosition = nuevaPosicion;
    }

    public void Respawn()
    {
        rb.linearVelocity = Vector3.zero;
        transform.position = checkpointPosition;

        // Si muere agachado, resetear el collider
        if (isCrouching)
        {
            isCrouching = false;
            animator.SetBool("crouch", false);
            capsuleCollider.height = alturaOriginal;
            capsuleCollider.center = centroOriginal;
        }
    }
}