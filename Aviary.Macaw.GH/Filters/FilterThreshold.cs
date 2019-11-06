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
        private enum FilterModes { Otsu, SIS, Bradley, Iterative, Nilback, Sauvola, WolfJolion}

        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public FilterThreshold()
          : base("Filter Threshold", "Threshold", "Apply threshold detection filters to an image" + Environment.NewLine + "Note: Not all filter modes use the additional parameter inputs." + Environment.NewLine + "Built on the Accord Imaging Library" + Environment.NewLine + "http://accord-framework.net/", "Aviary 1", "Image")
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
            pManager.AddGenericParameter("Image", "I", "An Aviary Image or Bitmap", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Mode", "M", "Select filter type", GH_ParamAccess.item, 0);
            pManager[1].Optional = true;
            pManager.AddNumberParameter("Not Used", "-", "Parameter not used by this filter", GH_ParamAccess.item, 0.5);
            pManager[2].Optional = true;
            pManager.AddNumberParameter("Not Used", "-", "Parameter not used by this filter", GH_ParamAccess.item, 0.5);
            pManager[3].Optional = true;
            pManager.AddIntegerParameter("Not Used", "-", "Parameter not used by this filter", GH_ParamAccess.item, 1);
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

            double numValA = 0.5;
            DA.GetData(2, ref numValA);

            double numValB = 0.5;
            DA.GetData(3, ref numValB);

            int numValC = 1;
            DA.GetData(4, ref numValC);

            Filter filter = new Filter();

            int[] indices = new int[] { 2, 3, 4 };

            switch ((FilterModes)mode)
            {
                case FilterModes.Otsu:
                    filter = new Otsu();
                    ClearParameters(indices);
                    image.Filters.Add(new Otsu());
                    break;
                case FilterModes.SIS:
                    filter = new SIS();
                    ClearParameters(indices);
                    image.Filters.Add(new SIS());
                    break;
                case FilterModes.Bradley:
                    SetParameter(2, "B", "Brightness", "Brightness difference limit");
                    SetParameter(3, "S", "Size", "Window size");
                    SetParameter(4);
                    filter = new Bradley(numValA,(int)numValB);
                    image.Filters.Add(new Bradley(numValA, (int)numValB));
                    break;
                case FilterModes.Iterative:
                    SetParameter(2, "M", "Minimum", "Minimum error value");
                    SetParameter(3, "T", "Threshold", "Threshold value");
                    SetParameter(4);
                    filter = new Iterative(numValA, numValB);
                    image.Filters.Add(new Iterative(numValA, numValB));
                    break;
                case FilterModes.Nilback:
                    SetParameter(2, "C", "C", "Mean offset C");
                    SetParameter(3, "K", "K", "Parameter K");
                    SetParameter(4, "R", "Radius", "Filter convolution radius");
                    filter = new Nilback(numValA, numValB, numValC);
                    image.Filters.Add(new Nilback(numValA, numValB,numValC));
                    break;
                case FilterModes.Sauvola:
                    SetParameter(2, "R", "R", "Dynamic range");
                    SetParameter(3, "K", "K", "Parameter K");
                    SetParameter(4, "R", "Radius", "Filter convolution radius");
                    filter = new Sauvola(numValA, numValB,numValC);
                    image.Filters.Add(new Sauvola(numValA, numValB, numValC));
                    break;
                case FilterModes.WolfJolion:
                    SetParameter(2, "R", "R", "Dynamic range");
                    SetParameter(3, "K", "K", "Parameter K");
                    SetParameter(4, "R", "Radius", "Filter convolution radius");
                    filter = new WolfJolion(numValA, numValB,numValC);
                    image.Filters.Add(new WolfJolion(numValA, numValB, numValC));
                    break;
            }

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
                return Properties.Resources.Threshold1;
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