using System;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

namespace Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera mainCamera;
        [SerializeField] private CinemachineCamera shopCamera;

        [Header("Camera Shakes")] 
        [SerializeField] private CameraShakeDto bigShake;
        [SerializeField] private CameraShakeDto smallShake;
        
        private CinemachineCamera _currentCamera;
        private Tweener _shakeTween;

        private void Awake()
        {
            _currentCamera = mainCamera;
            EnableMainCamera();
        }

        public void BigShake() => Shake(bigShake);
        
        public void SmallShake() => Shake(smallShake);

        public void Shake(CameraShakeDto data)
        {
            _shakeTween.Complete();
            _shakeTween = _currentCamera.transform.DOShakePosition(data.duration, data.strength, data.vibration);
        }

        public void EnableMainCamera()
        {
            mainCamera.gameObject.SetActive(true);
            shopCamera.gameObject.SetActive(false);
            _currentCamera = mainCamera;
        }
        
        public void EnableShopCamera()
        {
            mainCamera.gameObject.SetActive(false);
            shopCamera.gameObject.SetActive(true);
            _currentCamera = shopCamera;
        }
        
        public Transform GetMainCameraTransofrm =>  mainCamera.transform; 
    }
}