using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

using Aviary.Macaw.Filters;
using Aviary.Macaw.Filters.Adjustments;

namespace Aviary.Macaw.GH.Filters
{
    public class FilterAdjust : GH_Component
    {

        private enum FilterModes { Invert, Brightness, Contrast, Gamma, GrayWorld, Sepia, Histogram, Hue, Saturation, Stretch, WhitePatch,RGChromacity }
        /// <summary>
        /// Initializes a new instance of the AdjustFilters class.
        /// </summary>
        public FilterAdjust()
          : base("Adjust Filters", "Adjust", "Description", "Aviary 1", "Image")
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
            pManager.AddIntegerParameter("Mode", "M", "Select filter type", GH_ParamAccess.item, 0);
            pManager[1].Optional = true;
            pManager.AddNumberParameter("Value", "V", "---", GH_ParamAccess.item, 1.0);
            pManager[2].Optional = true;

            Param_Integer param = (Param_Integer)pManager[1];
            foreach (FilterModes value in Enum.GetValues(typeof(FilterModes)))
            {
                param.AddNamedValue(value.ToString(), (int)value);
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

            double numVal = 0;
            DA.GetData(2, ref numVal);

            Filter filter = new Filter();

            switch ((FilterModes)mode)
            {
                case FilterModes.GrayWorld:
                    filter = new GrayWorld();
                    image.Filters.Add(new GrayWorld());
                    break;
                case FilterModes.Histogram:
                    filter = new Histogram();
                    image.Filters.Add(new Histogram());
                    break;
                case FilterModes.Invert:
                    filter = new Invert();
                    image.Filters.Add(new Invert());
                    break;
                case FilterModes.Stretch:
                    filter = new Stretch();
                    image.Filters.Add(new Stretch());
                    break;
                case FilterModes.WhitePatch:
                    filter = new WhitePatch();
                    image.Filters.Add(new WhitePatch());
                    break;
                case FilterModes.Sepia:
                    filter = new Sepia();
                    image.Filters.Add(new Sepia());
                    break;
                case FilterModes.Brightness:
                    filter = new Brightness(numVal);
                    image.Filters.Add(new Brightness(numVal));
                    break;
                case FilterModes.Contrast:
                    filter = new Contrast(numVal);
                    image.Filters.Add(new Contrast(numVal));
                    break;
                case FilterModes.Gamma:
                    filter = new Gamma(numVal);
                    image.Filters.Add(new Gamma(numVal));
                    break;
                case FilterModes.Hue:
                    filter = new Hue(numVal);
                    image.Filters.Add(new Hue(numVal));
                    break;
                case FilterModes.Saturation:
                    filter = new Saturation(numVal);
                    image.Filters.Add(new Saturation(numVal));
                    break;
                case FilterModes.RGChromacity:
                    filter = new RGChromacity();
                    image.Filters.Add(new RGChromacity());
                    break;
            }

            DA.SetData(0, image);
            DA.SetData(1, image.GetFilteredBitmap());
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
            get { return new Guid("f5188034-5d64-4718-8e7f-498b1f2e5fca"); }
        }
    }
}