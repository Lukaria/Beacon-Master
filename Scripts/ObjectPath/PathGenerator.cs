using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Grid;
using Unity.Mathematics;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace ObjectPath
{
    public class PathGenerator
    {
        private ReadOnlyCollection<Vector3> _endPoints;
        
        private Vector3 _uniqueEndPoint;
        private float _uniqueEndPointChance = 1.5f;
        
        private GridManager _gridManager;
        
        [Inject]
        public void Construct(GridManager gridManager)
        {
            _gridManager = gridManager;
        }
        
        public void SetEndPoints(ReadOnlyCollection<Vector3> points) => _endPoints = points;
        public void SetUniqueEndPoint(Vector3 endPoint) => _uniqueEndPoint =  endPoint;
        public void SetUniqueEndPointChance(float chance) => _uniqueEndPointChance = chance;
        


        public List<Vector3> GetRandomPoints(int pointsNumber)
        {
            var points = _gridManager.GetPoints();
            var path = new List<Vector3>();
            for (var i = 0; i < pointsNumber; i++)
            {
                path.Add(points[Random.Range(0, points.Count)]);
            }

            path.Add(GetEndPoint());
            return path;
        }

        private Vector3 GetEndPoint() => Random.Range(0f, 1f) > _uniqueEndPointChance
            ? _endPoints[Random.Range(0, _endPoints.Count)]
            :  _uniqueEndPoint;
    }
}