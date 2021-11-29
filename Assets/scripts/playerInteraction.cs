using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerInteraction : MonoBehaviour
{
    public LayerMask TreeLayerMask;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject hitObject = this.InteractionObject(this.transform.position, this.transform.forward);
            if (hitObject.tag == "tree")
            {
                Destroy(hitObject);
            }

            Debug.Log(hitObject.name);
        }       
    }

    GameObject InteractionObject(Vector3 pos, Vector3 direction)
    {
        RaycastHit hit = Physics.RaycastAll(pos, direction, 5)[0];

        return hit.rigidbody.gameObject;
    }

    void OnDrawGizmos()
    {
        Color color = Color.blue;
        

    }
}