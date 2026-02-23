using System;
using System.Collections.Generic;
using UnityEngine;

namespace Location.Configs
{
    [CreateAssetMenu(fileName = "LocationConfig", menuName = "Configs/LocationConfig")]
    public class LocationConfig : ScriptableObject
    {
        [Header("General")]
        [SerializeField] private LocationId locationId;
        [SerializeField] private int price;
        [SerializeField] private bool isLocked;
        
        [Header("Text")]
        [SerializeField] private string title;
        [SerializeField] private string description;
        
        public LocationId LocationId => locationId;
        public int Price => price;
        public bool IsLocked => isLocked;
        public string Title => title;
        public string Description => description;
    }
}