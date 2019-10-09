using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

using Aviary.Macaw.Filters.Edges;

namespace Aviary.Macaw.GH.Filters
{
    public class FilterEdges : GH_Component
    {
        private enum FilterModes { Canny, Difference, Homogeneity, Kirsch, Robinson, Sobel,Compass }

        /// <summary>
        /// Initializes a new instance of the FilterEdges class.
        /// </summary>
        public FilterEdges()
          : base("Filter Edges", "Edges", "Description", "Aviary 1", "Image")
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
            pManager.AddIntegerParameter("Value B", "B", "---", GH_ParamAccess.item, 1);
            pManager[3].Optional = true;
            pManager.AddIntegerParameter("Value C", "C", "---", GH_ParamAccess.item, 1);
            pManager[4].Optional = true;
            pManager.AddIntegerParameter("Value D", "D", "---", GH_ParamAccess.item, 1);
            pManager[5].Optional = true;

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

            int numValB = 1;
            DA.GetData(3, ref numValB);

            int numValC = 1;
            DA.GetData(4, ref numValC);

            int numValD = 1;
            DA.GetData(5, ref numValD);

            Filter filter = new Filter();

            switch ((FilterModes)mode)
            {
                case FilterModes.Canny:
                    filter = new Canny(numValA, numValB, numValC, numValD);
                    image.Filters.Add(new Canny());
                    break;
                case FilterModes.Difference:
                    filter = new Difference();
                    image.Filters.Add(new Difference());
                    break;
                case FilterModes.Homogeneity:
                    filter = new Homogeneity();
                    image.Filters.Add(new Homogeneity());
                    break;
                case FilterModes.Kirsch:
                    filter = new Kirsch();
                    image.Filters.Add(new Kirsch());
                    break;
                case FilterModes.Robinson:
                    filter = new Robinson();
                    image.Filters.Add(new Robinson());
                    break;
                case FilterModes.Sobel:
                    filter = new Sobel();
                    image.Filters.Add(new Sobel());
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
            get { return new Guid("1c3c96c3-f6fa-41f6-81f7-caa5139f0167"); }
        }
    }
}