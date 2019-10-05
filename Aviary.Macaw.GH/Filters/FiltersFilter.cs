using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

using Aviary.Macaw.Filters;

namespace Aviary.Macaw.GH.Filters
{
    public class FiltersFilter : GH_Component
    {
        public enum FilterModes { Channel,RGB,HSL,YCbCr};


        /// <summary>
        /// Initializes a new instance of the FiltersFilter class.
        /// </summary>
        public FiltersFilter()
          : base("Filter", "Filter", "Description", "Aviary 1", "Image")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.quarternary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Image", "I", "The Layer Bitmap", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Mode", "M", "", GH_ParamAccess.item, 0);
            pManager[1].Optional = true;

            pManager.AddIntervalParameter("Red", "R", "---", GH_ParamAccess.item, new Interval(0, 1));
            pManager[2].Optional = true;
            pManager.AddIntervalParameter("Green", "G", "---", GH_ParamAccess.item, new Interval(0, 1));
            pManager[3].Optional = true;
            pManager.AddIntervalParameter("Blue", "B", "---", GH_ParamAccess.item, new Interval(0, 1));
            pManager[4].Optional = true;

            pManager.AddBooleanParameter("Flip", "F", "---", GH_ParamAccess.item, false);
            pManager[5].Optional = true;

            pManager.AddColourParameter("Color", "C", "---", GH_ParamAccess.item, Color.Black);
            pManager[6].Optional = true;

            Param_Integer paramA = (Param_Integer)pManager[1];
            foreach (FilterModes value in Enum.GetValues(typeof(FilterModes)))
            {
                paramA.AddNamedValue(value.ToString(), (int)value);
            }
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Image", "I", "The resulting image", GH_ParamAccess.item);
            pManager.AddGenericParameter("Bitmap", "B", "The resulting bitmap", GH_ParamAccess.item);
            pManager.AddGenericParameter("Filter", "F", "The resulting filter", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IGH_Goo goo = null;
            Image image = new Image();
            if (!DA.GetData(0, ref goo)) return;
            if (!goo.TryGetImage(ref image)) return;

            int mode = 0;
            DA.GetData(1, ref mode);

            Interval red = new Interval(0,1);
            DA.GetData(2, ref red);
            Interval green = new Interval(0, 1);
            DA.GetData(3, ref green);
            Interval blue = new Interval(0, 1);
            DA.GetData(4, ref blue);

            bool flip = false;
            DA.GetData(5, ref flip);

            Color color = Color.Black;
            DA.GetData(6, ref color);

            Filter filter = new Filter();

            switch ((FilterModes)mode)
            {
                case FilterModes.Channel:
                    filter = new ChannelFilter(red.T0,red.T1,green.T0,green.T1,blue.T0,blue.T1,flip);
                    image.Filters.Add(new ChannelFilter(red.T0, red.T1, green.T0, green.T1, blue.T0, blue.T1, flip));
                    break;
                case FilterModes.HSL:
                    filter = new ColorFilter(red.T0, red.T1, green.T0, green.T1, blue.T0, blue.T1, flip,color);
                    image.Filters.Add(new ColorFilter(red.T0, red.T1, green.T0, green.T1, blue.T0, blue.T1, flip,color));
                    break;
                case FilterModes.RGB:
                    filter = new HSLFilter(red.T0, red.T1, green.T0, green.T1, blue.T0, blue.T1, flip,color);
                    image.Filters.Add(new HSLFilter(red.T0, red.T1, green.T0, green.T1, blue.T0, blue.T1, flip,color));
                    break;
                case FilterModes.YCbCr:
                    filter = new YCbCrFilter(red.T0, red.T1, green.T0, green.T1, blue.T0, blue.T1, flip,color);
                    image.Filters.Add(new YCbCrFilter(red.T0, red.T1, green.T0, green.T1, blue.T0, blue.T1, flip, color));
                    break;
            }

            DA.SetData(0, image);
            DA.SetData(1, new Image(image.Bitmap,filter).GetFilteredBitmap());
            DA.SetData(2, filter);
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
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("8aec8998-8929-497e-bf2c-5ec96272b208"); }
        }
    }
}