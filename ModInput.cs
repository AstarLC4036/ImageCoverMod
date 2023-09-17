using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ImageCover
{
    public class ModInput
    {
        public static Vector3 GetMousePosInWorld()
        {
            Vector3 mousePosInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return new Vector3(mousePosInWorld.x, mousePosInWorld.y, 0);
        }
    }
}
