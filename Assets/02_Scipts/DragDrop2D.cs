using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class DragDrop2D : MonoBehaviour
{
    public SoundManager soundMgr;
    
    public Transform leftBlock;
    public Transform middleBlock;
    public Transform rightBlock;


    public bool isHorizontal = true; // 프리팹에서 수동 지정

    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private BoardManager boardManager;
    
    

    private Vector3 spawnPosition;
    private float size = 0.7f;  // 하단블럭의 사이즈
    private Vector3 offset;
    private Collider2D collider2d;
    public string destinationTag = "DropArea";

    void Awake()
    {
        collider2d = GetComponent<Collider2D>();
    }

    void Start()
    {
        spawnManager = FindObjectOfType<SpawnManager>();
        boardManager = FindObjectOfType<BoardManager>();
        soundMgr = FindObjectOfType<SoundManager>();
    }

    public void SetSpawnPosition(Vector3 position)
    {
        spawnPosition = position;
    }

    //마우스 클릭 시, 드래그 시작
    //마우스 위치와 오브젝트 사이 거리 저장(위로 살짝 띄우기)
    public void OnMouseDown()
    {
        
        transform.localScale = new Vector3(1, 1, 1);
        offset = transform.position - MouseWorldPosition() +new Vector3(0f, 2f, 0f);
    }

    //드래그 중인 동안 마우스 따라 움직임
    void OnMouseDrag()
    {
        transform.position = MouseWorldPosition() + offset;
    }

    //블록 그룹 놓기 → 위치 체크 → 유효하면 등록, 아니면 원래 자리로 복귀
    void OnMouseUp()
    {
        collider2d.enabled = false;

        Vector2 blockCenter = transform.position;
        float snapRadius = 1.0f;

        Collider2D[] hits = Physics2D.OverlapCircleAll(blockCenter, snapRadius);
        Collider2D closest = null;
        float closestDist = Mathf.Infinity;

        foreach (var hit in hits)
        {
            if (hit.CompareTag(destinationTag))
            {
                float dist = Vector2.Distance(blockCenter, hit.transform.position);
                if (dist < closestDist)
                {
                    closest = hit;
                    closestDist = dist;
                }
            }
        }

        if (closest != null)
        {
            Vector3 offsetToMiddle = transform.position - middleBlock.position;
            Vector3 candidatePosition = closest.transform.position + offsetToMiddle;

            Transform[] blocks = { leftBlock, middleBlock, rightBlock };
            bool allBlocksOnDropArea = true;

            foreach (Transform block in blocks)
            {
                Vector3 relativeOffset = block.position - transform.position;
                Vector3 newWorldPos = candidatePosition + relativeOffset;

                // 보드 좌표로 변환
                Vector2Int boardPos = boardManager.WorldToBoardPos(newWorldPos);

                // 범위 체크
                if (!boardManager.IsInsideBoard(boardPos.x, boardPos.y))
                {
                    allBlocksOnDropArea = false;
                    break;
                }

                // 실제 보드 상에 블럭이 존재하는지
                GameObject targetBlock = boardManager.GetBlock(boardPos.x, boardPos.y);
                if (targetBlock != null)
                {
                    //  어떤 블럭이든 존재하면 못 놓는다 (특수블럭 포함)
                    allBlocksOnDropArea = false;
                    break;
                }

                // 물리적으로 DropArea에 있는지도 체크
                Collider2D hit = Physics2D.OverlapPoint(newWorldPos);
                if (hit == null || !hit.CompareTag(destinationTag))
                {
                    allBlocksOnDropArea = false;
                    break;
                }
            }

            if (allBlocksOnDropArea)
            {
                transform.position = candidatePosition + new Vector3(0, 0, -0.01f);

                // 개별 블럭 분리
                foreach (Transform block in blocks)
                {
                    block.SetParent(null);
                    block.gameObject.AddComponent<BoxCollider2D>(); //  충돌 가능하게



                    Vector2Int boardPos = boardManager.WorldToBoardPos(block.position);
                    Debug.Log($"드롭된 블럭 {block.name} 등록 위치: {boardPos}");
                    boardManager.SetBlock(boardPos.x, boardPos.y, block.gameObject);
                }
                boardManager.CheckMatches();    //매치확인.
                Destroy(gameObject); // 그룹 삭제
                gameObject.tag = "Untagged";
                if (spawnManager != null)
                    spawnManager.OnGroupDropped();

                Debug.Log("블럭 DropArea에 떨어짐");
                soundMgr.DropSound();
            }
            else
            {
                transform.localScale = new Vector3(size, size, size);
                transform.position = spawnPosition;
                Debug.Log("모든블럭이 DropArea에 있지않음.");
                soundMgr.MissDropSound();
            }
        }
        else
        {
            transform.localScale = new Vector3(size, size, size);
            transform.position = spawnPosition;
            Debug.Log("어따놓냐");
            soundMgr.MissDropSound();

        }

        collider2d.enabled = true;

        //  게임오버 검사
        //if (spawnManager != null && !spawnManager.CanAnyGroupBePlaced())
        //{
        //    Debug.Log("게임오버");
        //}
    }

    Vector3 MouseWorldPosition()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }

    // 현재 이 그룹이 어딘가에 드롭 가능한지 검사 (게임오버 판단용)
    public bool CanBePlacedOnBoard()
    {
        if (boardManager == null) boardManager = FindObjectOfType<BoardManager>();

        int width = 9;
        int height = 10;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                bool canPlace = true;

                if (isHorizontal)
                {
                    if (x > width - 3) continue;

                    for (int i = 0; i < 3; i++)
                    {
                        if (!boardManager.IsTileActive(x + i, y))
                        {
                            canPlace = false;
                            break;
                        }
                        if (boardManager.GetBlock(x + i, y) != null)
                        {
                            canPlace = false;
                            break;
                        }
                        GameObject block = boardManager.GetBlock(x + i, y);
                        if (block != null)
                        {
                            //  특수 블럭 포함되어 있으면 안됨
                            if (block.GetComponent<SpecialBlock>() != null)
                            {
                                canPlace = false;
                                break;
                            }

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
                        if (!boardManager.IsTileActive(x + i, y))
                        {
                            canPlace = false;
                            break;
                        }
                        if (boardManager.GetBlock(x + i, y) != null)
                        {
                            canPlace = false;
                            break;
                        }
                        GameObject block = boardManager.GetBlock(x, y + i);
                        if (block != null)
                        {
                            if (block.GetComponent<SpecialBlock>() != null)
                            {
                                canPlace = false;
                                break;
                            }

                            canPlace = false;
                            break;
                        }
                    }
                }

                if (canPlace)
                {
                    Debug.Log($"[{gameObject.name}] 놓을 수 있음: {(isHorizontal ? "가로" : "세로")} ({x},{y})");
                    return true;
                }
            }
        }

        Debug.Log($"[{gameObject.name}] 놓을 수 없음");
        return false;
    }

}
