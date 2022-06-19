using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Rigidbody2D))]
public class P5_Crushable : MonoBehaviour
{
    [Range(10, 1000)]
    [SerializeField] private int crushForceThreshold;
    [Range(-0.1f, -0.9f)]
    [SerializeField] private float crushDotThreshold;
    [SerializeField] private float currentImpactForce;

    private float radioDetectionOffset = 0.02f;
    private Rigidbody2D rb;
    private Collider2D crushableCollider;
    List<ContactPoint2D> pointList = new List<ContactPoint2D>();
    List<GameObject> objectsOfPointList = new List<GameObject>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        crushableCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        Physics2D.showColliderContacts = true;
    }
    private void Update()
    {
        print("currentImpactForce: " + currentImpactForce);

        if (currentImpactForce >= crushForceThreshold)
        {
            Debug.LogWarning($"{gameObject.name} was crushed using {currentImpactForce} units of force.");
            gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        if (Application.isPlaying)
            Gizmos.DrawWireSphere(transform.position, crushableCollider.bounds.extents.x + radioDetectionOffset);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentImpactForce = collision.GetImpactForce();

        if (collision == null) return;
        //PrintContactPoints(collision);

        //DEBO DE SACAR DINÁMICAMENTE LOS PUNTOS QUE YA NO COLISIONAN EN ESTE PUNTO

        if (!objectsOfPointList.Contains(collision.collider.gameObject))
        {
            objectsOfPointList.Add(collision.collider.gameObject);
            pointList.Add(collision.GetContact(0));
        }

        if (!CanBeCrushed()) return;
        //Debug.Log("Object can be crushed");
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (objectsOfPointList.Contains(collision.collider.gameObject))
        {
            pointList.RemoveAt(objectsOfPointList.IndexOf(collision.collider.gameObject));
            objectsOfPointList.Remove(collision.collider.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //Calculate the current impact force. It's important to perform this in here instead of OnCollisionEnter
        currentImpactForce = collision.GetImpactForce();
    }

    private bool CanBeCrushed()
    {
        #region COMMENTS
        //IDEA: GUARDAR CADA COLLIDER QUE ENTRA EN LIST PARA HACER ESTE CÁLCULO. CUANDO HAGAN EXIT LOS QUITO DE LA LISTA
        //POR CADA PUNTO DE CONTACTO, CHECAR SU PRODUCTO PUNTO CON TODOS LOS DEMÁS
        //AL HABER UN PRODUCTO PUNTO QUE ENTRE EN EL RANGO, SE SALE Y CHECA AHORA LA FUERZA DE APLASTAMIENTO
        //SI LA FUERZA DE APLASTAMIENTO ES SUFICIENTEMENTE ALTA, ENTONCES DEBERÍA SER APLASTADO

        /*foreach (var i_Contact in collision.contacts)
        {
            //Debug.Log($"Normal in {i_Contact.point}: {i_Contact.normal}");
            //Debug.Log($"Contact: {contact.point}");
            Debug.Log($"Dot of {i_Contact.normal} and {j_Contact.normal} " + Vector2.Dot(i_Contact.normal, j_Contact.normal));
            if (Vector2.Dot(i_Contact.normal, j_Contact.normal) < crushDotThreshold)
            {
                Debug.Log("Object can be crushed");
                return true;
            }
        }*/

        //FALTA CHECAR POR QUÉ NO RECIBE LOS CONTACT POINTS COMPLETOS, SOLO EL PRIEMRO
        #endregion

        foreach (var i_Contact in pointList)
        {
            //HAY QUE PULIR PARA QUE FUNCIONE CON LAS FUERZAS QUE CUMPLEN CON EL PRODUCTO PUNTO, PUEDE HABER PROBLEMAS
            //CON LA LÓGICA ACTUAL, PUES PUEDE HACER CRUSH CON CIERTOS VALORES DE NORMALES

            foreach (var j_Contact in pointList)
            {
                //Debug.Log($"Dot of {i_Contact.collider.name} {i_Contact.normal} and " +
                //  $"{j_Contact.collider.name} {j_Contact.normal}: " + Vector2.Dot(i_Contact.normal, j_Contact.normal));

                if (Vector2.Dot(i_Contact.normal, j_Contact.normal) <= crushDotThreshold)
                {
                    return true;
                }
            }

        }

        return false;
    }

    private float GetTotalImpulse(Collision2D _collision)
    {
        Vector2 impulse = Vector2.zero;

        foreach (var contact in _collision.contacts)
        {
            impulse += contact.normal * contact.normalImpulse;
            impulse.x += contact.tangentImpulse * contact.normal.y;
            impulse.y -= contact.tangentImpulse * contact.normal.x;
        }

        return impulse.magnitude;
    }

    private void PrintContactPoints(Collision2D _collision)
    {
        foreach (ContactPoint2D contact in _collision.contacts)
        {
            Debug.Log(contact.point);
        }
    }

}
