using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviary.Macaw.GH
{
    public static class Extensions
    {

        public static bool TryGetBitmap(this IGH_Goo goo, ref Bitmap bitmap)
        {
            bool output = true;
            Bitmap bmp = new Bitmap(100,100);

            Image image = new Image();
            if (goo.CastTo<Bitmap>(out bmp))
            {
                bitmap = (Bitmap)bmp.Clone();
            }
            else
            {
                if (goo.CastTo<Image>(out image))
                {
                    bitmap = image.Bitmap;
                }
                else
                {
                    output = false;
                }
            }
            return output;
        }

        public static bool TryGetImage(this IGH_Goo goo, ref Image image)
        {
            bool output = true;
            Bitmap bmp = new Bitmap(100, 100);

            Image img = new Image();
            if (goo.CastTo<Bitmap>(out bmp))
            {
                image = new Image(bmp);
            }
            else
            {
                if (goo.CastTo<Image>(out img))
                {
                    image = new Image(img);
                }
                else
                {
                    output = false;
                }
            }
            return output;
        }

    }
}
