using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class GameController : MonoBehaviour {

	public GameObject BorderLeft;
	public GameObject BorderRight;
	public GameObject FinishLine;
	public Canvas MainCanvas;
    public Text tfScore;
	
	Vector3[] StartPositions;
	int[] GenChance = { 80, 15, 50, 30 };
	const int MaxTime = 60;
	const int CharsInLine = 5;
	float GenRepeatRate = 3.0f;
	Time StartingTime;
	int TimeCounter = 0;
	int LifeCounter = 0;
    int score;
    public int Score 
    {
        set
        {
            score = value;
            tfScore.text = score.ToString();
        }
        get
        {
            return score;
        }
    }


	void Start()
	{
		StartPositions = new Vector3[CharsInLine];
		for (int i = 0; i < CharsInLine; i++)
		{
			StartPositions[i] = GameObject.Find("StartPoint_" + i).transform.position;
		}

		Text tfResult = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(a => (a.name == "tfGameResult")).GetComponent<Text>();
		tfResult.text = "";

	}
	
	void GenerateLine()
	{
		int chance = 0;
		int i,c = 0;
		int sumChance = 0;
		int chsum = 0;
		Vector3 placePoint;

		for (i = 0; i < GenChance.Length; i++)
		{
			sumChance += GenChance[i];
		}
		for (i = 0; i < CharsInLine; i++)
		{
			chance = Random.Range(0, sumChance + 1);

			for (c = 0; c < GenChance.Length; c++)
			{
				chsum += GenChance[c];
                if (chance <= chsum) break;
			}
			chsum = 0;
			if (c > 0)
			{
				c--;
				placePoint = StartPositions[i];
				Instantiate(Resources.Load<GameObject>("char_" + c), placePoint, Quaternion.identity);
			}
		}
	}

    void UpdateTimer()
	{
		TimeCounter++;
		if(TimeCounter == MaxTime) {
			FinishGame();
		}
	}

    public void Begin()
    {
		TimeCounter = 0;
		LifeCounter = 3;
        Score = 0;
		
		InvokeRepeating("UpdateTimer", 0.0f, 1.0f);
		InvokeRepeating("GenerateLine", 1.0f, GenRepeatRate);
    }

	public void FinishGame(bool win = true)
	{
		Debug.Log(" { Game finished !!! }");

		GameObject[] allGO = Resources.FindObjectsOfTypeAll<GameObject>();
		CancelInvoke();
        ClearLevel();

		allGO.FirstOrDefault(a => (a.name == "GamePanel")).SetActive(false);
		allGO.FirstOrDefault(a => (a.name == "StartPanel")).SetActive(true);
		//MainCanvas.transform.Find("GamePanel").gameObject.SetActive(false);
		//MainCanvas.transform.Find("StartPanel").gameObject.SetActive(true);
        allGO.FirstOrDefault(a => (a.name == "btnBomb")).SetActive(true);
		for (int i = 1; i <= 3; i++)
		{
			allGO.FirstOrDefault(a => (a.name == "life_" + i)).SetActive(true);
		}

		Text tfResult = allGO.FirstOrDefault(a => (a.name == "tfGameResult")).GetComponent<Text>();
		tfResult.text = (win ? "LEVEL\nCOMPLETED!" : "YOU\nLOOSE!");
	}

	public void ClearLevel() {
		CharController[] chars = FindObjectsOfType<CharController>();
		Debug.Log("chars length = " + chars.Length);
		foreach (CharController item in chars)
		{
			item.Speed = 0;
			Destroy(item.gameObject);
		}
	}

	public void DecreaseLife()
	{
		Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(a => (a.name == "life_" + LifeCounter)).SetActive(false);
        LifeCounter--;
		if (LifeCounter == 0)
		{
			FinishGame(false);
		}
	}

    public void DragBomb()
    {
        GameObject bomb = Instantiate(Resources.Load<GameObject>("bomb")) as GameObject;
        //bomb.transform.rotation = Quaternion.Euler(new Vector3(270, 0, 0));
    }
    
}
