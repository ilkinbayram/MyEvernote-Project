using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEvernote.Web.ViewModels
{
    public class ColorNotify
    {
        public static List<string> colors = new List<string>
        {
            "rgb(72, 241, 230)",
            "rgb(252, 236, 147)",
            "rgb(254, 213, 213)"
        };

        public static string GetColor(NameOfColors colorsEnum=NameOfColors.successLightPlus)
        {
            int indexSelectedColor = (int) colorsEnum;
            string selectedColor = colors[indexSelectedColor];
            return selectedColor;
        }
    }

    public enum NameOfColors
    {
        successLightPlus = 0,
        warningLightPlus = 1,
        dangerLightPlus = 2
    }

    
}