using UnityEngine;
using System.Collections.Generic;

namespace IMG.Grid.Pathfinding
{
    public class Pathfinder : MonoBehaviour
    {
        //  _________________________________________________   ������ ��������� ������ ����:

        //  _________________________________________________   ����������� �������� ��������� ����:
        
        private const int STRAIGHT_MOVE_COST = 10;
        private const int DIAGONAL_MOVE_COST = 14;

        //  _________________________________________________   ����������� ������ ���� �� ���������:

        [Header("���������� ����������� ��� ������ ����:")]
        [Tooltip("������������ ������ ��������� ��� ������ ����.")]
        [SerializeField] private bool _canMoveDiagonaly = false;

        //  _________________________________________________   ����� �������� ������:

        private SceneGrid _grid;

        //  _________________________________________________   ����������� ������ ���������:

        private List<PathNode> _openList;
        private HashSet<PathNode> _closedSet;

        private PathNode _startNode;
        private PathNode _targetNode;

        //  __________________________________________________  �������:
        
        [Space(30f)]
        [Tooltip("��������� ���������� � ������� ���������� ��������� ������ ����.")]
        public bool ShowDebugMessages = false;



        #region �������������

        //  ##################################################  ������������� �����

        private void Awake()
        {
            _grid = FindObjectOfType<SceneGrid>();
        }

        //  __________________________________________________  �������� ������� �����:

        private void Start()
        {
            InstantiatePathNode();
        }

        private void InstantiatePathNode()
        {
            for (int x = 0; x < _grid.Data.GetLength(0); x++)
            {
                for (int y = 0; y < _grid.Data.GetLength(1); y++)
                {
                    PathNode node = new PathNode();
                    node.GCost = 0;
                    node.HCost = int.MaxValue;
                    Vector3 worldPosition = _grid.Data[x, y].transform.position;
                    worldPosition = new Vector3(worldPosition.x, 1.9f, worldPosition.z);
                    node.SetWorldPosition(worldPosition);
                    node.SetParentCell(_grid.CellsComponent[x, y]);
                    node.SetWalkable(!node.ParentCell.IsObstacle);
                    node.SetParentNode(null);

                    _grid.CellsComponent[x, y].SetPathNode(node);
                }
            }

            Vector2Int[] neigborGridOffset;

            if (_canMoveDiagonaly == true)
            {
                neigborGridOffset = new Vector2Int[8]
            {
                new Vector2Int(0, 1), // �����
                new Vector2Int(0, -1), // ���� 
                new Vector2Int(-1, 0), // �����
                new Vector2Int(1, 0), // ������
                new Vector2Int(-1, 1), // ����� - �����
                new Vector2Int(1, 1), // ����� - ������
                new Vector2Int(-1, -1), // ���� - �����
                new Vector2Int(1, -1) // ���� - ������
            };
            }
            else
            {
                neigborGridOffset = new Vector2Int[4]
                {
                new Vector2Int(0, 1), // �����
                new Vector2Int(0, -1), // ���� 
                new Vector2Int(-1, 0), // �����
                new Vector2Int(1, 0), // ������
                };
            }



            List<PathNode> neighboursNodeSet;

            for (int x = 0; x < _grid.CellsComponent.GetLength(0); x++)
            {
                for (int y = 0; y < _grid.CellsComponent.GetLength(1); y++)
                {
                    PathNode currentNode = _grid.CellsComponent[x, y].CellPathNode;
                    neighboursNodeSet = new List<PathNode>();

                    for (int i = 0; i < neigborGridOffset.Length; i++)
                    {
                        Vector2Int checkedCell = new Vector2Int(x + neigborGridOffset[i].x, y + neigborGridOffset[i].y);

                        if (_grid.IsInsideGrid(checkedCell.x, checkedCell.y) == false)
                        {
                            continue;
                        }

                        PathNode neighbour = _grid.CellsComponent[checkedCell.x, checkedCell.y].CellPathNode;

                        if (neighbour.Walkable == false)
                        {
                            continue;
                        }

                        neighboursNodeSet.Add(neighbour);
                    }

                    currentNode.SetNeighbours(neighboursNodeSet.ToArray());
                    _grid.CellsComponent[x, y].SetPathNode(currentNode);
                }
            }
        }

        #endregion

        #region ����� ����

        //  __________________________________________________  �������� ������ ���� A*(modified):

        public List<Vector3> FindPath(Vector3 startPosition, Vector3 targetPosition)
        {
            _openList = new List<PathNode>();
            _closedSet = new HashSet<PathNode>();

            if (_grid.IsInsideGrid(startPosition) == false)
            {
                ShowDebugMessage("��������� ������� �� ��������� �����!");

                return null;
            }

            if (_grid.IsInsideGrid(targetPosition) == false)
            {
                ShowDebugMessage("�������� ������� �� ��������� �����!");

                return null;
            }

            _startNode = _grid.GetGridCell(startPosition).CellPathNode;
            _targetNode = _grid.GetGridCell(targetPosition).CellPathNode;

            _startNode.GCost = 0;
            _startNode.HCost = CalculateDistance(_startNode, _targetNode);
            _startNode.SetParentNode(null);

            _openList.Add(_startNode);

            while (_openList.Count > 0)
            {
                PathNode currentNode = _openList[0];

                for (int i = 1; i < _openList.Count; i++)
                {
                    if (_openList[i].FCost < currentNode.FCost)
                    {
                        currentNode = _openList[i];
                    }

                    if (_openList[i].FCost == currentNode.FCost && _openList[i].HCost < currentNode.HCost)
                    {
                        currentNode = _openList[i];
                    }
                }

                _openList.Remove(currentNode);
                _closedSet.Add(currentNode);

                if (currentNode == _targetNode)
                {
                    ShowDebugMessage("���� ������!");

                    return CalculatePath(_startNode, currentNode);
                }

                foreach (PathNode neighbour in currentNode.NeighboursNode)
                {
                    if (_closedSet.Contains(neighbour) == true)
                    {
                        continue;
                    }

                    if (neighbour.Walkable == false)
                    {
                        continue;
                    }

                    int newCostToNeighbour = currentNode.GCost + CalculateDistance(currentNode, neighbour);

                    if (newCostToNeighbour < neighbour.GCost || _openList.Contains(neighbour) == false)
                    {
                        neighbour.GCost = newCostToNeighbour;
                        neighbour.HCost = CalculateDistance(neighbour, _targetNode);
                        neighbour.SetParentNode(currentNode);

                        if (_openList.Contains(neighbour) == false)
                        {
                            _openList.Add(neighbour);
                        }
                    }
                }

            }

            ShowDebugMessage("���� �� ������!");

            return null;
        }

        //  __________________________________________________  ���������� ���� �� ����:

        private List<Vector3> CalculatePath(PathNode startNode, PathNode endNode)
        {
            List<Vector3> path = new List<Vector3>();
            PathNode currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode.WorldPosition);
                currentNode = currentNode.ParentNode;
            }

            path.Reverse();
            return path;
        }

        //  __________________________________________________  ��������� �������������� ��������� ����� ������:

        private int CalculateDistance(PathNode nodeA, PathNode nodeB)
        {
            int xDistance = Mathf.RoundToInt(Mathf.Abs(nodeA.WorldPosition.x - nodeB.WorldPosition.x));
            int yDistance = Mathf.RoundToInt(Mathf.Abs(nodeA.WorldPosition.y - nodeB.WorldPosition.y));
            int remainder = Mathf.Abs(xDistance - yDistance);

            return DIAGONAL_MOVE_COST * Mathf.Min(xDistance, yDistance) + STRAIGHT_MOVE_COST * remainder;
        }

        #endregion

        #region �������

        //  ##################################################  �������

        private void ShowDebugMessage(string message)
        {
            if (ShowDebugMessages == true)
            {
                Debug.Log(message);
            }
        }

        public void OnDrawGizmosSelected()
        {
            if (_startNode != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(_startNode.WorldPosition, new Vector3(2f, 2f, 2f));
            }

            if (_targetNode != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(_targetNode.WorldPosition, new Vector3(2f, 2f, 2f));
            }

            if (_openList != null && _openList.Count > 0)
            {
                Gizmos.color = Color.yellow;

                for (int i = 0; i < _openList.Count; i++)
                {
                    Gizmos.DrawSphere(_openList[i].WorldPosition, 1.5f);
                }
            }
        }

        #endregion
    }
}