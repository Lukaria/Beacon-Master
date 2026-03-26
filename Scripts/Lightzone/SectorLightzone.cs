using UnityEngine;
using System.Collections.Generic;
using Input;
using Lightzone.Dto;
using Lightzone.Interfaces;
using Zenject;


namespace Lightzone
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
    public class SectorLightzone : LightzoneBase
    {
        [Range(0f, 360f)] public float angle = 45f;
        [Range(0f, 2f)] public float speedScaler = 2f;
        [Range(12, 128)] public int radialSegments = 32;

        [Header("Physics Settings (Invisible 3D)")] [Range(0.1f, 10f)]
        public float colliderHeight = 2f;

        [Range(0f, 2f)] public float verticalOffset = -1f;
        
        private MeshFilter _meshFilter;
        private MeshCollider _meshCollider;

        private Mesh _visualMesh;
        private Mesh _physicsMesh;

        private bool _isDirty = true;

        private readonly List<Vector3> _verts = new();
        private readonly List<int> _tris = new();
        private readonly List<Vector2> _uvs = new();
        
        private InputManager _inputManager;

        public override ILightzoneThreshold Threshold { get; } = null;
        
        [Inject]
        public void Construct(InputManager inputManager)
        {
            _inputManager = inputManager;
        }

        private void OnEnable()
        {
            _meshFilter = GetComponent<MeshFilter>();
            _meshCollider = GetComponent<MeshCollider>();

            if (_visualMesh == null)
            {
                _visualMesh = new Mesh { name = "Visual_Flat" };
                _visualMesh.MarkDynamic();
            }

            _meshFilter.sharedMesh = _visualMesh;

            if (_physicsMesh == null)
            {
                _physicsMesh = new Mesh { name = "Physics_Tall" };
                _physicsMesh.MarkDynamic();
            }
            _isDirty = true;
        }

        public override void Initialize(Vector3 center, LightzoneDto dto)
        {
            Stats = dto;
            _isDirty = true;
        }

        public override void PerformUpdate(float dt)
        {
            if (_isDirty)
            {
                RebuildAll();
                _isDirty = false;
            }

            if (_inputManager.IsPressed()) return;
            
            transform.Rotate(Vector3.up, Stats.Speed * dt * speedScaler);
        }


        private void RebuildAll()
        {
            BuildVisualMesh();
            BuildPhysicsMesh();
        }

        private void BuildVisualMesh()
        {
            _verts.Clear();
            _tris.Clear();
            _uvs.Clear();

            int segs = Mathf.Max(2, Mathf.CeilToInt(radialSegments * (angle / 360f)));
            float angleStep = angle / segs;
            float startAngle = -angle / 2f;

            for (int i = 0; i <= segs; i++)
            {
                float rad = Mathf.Deg2Rad * (startAngle + (angleStep * i));
                float s = Mathf.Sin(rad);
                float c = Mathf.Cos(rad);

                _verts.Add(new Vector3(s * Stats.MaxAngle, 0f, c * Stats.MaxAngle));
                _verts.Add(new Vector3(s * Stats.MinAngle, 0f, c * Stats.MinAngle));

                float t = (float)i / segs;
                _uvs.Add(new Vector2(t, 1));
                _uvs.Add(new Vector2(t, 0));
            }

            for (int i = 0; i < segs; i++)
            {
                int root = i * 2;
                _tris.Add(root);
                _tris.Add(root + 1);
                _tris.Add(root + 2);

                _tris.Add(root + 1);
                _tris.Add(root + 3);
                _tris.Add(root + 2);
            }

            _visualMesh.Clear();
            _visualMesh.SetVertices(_verts);
            _visualMesh.SetTriangles(_tris, 0);
            _visualMesh.SetUVs(0, _uvs);
            _visualMesh.RecalculateNormals();
            _visualMesh.RecalculateBounds();
        }

        private void BuildPhysicsMesh()
        {
            _verts.Clear();
            _tris.Clear();
            
            int segs = Mathf.Max(2, Mathf.CeilToInt(radialSegments * (angle / 360f)));
            float angleStep = angle / segs;
            float startAngle = -angle / 2f;

            float yBottom = verticalOffset;
            float yTop = verticalOffset + colliderHeight;

            int numVertsPerRing = segs + 1;

            for (int i = 0; i < numVertsPerRing; i++)
            {
                float rad = Mathf.Deg2Rad * (startAngle + (angleStep * i));
                float s = Mathf.Sin(rad);
                float c = Mathf.Cos(rad);

                _verts.Add(new Vector3(s * Stats.MaxAngle, yTop, c * Stats.MaxAngle)); // 0
                _verts.Add(new Vector3(s * Stats.MinAngle, yTop, c * Stats.MinAngle)); // 1
                
                _verts.Add(new Vector3(s * Stats.MaxAngle, yBottom, c * Stats.MaxAngle)); // 2
                _verts.Add(new Vector3(s * Stats.MinAngle, yBottom, c * Stats.MinAngle)); // 3
            }

            for (int i = 0; i < segs; i++)
            {
                int baseIdx = i * 4;
                int nextIdx = (i + 1) * 4;

                int to = baseIdx, ti = baseIdx + 1, bo = baseIdx + 2, bi = baseIdx + 3;
                
                int nto = nextIdx, nti = nextIdx + 1, nbo = nextIdx + 2, nbi = nextIdx + 3;


                // Top
                _tris.Add(to);
                _tris.Add(nti);
                _tris.Add(ti);
                _tris.Add(ti);
                _tris.Add(nti);
                _tris.Add(nto);

                // Bottom
                _tris.Add(bo);
                _tris.Add(bi);
                _tris.Add(nbo);
                _tris.Add(bi);
                _tris.Add(nbi);
                _tris.Add(nbo);

                // Outer Wall
                _tris.Add(to);
                _tris.Add(nto);
                _tris.Add(bo);
                _tris.Add(bo);
                _tris.Add(nto);
                _tris.Add(nbo);

                // Inner Wall
                _tris.Add(ti);
                _tris.Add(bi);
                _tris.Add(nti);
                _tris.Add(bi);
                _tris.Add(nbi);
                _tris.Add(nti);
            }

            _tris.Add(0);
            _tris.Add(1);
            _tris.Add(2);
            _tris.Add(2);
            _tris.Add(1);
            _tris.Add(3);

            int e = segs * 4;
            _tris.Add(e);
            _tris.Add(e + 2);
            _tris.Add(e + 1);
            _tris.Add(e + 2);
            _tris.Add(e + 3);
            _tris.Add(e + 1);

            _physicsMesh.Clear();
            _physicsMesh.SetVertices(_verts);
            _physicsMesh.SetTriangles(_tris, 0);
            
            _physicsMesh.RecalculateBounds();

            if (_meshCollider == null) return;
            
            _meshCollider.cookingOptions = MeshColliderCookingOptions.None;
            _meshCollider.sharedMesh = null;
            _meshCollider.sharedMesh = _physicsMesh;
        }
    }
}