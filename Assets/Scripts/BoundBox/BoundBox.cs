using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DimBoxes
{
    [ExecuteInEditMode]
    public class BoundBox : MonoBehaviour
    {
        public enum BoundSource
        {
            meshes,
            boxCollider,
        }

        public BoundSource boundSource = BoundSource.meshes;

        [Header("Rendering methods")]

        protected Bounds bound;
        protected Vector3 boundOffset;
        [HideInInspector]
        public Bounds meshBound;
        [HideInInspector]
        public Vector3 meshBoundOffset;

        public Vector3[] corners = new Vector3[0];

        private Quaternion quat;

        public BoundBoxLine[] lineList;


        private Vector3 topFrontLeft;
        private Vector3 topFrontRight;
        private Vector3 topBackLeft;
        private Vector3 topBackRight;
        private Vector3 bottomFrontLeft;
        private Vector3 bottomFrontRight;
        private Vector3 bottomBackLeft;
        private Vector3 bottomBackRight;

        [HideInInspector]
        public Vector3 startingScale;
        
        public Vector3[] GetCorner()
        {
            return corners;
        }
        
        void Reset()
        {
            //CalculateBounds();
            AccurateBounds();
            Start();
        }


        void Start()
        {
            startingScale = transform.localScale;
            Init();
        }

        public void UpdateBounds()
        {
            CalculateBounds();
            SetPoints();
            SetLines();
        }

        public virtual void Init()
        {
            SetPoints();
            SetLines();
        }
        
        public void CalculateBounds()
        {
            quat = transform.rotation;//object axis AABB
            Vector3 locScale = transform.localScale;

            meshBound = new Bounds();
            MeshFilter[] meshes = GetComponentsInChildren<MeshFilter>();
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            transform.localScale = Vector3.one;

            for (int i = 0; i < meshes.Length; i++)
            {
                if (meshes[i].gameObject.layer == 7) continue;
                
                Mesh ms = meshes[i].sharedMesh;
                int vc = ms.vertexCount;
                for (int j = 0; j < vc; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        meshBound = new Bounds(meshes[i].transform.TransformPoint(ms.vertices[j]), Vector3.zero);
                    }
                    else
                    {
                        meshBound.Encapsulate(meshes[i].transform.TransformPoint(ms.vertices[j]));
                    }
                }
            }
            transform.rotation = quat;
            transform.localScale = locScale;
            meshBoundOffset = meshBound.center - transform.position;
        }

        void SetPoints()
        {

            if (boundSource == BoundSource.boxCollider)
            {
                BoxCollider bc = GetComponent<BoxCollider>();
                if (!bc)
                {
                    Debug.LogError("no BoxCollider - add BoxCollider to " + gameObject.name + " gameObject");
                    return;

                }
                bound = new Bounds(bc.center, bc.size);
                boundOffset = bc.center;
            }

            else
            {
                bound = meshBound;
                boundOffset = meshBoundOffset;
            }
            bound.size = new Vector3(bound.size.x * transform.localScale.x / startingScale.x, bound.size.y * transform.localScale.y / startingScale.y, bound.size.z * transform.localScale.z / startingScale.z);
            boundOffset = new Vector3(boundOffset.x * transform.localScale.x / startingScale.x, boundOffset.y * transform.localScale.y / startingScale.y, boundOffset.z * transform.localScale.z / startingScale.z);

            topFrontRight = boundOffset + Vector3.Scale(bound.extents, new Vector3(1, 1, 1));
            topFrontLeft = boundOffset + Vector3.Scale(bound.extents, new Vector3(-1, 1, 1));
            topBackLeft = boundOffset + Vector3.Scale(bound.extents, new Vector3(-1, 1, -1));
            topBackRight = boundOffset + Vector3.Scale(bound.extents, new Vector3(1, 1, -1));
            bottomFrontRight = boundOffset + Vector3.Scale(bound.extents, new Vector3(1, -1, 1));
            bottomFrontLeft = boundOffset + Vector3.Scale(bound.extents, new Vector3(-1, -1, 1));
            bottomBackLeft = boundOffset + Vector3.Scale(bound.extents, new Vector3(-1, -1, -1));
            bottomBackRight = boundOffset + Vector3.Scale(bound.extents, new Vector3(1, -1, -1));

            corners = new Vector3[] { topFrontRight, topFrontLeft, topBackLeft, topBackRight, bottomFrontRight, bottomFrontLeft, bottomBackLeft, bottomBackRight };
        }

        public virtual void SetLines()
        {
            Debug.Log("BB-lr");
            if (XRSelector.Instance.GetLineList() != null)
                lineList = XRSelector.Instance.GetLineList();
            XRSelector.Instance.transformByVertexHandler.Init();
        }

        void OnEnable()
        {
            Init();
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            if (EditorApplication.isPlaying) return;
            if (XRSelector.Instance &&XRSelector.Instance.boundBox && XRSelector.Instance.boundBox == this) enabled = true;
            else
            {
                enabled = false;
                return;
            }
            Init();
        }


#endif

        public void AccurateBounds()
        {

            MeshFilter[] meshes = GetComponentsInChildren<MeshFilter>();
#if UNITY_EDITOR
            if (meshes.Length == 0)
            {
                EditorUtility.DisplayDialog("Dimbox message", "The object contains no meshes!\n- please reassign", "Continue");
            }
#endif
            VertexData[] vertexData = new VertexData[meshes.Length];
            for (int i = 0; i < meshes.Length; i++)
            {
                Mesh ms = meshes[i].sharedMesh;
                vertexData[i] = new VertexData(ms.vertices, meshes[i].transform.localToWorldMatrix);
            }
            Vector3 v1 = transform.right;
            Vector3 v2 = transform.up;
            Vector3 v3 = transform.forward;

            meshBound = OrientedBounds.OBB(vertexData, v1, v2, v3);
            meshBoundOffset = transform.InverseTransformPoint(meshBound.center);

        }
    }
}