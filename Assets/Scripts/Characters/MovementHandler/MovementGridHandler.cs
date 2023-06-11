using UnityEngine;
using IMG.Grid;

public static class MovementGridHandler
{
    public static void UpdatePosition(SceneGrid grid, Vector3 notWallkablePosition)
    {
        PathNode notWallkableNode = grid.GetGridCell(notWallkablePosition).CellPathNode;

        if (grid != null)
        {
            if (notWallkableNode != null)
            {
                notWallkableNode.SetWalkable(false);
                grid.GetGridCell(notWallkablePosition).SetPathNode(notWallkableNode);
                grid.GetGridCell(notWallkablePosition).SetObstacle(true);
            }
        }
        else
        {
            Debug.LogError("Сетка уровня отсутствует!");
        }

    }

    public static void UpdatePosition(SceneGrid grid, Vector3 previousPosition, Vector3 currentPosition, Vector3 targetPosition)
    {
        PathNode previousNode = grid.GetGridCell(previousPosition).CellPathNode;
        PathNode currentNode = grid.GetGridCell(currentPosition).CellPathNode;
        PathNode targetNode = grid.GetGridCell(targetPosition).CellPathNode;

        if (grid != null)
        {
            if (previousNode != null)
            {
                previousNode.SetWalkable(true);
                grid.GetGridCell(previousPosition).SetPathNode(previousNode);
                grid.GetGridCell(previousPosition).SetObstacle(false);
            }

            if (currentNode != null)
            {
                currentNode.SetWalkable(false);
                grid.GetGridCell(currentPosition).SetPathNode(currentNode);
                grid.GetGridCell(currentPosition).SetObstacle(true);
            }

            if (targetNode != null)
            {
                targetNode.SetWalkable(false);
                grid.GetGridCell(targetPosition).SetPathNode(targetNode);
                grid.GetGridCell(targetPosition).SetObstacle(true);
            }
        }
        else
        {
            Debug.LogError("Сетка уровня отсутствует!");
        }
    }
}
