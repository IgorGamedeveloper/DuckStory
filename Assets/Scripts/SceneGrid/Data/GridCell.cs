using UnityEngine;

namespace IMG.Grid
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider))]
    public class GridCell : MonoBehaviour
    {
        //  _________________________________________________________   ���������� ������:

        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        private BoxCollider _boxCollider;

        //  _________________________________________________________   ������ ������ � ����������:

        [Header("������ ����� � �����������:")]
        [Tooltip("������ ������� �����.")]
        [SerializeField] private GameObject[] _cellsModels;
        [Space(10f)]
        [Tooltip("������ ������� �����������.")]
        [SerializeField] private GameObject[] _obstaclesModels;

        //  _________________________________________________________   ������� ��������� ������ � �����������:

        [Space(25f)]
        [Header("��������� ���������:")]
        [Tooltip("��������� ���������� ������ ��������� ��� ������.")]
        [SerializeField] private Vector3 _cellColliderCenter = Vector3.zero;
        [Space(10f)]
        [Tooltip("������ ��������� ������.")]
        [SerializeField] private Vector3 _cellColliderSize = new Vector3(6.4f, 0.64f, 6.4f);

        [Space(20f)]
        [Tooltip("��������� ���������� ������ ��������� ��� �����������.")]
        [SerializeField] private Vector3 _obstacleColliderCenter = new Vector3(0f, 1.28f, 0f);
        [Space(10f)]
        [Tooltip("������ ��������� �����������.")]
        [SerializeField] private Vector3 _obstacleColliderSize = new Vector3(6.4f, 3.2f, 6.4f);

        //  _________________________________________________________   ������������ ������:

        public bool IsObstacle { get; private set; }

        //  _________________________________________________________   ���� ������ ���� ������

        public PathNode CellPathNode { get; private set; }



        #region ������������� ������

        //  #########################################################   ������������� 

        private void Awake()
        {
            InitializeComponent();
        }

        //  ________________________________________________________    ������������� �����������:

        private void InitializeComponent()
        {
            _meshFilter = GetComponent<MeshFilter>();
            _meshRenderer = GetComponent<MeshRenderer>();
            _boxCollider = GetComponent<BoxCollider>();
        }

        #endregion

        #region �������� ��������� ������

        //  #########################################################   ����� ��������� ������

        //  _________________________________________________________   ����� ������ ������:

        public void SelectRandomCellModel()
        {
            if (_cellsModels == null || _cellsModels.Length < 1)
            {
                Debug.LogError($"� ������ ���������� ������ ������, �� ���������� ���������� ������ ��� {gameObject}");
                return;
            }

            if (_meshFilter == null)
            {
                _meshFilter = GetComponent<MeshFilter>();
            }

            if (_meshRenderer == null)
            {
                _meshRenderer = GetComponent<MeshRenderer>();
            }

            int randomIndex = Random.Range(0, _cellsModels.Length);

            _meshFilter.mesh = _cellsModels[randomIndex].GetComponent<MeshFilter>().sharedMesh;
            _meshRenderer.sharedMaterials = _cellsModels[randomIndex].GetComponent<MeshRenderer>().sharedMaterials;

            SetObstacle(false);
        }

        public void SelectRandomObstacleModel()
        {
            if (_obstaclesModels == null || _obstaclesModels.Length < 1)
            {
                Debug.LogError($"� ������ ���������� ������ �����������, �� ���������� ���������� ������ ��� {gameObject}");
                return;
            }

            if (_meshFilter == null)
            {
                _meshFilter = GetComponent<MeshFilter>();
            }

            if (_meshRenderer == null)
            {
                _meshRenderer = GetComponent<MeshRenderer>();
            }

            int randomIndex = Random.Range(0, _obstaclesModels.Length);

            _meshFilter.mesh = _obstaclesModels[randomIndex].GetComponent<MeshFilter>().sharedMesh;
            _meshRenderer.sharedMaterials = _obstaclesModels[randomIndex].GetComponent<MeshRenderer>().sharedMaterials;

            SetObstacle(true);
        }

        //  _________________________________________________________   ��������� ������������ ������:

        public void SetObstacle(bool isObstacle)
        {
            IsObstacle = isObstacle;
            ChangeBoxCollider(isObstacle);
        }

        //  _________________________________________________________   ����� ������� ���������:

        private void ChangeBoxCollider(bool isObstacle)
        {
            if (_boxCollider == null)
            {
                _boxCollider = GetComponent<BoxCollider>();
            }

            if (isObstacle == true)
            {
                _boxCollider.center = _obstacleColliderCenter;
                _boxCollider.size = _obstacleColliderSize;
            }
            else
            {
                _boxCollider.center = _cellColliderCenter;
                _boxCollider.size = _cellColliderSize;
            }
        }

        //  _________________________________________________________   ��������� ���� ������ ����:

        public void SetPathNode(PathNode pathNode)
        {
            CellPathNode = pathNode;
        }
    }

    #endregion
}
