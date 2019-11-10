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
        private enum FilterModes { Adaptive, Conservative, Median, Gaussian, HighBoost, Mean, Simple }

        /// <summary>
        /// Initializes a new instance of the FilterSharpen class.
        /// </summary>
        public FilterSharpen()
          : base("Filter Sharpen", "Sharpen", "Apply sharpen filters to an image" + Environment.NewLine + "Note: Not all filter modes use the additional parameter inputs." + Environment.NewLine + "Built on the Accord Imaging Library" + Environment.NewLine + "http://accord-framework.net/", "Aviary 1", "Image")
        {
        }

        string message = FilterModes.Adaptive.ToString();

        private void UpdateMessage()
        {
            Message = message;
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
            pManager.AddGenericParameter("Image", "I", "An Aviary Image or Bitmap", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Mode", "M", "Select filter type", GH_ParamAccess.item, 0);
            pManager[1].Optional = true;
            pManager.AddIntegerParameter("Not Used", "-", "Parameter not used by this filter", GH_ParamAccess.item, 1);
            pManager[2].Optional = true;
            pManager.AddIntegerParameter("Not Used", "-", "Parameter not used by this filter", GH_ParamAccess.item, 1);
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
            pManager.AddGenericParameter("Image", "I", "An Aviary Image with the filter added to it", GH_ParamAccess.item);
            pManager.AddGenericParameter("Filter", "F", "The specified Filter", GH_ParamAccess.item);
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

            int[] indices = new int[] { 2, 3 };

            switch ((FilterModes)mode)
            {
                case FilterModes.Adaptive:
                    ClearParameters(indices);
                    filter = new Adaptive();
                    image.Filters.Add(new Adaptive());
                    break;
                case FilterModes.Conservative:
                    ClearParameters(indices);
                    filter = new Conservative();
                    image.Filters.Add(new Conservative());
                    break;
                case FilterModes.Median:
                    ClearParameters(indices);
                    filter = new Median();
                    image.Filters.Add(new Median());
                    break;
                case FilterModes.Gaussian:
                    SetParameter(2, "D", "Divisor", "Division factor");
                    SetParameter(3, "T", "Threshold", "Threshold weighted sum");
                    filter = new Gaussian(numValA,numValB);
                    image.Filters.Add(new Gaussian(numValA, numValB));
                    break;
                case FilterModes.HighBoost:
                    SetParameter(2, "D", "Divisor", "Division factor");
                    SetParameter(3, "T", "Threshold", "Threshold weighted sum");
                    filter = new HighBoost(numValA, numValB);
                    image.Filters.Add(new HighBoost(numValA, numValB));
                    break;
                case FilterModes.Mean:
                    SetParameter(2, "D", "Divisor", "Division factor");
                    SetParameter(3, "T", "Threshold", "Threshold weighted sum");
                    filter = new Mean(numValA, numValB);
                    image.Filters.Add(new Mean(numValA, numValB));
                    break;
                case FilterModes.Simple:
                    SetParameter(2, "D", "Divisor", "Division factor");
                    SetParameter(3, "T", "Threshold", "Threshold weighted sum");
                    filter = new Simple(numValA, numValB);
                    image.Filters.Add(new Simple(numValA, numValB));
                    break;
            }

            message = ((FilterModes)mode).ToString();
            UpdateMessage();

            DA.SetData(0, image);
            DA.SetData(1, filter);
        }

        protected void ClearParameters(int[] indices)
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
                return Properties.Resources.Sharpen1;
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