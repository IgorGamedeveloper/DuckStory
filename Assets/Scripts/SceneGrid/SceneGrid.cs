using System.Collections.Generic;
using UnityEngine;

namespace IMG.Grid
{
    public class SceneGrid : MonoBehaviour, ISerializationCallbackReceiver
    {
        //  ________________________________________________________    ��������� �����:
        
        public static SceneGrid Instance { get; private set; }

        [Header("���� � ����:")]
        [Tooltip("��� ����� � �����.")]
        [SerializeField] private string _gridTag = "Tag_Grid";
        [Space(5f)]
        [Tooltip("���� ����� � �����.")]
        [SerializeField] private string _gridLayer = "Layer_Grid";

        [Space(15f)]
        [Header("��������� �����:")]
        [Tooltip("������ �����.")]
        [SerializeField] private int _width = 10;
        [Space(5f)]
        [Tooltip("������ �����.")]
        [SerializeField] private int _height = 10;
        [Space(5f)]
        [Tooltip("������ ������ �����.")]
        [SerializeField] private float _cellSize = 6.4f;
        [Space(10f)]
        [Tooltip("��������� ����� ������� �����.")]
        [SerializeField] private Vector3 _originPosition = Vector3.zero;

        //  ________________________________________________________    ������ ������ �����:
        [Space(20f)]
        [Header("�������:")]
        [Tooltip("������ ������.")]
        [SerializeField] private GameObject _cellPrefab;

        //  ________________________________________________________    ������������� ������ �����:

        public GameObject[,] Data { get; private set; }

        [HideInInspector] public List<SerializebleMultidimensionalArray<GameObject>> _serializebleData;

        //  ________________________________________________________    ��� ����������� ���������� �������� �������� DATA

        public GridCell[,] CellsComponent { get; private set; }

        //  ________________________________________________________    ���������� ��������� ����� � ���������:

        [Space(30f)]
        [Header("�������!")]
        public bool DrawGizmos = true;
        [Space(10f)]
        public Vector3 GizmosCellSize = new Vector3(6.4f, 0.64f, 6.4f);



        #region �������� � ������������� �����

        // #########################################################    ������������� �����:

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            InitializeCellsComponent();
        }

        //  ________________________________________________________    ������������� �����������:

        private void InitializeCellsComponent()
        {
            CellsComponent = new GridCell[Data.GetLength(0), Data.GetLength(1)];

            for (int i = 0; i < CellsComponent.GetLength(0); i++)
            {
                for (int j = 0; j < CellsComponent.GetLength(1); j++)
                {
                    if (Data[i, j] != null && Data[i, j].TryGetComponent(out GridCell component) == true)
                    {
                        CellsComponent[i, j] = component;
                    }
                    else
                    {
                        Debug.LogError($"�� ������� �������� ��������� � ������ ����� {Data[i, j]}!");
                    }
                }
            }
        }

        //  ________________________________________________________    �������� �����:

        public void InstantiateGrid()
        {
            DestroyGrid();

            Data = new GameObject[_width, _height];

            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    GameObject cell = Instantiate(_cellPrefab, gameObject.transform);
                    cell.GetComponent<GridCell>().SelectRandomCellModel();
                    cell.transform.position = GetWorldPosition(i, j);
                    cell.tag = _gridTag;
                    cell.layer = LayerMask.NameToLayer(_gridLayer);
                    Data[i, j] = cell;
                }
            }

            InstantiateSideboard();
        }

        //  ________________________________________________________    �������� ������ �������:

        private void InstantiateSideboard()
        {
            int min = -1;
            int maxX = _width + 1;

            for (int i = min; i < maxX; i++)
            {
                InstanceSideboard(i, min);
            }

            for (int i = min; i < maxX; i++)
            {
                InstanceSideboard(i, _height);
            }

            for (int i = 0; i < _width; i++)
            {
                InstanceSideboard(min, i);
            }

            for (int i = 0; i < _width; i++)
            {
                InstanceSideboard(_width, i);
            }
        }

        private void InstanceSideboard(int xCell, int yCell)
        {
            GameObject sideboard = Instantiate(_cellPrefab, gameObject.transform);
            sideboard.transform.position = GetWorldPosition(xCell, yCell);
            sideboard.GetComponent<GridCell>().SelectRandomObstacleModel();
            sideboard.tag = _gridTag;
            sideboard.layer = LayerMask.NameToLayer(_gridLayer);
        }

        //  ________________________________________________________    ����������� �����:

        public void DestroyGrid()
        {

            GameObject[] temporaryData = GameObject.FindGameObjectsWithTag(_gridTag);

            for (int i = 0; i < temporaryData.Length; i++)
            {
                DestroyImmediate(temporaryData[i]);
            }


            Data = null;
        }
        #endregion

        #region ���������������� �� ����� � ��������� ������ � �����

        //  ########################################################    ������ � ������:

        //  _________________________________________________________   ��������� ������ �����:

        public void GetXY(Vector3 worldPosition, out int xCell, out int yCell)
        {
            xCell = Mathf.RoundToInt((worldPosition.x - _originPosition.x) / _cellSize);

            yCell = Mathf.RoundToInt((worldPosition.z - _originPosition.z) / _cellSize);
        }

        //  ________________________________________________________    ��������� ����������� ������ �� �����:

        public GridCell GetGridCell(int xCell, int yCell)
        {
            return CellsComponent[xCell, yCell];
        }

        public GridCell GetGridCell(Vector3 worldPosition)
        {
            GetXY(worldPosition, out int xCell, out int yCell);
            return GetGridCell(xCell, yCell);
        }

        //  _________________________________________________________   ��������� ������� ��������� �����:

        public Vector3 GetWorldPosition(int xCell, int yCell)
        {
            Vector3 worldPosition = Vector3.zero;

            worldPosition.x = xCell * _cellSize + _originPosition.x;

            worldPosition.z = yCell * _cellSize + _originPosition.z;

            return worldPosition;
        }


        //  _________________________________________________________   �������� �� ���������� � ��������� �����:

        public bool IsInsideGrid(int xCell, int yCell)
        {
            if (xCell >= 0 && xCell < _width && yCell >= 0 && yCell < _height)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsInsideGrid(Vector3 worldPosition)
        {
            GetXY(worldPosition, out int xCell, out int yCell);
            return IsInsideGrid(xCell, yCell);
        }

        #endregion

        #region ������������ ����������� ������� ������ �����

        //  ________________________________________________________    ������������:

        public void OnBeforeSerialize()
        {
            if (Data != null)
            {
                _serializebleData = new List<SerializebleMultidimensionalArray<GameObject>>();

                for (int i = 0; i < Data.GetLength(0); i++)
                {
                    for (int j = 0; j < Data.GetLength(1); j++)
                    {
                        _serializebleData.Add(new SerializebleMultidimensionalArray<GameObject>(i, j, Data[i, j]));
                    }
                }
            }
        }

        //  __________________________________________________________  ��������������:

        public void OnAfterDeserialize()
        {
            Data = new GameObject[_width, _height];

            foreach (var currentElement in _serializebleData)
            {
                Data[currentElement.Length_0, currentElement.Length_1] = currentElement.Element;
            }
        }

        #endregion

        public void OnDrawGizmosSelected()
        {
            if (DrawGizmos == true)
            {
                if (_width > 0 && _height > 0 && _cellSize > 0)
                {
                    Gizmos.color = Color.cyan;

                    for (int i = 0; i < _width; i++)
                    {
                        for (int j = 0; j < _height; j++)
                        {
                            Gizmos.DrawWireCube(GetWorldPosition(i, j), GizmosCellSize);
                        }
                    }
                }
            }
        }
    }
}
