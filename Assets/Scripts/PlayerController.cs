using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private int numberOfMoves;
    [SerializeField]
    private GameObject path;
    [SerializeField]
    private GameObject PathWall;
    [SerializeField]
    private CanvasGroup ResultCanvas;
    [SerializeField]
    private Text ResultText;

    private float lastTime;
    private ArrayList[] position;
    private int[] directions;
    private int onStep;

    private void Start()
    {

        GeneratePath();
        ShowPath();
    }

    void Update()
    {
        Vector3 v3 = transform.position;

        bool canContinue = (Time.time - lastTime) > 0.2f;
        if (Input.GetKey("up") && canContinue && this.transform.position.z < 58)
        {
            DestroyPath();
            v3.z += 4;
            lastTime = Time.time;
            OnMove(2);
        }
        else if (Input.GetKey("down") && canContinue && this.transform.position.z > 2)
        {
            DestroyPath();
            v3.z -= 4;
            lastTime = Time.time;
            OnMove(-2);
        }
        else if (Input.GetKey("right") && canContinue && this.transform.position.x < 58)
        {
            DestroyPath();
            v3.x += 4;
            lastTime = Time.time;
            OnMove(1);
        }
        else if (Input.GetKey("left") && canContinue && this.transform.position.x > 2)
        {
            DestroyPath();
            v3.x -= 4;
            lastTime = Time.time;
            OnMove(-1);
        }
        transform.position = v3;
    }


    private void OnMove(int move)
    {
        if (onStep < position[0].Count - 1 && move != directions[onStep])
        {
            ResultCanvas.alpha = 1f;
            ResultCanvas.interactable = true;
            ResultText.text = "Fail";
        }
        else
        {
            onStep++;
            if (onStep >= position[0].Count - 1)
            {
                ResultCanvas.alpha = 1f;
                ResultCanvas.interactable = true;
                ResultText.text = "Win";
            }
            //Start();
        }
    }


    private void GeneratePath()
    {

        onStep = 0;

        position = new ArrayList[2];
        position[0] = new ArrayList();
        position[1] = new ArrayList();

        position[0].Add(7);
        position[1].Add(7);
        bool[,] been = new bool[15, 15];
        been[7, 7] = true;

        for (int i = 1; i < numberOfMoves; i++)
        {
            int coor = Random.Range(0, 2);
            int move = Random.Range(0, 2);
            if (move == 0) move = -1;
            int newpos = (int)position[coor][position[0].Count - 1] + move;
            if (newpos >= 0 && newpos <= 14)
            {

                int newX = (coor == 0) ? newpos : (int)position[0][position[0].Count - 1];
                int newY = (coor == 1) ? newpos : (int)position[1][position[1].Count - 1];

                if (!been[newX, newY])
                {
                    position[0].Add(newX);
                    position[1].Add(newY);
                    been[newX, newY] = true;
                }
                else
                {
                    i--;
                }
            }
            else
            {
                i--;
            }
        }
    }


    private void ShowPath()
    {
        directions = new int[position[0].Count];

        int PastDir = 0;
        for (int i = 0; i < position[0].Count; i++)
        {
            int x = (int)position[0][i];
            int z = (int)position[1][i];
            Vector3 v3 = new Vector3(x * 4 + 2, 0.5f, z * 4 + 2);
            path.transform.position = v3;
            Instantiate(path);

            int dir1 = 0, dir2 = 0; //1 = x+, -1 = x-, 2 = z+, -2 = z-
            if (i != 0) dir1 = -PastDir;
            if (i != position[0].Count - 1)
            {
                dir2 = GetDir(i, i + 1);
                PastDir = dir2;
            }

            directions[i] = dir2;

            if (dir1 != 1 && dir2 != 1)
            {
                int xpos = (x + 1) * 4;
                int zpos = z * 4 + 2;
                PathWall.transform.position = new Vector3(xpos, 1, zpos);
                PathWall.transform.eulerAngles = new Vector3(0, 90, 0);
                Instantiate(PathWall);
            }
            if (dir1 != -1 && dir2 != -1)
            {
                int xpos = x * 4;
                int zpos = z * 4 + 2;
                PathWall.transform.position = new Vector3(xpos, 1, zpos);
                PathWall.transform.eulerAngles = new Vector3(0, 90, 0);
                Instantiate(PathWall);
            }
            if (dir1 != 2 && dir2 != 2)
            {
                int xpos = x * 4 + 2;
                int zpos = (z + 1) * 4;
                PathWall.transform.position = new Vector3(xpos, 1, zpos);
                PathWall.transform.eulerAngles = new Vector3(0, 0, 0);
                Instantiate(PathWall);
            }
            if (dir1 != -2 && dir2 != -2)
            {
                int xpos = x * 4 + 2;
                int zpos = z * 4;
                PathWall.transform.position = new Vector3(xpos, 1, zpos);
                PathWall.transform.eulerAngles = new Vector3(0, 0, 0);
                Instantiate(PathWall);
            }
        }
    }


    private int GetDir(int from, int to)
    {
        if ((int)position[0][to] == (int)position[0][from] + 1)
        {
            return 1;
        }
        else if ((int)position[0][to] == (int)position[0][from] - 1)
        {
            return -1;
        }
        else if ((int)position[1][to] == (int)position[1][from] + 1)
        {
            return 2;
        }
        return -2;
    }


    private void DestroyPath()
    {
        GameObject[] Paths = GameObject.FindGameObjectsWithTag("Path");
        foreach (GameObject curPath in Paths) Destroy(curPath);

        GameObject[] PathWalls = GameObject.FindGameObjectsWithTag("PathWall");
        foreach (GameObject curPathWall in PathWalls) Destroy(curPathWall);
    }
}

