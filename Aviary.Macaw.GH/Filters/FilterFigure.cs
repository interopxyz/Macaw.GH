using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

using Aviary.Macaw.Filters;
using Aviary.Macaw.Filters.Figures;

namespace Aviary.Macaw.GH.Filters
{
    public class FilterFigure : GH_Component
    {

        private enum FilterModes { Closing, Dilation, DilationBinary, Erosion, ErosionBinary, HatBottom, HatTop, Opening, Skeletonization, SkeletonizationZhangSuen, HorizontalBands,VerticalBands,FillHoles}

        /// <summary>
        /// Initializes a new instance of the FilterFigure class.
        /// </summary>
        public FilterFigure()
          : base("Figure Filters", "Figure", "Description", "Aviary 1", "Image")
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
            pManager.AddBooleanParameter("Value A", "A", "", GH_ParamAccess.item, false);
            pManager[2].Optional = true;
            pManager.AddIntegerParameter("Value B", "B", "", GH_ParamAccess.item, 1);
            pManager[3].Optional = true;
            pManager.AddIntegerParameter("Value C", "C", "", GH_ParamAccess.item, 1);
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

            bool valueA = false;
            DA.GetData(2, ref valueA);

            int valueB = 1;
            DA.GetData(3, ref valueB);

            int valueC = 1;
            DA.GetData(4, ref valueC);

            Filter filter = new Filter();

            switch ((FilterModes)mode)
            {
                case FilterModes.Closing:
                    filter = new Closing();
                    image.Filters.Add(new Closing());
                    break;
                case FilterModes.Dilation:
                    filter = new Dilation();
                    image.Filters.Add(new Dilation());
                    break;
                case FilterModes.DilationBinary:
                    filter = new DilationBinary();
                    image.Filters.Add(new DilationBinary());
                    break;
                case FilterModes.Erosion:
                    filter = new Erosion();
                    image.Filters.Add(new Erosion());
                    break;
                case FilterModes.ErosionBinary:
                    filter = new ErosionBinary();
                    image.Filters.Add(new ErosionBinary());
                    break;
                case FilterModes.HatBottom:
                    filter = new HatBottom();
                    image.Filters.Add(new HatBottom());
                    break;
                case FilterModes.HatTop:
                    filter = new HatTop();
                    image.Filters.Add(new HatTop());
                    break;
                case FilterModes.Opening:
                    filter = new Opening();
                    image.Filters.Add(new Opening());
                    break;
                case FilterModes.Skeletonization:
                    filter = new Skeletonization();
                    image.Filters.Add(new Skeletonization());
                    break;
                case FilterModes.SkeletonizationZhangSuen:
                    filter = new SkeletonizationZhangSuen();
                    image.Filters.Add(new SkeletonizationZhangSuen());
                    break;
                case FilterModes.HorizontalBands:
                    filter = new BandsHorizontal(valueB, valueA);
                    image.Filters.Add(new BandsHorizontal(valueB, valueA));
                    break;
                case FilterModes.VerticalBands:
                    filter = new BandsVertical(valueB,valueA);
                    image.Filters.Add(new BandsVertical(valueB, valueA));
                    break;
                case FilterModes.FillHoles:
                    filter = new FillHoles(valueB,valueC,valueA);
                    image.Filters.Add(new FillHoles(valueB, valueC, valueA));
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
            get { return new Guid("c820fa53-7976-4384-98a0-b3e10f432181"); }
        }
    }
}