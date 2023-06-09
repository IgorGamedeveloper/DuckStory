
using UnityEngine;

namespace IMG.Grid
{
    public class PathNode
    {
        public GridCell ParentCell { get; private set; }

        public int GCost;
        public int HCost;
        public int FCost { get { return GCost + HCost; } }

        public bool Walkable { get; private set; }

        public Vector3 WorldPosition { get; private set; }

        public PathNode ParentNode { get; private set; }

        public PathNode[] NeighboursNode { get; private set; }



        public void SetParentCell(GridCell cell)
        {
            ParentCell = cell;
        }

        public void SetWalkable(bool isWalkable)
        {
            Walkable = isWalkable;
        }

        public void SetWorldPosition(Vector3 worldPosition)
        {
            WorldPosition = new Vector3(worldPosition.x, 1.9f, worldPosition.z);
        }

        public void SetParentNode(PathNode parentNode)
        {
            ParentNode = parentNode;
        }

        public void SetNeighbours(PathNode[] neighbours)
        {
            NeighboursNode = neighbours;
        }
    }
}
