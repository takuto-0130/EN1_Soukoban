using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class GameManegerScript : MonoBehaviour
{
    public GameObject playerPrefab;
    int[,] map;
    GameObject[,] field;
    public GameObject boxPrefab;


    /// <summary>
    /// プレイヤーのインデックスを取得する
    /// </summary>
    /// <returns>プレイヤーのインデックス。見つからない場合は-1を返す。</returns>
    Vector2Int GetPlayerIndex()
    {
        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                if (field[y, x] == null) { continue; }
                if (field[y, x].tag == "Player") { return new Vector2Int(x, y); }
            }
        }
        return new Vector2Int(-1, -1);
    }

    bool MoveObject(string tag, Vector2Int moveFrom, Vector2Int moveTo)
    {
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1)) { return false; }
        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0)) { return false; }

        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box")
        {
            Vector2Int velocity = moveTo - moveFrom;

            bool success = MoveObject("Box", moveTo, moveTo + velocity);
            if (!success) { return false; }
        }
        field[moveFrom.y, moveFrom.x].transform.position = new Vector3(moveTo.x - map.GetLength(1) / 2, -moveTo.y + map.GetLength(0) / 2, 0);
        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        map = new int[,] {
           {0, 0, 0, 0, 0},
           {0, 0, 0, 0, 0},
           {0, 2, 1, 2, 0},
           {0, 2, 0, 0, 0},
           {0, 0, 0, 0, 0},
        };
        field = new GameObject[map.GetLength(0), map.GetLength(1)];
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y,x] == 1)
                {
                    field[y,x] = Instantiate(
                       playerPrefab,
                       new Vector3(x - map.GetLength(1) / 2, -y + map.GetLength(0) / 2, 0),
                       Quaternion.identity);
                }
                if (map[y,x] == 2)
                {
                    field[y, x] = Instantiate(
                       boxPrefab,
                       new Vector3(x - map.GetLength(1) / 2, -y + map.GetLength(0) / 2, 0),
                       Quaternion.identity);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            MoveObject("Player", playerIndex, playerIndex + new Vector2Int(1, 0));
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            MoveObject("Player", playerIndex, playerIndex + new Vector2Int(-1, 0));
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            MoveObject("Player", playerIndex, playerIndex + new Vector2Int(0, -1));
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            MoveObject("Player", playerIndex, playerIndex + new Vector2Int(0, 1));
        }
    }
}
