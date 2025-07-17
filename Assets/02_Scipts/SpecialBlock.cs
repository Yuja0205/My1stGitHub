using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBlock : MonoBehaviour
{
    public void TriggerEffect(BoardManager board)
    {
        Vector2Int pos = board.WorldToBoardPos(transform.position);

        // 좌우 제거
        for (int x = 0; x < board.width; x++)
        {
            
            board.DestroyBlock(x, pos.y);
        }

        // 상하 제거
        for (int y = 0; y < board.height; y++)
        {
            
            board.DestroyBlock(pos.x, y);
        }

        board.DestroyBlock(pos.x, pos.y); // 자기 자신
    }
    public void TriggerEffect2(BoardManager board)
    {
        Vector2Int pos = board.WorldToBoardPos(transform.position);
        int[] dx = { 1, -1, -1, 1 };
        int[] dy = { 1, 1, -1, -1 };

        // 대각선 제거
        for (int i = 0; i < 4; i++)
        {
            int x = pos.x + dx[i];
            int y = pos.y + dy[i];

            while (board.IsInsideBoard(x, y))
            {
                board.DestroyBlock(x, y);
                x += dx[i];
                y += dy[i];
            }
        }


        board.DestroyBlock(pos.x, pos.y); // 자기 자신
    }
}
