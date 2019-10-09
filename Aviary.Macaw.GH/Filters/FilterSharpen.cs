using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

using Grasshopper.Kernel.Types;

using Aviary.Macaw.Filters;
using Aviary.Macaw.Filters.Sharpening;
using Aviary.Macaw.Filters.Smoothing;

namespace Aviary.Macaw.GH.Filters
{
    public class FilterSharpen : GH_Component
    {
        private enum FilterModes { Adaptive, Conservative, Mean, Median, Gaussian, HighBoost, Simple }

        /// <summary>
        /// Initializes a new instance of the FilterSharpen class.
        /// </summary>
        public FilterSharpen()
          : base("Filter Sharpen", "Nickname", "Description", "Aviary 1", "Image")
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
            pManager.AddIntegerParameter("Value A", "A", "---", GH_ParamAccess.item, 1);
            pManager[2].Optional = true;
            pManager.AddIntegerParameter("Value B", "B", "---", GH_ParamAccess.item, 1);
            pManager[3].Optional = true;

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

            int numValA = 1;
            DA.GetData(2, ref numValA);

            int numValB = 1;
            DA.GetData(3, ref numValB);

            Filter filter = new Filter();

            switch ((FilterModes)mode)
            {
                case FilterModes.Adaptive:
                    filter = new Adaptive();
                    image.Filters.Add(new Adaptive());
                    break;
                case FilterModes.Conservative:
                    filter = new Conservative();
                    image.Filters.Add(new Conservative());
                    break;
                case FilterModes.Gaussian:
                    filter = new Gaussian(numValA,numValB);
                    image.Filters.Add(new Gaussian(numValA, numValB));
                    break;
                case FilterModes.HighBoost:
                    filter = new HighBoost(numValA, numValB);
                    image.Filters.Add(new HighBoost(numValA, numValB));
                    break;
                case FilterModes.Mean:
                    filter = new Mean(numValA, numValB);
                    image.Filters.Add(new Mean(numValA, numValB));
                    break;
                case FilterModes.Median:
                    filter = new Median();
                    image.Filters.Add(new Median());
                    break;
                case FilterModes.Simple:
                    filter = new Simple((int)numValA, (int)numValB);
                    image.Filters.Add(new Simple((int)numValA, (int)numValB));
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
            get { return new Guid("39bae9bc-faa2-431a-a8ed-c08fe44e8e4d"); }
        }
    }
}