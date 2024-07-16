using SFS.UI.ModGUI;
using Type = SFS.UI.ModGUI.Type;
using UnityEngine;
using System;

namespace ImageCover.UI
{
    public class ModTabGUI : MonoBehaviour
    {
        //TabMenuGUI Window
        public GameObject tabWindowHolder;
        public readonly int tabWindowID = Builder.GetRandomID();
        public Window tabWindow;
        public RectInt tabWindowRect = new RectInt(0, 0, 800, 760);

        //TabMenuGUI Var
        public GameObject imageObject;
        public DraggableImage draggableImageComp;
        public SpriteRenderer imgSr;
        private bool tabExist = false;

        void Start()
        {
            imageObject = gameObject;
            draggableImageComp = gameObject.GetComponent<DraggableImage>();
            imgSr = gameObject.GetComponent<SpriteRenderer>();
        }

        public void Show()
        {
            if (!tabExist)
            {
                ShowTabGUI();
            }
            else
            {
                try
                {
                    tabWindowHolder.SetActive(true);
                }
                catch (NullReferenceException)
                {
                    ShowTabGUI();
                }
            }
        }

        //show tab menu
        public void ShowTabGUI()
        {
            tabWindowHolder = Builder.CreateHolder(Builder.SceneToAttach.CurrentScene, "ICMEditTab" + tabWindowID);
            tabWindow = Builder.CreateWindow(tabWindowHolder.transform, tabWindowID, tabWindowRect.width, tabWindowRect.height, tabWindowRect.x, tabWindowRect.y, true, true, 0.9f, "[ICM]Edit Image");
            tabWindow.CreateLayoutGroup(Type.Vertical);
            Builder.CreateToggleWithLabel(tabWindow, 370, 50, () => { return draggableImageComp.draggable; }, OnToggleMoveChange, 0, 0, "Draggable");

            Box settingsBox = Builder.CreateBox(tabWindow, 700, 420);
            settingsBox.CreateLayoutGroup(Type.Vertical, TextAnchor.UpperCenter);
            Container imgSettingContainer = Builder.CreateContainer(settingsBox);
            imgSettingContainer.CreateLayoutGroup(Type.Vertical);
            Builder.CreateLabel(imgSettingContainer, 370, 55, 0, 0, "Image Settings");
            Builder.CreateInputWithLabel(imgSettingContainer, 670, 50, 0, 0, "Transparency", "0", OnTransparencyChange);
            Builder.CreateInputWithLabel(imgSettingContainer, 670, 50, 0, 80, "Height Scale", "1", OnHeightScaleChange);
            Builder.CreateInputWithLabel(imgSettingContainer, 670, 50, 0, 160, "Width Scale", "1", OnWidthScaleChange);
            Builder.CreateInputWithLabel(imgSettingContainer, 670, 50, 0, 240, "Rotation", "0", OnRoatationChange);
            Builder.CreateInputWithLabel(imgSettingContainer, 670, 50, 0, 320, "Sorting Order", "1", OnSortingOdrChange);

            Builder.CreateButton(tabWindow, 370, 50, 0, 430, CloseTab, "Close Tab");
            Builder.CreateButton(tabWindow, 370, 50, 0, 510, DeleteBehavior, "Delete Image");
            tabExist = true;
        }

        //Destroy tab(Delete Image)
        public void DeleteBehavior()
        {
            //复用去他妈，快乐你我他。
            Destroy(imageObject);
            Destroy(tabWindowHolder);
            tabExist = false;
            Main.ImageCount -= 1;
        }

        //Close tab
        public void CloseTab()
        {
            tabWindowHolder.SetActive(false);
            draggableImageComp.editing = false;
        }

        //On draggable(bool) change
        public void OnToggleMoveChange()
        {
            draggableImageComp.draggable = !draggableImageComp.draggable;
            draggableImageComp.editing = false;
        }

        //Text change
        public static float Str2Float(string str, float errTypeOutValue)
        {
            float result;
            try
            {
                result = (float)Convert.ToDouble(str);
            } catch
            {
                result = errTypeOutValue;
            }

            return result;
        }

        public static int Str2Int32(string str, int errTypeOutValue)
        {
            int result;
            try
            {
                result = Convert.ToInt32(str);
            }
            catch
            {
                result = errTypeOutValue;
            }

            return result;
        }

        public void OnTransparencyChange(string arg)
        {
            float transparency = Str2Float(arg, 0);
            imgSr.color = new Color(imgSr.color.r, imgSr.color.g, imgSr.color.b, 1 - transparency);
        }

        public void OnHeightScaleChange(string arg)
        {
            float heightScale = Str2Float(arg, 1);
            imageObject.transform.localScale = new Vector3(imageObject.transform.localScale.x, heightScale);
        }

        public void OnWidthScaleChange(string arg)
        {
            float widthScale = Str2Float(arg, 1);
            imageObject.transform.localScale = new Vector3(widthScale, imageObject.transform.localScale.y);
        }

        public void OnRoatationChange(string arg)
        {
            float rotation = Str2Float(arg, 0);
            imageObject.transform.localEulerAngles = new Vector3(imageObject.transform.localEulerAngles.x, imageObject.transform.localEulerAngles.y, rotation);
        }

        public void OnSortingOdrChange(string arg)
        {
            int sortingOdr = Str2Int32(arg, 0);
            imgSr.sortingOrder = sortingOdr;
        }
    }
}
