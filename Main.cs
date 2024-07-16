using ModLoader;
using HarmonyLib;
using UnityEngine;
using ModLoader.Helpers;
using System.IO;
using System.Collections.Generic;
using Image = System.Drawing.Image;
using System;
using ImageCover.UI;

namespace ImageCover
{
    public class Main : Mod
    {
        public override string ModNameID => "imageCoverMod";
        public override string DisplayName => "Image Cover";
        public override string Author => "AstarLC";
        public override string Description => "A simple mod to place custom image to the build scene";
        public override string ModVersion => "v1.0.0";
        public override string MinimumGameVersionNecessary => "1.5.9.8";

        public static List<Sprite> imageSprites = new List<Sprite>();
        public static List<Texture2D> imageTexs = new List<Texture2D>();

        public static int ImageCount = 0;

        static Harmony patcher;

        private ModUI modMainUI;

        public ModUI ModMainUI => modMainUI;

        public override void Load()
        {
            LoadImages();
            modMainUI = new ModUI(this);

            SceneHelper.OnBuildSceneLoaded += () =>
            {
                ImageCount = 0;
                ModMainUI.ShowGUI();
            };

            //Debug
            /*
            SceneHelper.OnHomeSceneLoaded += () =>
            {
                ModMainUI.ShowGUI();
            };
            */
        }

        public void LoadImages()
        {
            imageTexs.Clear();
            imageSprites.Clear();
            
            //load image directory
            ICModLog("Loading Images...");
            DirectoryInfo imageDir = new DirectoryInfo(ModFolder + @"\Images");
            if (imageDir.Exists)
            {
                //Load images
                FileInfo[] images = imageDir.GetFiles();
                foreach (FileInfo image in images)
                {
                    imageTexs.Add(LoadTexture2DImage(image.FullName));
                }
            }
            //No images directory
            else
            {
                try
                {
                    //Create images directory
                    ICModLog("Mod image directory not exists, creating image directory...");
                    Directory.CreateDirectory(imageDir.FullName);

                    //Load images
                    FileInfo[] images = imageDir.GetFiles();
                    foreach (FileInfo image in images)
                    {
                        imageTexs.Add(LoadTexture2DImage(image.FullName));
                    }
                }
                catch (Exception e)
                {
                    //WHY?
                    ICModLog("Load image failed. Mod GUI will not show in build scene. \nReason: Create image directory failed: \n" + e.Message);
                    return;
                }
            }

            ICModLog("Load image complete.");

            //Load images as sprites
            imageTexs.ForEach((Texture2D imageTex) =>
            {
                Sprite sprite = Sprite.Create(imageTex, new Rect(0, 0, imageTex.width, imageTex.height), new Vector2(0f, 0f));
                sprite.name = imageTex.name;
                imageSprites.Add(sprite);
            });
        }

        //Mod Log
        public static void ICModLog(string msg)
        {
            Debug.Log("[ImageCoverMod] " + msg);
        }

        /// <summary>
        /// Load Texture2D from the given path
        /// </summary>
        /// <param name="path">The full path of the image to be loaded.</param>
        /// <returns>The Texture2D image loaded from the given path.</returns>
        public static Texture2D LoadTexture2DImage(string path)
        {
            FileInfo imgFile = new FileInfo(path);

            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            int byteLength = (int)fs.Length;
            byte[] imgBytes = new byte[byteLength];
            fs.Read(imgBytes, 0, byteLength);
            fs.Close();
            fs.Dispose();

            Image img = Image.FromStream(new MemoryStream(imgBytes));
            Texture2D resultT2d = new Texture2D(img.Width, img.Height);
            img.Dispose();

            resultT2d.LoadImage(imgBytes);
            resultT2d.Apply();
            resultT2d.name = imgFile.Name;

            return resultT2d;
        }
    }
}
