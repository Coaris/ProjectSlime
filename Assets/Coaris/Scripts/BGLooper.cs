using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BGLooper : MonoBehaviour {
        Transform mainCamera;
        List<Transform> bgList = new List<Transform>();

        [SerializeField] Sprite bgImage;
        [SerializeField] [Tooltip("背景图水平移动的速度，1为跟随相机移动,值越小移动越快"), Range(0, 1)] float xFactor = 1;
        [SerializeField] [Tooltip("背景图垂直移动的速度，1为跟随相机移动，值越小移动越快"), Range(0, 1)] float yFactor = 1;

        Vector3 framePosition;
        Vector3 tempPos;
        float imageWidth;
        float totalWidth;

        void Awake() {
                mainCamera = Camera.main.transform;
                Transform[] children = GetComponentsInChildren<Transform>();
                foreach (Transform child in children) {
                        if (child != transform) {
                                bgList.Add(child);
                        }
                }
                if (bgImage != null) SetNewImage(bgImage);
        }

        void Update() {
#if UNITY_EDITOR
                if (bgImage == null) {

                        Debug.LogError("请给BGLooper放置图片");
                        return;
                }
#endif
                FollowCamera();
                LoopBG();
        }

        //更换背景的图片
        public void SetNewImage(Sprite newSprite) {
                foreach (Transform i in bgList) {
                        i.GetComponent<SpriteRenderer>().sprite = newSprite;
                }
                SetInitialPosition(bgList);
        }
        //更换背景图片后，调整图片之间的相对位置
        void SetInitialPosition(List<Transform> bgList) {
                imageWidth = bgList[0].GetComponent<SpriteRenderer>().bounds.size.x;
                totalWidth = imageWidth * 3;
                bgList[0].position = new Vector3(bgList[1].position.x - imageWidth, bgList[1].position.y, bgList[1].position.z);
                bgList[2].position = new Vector3(bgList[1].position.x + imageWidth, bgList[1].position.y, bgList[1].position.z);
        }
        //BGLooper跟随相机
        void FollowCamera() {
                framePosition.x = mainCamera.position.x * xFactor;
                framePosition.y = mainCamera.position.y * yFactor;
                transform.position = framePosition;
        }
        //循环背景图片
        void LoopBG() {
                foreach (Transform i in bgList) {
                        tempPos = i.position;
                        if (mainCamera.position.x > tempPos.x + totalWidth / 2) {
                                tempPos.x += totalWidth;
                                i.position = tempPos;
                        }
                        else if (mainCamera.position.x < tempPos.x - totalWidth / 2) {
                                tempPos.x -= totalWidth;
                                i.position = tempPos;
                        }
                }
        }
}
