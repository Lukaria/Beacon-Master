using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Grid
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private Transform originPoint;
        [SerializeField] private float deltaX;
        [SerializeField] private float deltaY;
        [SerializeField, Range(0, 50)] private int pointsX;
        [SerializeField, Range(0, 50)] private int pointsY;

        private readonly List<Vector3> _points = new();

        private void Awake()
        {
            GenerateGrid();
        }
        private void OnValidate()
        {
            GenerateGrid();
        }

        private void GenerateGrid()
        {
            _points.Clear();
            for (var i = 0; i < pointsX; i++)
            {
                for (var j = 0; j < pointsY; j++)
                {
                    _points.Add(originPoint.position + new Vector3(deltaX * i, 0, deltaY * j));
                }
            } 
            Debug.Log("grid generated");
        }

        private void OnDrawGizmosSelected()
        {
            foreach (var point in _points)
            {
                Gizmos.color = Color.brown;
                Gizmos.DrawSphere(point, 0.1f);
            }
        }

        public ReadOnlyCollection<Vector3> GetPoints() => _points.AsReadOnly();

        public ReadOnlyCollection<Vector3> GetRandomPoints(int count)
        {
            var result = new List<Vector3>();
            for (var i = 0; i < count; ++i)
            {
                var index = Random.Range(0, _points.Count);
                result.Add(_points[index]);
            }

            return result.AsReadOnly();
        }
    }
}