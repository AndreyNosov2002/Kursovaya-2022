using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.PostProcessing;

public class GameController : MonoBehaviour
{

	private CubePos nowCube = new CubePos(0, 1, 0);
	public float cubeChangePlaceSpeed = 0.5f;
	public Transform cubeToPlace;
    private float canMoveToPosition, camMoveSpeed = 2f;
    public Text scoreTxt;
    //массив всех цветов кубов
    public GameObject[] cubesToCreate;
	public GameObject  allCubes, vfx;
    public GameObject[] canvasStartPage;
    private Rigidbody allCubesRb;
 
    public Color[] BgColors;
    private Color toCameraColor;
   
    private bool IsLose, firstCube;

	private List<Vector3> allCubesPositions = new List<Vector3> {
		new Vector3(0, 0, 0),
		new Vector3(1, 0, 0),
		new Vector3(-1, 0, 0),
		new Vector3(0, 1, 0),
		new Vector3(0, 0, 1),
		new Vector3(0, 0, -1),
		new Vector3(1, 0, 1),
		new Vector3(-1, 0, -1),
		new Vector3(-1, 0, 1),
		new Vector3(1, 0, -1),
	};

    private int prevCountMaxHorizontal;
    private Transform mainCam;

	private Coroutine showCubePlace;
    //список открытых цветов кубов
    private List<GameObject> possibleCubesToCreate;
    private int[] map = new int[] { 0, 4, 8, 12, 16, 20, 24, 28, 32, 50 };
    private void Start()
    {
        //проверки на открытие цветов кубов
        possibleCubesToCreate = new List<GameObject>();
        var rec = PlayerPrefs.GetInt("score");
        for (int i = 0; i < map.Length; i++)
            if (rec >= map[i])
                possibleCubesToCreate.Add(cubesToCreate[i]);
            else
                break;


        scoreTxt.text = "<size=35>best:</size>" + PlayerPrefs.GetInt("score") + "\n<size=35>now:</size>0";
        toCameraColor = Camera.main.backgroundColor;
        mainCam = Camera.main.transform;
        canMoveToPosition = 5.9f + nowCube.y - 1f;

		allCubesRb = allCubes.GetComponent<Rigidbody>();
        showCubePlace= StartCoroutine(ShowCubePlace());
	}

    private void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.touchCount > 0)&&cubeToPlace!=null&& allCubes!=null &&!EventSystem.current.IsPointerOverGameObject())
        {
#if !UNITY_EDITOR
        if(Input.GetTouch(0).phase!=TouchPhase.Began)
			return;
#endif
           

            if (!firstCube)
            {
                firstCube = true;
                foreach (GameObject obj in canvasStartPage)
                {
                    Destroy((obj));
                }
            }
           

          GameObject newCube=  Instantiate(possibleCubesToCreate[UnityEngine.Random.Range(0, possibleCubesToCreate.Count)],
            cubeToPlace.position, Quaternion.identity) as GameObject;
		  newCube.transform.SetParent(allCubes.transform);
		  nowCube.setVector(cubeToPlace.position);
		  allCubesPositions.Add(nowCube.getVector());

          GameObject newVFX = Instantiate(vfx, newCube.transform.position, Quaternion.identity) as GameObject;

          Destroy(newVFX, 1.5f);

          allCubesRb.isKinematic = true;
          allCubesRb.isKinematic = false;
          SpawnPosition();
          MoveCameraChangeBg();

        }
		if(!IsLose && allCubesRb.velocity.magnitude>0.1f)
        {
            Destroy(cubeToPlace.gameObject);
            IsLose = true;
			StopCoroutine(showCubePlace);
		}

        mainCam.localPosition = Vector3.MoveTowards(mainCam.localPosition,
            new Vector3(mainCam.localPosition.x, canMoveToPosition, mainCam.localPosition.z),
            camMoveSpeed=Time.deltaTime);

        if (Camera.main.backgroundColor != toCameraColor)
        {
            Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, toCameraColor, Time.deltaTime / 1.5f);

        }
    }

	IEnumerator ShowCubePlace()
	{
		while (true)
		{
			SpawnPosition();
			yield return new WaitForSeconds(cubeChangePlaceSpeed);
		}
	}

	private void SpawnPosition()
	{
		List<Vector3> positions = new List<Vector3>();
		if (IsPositionEmpty(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z)) && nowCube.x + 1 != cubeToPlace.position.x)
			positions.Add(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z));
		if (IsPositionEmpty(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z)) && nowCube.x - 1 != cubeToPlace.position.x)
			positions.Add(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z));
		if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y + 1, nowCube.z)) && nowCube.y + 1 != cubeToPlace.position.y)
			positions.Add(new Vector3(nowCube.x, nowCube.y + 1, nowCube.z));
		if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y - 1, nowCube.z)) && nowCube.y - 1 != cubeToPlace.position.y)
			positions.Add(new Vector3(nowCube.x, nowCube.y - 1, nowCube.z));
		if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z + 1)) && nowCube.z + 1 != cubeToPlace.position.z)
			positions.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z + 1));
		if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z - 1)) && nowCube.z - 1 != cubeToPlace.position.z)
			positions.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z - 1));

        if (positions.Count > 1)
        {
            cubeToPlace.position = positions[UnityEngine.Random.Range(0, positions.Count)];


		}
		else if (positions.Count == 0)
        {
            IsLose = true;
        }
		else
        {
            cubeToPlace.position = positions[0];


        }
    }

	private bool IsPositionEmpty(Vector3 targetPos)
	{
		if (targetPos.y == 0)
			return false;
		foreach (Vector3 pos in allCubesPositions)
			if (pos.x == targetPos.x && pos.y == targetPos.y && pos.z == targetPos.z)
				return false;
		return true;
	}

    private void MoveCameraChangeBg()
    {
        int maxX = 0;
        int maxY = 0;
        int maxZ = 0;
        int maxHor;

		foreach (Vector3 pos in allCubesPositions)
        {
            if (Mathf.Abs(Convert.ToInt32(pos.x)) > maxX)
                maxX = Mathf.Abs(Convert.ToInt32(pos.x));      

            if ((Convert.ToInt32(pos.y)) > maxY)
                maxY = (Convert.ToInt32(pos.y));
            
            if (Mathf.Abs(Convert.ToInt32(pos.z)) > maxZ)
                maxZ = Mathf.Abs(Convert.ToInt32(pos.z));
        }


        //вывод на экран текущее количество кубов и рекорд
        maxY--;
        if (PlayerPrefs.GetInt("score") < maxY)
            PlayerPrefs.SetInt("score", maxY);

        scoreTxt.text = "<size=35>best:</size>" + PlayerPrefs.GetInt("score") + "\n<size=35>now:</size>" + maxY;


        canMoveToPosition = 5.9f + nowCube.y - 1f;

        maxHor = maxX > maxZ ? maxX : maxZ;

        if (maxHor % 3 == 0 && prevCountMaxHorizontal!=maxHor)
        {
            mainCam.localPosition += new Vector3(0, 0, -2f);
            prevCountMaxHorizontal = maxHor;
        }

        if (maxY >= 7)
        {
            toCameraColor = BgColors[2];
		}
		else if (maxY >= 5)
        {
            toCameraColor = BgColors[1];
        }
        else if (maxY >= 2)
        {
            toCameraColor = BgColors[0];
        }
      




    }
  
}

struct CubePos
{
	public int x, y, z;

	public CubePos(int x, int y, int z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public Vector3 getVector()
	{
		return new Vector3(x, y, z);
	}

	public void setVector(Vector3 pos)
	{
		x = Convert.ToInt32(pos.x);
		y = Convert.ToInt32(pos.y);
		z = Convert.ToInt32(pos.z);
	}
}