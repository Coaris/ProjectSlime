using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BGLooper : MonoBehaviour {
        Transform mainCamera;
        List<Transform> bgList = new List<Transform>();

        [SerializeField] Sprite bgImage;
        [SerializeField] [Tooltip("����ͼˮƽ�ƶ����ٶȣ�1Ϊ��������ƶ�,ֵԽС�ƶ�Խ��"), Range(0, 1)] float xFactor = 1;
        [SerializeField] [Tooltip("����ͼ��ֱ�ƶ����ٶȣ�1Ϊ��������ƶ���ֵԽС�ƶ�Խ��"), Range(0, 1)] float yFactor = 1;

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

                        Debug.LogError("���BGLooper����ͼƬ");
                        return;
                }
#endif
                FollowCamera();
                LoopBG();
        }

        //����������ͼƬ
        public void SetNewImage(Sprite newSprite) {
                foreach (Transform i in bgList) {
                        i.GetComponent<SpriteRenderer>().sprite = newSprite;
                }
                SetInitialPosition(bgList);
        }
        //��������ͼƬ�󣬵���ͼƬ֮������λ��
        void SetInitialPosition(List<Transform> bgList) {
                imageWidth = bgList[0].GetComponent<SpriteRenderer>().bounds.size.x;
                totalWidth = imageWidth * 3;
                bgList[0].position = new Vector3(bgList[1].position.x - imageWidth, bgList[1].position.y, bgList[1].position.z);
                bgList[2].position = new Vector3(bgList[1].position.x + imageWidth, bgList[1].position.y, bgList[1].position.z);
        }
        //BGLooper�������
        void FollowCamera() {
                framePosition.x = mainCamera.position.x * xFactor;
                framePosition.y = mainCamera.position.y * yFactor;
                transform.position = framePosition;
        }
        //ѭ������ͼƬ
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
