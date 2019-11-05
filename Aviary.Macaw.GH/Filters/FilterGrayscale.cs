using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

using Aviary.Macaw.Filters.Grayscale;
using Grasshopper.Kernel.Types;

namespace Aviary.Macaw.GH.Filters
{
    public class FilterGrayscale : GH_Component
    {
        private enum FilterModes { Y, RMY, BT709, Simple }

        /// <summary>
        /// Initializes a new instance of the FilterGrayscale class.
        /// </summary>
        public FilterGrayscale()
          : base("Filter Grayscale", "Grayscale", "Apply grayscale filters to an image" + Environment.NewLine + "Built on the Accord Imaging Library" + Environment.NewLine + "http://accord-framework.net/", "Aviary 1", "Image")
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
            pManager.AddNumberParameter("Not Used", "-", "Parameter not used by this filter", GH_ParamAccess.item, 0.125);
            pManager[2].Optional = true;
            pManager.AddNumberParameter("Not Used", "-", "Parameter not used by this filter", GH_ParamAccess.item, 0.125);
            pManager[3].Optional = true;
            pManager.AddNumberParameter("Not Used", "-", "Parameter not used by this filter", GH_ParamAccess.item, 0.125);
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

            double numValB = 1.0;
            DA.GetData(3, ref numValB);

            double numValC = 1.0;
            DA.GetData(3, ref numValC);

            Filter filter = new Filter();

            int[] indices = new int[] { 2, 3, 4 };

            switch ((FilterModes)mode)
            {
                case FilterModes.BT709:
                    ClearParameter(indices);
                    filter = new GrayscaleBT709();
                    image.Filters.Add(new GrayscaleBT709());
                    break;
                case FilterModes.RMY:
                    ClearParameter(indices);
                    filter = new GrayscaleRMY();
                    image.Filters.Add(new GrayscaleRMY());
                    break;
                case FilterModes.Y:
                    ClearParameter(indices);
                    filter = new GrayscaleY();
                    image.Filters.Add(new GrayscaleY());
                    break;
                case FilterModes.Simple:
                    SetParameter(2, "R", "Red", "The Red coefficient");
                    SetParameter(3, "G", "Green", "The Green coefficient");
                    SetParameter(4, "B", "Blue", "The Blue coefficient");
                    filter = new Simple(numValA, numValB, numValC);
                    image.Filters.Add(new Simple(numValA, numValB, numValC));
                    break;
            }

            DA.SetData(0, image);
            DA.SetData(1, image.GetFilteredBitmap());
            DA.SetData(2, filter);
        }

        protected void ClearParameter(int[] indices)
        {
            foreach (int i in indices)
            {
                SetParameter(i);
            }
        }

        protected void SetParameter(int index, string nickname = "-", string name = "Not Used", string description = "Parameter not used by this filter")
        {
            Params.Input[index].NickName = nickname;
            Params.Input[index].Name = name;
            Params.Input[index].Description = description;
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
                return Properties.Resources.Grayscale1;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("fa3e2621-02c5-4f88-a5dd-f4e6fe2c0956"); }
        }
    }
}