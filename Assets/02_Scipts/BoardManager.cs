﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public ScoreManager scoreMgr;
    public GameObject[] specialBlockPrefab;
    

    public int width = 9;
    public int height = 10;
    public GameObject[,] board; //블록이 놓인 위치를 저장하는 2차원 배열

    public float destroyDelay = 0.2f;

    void Start()
    {
        board = new GameObject[width, height]; // board 배열 초기화
        
        
    }

    // 매치 확인
    public void CheckMatches()
    {
        bool[,] visited = new bool[width, height];
        List<List<Vector2Int>> allMatches = new List<List<Vector2Int>>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (board[x, y] != null && !visited[x, y]) //visited 배열로 이미 확인한 위치는 스킵
                {
                    List<Vector2Int> match = GetConnectedBlocks(x, y, visited);
                    if (match.Count >= 3)
                    {
                        allMatches.Add(match);
                    }
                }
            }
        }

        foreach (var match in allMatches)
        {
            StartCoroutine(DestroyMatched(match));
        }
    }

    
    //매치 상세 로직(BFS)
    List<Vector2Int> GetConnectedBlocks(int startX, int startY, bool[,] visited)
    {
        List<Vector2Int> matched = new List<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();

        if (board[startX, startY] == null) return matched;

        BlockColor colorComp = board[startX, startY].GetComponent<BlockColor>();
        if (colorComp == null) return matched;
        string targetColor = colorComp.randColor;

        queue.Enqueue(new Vector2Int(startX, startY));
        visited[startX, startY] = true;

        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            matched.Add(current);

            for (int i = 0; i < 4; i++)
            {
                int nx = current.x + dx[i];
                int ny = current.y + dy[i];

                // 보드 안이고, 같은 색이면 큐에 추가
                if (IsInsideBoard(nx, ny) && !visited[nx, ny] && board[nx, ny] != null)
                {
                    BlockColor neighborColor = board[nx, ny].GetComponent<BlockColor>();
                    if (neighborColor != null && neighborColor.randColor == targetColor)
                    {
                        queue.Enqueue(new Vector2Int(nx, ny));
                        visited[nx, ny] = true;
                    }
                }
            }
        }

        return matched;
    }


    //매치된 블럭 제거
    IEnumerator DestroyMatched(List<Vector2Int> matched)
    {
        //  먼저 주변 특수블럭 검사
        TriggerNearbySpecialBlocks(matched);
        int randomIndex = Random.Range(0, specialBlockPrefab.Length);
        GameObject randomSpecial = specialBlockPrefab[randomIndex];
        //  특수블럭 생성 조건
        if (matched.Count >= 5)
        {
            Vector2Int? spawnPos = FindRandomAvailablePosition();
            if (spawnPos.HasValue)
            {
                Vector3 world = BoardToWorldPos(spawnPos.Value);
                GameObject sp = Instantiate(randomSpecial, world, Quaternion.identity);
                if (sp.name.Contains("(Clone)"))
                {
                    int cloneIndex = sp.name.IndexOf("(Clone)");
                    sp.name = sp.name.Substring(0, cloneIndex);
                }
                board[spawnPos.Value.x, spawnPos.Value.y] = sp;
            }
        }

        foreach (var pos in matched)
        {
            DestroyBlock(pos.x, pos.y); //  동일한 함수로 점수 처리
            yield return new WaitForSeconds(destroyDelay);
        }

        
    }


    void TriggerNearbySpecialBlocks(List<Vector2Int> matched)
    {
        HashSet<Vector2Int> triggered = new HashSet<Vector2Int>();

        foreach (var pos in matched)
        {
            int[] dx = { 1, -1, 0, 0 };
            int[] dy = { 0, 0, 1, -1 };

            for (int i = 0; i < 4; i++)
            {
                int nx = pos.x + dx[i];
                int ny = pos.y + dy[i];

                if (IsInsideBoard(nx, ny))
                {
                    Vector2Int neighborPos = new Vector2Int(nx, ny);
                    if (triggered.Contains(neighborPos)) continue;

                    GameObject neighbor = board[nx, ny];
                    if (neighbor != null)
                    {
                        SpecialBlock sp = neighbor.GetComponent<SpecialBlock>();
                        if (sp!=null&&sp.name == "DragonEye")
                        {
                            sp.TriggerEffect(this);
                            triggered.Add(neighborPos);
                        }
                        else if (sp != null && sp.name == "UnicornHorn")
                        {
                            sp.TriggerEffect2(this);
                            triggered.Add(neighborPos);
                        }
                    }
                }
            }
        }
    }


    public Vector2Int WorldToBoardPos(Vector3 worldPos)
    {
        int x = Mathf.RoundToInt(worldPos.x + 3f);
        int y = Mathf.RoundToInt(worldPos.y + 1f);
        return new Vector2Int(x, y);
    }

    public Vector3 BoardToWorldPos(Vector2Int boardPos)
    {
        //float worldX = boardPos.x - 3f + 1f;
        float worldX = boardPos.x-4f+ 1f;

        //float worldY = boardPos.y - 1f + 1.15f;
        float worldY = boardPos.y-2f+ 1.15f;

        return new Vector3(worldX, worldY, 0f);
    }

    public GameObject GetBlock(int x, int y) => board[x, y];
    public void SetBlock(int x, int y, GameObject block) => board[x, y] = block;
    public bool IsInsideBoard(int x, int y) => x >= 0 && x < width && y >= 0 && y < height;
    Vector2Int? FindRandomAvailablePosition()
    {
        List<Vector2Int> available = new List<Vector2Int>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (board[x, y] == null)
                    available.Add(new Vector2Int(x, y));
            }
        }

        if (available.Count == 0)
            return null;

        return available[Random.Range(0, available.Count)];
    }
    public void DestroyBlock(int x, int y)
    {
        GameObject block = GetBlock(x, y);
        if (block != null)
        {
            SetBlock(x, y, null);
            Destroy(block);
            scoreMgr.AddScore();  //  이 한 줄로 점수 일관 처리
        }
    }

}
