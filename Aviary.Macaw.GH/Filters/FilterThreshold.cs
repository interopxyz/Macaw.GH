using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

using Aviary.Macaw.Filters.Threshold;
using Grasshopper.Kernel.Types;

namespace Aviary.Macaw.GH.Filters
{
    public class FilterThreshold : GH_Component
    {
        private enum FilterModes { Bradley, Iterative, Nilback, Otsu, Sauvola, SIS, WolfJolion}

        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public FilterThreshold()
          : base("Filter Threshold", "Threshold", "Description", "Aviary 1", "Image")
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
            pManager.AddNumberParameter("Value A", "A", "---", GH_ParamAccess.item, 1);
            pManager[2].Optional = true;
            pManager.AddNumberParameter("Value B", "B", "---", GH_ParamAccess.item, 1);
            pManager[3].Optional = true;
            pManager.AddIntegerParameter("Value C", "C", "---", GH_ParamAccess.item, 1);
            pManager[4].Optional = true;

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

            double numValA = 1.0;
            DA.GetData(2, ref numValA);

            double numValB = 1;
            DA.GetData(3, ref numValB);

            int numValC = 1;
            DA.GetData(4, ref numValC);

            Filter filter = new Filter();

            switch ((FilterModes)mode)
            {
                case FilterModes.Bradley:
                    filter = new Bradley(numValA,(int)numValB);
                    image.Filters.Add(new Bradley(numValA, (int)numValB));
                    break;
                case FilterModes.Iterative:
                    filter = new Iterative((int)numValA, (int)numValB);
                    image.Filters.Add(new Iterative((int)numValA, (int)numValB));
                    break;
                case FilterModes.Nilback:
                    filter = new Nilback(numValA, numValB, numValC);
                    image.Filters.Add(new Nilback(numValA, numValB,numValC));
                    break;
                case FilterModes.Otsu:
                    filter = new Otsu();
                    image.Filters.Add(new Otsu());
                    break;
                case FilterModes.Sauvola:
                    filter = new Sauvola(numValA, numValB,numValC);
                    image.Filters.Add(new Sauvola(numValA, numValB, numValC));
                    break;
                case FilterModes.SIS:
                    filter = new SIS();
                    image.Filters.Add(new SIS());
                    break;
                case FilterModes.WolfJolion:
                    filter = new WolfJolion(numValA, numValB,numValC);
                    image.Filters.Add(new WolfJolion(numValA, numValB, numValC));
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
            get { return new Guid("acb3a8ca-4720-4af5-ba76-d2c27b625f97"); }
        }
    }
}