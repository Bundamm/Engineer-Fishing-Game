using System.Linq;
using UnityEngine;

public class Water : MonoBehaviour
{
    
    /// <summary>
    /// Based on the following tutorial: https://youtu.be/TbGEKpdsmCI?si=yFig8GSb44o3kzDf
    /// </summary>
    
    
    #region Mesh Variables
    [Header("Mesh")]
    [SerializeField]
    private int amountOfVerticesOnTop = 100;
    [SerializeField]
    private float meshHeight = 5f;
    [SerializeField]
    private float meshWidth = 50f;
    private Mesh _waterMesh;
    private Vector3[] _bottomVertices;
    private Vector3[] _topVertices;
    private Vector3[] _vertices;
    private Vector2[] _uvs;
    private int[] _triangles;
    private WaterPoint[]  _waterPoints;
    private float _realMeshWidth;
    #endregion
    #region Collisions with surface
    [Header("Collision with surface")]
    private EdgeCollider2D _edgeCollider;
    [SerializeField, Range(1f, 10f)] 
    private float playerCollisionRadiusMult = 4.15f;
    public float forceMultiplier = 0.2f;
    public float maxForce = 5f;
    #endregion
     
    #region Springs
    [Header("Spring")] 
    [SerializeField]
    private float springConst = 1.4f;
    [SerializeField]
    private float damping = 1.1f;
    [SerializeField]
    private int propagationIterations = 3;
    [SerializeField] 
    private float spread = 6.5f;
    #endregion
    
    #region
    [SerializeField]
    private Camera waterCamera;
    #endregion

    void Awake()
    {
        _edgeCollider = GetComponent<EdgeCollider2D>();
        CreateWaterMesh();
    }

    void FixedUpdate()
    {
        SimulatePoints();
        WavePropagation();
        _waterMesh.vertices = _vertices;
    }
    
    private class WaterPoint
    {
        public float Velocity;
        public float Pos;
        public float TargetPos;
    }
    
    #region Water Mesh functions
    private void CreateWaterMesh()
    {
        _waterMesh = new Mesh
        {
            name = "Water"
        };
        _bottomVertices = new Vector3[amountOfVerticesOnTop];
        _topVertices = new Vector3[amountOfVerticesOnTop];
        _vertices = new Vector3[amountOfVerticesOnTop * 2];
        _uvs = new Vector2[amountOfVerticesOnTop * 2];
        _triangles = new int[(amountOfVerticesOnTop - 1) * 6];
        _realMeshWidth = meshWidth / (amountOfVerticesOnTop - 1);
        for (int i = 0; i < amountOfVerticesOnTop; i++)
        {
            _bottomVertices[i] = new Vector3(i * _realMeshWidth, 0);
            _topVertices[i] = new Vector3(i * _realMeshWidth, meshHeight);
            _vertices[2*i] = _bottomVertices[i];
            _vertices[2 * i + 1] = _topVertices[i];
            
            float u = (float)i / (amountOfVerticesOnTop - 1);
            
            _uvs[2 * i] = new Vector2(u, 0f);
            _uvs[2*i + 1] = new Vector2(u, 1f);
        }
        
        for (int i = 0; i < amountOfVerticesOnTop - 1; i++)
        {
            int bl = 2*i;     
            int tl = 2 * i + 1; 
            int tr = 2*i + 3; 
            int br = 2*i + 2;

            _triangles[i * 6 + 0] = bl;
            _triangles[i * 6 + 1] = tl;
            _triangles[i * 6 + 2] = br;
            _triangles[i * 6 + 3] = tl;
            _triangles[i * 6 + 4] = tr;
            _triangles[i * 6 + 5] = br;
        }
        
                     
        _waterMesh.vertices = _vertices;
        _waterMesh.uv = _uvs;
        _waterMesh.triangles = _triangles;
        GetComponent<MeshFilter>().mesh = _waterMesh;  
        CreateWaterPoints();
        UpdateColliderAndMesh();
        
    }
    
    

    

    private void UpdateColliderAndMesh()
    {
        
        Vector2[] edgeCollider =  new Vector2[_topVertices.Length];
        for (int i = 0; i < _topVertices.Length; i++)
        {
            edgeCollider[i] = new Vector2(_topVertices[i].x, _topVertices[i].y);
            _topVertices[i].y = _waterPoints[i].Pos;
            _vertices[i*2+1] =  _topVertices[i];

        }
        _edgeCollider.points = edgeCollider;
        _waterMesh.vertices = _vertices;
    }
    
    private void CreateWaterPoints()
    {
        _waterPoints = new WaterPoint[amountOfVerticesOnTop];
        for (int i = 0; i < _topVertices.Length; i++)
        {
            Vector3 pos = _topVertices[i];
            _waterPoints[i] = new WaterPoint
            {
                Pos = pos.y,
                TargetPos = pos.y
            };
        }
    }

    private void SetWaterCamera()
    {
        waterCamera.transform.position = _waterMesh.bounds.center;
        waterCamera.orthographicSize = _waterMesh.bounds.extents.magnitude;
    }
    #endregion


    #region Water Simulation
    private void SimulatePoints()
    {
        for (int i = 1; i < _waterPoints.Length - 1; i++)
        {
            float heightDif =  _waterPoints[i].Pos - _waterPoints[i].TargetPos;
            float acceleration = -springConst  * heightDif - damping * _waterPoints[i].Velocity;

            _waterPoints[i].Pos += _waterPoints[i].Velocity * Time.fixedDeltaTime;
            _waterPoints[i].Velocity += acceleration;
            _vertices[2*i+1].y =  _waterPoints[i].Pos;
        }
    }

    private void WavePropagation()
    {   
        for (int j = 0; j < propagationIterations; j++)
        {
            for (int i = 1; i < _waterPoints.Length - 1; i++)
            {
                if (i > 0)
                {
                    float leftDelta = spread * (_waterPoints[i].Pos - _waterPoints[i - 1].Pos);
                    _waterPoints[i - 1].Velocity += leftDelta;
                }
                if (i < _waterPoints.Length - 1)
                {
                    float rightDelta = spread * (_waterPoints[i].Pos - _waterPoints[i + 1].Pos);
                    _waterPoints[i + 1].Velocity += rightDelta;
                }
            }
        }
    }
    

    public void Splash(Collider2D collision, float force)
    {
        float radius = collision.bounds.extents.x * playerCollisionRadiusMult;
        Vector2 center = collision.transform.position;

        for (int i = 0; i < amountOfVerticesOnTop; i++)
        {
            Vector2 waterParticlePosition = transform.TransformPoint(_vertices[2 * i + 1]);

            if (IsPointInsideCircle(waterParticlePosition, center, radius))
            {
                _waterPoints[i].Velocity = force;
            }
        }
    }
    
    #endregion

    private bool IsPointInsideCircle(Vector2 point, Vector2 center, float radius)
    {
        float sqrDistance = (point - center).sqrMagnitude;
        return sqrDistance <= radius * radius;
    }

    public float GetMeshWidth()
    {
        return meshWidth;
    }

    public float GetMeshHeight()
    {
        return meshHeight;
    }

    public Vector2 GetMeshCenter()
    {
        return _waterMesh.bounds.center;
    }
}

