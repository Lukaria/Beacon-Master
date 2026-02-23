using UnityEngine;
using UnityEngine.Serialization;

namespace Camera
{
    [System.Serializable]
    public record CameraShakeDto
    {
        public float duration;
        public Vector3 strength;
        public int vibration;
    }
}