using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

using Aviary.Macaw.Filters;
using Grasshopper.Kernel.Types;

namespace Aviary.Macaw.GH.Filters
{
    public class FiltersAdjust : GH_Component
    {

        private enum FilterModes { Invert,Brightness,Contrast,Gamma,GrayWorld,Histogram,Hue,Saturation,Stretch,WhitePatch}
        /// <summary>
        /// Initializes a new instance of the AdjustFilters class.
        /// </summary>
        public FiltersAdjust()
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
                case FilterModes.Brightness:
                    filter = new FilterBrightness((int)numVal);
                    image.Filters.Add(new FilterBrightness((int)numVal));
                    break;
                case FilterModes.Contrast:
                    filter = new FilterContrast((int)numVal);
                    image.Filters.Add(new FilterContrast((int)numVal));
                    break;
                case FilterModes.Gamma:
                    filter = new FilterGamma(numVal);
                    image.Filters.Add(new FilterGamma(numVal));
                    break;
                case FilterModes.GrayWorld:
                    filter = new FilterGrayWorld();
                    image.Filters.Add(new FilterGrayWorld());
                    break;
                case FilterModes.Histogram:
                    filter = new FilterHistogram();
                    image.Filters.Add(new FilterHistogram());
                    break;
                case FilterModes.Hue:
                    filter = new FilterHue((int)numVal);
                    image.Filters.Add(new FilterHue((int)numVal));
                    break;
                case FilterModes.Invert:
                    filter = new FilterInvert();
                    image.Filters.Add(new FilterInvert());
                    break;
                case FilterModes.Saturation:
                    filter = new FilterSaturation(numVal);
                    image.Filters.Add(new FilterSaturation(numVal));
                    break;
                case FilterModes.Stretch:
                    filter = new FilterStretch();
                    image.Filters.Add(new FilterStretch());
                    break;
                case FilterModes.WhitePatch:
                    filter = new FilterWhitePatch();
                    image.Filters.Add(new FilterWhitePatch());
                    break;
            }

            DA.SetData(0, image);
            DA.SetData(1, filter);
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