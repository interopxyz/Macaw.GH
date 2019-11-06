using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

using Aviary.Macaw;
using Aviary.Macaw.Filters.Levels;

using Aviary.Wind;

using Grasshopper.Kernel.Types;

namespace Aviary.Macaw.GH.Filters
{
    public class FilterLevels : GH_Component
    {
        private enum FilterModes { HSL, RGB, YCbCr }

        /// <summary>
        /// Initializes a new instance of the FiltersLevels class.
        /// </summary>
        public FilterLevels()
          : base("Filter Levels", "Levels", "Modify the levels of an image" + Environment.NewLine + "Note: Not all filter modes use the additional parameter inputs." + Environment.NewLine + "Built on the Accord Imaging Library" + Environment.NewLine + "http://accord-framework.net/", "Aviary 1", "Image")
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
            pManager.AddIntervalParameter("Saturation In", "Si", "Domain [0,1]", GH_ParamAccess.item, new Interval(0,1));
            pManager[2].Optional = true;
            pManager.AddIntervalParameter("Saturation Out", "So", "Domain [0,1]", GH_ParamAccess.item, new Interval(0, 1));
            pManager[3].Optional = true;
            pManager.AddIntervalParameter("Luminance In", "Li", "Domain [0,1]", GH_ParamAccess.item, new Interval(0, 1));
            pManager[4].Optional = true;
            pManager.AddIntervalParameter("Luminance Out", "Lo", "Domain [0,1]", GH_ParamAccess.item, new Interval(0, 1));
            pManager[5].Optional = true;
            pManager.AddIntervalParameter("Not Used", "-", "Parameter not used by this filter", GH_ParamAccess.item, new Interval(0, 1));
            pManager[6].Optional = true;
            pManager.AddIntervalParameter("Not Used", "-", "Parameter not used by this filter", GH_ParamAccess.item, new Interval(0, 1));
            pManager[7].Optional = true;

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

            Interval numValA = new Interval(0,1);
            DA.GetData(2, ref numValA);

            Interval numValB = new Interval(0,1);
            DA.GetData(3, ref numValB);

            Interval numValC = new Interval(0, 1);
            DA.GetData(4, ref numValC);

            Interval numValD = new Interval(0, 1);
            DA.GetData(5, ref numValD);

            Interval numValE = new Interval(0, 1);
            DA.GetData(6, ref numValE);

            Interval numValF = new Interval(0, 1);
            DA.GetData(7, ref numValF);

            Filter filter = new Filter();

            switch ((FilterModes)mode)
            {
                case FilterModes.HSL:
                    SetParameter(2, "Si", "Saturation In", "Domain [0,1]");
                    SetParameter(3, "So", "Saturation Out", "Domain [0,1]");
                    SetParameter(4, "Li", "Luminance In", "Domain [0,1]");
                    SetParameter(5, "Lo", "Luminance Out", "Domain [0,1]");
                    SetParameter(6, "-", "Not Used", "Parameter not used by this filter");
                    SetParameter(7, "-", "Not Used", "Parameter not used by this filter");
                    filter = new HSL(numValA.ToDomain(), numValB.ToDomain(), numValC.ToDomain(), numValD.ToDomain());
                    image.Filters.Add(new HSL(numValA.ToDomain(), numValB.ToDomain(), numValC.ToDomain(), numValD.ToDomain()));
                    break;
                case FilterModes.RGB:
                    SetParameter(2, "Ri", "Red In", "Domain [0,1]");
                    SetParameter(3, "Ro", "Red Out", "Domain [0,1]");
                    SetParameter(4, "Gi", "Green In", "Domain [0,1]");
                    SetParameter(5, "Go", "Green Out", "Domain [0,1]");
                    SetParameter(6, "Bi", "Blue In", "Domain [0,1]");
                    SetParameter(7, "Bo", "Blue Out", "Domain [0,1]");
                    filter = new RGB(numValA.ToDomain(), numValB.ToDomain(), numValC.ToDomain(), numValD.ToDomain(), numValE.ToDomain(), numValF.ToDomain());
                    image.Filters.Add(new RGB(numValA.ToDomain(), numValB.ToDomain(), numValC.ToDomain(), numValD.ToDomain(), numValE.ToDomain(), numValF.ToDomain()));
                    break;
                case FilterModes.YCbCr:
                    SetParameter(2, "Ri", "Red In", "Domain [0,1]");
                    SetParameter(3, "Ro", "Red Out", "Domain [0,1]");
                    SetParameter(4, "Yi", "Y In", "Domain [0,1]");
                    SetParameter(5, "Yo", "Y Out", "Domain [0,1]");
                    SetParameter(6, "Bi", "Blue In", "Domain [0,1]");
                    SetParameter(7, "Bo", "Blue Out", "Domain [0,1]");
                    filter = new YCbCr(numValC.ToDomain(), numValD.ToDomain(), numValE.ToDomain(), numValF.ToDomain(), numValA.ToDomain(), numValB.ToDomain());
                    image.Filters.Add(new YCbCr(numValC.ToDomain(), numValD.ToDomain(), numValE.ToDomain(), numValF.ToDomain(), numValA.ToDomain(), numValB.ToDomain()));
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
                return Properties.Resources.Levels1;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("16074c9b-12f2-4335-977a-80bfe616eb87"); }
        }
    }
}