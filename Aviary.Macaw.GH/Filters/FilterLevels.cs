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
        private enum FilterModes { HSL, RGB, RGB16, YCbCr }

        /// <summary>
        /// Initializes a new instance of the FiltersLevels class.
        /// </summary>
        public FilterLevels()
          : base("Filters Levels", "Levels", "Description", "Aviary 1", "Image")
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
            pManager.AddIntervalParameter("Value A", "A", "---", GH_ParamAccess.item, new Interval(0,1));
            pManager[2].Optional = true;
            pManager.AddIntervalParameter("Value B", "B", "---", GH_ParamAccess.item, new Interval(0, 1));
            pManager[3].Optional = true;
            pManager.AddIntervalParameter("Value C", "C", "---", GH_ParamAccess.item, new Interval(0, 1));
            pManager[4].Optional = true;
            pManager.AddIntervalParameter("Value D", "D", "---", GH_ParamAccess.item, new Interval(0, 1));
            pManager[5].Optional = true;
            pManager.AddIntervalParameter("Value E", "E", "---", GH_ParamAccess.item, new Interval(0, 1));
            pManager[6].Optional = true;
            pManager.AddIntervalParameter("Value F", "F", "---", GH_ParamAccess.item, new Interval(0, 1));
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

            Interval numValA = new Interval(0,1);
            DA.GetData(2, ref numValA);

            Interval numValB = new Interval(0,1);
            DA.GetData(3, ref numValB);

            Interval numValC = new Interval(0, 1);
            DA.GetData(4, ref numValA);

            Interval numValD = new Interval(0, 1);
            DA.GetData(5, ref numValB);

            Interval numValE = new Interval(0, 1);
            DA.GetData(6, ref numValA);

            Interval numValF = new Interval(0, 1);
            DA.GetData(7, ref numValB);

            Filter filter = new Filter();

            switch ((FilterModes)mode)
            {
                case FilterModes.HSL:
                    filter = new HSL(numValA.ToDomain(), numValB.ToDomain(), numValC.ToDomain(), numValD.ToDomain());
                    image.Filters.Add(new HSL(numValA.ToDomain(), numValB.ToDomain(), numValC.ToDomain(), numValD.ToDomain()));
                    break;
                case FilterModes.RGB:
                    filter = new RGB(numValA.ToDomain(), numValB.ToDomain(), numValC.ToDomain(), numValD.ToDomain(), numValE.ToDomain(), numValF.ToDomain());
                    image.Filters.Add(new RGB(numValA.ToDomain(), numValB.ToDomain(), numValC.ToDomain(), numValD.ToDomain(), numValE.ToDomain(), numValF.ToDomain()));
                    break;
                case FilterModes.RGB16:
                    filter = new RGB16(numValA.ToDomain(), numValB.ToDomain(), numValC.ToDomain(), numValD.ToDomain(), numValE.ToDomain(), numValF.ToDomain());
                    image.Filters.Add(new RGB16(numValA.ToDomain(), numValB.ToDomain(), numValC.ToDomain(), numValD.ToDomain(), numValE.ToDomain(), numValF.ToDomain()));
                    break;
                case FilterModes.YCbCr:
                    filter = new YCbCr(numValA.ToDomain(), numValB.ToDomain(), numValC.ToDomain(), numValD.ToDomain(), numValE.ToDomain(), numValF.ToDomain());
                    image.Filters.Add(new YCbCr(numValA.ToDomain(), numValB.ToDomain(), numValC.ToDomain(), numValD.ToDomain(), numValE.ToDomain(), numValF.ToDomain()));
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
            get { return new Guid("16074c9b-12f2-4335-977a-80bfe616eb87"); }
        }
    }
}