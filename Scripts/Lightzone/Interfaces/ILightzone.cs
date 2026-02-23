using Lighthouse.Stats;
using Lightzone.Dto;
using UnityEngine;

namespace Lightzone.Interfaces
{
    public interface ILightzone
    {
        public LightzoneDto Stats { get; }

        public void Initialize(Vector3 center, LightzoneDto stats);
    }
}