using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Aviary.Macaw.GH.Output
{
    public class ImageValueAt : GH_Component
    {

        private enum ValueModes { Color, Alpha, Red, Green, Blue, Hue, Saturation, Brightness };

        /// <summary>
        /// Initializes a new instance of the ImageValueAt class.
        /// </summary>
        public ImageValueAt()
          : base("Image Value At", "Value At", "Get a requested value at a specific pixel location", "Aviary 1", "Image")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.quinary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Bitmap", "B", "A bitmap object", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Modes", "M", "The value return mode", GH_ParamAccess.item, 0);
            pManager[1].Optional = true;
            pManager.AddBooleanParameter("Unitize", "U", "If true, point location should be unitized (0-1). If false, the direct pixel location", GH_ParamAccess.item, false);
            pManager[2].Optional = true;
            pManager.AddPointParameter("Pixel", "P", "The pixel location to sample", GH_ParamAccess.list);

            Param_Integer paramA = (Param_Integer)pManager[1];
            foreach (ValueModes value in Enum.GetValues(typeof(ValueModes)))
            {
                paramA.AddNamedValue(value.ToString(), (int)value);
            }
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Values", "V", "The resulting values", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IGH_Goo goo = null;
            Bitmap bitmap = new Bitmap(100, 100);
            if (!DA.GetData(0, ref goo)) return;
            if (!goo.TryGetBitmap(ref bitmap)) return;

            int mode = 0;
            DA.GetData(1, ref mode);

            bool unitize = false;
            DA.GetData(2, ref unitize);

            List<Point3d> points = new List<Point3d>();
            if (!DA.GetDataList(3, points)) return;

            List<Color> colors = new List<Color>();
            List<int> ints = new List<int>();
            List<double> numbers = new List<double>();

            double w = 1;
            if (unitize) w = bitmap.Width-1;
            double h = 1;
            if (unitize) h = bitmap.Height - 1;

            switch ((ValueModes)mode)
            {
                default:
                    foreach(Point3d point in points)
                    {
                        colors.Add(bitmap.GetPixel((int)(point.X*w), (int)(point.Y*h)));
                    }
                    DA.SetDataList(0, colors);
                    break;
                case ValueModes.Alpha:
                    foreach (Point3d point in points)
                    {
                        ints.Add(bitmap.GetPixel((int)(point.X * w), (int)(point.Y * h)).A);
                    }
                    DA.SetDataList(0, ints);
                    break;
                case ValueModes.Red:
                    foreach (Point3d point in points)
                    {
                        ints.Add(bitmap.GetPixel((int)(point.X * w), (int)(point.Y * h)).R);
                    }
                    DA.SetDataList(0, ints);
                    break;
                case ValueModes.Green:
                    foreach (Point3d point in points)
                    {
                        ints.Add(bitmap.GetPixel((int)(point.X * w), (int)(point.Y * h)).G);
                    }
                    DA.SetDataList(0, ints);
                    break;
                case ValueModes.Blue:
                    foreach (Point3d point in points)
                    {
                        ints.Add(bitmap.GetPixel((int)(point.X * w), (int)(point.Y * h)).B);
                    }
                    DA.SetDataList(0, ints);
                    break;
                case ValueModes.Hue:
                    foreach (Point3d point in points)
                    {
                        numbers.Add(bitmap.GetPixel((int)(point.X * w), (int)(point.Y * h)).GetHue());
                    }
                    DA.SetDataList(0, numbers);
                    break;
                case ValueModes.Saturation:
                    foreach (Point3d point in points)
                    {
                        numbers.Add(bitmap.GetPixel((int)(point.X * w), (int)(point.Y * h)).GetSaturation());
                    }
                    DA.SetDataList(0, numbers);
                    break;
                case ValueModes.Brightness:
                    foreach (Point3d point in points)
                    {
                        numbers.Add(bitmap.GetPixel((int)(point.X * w), (int)(point.Y * h)).GetBrightness());
                    }
                    DA.SetDataList(0, numbers);
                    break;
            }
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Properties.Resources.GetPixel;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("77e5ce04-cc79-4692-a5a0-6ddb9d5f8862"); }
        }
    }
}