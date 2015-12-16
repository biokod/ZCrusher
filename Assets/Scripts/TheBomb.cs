using UnityEngine;
using System.Collections;

public class TheBomb : MonoBehaviour {

	float radius = 30f;
	bool canDrag = true;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Drag();
        if (Input.GetMouseButtonDown(0))
        {
            Explode();
        }
	}


	void Explode()
	{
        CharController item;
        canDrag = false;
        GameObject explode = Instantiate(Resources.Load<GameObject>("Explosion"));;

        RaycastHit rayhit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayhit))
        {
            explode.transform.position = rayhit.point;
        }

		Collider[] colliders = Physics.OverlapSphere(explode.transform.position, radius);

        //Debug.Log("KILL = " + colliders.Length);
        for (int i = 0; i < colliders.Length; i++)
        {
            item = colliders[i].GetComponent<CharController>();
            if (item != null)
            {
                item.kill();
            }
        }
        Destroy(gameObject);
	}

	void Drag()
	{
		if (canDrag)
		{
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y);
            Vector3 objPos = Camera.main.ScreenToWorldPoint(mousePos);
            transform.position = objPos;

		}

	}

}
