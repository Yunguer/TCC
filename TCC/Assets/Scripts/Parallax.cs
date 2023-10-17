using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetoTCC
{
    public class Parallax : MonoBehaviour
    {
        [SerializeField]
        private Transform background;

        [SerializeField]
        private float parallaxScale;

        [SerializeField]
        private float speed;

        [SerializeField]
        private Transform camera;
        private Vector3 previewCameraPosition;

        void Start()
        {
            camera = Camera.main.transform;
            previewCameraPosition = camera.position;
        }

        void LateUpdate()
        {
            float parallaxEffect = (previewCameraPosition.x - camera.position.x) * parallaxScale;
            float targetXBackground = background.position.x + parallaxEffect;

            Vector3 backgroundPosition = new Vector3(targetXBackground, background.position.y, background.localPosition.z);
            background.position = Vector3.Lerp(background.position, backgroundPosition, speed * Time.deltaTime);

            previewCameraPosition = camera.position;
        }
    }

}