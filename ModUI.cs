using SFS.UI.ModGUI;
using Type = SFS.UI.ModGUI.Type;
using UnityEngine;
using ImageCoverMod;

namespace ImageCover.UI
{
    public class ModUI
    {
        public static GameObject windowHolder;
        public static readonly int windowID = Builder.GetRandomID();
        public static Window window;
        public static RectInt windowRect = new RectInt(0, 0, 700, 200);

        //show create menu
        public static void ShowGUI()
        {
            windowHolder = Builder.CreateHolder(Builder.SceneToAttach.CurrentScene, "Image Cover Mod");
            window = Builder.CreateWindow(windowHolder.transform, windowID, windowRect.width, windowRect.height, windowRect.x, windowRect.y, true, true, 0.9f, "Image Cover Mod");
            window.CreateLayoutGroup(Type.Vertical);
            Builder.CreateButtonWithLabel(window, 670, 50, 0, 0, "Spawn Cover Image", "Spawn", BuildBehavior);

            //Find a new bug(Slider UI dosen't work in the Build Scene)
            /*
            Box box = Builder.CreateBox(window, 670, 250);
            box.CreateLayoutGroup(Type.Vertical, TextAnchor.UpperLeft);
            Builder.CreateLabel(box, 670, 30, 0, 0, "test slider");
            Container sliderContainer = Builder.CreateContainer(box);
            sliderContainer.CreateLayoutGroup(Type.Horizontal);
            Builder.CreateLabel(sliderContainer, 200, 50, 0, 0, "Slider");
            Builder.CreateSlider(sliderContainer, 300, 0, (0, 1), false, inA, inB);
            */
        }

        //create cover image
        public static void BuildBehavior()
        {
            if(Main.ImageCount < ICModSettings.maxImages)
            {
                float textureHalfHeightU = Main.imageSprite.textureRect.height / ICModProp.unit2pixel / 2;
                float textureHalfWidthU = Main.imageSprite.textureRect.width / ICModProp.unit2pixel / 2;

                GameObject imageObj = new GameObject("CoverImage");
                imageObj.transform.position = new Vector3(Camera.main.transform.position.x - textureHalfWidthU, Camera.main.transform.position.y - textureHalfHeightU);
                SpriteRenderer imgSr = imageObj.AddComponent<SpriteRenderer>();
                imgSr.sprite = Main.imageSprite;
                imgSr.sortingOrder = 1;
                DraggableImage dragableImg = imageObj.AddComponent<DraggableImage>();
                dragableImg.originImageRect = Main.imageSprite.rect;
                Main.ImageCount += 1;
            }
        }
    }
}
