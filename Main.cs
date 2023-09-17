using ModLoader;
using HarmonyLib;
using UnityEngine;
using ModLoader.Helpers;
using System.IO;
using System.Drawing;
using Image = System.Drawing.Image;
using UnityEngine.UI;
using System.Runtime;

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

        public static Sprite imageSprite;
        public static Texture2D imageTex;

        public static int ImageCount = 0;

        static Harmony patcher;

        public static TextAsset imageContent;

        public override void Early_Load()
        {
            //load image
            //ICModLog("Loading Images...");
            try
            {
                DirectoryInfo imageDir = new DirectoryInfo(Directory.GetCurrentDirectory() + @"\Spaceflight Simulator Game\Mods\ImageCoverMod\Images");
                FileInfo[] images = imageDir.GetFiles();
                imageTex = GetT2D4Image(images[0].FullName);
                //Debug.Log(images[0].Name);
            }
            catch (DirectoryNotFoundException exp)
            {
                ICModLog("Load Failed: Mod image path not found.");
                ICModLog("This may be because you didn't start the game in Steam.");
                ICModLog(exp.Message);
                ICModLog("Try to load mod from local path...");
                DirectoryInfo imageDir = new DirectoryInfo(Directory.GetCurrentDirectory() + @"\Mods\ImageCoverMod\Images");
                FileInfo[] images = imageDir.GetFiles();
                imageTex = GetT2D4Image(images[0].FullName);
            }
            ICModLog("Load image complete.");

        }
        public override void Load()
        {
            //ICModLog("Image Cover Mod Loaded");

            imageSprite = Sprite.Create(imageTex, new Rect(0, 0, imageTex.width, imageTex.height), new Vector2(0f, 0f));
            imageSprite.name = "CoverImage";

            SceneHelper.OnBuildSceneLoaded += () =>
            {
                ImageCount = 0;
                UI.ModUI.ShowGUI();
            };

            //Debug
            /*
            SceneHelper.OnHomeSceneLoaded += () =>
            {
                UI.ModUI.ShowGUI();
            };
            */
        }

        //mod log
        public static void ICModLog(string msg)
        {
            Debug.Log("[ImageCoverMod] " + msg);
        }

        //get texture2d from path
        public Texture2D GetT2D4Image(string path)
        {
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
            return resultT2d;
        }
    }
}