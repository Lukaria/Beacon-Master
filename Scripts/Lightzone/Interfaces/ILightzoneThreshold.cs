using UnityEngine;
using Vector3 = UnityEngine.Vector3;


namespace Lightzone.Interfaces
{
    public interface ILightzoneThreshold
    {
        [Header("Settings")] 
        public Vector3 CenterPosition { get;} 
        public float InnerRadius { get; } 
        public float OuterRadius { get; }
        
        
        /// <summary>
        /// Корректирует желаемую позицию, чтобы она оставалась в границах.
        /// </summary>
        Vector3 GetConstrainedPosition(Vector3 currentPos, Vector3 desiredPos);
    
        /// <summary>
        /// Проверяет, блокирует ли ограничение прямой путь к цели (для обхода препятствий).
        /// </summary>
        bool IsPathObstructed(Vector3 start, Vector3 end, out Vector3 avoidanceTangent);
    }
}