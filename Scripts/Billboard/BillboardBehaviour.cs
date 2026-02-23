using System;
using Camera;
using GameResources.Interfaces;
using GameResources.Sprites;
using Input.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Billboard
{
    public class BillboardBehaviour : MonoBehaviour, IInteractable
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        private Transform _cameraTransform;
        private BillboardType _type = BillboardType.Cash;
        private const string AtlasName = nameof(BillboardType);
        private ISpriteAtlasHandler _atlasHandler;
        private SpriteAtlasManager _spriteAtlasManager;

        [Inject]
        public void Construct(CameraController cameraController, SpriteAtlasManager spriteAtlasManager)
        {
            _cameraTransform = cameraController.GetMainCameraTransofrm;
            _spriteAtlasManager = spriteAtlasManager;

        }

        private void Awake()
        {
            _atlasHandler = _spriteAtlasManager.GetAtlasHandler(AtlasName);
        }

        public void SetBillboardType(BillboardType billboardType) => _type = billboardType;

        public void OnEnable()
        {
            spriteRenderer.sprite = _atlasHandler.GetSprite(_type);
        }

        private void FixedUpdate()
        {
            transform.LookAt(_cameraTransform);
        }


        public void OnInteract()
        {
            Debug.Log("OnInteract");
        }
    }
}