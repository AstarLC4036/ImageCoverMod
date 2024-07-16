using ImageCoverMod;
using SFS.Builds;
using SFS.Tutorials;
using System.Collections;
using UnityEngine;

namespace ImageCover
{
    public class DraggableImage : MonoBehaviour
    {
        public Rect originImageRect;
        public Rect imageRect;
        public bool editing = false;
        public bool startEditProp = false;
        public bool draggable = true;
        private UI.ModTabGUI tabGUI;
        private Vector2 originMousePos = Vector2.zero;

        bool Touchable(Vector2 posA, Vector2 posB, Rect rect)
        {
            //Cov rect pixels to unity unit size
            //1000 pixels => 1 unit
            //unit = pixels / unit2pixel
            //TODO: trans unit2pixel to SpriteRenderer.size
            float rectUnitWidth = rect.width / ICModProp.unit2pixel;
            float rectUnitHeight = rect.height / ICModProp.unit2pixel;
            bool result = posA.x >= posB.x + rect.x && posA.x <= posB.x + rect.x + rectUnitWidth && posA.y >= posB.y + rect.y && posA.y <= posB.y + rect.y + rectUnitHeight;
            return result;
        }

        void Start()
        {
            imageRect = new Rect(originImageRect.position, originImageRect.size);
            tabGUI = gameObject.AddComponent<UI.ModTabGUI>();
        }

        // Update is called once per frame
        void Update()
        {
            //TODO: Make click area match the rotation of the image cover gameobject
            imageRect.height = originImageRect.height * transform.localScale.y;
            imageRect.width = originImageRect.width * transform.localScale.x;

            if (!editing && Input.GetMouseButtonDown(0) && Touchable(ModInput.GetMousePosInWorld(), transform.position, imageRect) && draggable) 
            {
                originMousePos = transform.position - ModInput.GetMousePosInWorld();
                editing = true;
            }
            else if(editing && Input.GetMouseButtonDown(0))
            {
                originMousePos = Vector2.zero;
                editing = false;
            }

            if(Input.GetMouseButtonDown(1) && Touchable(ModInput.GetMousePosInWorld(), transform.position, imageRect))
            {
                tabGUI.Show();
            }

            if(editing)
            {
                transform.position = new Vector2(ModInput.GetMousePosInWorld().x + originMousePos.x, ModInput.GetMousePosInWorld().y + originMousePos.y);
            }
        }
    }
}