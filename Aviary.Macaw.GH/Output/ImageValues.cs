using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Aviary.Macaw.GH.Output
{
    public class ImageValues : GH_Component
    {

        private enum ValueModes { Color, Alpha, Red, Green, Blue, Hue, Saturation, Brightness };

        /// <summary>
        /// Initializes a new instance of the ImageValue class.
        /// </summary>
        public ImageValues()
          : base("Image Values", "Values", "Get a value type for each pixel" + Environment.NewLine + "Built on the Accord Imaging Library" + Environment.NewLine + "http://accord-framework.net/", "Aviary 1", "Image")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.senary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Image", "I", "An Aviary Image or Bitmap", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Modes", "M", "The value return mode", GH_ParamAccess.item, 0);
            pManager[1].Optional = true;

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

            switch((ValueModes)mode)
            {
                default:
                    DA.SetDataList(0, bitmap.GetColorValues());
                    break;
                case ValueModes.Alpha:
                    DA.SetDataList(0, bitmap.GetAValues());
                    break;
                case ValueModes.Red:
                    DA.SetDataList(0, bitmap.GetRValues());
                    break;
                case ValueModes.Green:
                    DA.SetDataList(0, bitmap.GetGValues());
                    break;
                case ValueModes.Blue:
                    DA.SetDataList(0, bitmap.GetBValues());
                    break;
                case ValueModes.Hue:
                    DA.SetDataList(0, bitmap.GetHueValues());
                    break;
                case ValueModes.Saturation:
                    DA.SetDataList(0, bitmap.GetSaturationValues());
                    break;
                case ValueModes.Brightness:
                    DA.SetDataList(0, bitmap.GetBrightnessValues());
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
                return Properties.Resources.GetPixels;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("6c864288-a0c5-42f5-9502-18ef60f6be08"); }
        }
    }
}