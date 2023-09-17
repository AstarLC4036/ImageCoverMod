using ImageCoverMod;
using SFS.Builds;
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
        private bool startEdit = false;
        private Vector2 dPosMouse = Vector2.zero;

        bool touchable(Vector2 posA, Vector2 posB, Rect rect)
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
            gameObject.AddComponent<UI.ModTabGUI>();
        }

        // Update is called once per frame
        void Update()
        {
            //TODO: Make click area match the rotation of the image cover gameobject
            imageRect.height = originImageRect.height * transform.localScale.y;
            imageRect.width = originImageRect.width * transform.localScale.x;

            if (!editing && Input.GetMouseButtonDown(0) && touchable(ModInput.GetMousePosInWorld(), transform.position, imageRect) && draggable) 
            {
                startEdit = true;
                editing = true;
            }
            else if(editing && Input.GetMouseButtonDown(0))
            {
                editing = false;
                startEdit = false;
                dPosMouse = Vector2.zero;
            }

            if(!startEditProp && Input.GetMouseButtonDown(1) && touchable(ModInput.GetMousePosInWorld(), transform.position, imageRect))
            {
                startEditProp = true;
            }

            if(startEdit)
            {
                dPosMouse = transform.position - ModInput.GetMousePosInWorld();
                startEdit = false;
            }

            if(editing && !startEdit)
            {
                transform.position = new Vector2(ModInput.GetMousePosInWorld().x + dPosMouse.x, ModInput.GetMousePosInWorld().y + dPosMouse.y);
            }
        }
    }
}