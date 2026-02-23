using Lightzone.Interfaces;
using Vector3 = UnityEngine.Vector3;

namespace Lightzone.Thresholds
{
    public class CylinderThreshold : ILightzoneThreshold
    {
        // Кэшируем эффективные радиусы для оптимизации
        private float _minDist, _maxDist;
        private float _minDistSqr, _maxDistSqr;
        private Vector3 _centerPosition;
        private float _innerRadius;
        private float _outerRadius;

        public void Recalculate(Vector3 centerPosition, float innerRadius, float outerRadius, float zoneRadius)
        {
            _innerRadius = innerRadius;
            _outerRadius = outerRadius;
            // Учитываем радиус самого объекта
            _minDist = _innerRadius + zoneRadius;
            _maxDist = _outerRadius - zoneRadius;

            _minDistSqr = _minDist * _minDist;
            _maxDistSqr = _maxDist * _maxDist;

            _centerPosition = centerPosition;
        }


        public Vector3 CenterPosition => _centerPosition;

        public float InnerRadius => _innerRadius;

        public float OuterRadius => _outerRadius;

        public Vector3 GetConstrainedPosition(Vector3 currentPos, Vector3 desiredPos)
        {
            var offset = desiredPos - _centerPosition;

            var distSqr = offset.sqrMagnitude;

            if (distSqr > _maxDistSqr)
            {
                offset = offset.normalized * _maxDist;
                desiredPos.x = _centerPosition.x + offset.x;
                desiredPos.z = _centerPosition.z + offset.z;
            }
            else if (distSqr < _minDistSqr)
            {
                if (distSqr < 0.0001f) offset = Vector3.forward;

                offset = offset.normalized * _minDist;
                desiredPos.x = _centerPosition.x + offset.x;
                desiredPos.z = _centerPosition.z + offset.z;
            }

            return desiredPos;
        }

        // Умная логика для обхода малого круга
        public bool IsPathObstructed(Vector3 start, Vector3 target, out Vector3 avoidanceDir)
        {
            avoidanceDir = Vector3.zero;

            Vector3 startRel = start - _centerPosition;
            Vector3 targetRel = target - _centerPosition;
            startRel.y = 0;
            targetRel.y = 0;

            // Если мы далеко от внутреннего круга, можно упростить проверку
            // Но правильная геометрическая проверка: пересекает ли отрезок Start->End окружность InnerRadius?

            // Упрощенная эвристика для производительности:
            // Если цель "за" центром относительно нас, и расстояние от центра до линии движения меньше радиуса

            Vector3 dirToTarget = (target - start);
            float distToTarget = dirToTarget.magnitude;

            // Проецируем центр круга на вектор движения, чтобы найти ближайшую точку
            Vector3 dirNorm = dirToTarget / distToTarget;
            Vector3 vecToCenter = _centerPosition - start;
            float t = Vector3.Dot(vecToCenter, dirNorm);

            // t - расстояние вдоль пути до ближайшей к центру точки
            if (t > 0 && t < distToTarget)
            {
                Vector3 closestPointOnLine = start + dirNorm * t;
                float distFromCenterToLineSqr = (closestPointOnLine - _centerPosition).sqrMagnitude;

                // Если линия движения проходит сквозь малый круг (с небольшим запасом)
                if (distFromCenterToLineSqr < _minDistSqr)
                {
                    // Нам нужно обойти. Определяем направление обхода (Cross Product)
                    // Определяем, с какой стороны быстрее обогнуть
                    float cross = startRel.x * targetRel.z - startRel.z * targetRel.x;

                    // Строим касательную. 
                    // Если cross > 0, цель "слева", идем по часовой или наоборот.
                    // Самый простой способ получить касательную к кругу в точке Start:
                    // Вектор от центра к Start (-z, x) или (z, -x)

                    Vector3 tangent;
                    if (cross > 0) // Цель "слева" (в координатах Unity, Y-up)
                        tangent = new Vector3(-startRel.z, 0, startRel.x); // Поворот на 90
                    else
                        tangent = new Vector3(startRel.z, 0, -startRel.x); // Поворот на -90

                    avoidanceDir = tangent.normalized;
                    return true;
                }
            }

            return false;
        }
    }
}