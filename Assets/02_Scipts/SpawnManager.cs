using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public GameManager gamemanger;
    public GameObject panel;
    [SerializeField] private GameObject hGroupPrefab;                       // H_Group 프리펩
    [SerializeField] private GameObject vGroupPrefab;                       // V_Group 프리펩
    [SerializeField] private Transform[] spawnPoints = new Transform[3];    // 생성포인트 배열

    BoardManager boardManager;


    private Vector2 spawnPoint_P;
    public Vector2 SpawnPoint_P
    {
        get { return spawnPoint_P; }
    }
    public Transform[] SpawnPoints
    {
        get { return spawnPoints; }
    }

    private int droppedCount = 0;   // 드랍카운트

    void Start()
    {
        boardManager = FindObjectOfType<BoardManager>();
        SpawnNewGroups();   //게임시작 하단보드 초기화
        
    }
    

    public void OnGroupDropped()    // 메인보드에 드랍확인
    {
        droppedCount++;
        // 게임오버 검사
        StartCoroutine(DelayedGameOverCheck());
        Debug.Log(droppedCount);
        if (droppedCount >= 3)  // 3개 다 놓으면 블럭 생성
        {
            droppedCount = 0;
            SpawnNewGroups();
            
        }
    }

    void SpawnNewGroups()   // 생성포인트에 프리펩 생성
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject prefabToSpawn = (Random.value < 0.5f) ? hGroupPrefab : vGroupPrefab;
            GameObject group = Instantiate(prefabToSpawn, spawnPoints[i].position, Quaternion.identity);    //개별오브젝트
            // 위치 기억시키기
            DragDrop2D dragDrop = group.GetComponent<DragDrop2D>();
            if (dragDrop != null)
            {
                dragDrop.SetSpawnPosition(spawnPoints[i].position);
            }
            
        }
        

    }
    Vector2Int WorldToBoardPos(Vector3 worldPos)
    {
        int x = Mathf.FloorToInt(worldPos.x);
        int y = Mathf.FloorToInt(worldPos.y);
        return new Vector2Int(x, y);
    }


    //public bool CanAnyGroupBePlaced()
    //{
    //    GameObject[] remainingGroups = GameObject.FindGameObjectsWithTag("BlockGroup");

    //    foreach (var group in remainingGroups)
    //    {
    //        DragDrop2D dragDrop = group.GetComponent<DragDrop2D>();
    //        if (dragDrop != null && dragDrop.CanBePlacedOnBoard())
    //        {
    //            return true; // 하나라도 놓을 수 있다면 게임 진행 가능
    //        }
    //    }

    //    return false; // 아무것도 못 놓음 → 게임오버
    //}
    public bool CanAnyGroupBePlaced()
    {
        GameObject[] remainingGroups = GameObject.FindGameObjectsWithTag("BlockGroup");

        foreach (var group in remainingGroups)
        {
            DragDrop2D dragDrop = group.GetComponent<DragDrop2D>();
            if (dragDrop != null)
            {
                int width = boardManager.width;
                int height = boardManager.height;

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        bool canPlace = true;

                        if (dragDrop.isHorizontal)
                        {
                            if (x > width - 3) continue;

                            for (int i = 0; i < 3; i++)
                            {
                                // *** 비활성화된 타일 체크 ***
                                if (!boardManager.IsTileActive(x + i, y) ||
                                    boardManager.GetBlock(x + i, y) != null)
                                {
                                    canPlace = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (y > height - 3) continue;

                            for (int i = 0; i < 3; i++)
                            {
                                // *** 비활성화된 타일 체크 ***
                                if (!boardManager.IsTileActive(x, y + i) ||
                                    boardManager.GetBlock(x, y + i) != null)
                                {
                                    canPlace = false;
                                    break;
                                }
                            }
                        }

                        if (canPlace) return true; // 한 군데라도 가능하면 게임 계속
                    }
                }
            }
        }

        return false; // 아무 그룹도 못 놓으면 게임오버
    }
    IEnumerator DelayedGameOverCheck()
    {
        
        yield return new WaitForSeconds(2f); // 매치 제거 시간 고려

        //  다시 검사
        if (!CanAnyGroupBePlaced())
        {
            Debug.Log("게임오버");
            //panel.SetActive(true);
            
            gamemanger.StartCoroutine(gamemanger.twoSecond());
            
        }
    }

}
