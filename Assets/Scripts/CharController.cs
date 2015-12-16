using UnityEngine;
using System.Collections;

public class CharController : MonoBehaviour {

    public float Speed { get; set; }
    public float WalkSpeed = 20.0f;

    public CharState State = CharState.Run;
	public CharType Type;
    GameObject blood;
    Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();
        Speed = WalkSpeed;

        if(Type == CharType.Zombie2) 
			InvokeRepeating("ChangeDirection", 3.0f, Random.Range(4, 9));
        
    }

    void Update()
    {
        Move();
       
    }

    void Move()
    {
        if (State == CharState.Run)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, Speed * Time.deltaTime);
        }
    }

    
    void ChangeDirection()
    {
		int[] angles = { -45, 45 };
		transform.rotation = Quaternion.Euler(0, angles[Random.Range(0, 2)], 0);
    }
    
    void OnMouseDown()
    {
        if (State != CharState.Death)
        {
            kill();
        }
    }

    public void kill()
    {
        State = CharState.Death;
        Speed = 0;
        blood = Instantiate(Resources.Load<GameObject>("Blood")) as GameObject;
        blood.transform.position = transform.position;
        if (Type == CharType.Human)
        {
            FindObjectOfType<GameController>().FinishGame(false);
        }
        else
        {
            FindObjectOfType<GameController>().Score++;
        }
        Destroy(blood, 1.0f);
		Destroy(gameObject, 0.3f);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "border_right")
		{
			transform.rotation = Quaternion.Euler(0, 45, 0);
		}

		if (other.gameObject.name == "border_left")
		{
			transform.rotation = Quaternion.Euler(0, -45, 0);
		}

		if (other.gameObject.name == "finish")
		{
			if(Type != CharType.Human) FindObjectOfType<GameController>().DecreaseLife();
			Destroy(gameObject, 0.3f);
		}
	}
}
