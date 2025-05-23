using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCx
{
    public static class ExtensionMethods 
    {
        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            if ((to1 - from1) == 0 || (to2 - from2) == 0 || (to1 - from1) * (to2 - from2) + from2 == 0)
            {
                return 0;
            }
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
    }
}

