using SFS.UI.ModGUI;
using Type = SFS.UI.ModGUI.Type;
using UnityEngine;
using ImageCoverMod;

namespace ImageCover.UI
{
    public class ModUI
    {
        public GameObject windowHolder;
        public readonly int windowID = Builder.GetRandomID();
        public Window window;
        public RectInt windowRect = new RectInt(0, 0, 700, 250 + Main.imageSprites.Count * 70);
        public Main main;

        public ModUI(Main mainClass)
        {
            main = mainClass;
        }

        //show create menu
        public void ShowGUI()
        {
            windowHolder = Builder.CreateHolder(Builder.SceneToAttach.CurrentScene, "Image Cover Mod");
            window = Builder.CreateWindow(windowHolder.transform, windowID, windowRect.width, windowRect.height, windowRect.x, windowRect.y, true, true, 0.9f, "Image Cover Mod");
            window.CreateLayoutGroup(Type.Vertical);

            if (Main.imageSprites.Count == 0)
            {
                Builder.CreateLabel(window, 670, 50, 0, 0, "No Any Image Available");
            }
            else
            {
                Main.imageSprites.ForEach((Sprite imageSprite) =>
                {
                    Builder.CreateButtonWithLabel(window, 670, 50, 0, 0, imageSprite.name, "Load", () =>
                    {
                        if (Main.ImageCount < ICModSettings.maxImages)
                        {
                            float textureHalfHeightU = imageSprite.textureRect.height / ICModProp.unit2pixel / 2;
                            float textureHalfWidthU = imageSprite.textureRect.width / ICModProp.unit2pixel / 2;

                            GameObject imageObj = new GameObject(imageSprite.name);
                            imageObj.transform.position = new Vector3(Camera.main.transform.position.x - textureHalfWidthU, Camera.main.transform.position.y - textureHalfHeightU);

                            SpriteRenderer imgSR = imageObj.AddComponent<SpriteRenderer>();
                            imgSR.sprite = imageSprite;
                            imgSR.sortingOrder = 1;

                            DraggableImage dragableImg = imageObj.AddComponent<DraggableImage>();
                            dragableImg.originImageRect = imageSprite.rect;
                            Main.ImageCount += 1;
                        }
                    });
                });
            }

            Builder.CreateSeparator(window, 670, 0, 0);
            Builder.CreateButton(window, 600, 50, 0, 0, () => {
                main.LoadImages(); 
                GameObject.Destroy(windowHolder);
                windowRect = new RectInt(0, 0, 700, 250 + Main.imageSprites.Count * 70);
                ShowGUI(); 
            }, "Reload Images");


            //Find a new bug(Slider UI dosen't work in the Build Scene)
            //Still not fix 2024.7.16 12:20 (UTC/GMT + 8)
            /*
            Box box = Builder.CreateBox(window, 670, 250);
            box.CreateLayoutGroup(Type.Vertical, TextAnchor.UpperLeft);
            Builder.CreateLabel(box, 670, 30, 0, 0, "test slider");
            Container sliderContainer = Builder.CreateContainer(box);
            sliderContainer.CreateLayoutGroup(Type.Horizontal);
            Builder.CreateLabel(sliderContainer, 200, 50, 0, 0, "Slider");
            Builder.CreateSlider(sliderContainer, 300, 0, (0, 1), false, (float f) => { }, (float f) => { return f.ToString(); });
            */
        }

        //Create cover image(old)
        /*
        public static void BuildBehavior()
        {
            if(Main.ImageCount < ICModSettings.maxImages)
            {
                //TODO: Multi sprites
                //TODO completed
                //Maybe someday I will remember this and complete it...?[im lazy :)]

                if (Main.imageSprites.Count == 0)
                    return;

                Sprite imageSprite = Main.imageSprites.ToArray()[0];

                float textureHalfHeightU = imageSprite.textureRect.height / ICModProp.unit2pixel / 2;
                float textureHalfWidthU = imageSprite.textureRect.width / ICModProp.unit2pixel / 2;

                GameObject imageObj = new GameObject("C-Image");
                imageObj.transform.position = new Vector3(Camera.main.transform.position.x - textureHalfWidthU, Camera.main.transform.position.y - textureHalfHeightU);
                SpriteRenderer imgSR = imageObj.AddComponent<SpriteRenderer>();
                imgSR.sprite = imageSprite;
                imgSR.sortingOrder = 1;
                DraggableImage dragableImg = imageObj.AddComponent<DraggableImage>();
                dragableImg.originImageRect = imageSprite.rect;
                Main.ImageCount += 1;
            }
        }
        */
    }
}
