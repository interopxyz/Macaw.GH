using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

using Aviary.Macaw.Filters;
using Grasshopper.Kernel.Types;
using Aviary.Macaw.Filters.Effects;

namespace Aviary.Macaw.GH.Filters
{
    public class FilterEffects : GH_Component
    {

        private enum FilterModes { Additive, SaltPepper, Daube, Jitter, Kuwahara, Blur, GaussianBlur, Pixellate, Posterize }
        /// <summary>
        /// Initializes a new instance of the AdjustFilters class.
        /// </summary>
        public FilterEffects()
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

            double numValA = 0;
            DA.GetData(2, ref numValA);

            double numValB = 0;
            DA.GetData(3, ref numValB);

            Filter filter = new Filter();

            switch ((FilterModes)mode)
            {
                case FilterModes.Additive:
                    filter = new Additive();
                    image.Filters.Add(new Additive());
                    break;
                case FilterModes.Daube:
                    filter = new Daube((int)numValA);
                    image.Filters.Add(new Daube((int)numValA));
                    break;
                case FilterModes.SaltPepper:
                    filter = new SaltPepper((int)numValA);
                    image.Filters.Add(new SaltPepper((int)numValA));
                    break;
                case FilterModes.Jitter:
                    filter = new Jitter((int)numValA);
                    image.Filters.Add(new Jitter((int)numValA));
                    break;
                case FilterModes.Kuwahara:
                    filter = new Kuwahara((int)numValA);
                    image.Filters.Add(new Kuwahara((int)numValA));
                    break;
                case FilterModes.GaussianBlur:
                    filter = new GaussianBlur(numValA, (int)numValB);
                    image.Filters.Add(new GaussianBlur(numValA, (int)numValB));
                    break;
                case FilterModes.Pixellate:
                    filter = new Pixellate((int)numValA, (int)numValB);
                    image.Filters.Add(new Pixellate((int)numValA, (int)numValB));
                    break;
                case FilterModes.Posterize:
                    filter = new Posterize((int)numValA, (int)numValB);
                    image.Filters.Add(new Posterize((int)numValA, (int)numValB));
                    break;
                case FilterModes.Blur:
                    filter = new Blur((int)numValA, (int)numValB);
                    image.Filters.Add(new Blur((int)numValA, (int)numValB));
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
            get { return new Guid("97575abe-3ac9-4c67-9541-63041a127d4e"); }
        }
    }
}