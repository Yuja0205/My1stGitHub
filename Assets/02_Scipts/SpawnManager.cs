using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public GameManager gamemanger;
    public GameObject panel;
    [SerializeField] private GameObject hGroupPrefab;                       // H_Group ������
    [SerializeField] private GameObject vGroupPrefab;                       // V_Group ������
    [SerializeField] private Transform[] spawnPoints = new Transform[3];    // ��������Ʈ �迭

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

    private int droppedCount = 0;   // ���ī��Ʈ

    void Start()
    {
        boardManager = FindObjectOfType<BoardManager>();
        SpawnNewGroups();   //���ӽ��� �ϴܺ��� �ʱ�ȭ
        
    }
    

    public void OnGroupDropped()    // ���κ��忡 ���Ȯ��
    {
        droppedCount++;
        // ���ӿ��� �˻�
        StartCoroutine(DelayedGameOverCheck());
        Debug.Log(droppedCount);
        if (droppedCount >= 3)  // 3�� �� ������ �� ����
        {
            droppedCount = 0;
            SpawnNewGroups();
            
        }
    }

    void SpawnNewGroups()   // ��������Ʈ�� ������ ����
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject prefabToSpawn = (Random.value < 0.5f) ? hGroupPrefab : vGroupPrefab;
            GameObject group = Instantiate(prefabToSpawn, spawnPoints[i].position, Quaternion.identity);    //����������Ʈ
            // ��ġ ����Ű��
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
    //            return true; // �ϳ��� ���� �� �ִٸ� ���� ���� ����
    //        }
    //    }

    //    return false; // �ƹ��͵� �� ���� �� ���ӿ���
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
                                // *** ��Ȱ��ȭ�� Ÿ�� üũ ***
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
                                // *** ��Ȱ��ȭ�� Ÿ�� üũ ***
                                if (!boardManager.IsTileActive(x, y + i) ||
                                    boardManager.GetBlock(x, y + i) != null)
                                {
                                    canPlace = false;
                                    break;
                                }
                            }
                        }

                        if (canPlace) return true; // �� ������ �����ϸ� ���� ���
                    }
                }
            }
        }

        return false; // �ƹ� �׷쵵 �� ������ ���ӿ���
    }
    IEnumerator DelayedGameOverCheck()
    {
        
        yield return new WaitForSeconds(2f); // ��ġ ���� �ð� ���

        //  �ٽ� �˻�
        if (!CanAnyGroupBePlaced())
        {
            Debug.Log("���ӿ���");
            //panel.SetActive(true);
            
            gamemanger.StartCoroutine(gamemanger.twoSecond());
            
        }
    }

}
