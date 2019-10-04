using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

using Aviary.Macaw.Filters;
using Grasshopper.Kernel.Types;

namespace Aviary.Macaw.GH.Filters
{
    public class FiltersEffects : GH_Component
    {

        private enum FilterModes { Additive, SaltPepper, Daube, Jitter, Kuwahara, Blur, BoxBlur, GaussianBlur, Pixelate, Posterize }
        /// <summary>
        /// Initializes a new instance of the AdjustFilters class.
        /// </summary>
        public FiltersEffects()
          : base("Effects Filters", "Effects", "Description", "Aviary 1", "Image")
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
            pManager.AddNumberParameter("Value A", "A", "---", GH_ParamAccess.item, 1.0);
            pManager[2].Optional = true;
            pManager.AddNumberParameter("Value B", "B", "---", GH_ParamAccess.item, 1.0);
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

            double numValA = 0;
            DA.GetData(2, ref numValA);

            double numValB = 0;
            DA.GetData(3, ref numValB);

            Filter filter = new Filter();

            switch ((FilterModes)mode)
            {
                case FilterModes.Additive:
                    filter = new FilterAdditive();
                    image.Filters.Add(new FilterAdditive());
                    break;
                case FilterModes.Daube:
                    filter = new FilterDaube((int)numValA);
                    image.Filters.Add(new FilterDaube((int)numValA));
                    break;
                case FilterModes.SaltPepper:
                    filter = new FilterSaltPepper((int)numValA);
                    image.Filters.Add(new FilterSaltPepper((int)numValA));
                    break;
                case FilterModes.Jitter:
                    filter = new FilterJitter((int)numValA);
                    image.Filters.Add(new FilterJitter((int)numValA));
                    break;
                case FilterModes.Kuwahara:
                    filter = new FilterKuwahara((int)numValA);
                    image.Filters.Add(new FilterKuwahara((int)numValA));
                    break;
                case FilterModes.GaussianBlur:
                    filter = new FilterGaussianBlur(numValA, (int)numValB);
                    image.Filters.Add(new FilterGaussianBlur(numValA, (int)numValB));
                    break;
                case FilterModes.Pixelate:
                    filter = new FilterPixelate((int)numValA, (int)numValB);
                    image.Filters.Add(new FilterPixelate((int)numValA, (int)numValB));
                    break;
                case FilterModes.Posterize:
                    filter = new FilterPosterize((int)numValA, (int)numValB);
                    image.Filters.Add(new FilterPosterize((int)numValA, (int)numValB));
                    break;
                case FilterModes.Blur:
                    filter = new FilterBlur((int)numValA, (int)numValB);
                    image.Filters.Add(new FilterBlur((int)numValA, (int)numValB));
                    break;
                case FilterModes.BoxBlur:
                    filter = new FilterBoxBlur((int)numValA, (int)numValB);
                    image.Filters.Add(new FilterBoxBlur((int)numValA, (int)numValB));
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
            get { return new Guid("97575abe-3ac9-4c67-9541-63041a127d4e"); }
        }
    }
}