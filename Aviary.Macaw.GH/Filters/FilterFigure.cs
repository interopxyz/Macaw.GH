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
            pManager.AddBooleanParameter("Not Used", "-", "Parameter not used by this filter", GH_ParamAccess.item, false);
            pManager[2].Optional = true;
            pManager.AddIntegerParameter("Not Used", "-", "Parameter not used by this filter", GH_ParamAccess.item, 1);
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
                    SetParameter(2);
                    SetParameter(3);
                    SetParameter(4);
                    filter = new Closing();
                    image.Filters.Add(new Closing());
                    break;
                case FilterModes.Dilation:
                    SetParameter(2);
                    SetParameter(3);
                    SetParameter(4);
                    filter = new Dilation();
                    image.Filters.Add(new Dilation());
                    break;
                case FilterModes.DilationBinary:
                    SetParameter(2);
                    SetParameter(3);
                    SetParameter(4);
                    filter = new DilationBinary();
                    image.Filters.Add(new DilationBinary());
                    break;
                case FilterModes.Erosion:
                    SetParameter(2);
                    SetParameter(3);
                    SetParameter(4);
                    filter = new Erosion();
                    image.Filters.Add(new Erosion());
                    break;
                case FilterModes.ErosionBinary:
                    SetParameter(2);
                    SetParameter(3);
                    SetParameter(4);
                    filter = new ErosionBinary();
                    image.Filters.Add(new ErosionBinary());
                    break;
                case FilterModes.HatBottom:
                    SetParameter(2);
                    SetParameter(3);
                    SetParameter(4);
                    filter = new HatBottom();
                    image.Filters.Add(new HatBottom());
                    break;
                case FilterModes.HatTop:
                    SetParameter(2);
                    SetParameter(3);
                    SetParameter(4);
                    filter = new HatTop();
                    image.Filters.Add(new HatTop());
                    break;
                case FilterModes.Opening:
                    SetParameter(2);
                    SetParameter(3);
                    SetParameter(4);
                    filter = new Opening();
                    image.Filters.Add(new Opening());
                    break;
                case FilterModes.Skeletonization:
                    SetParameter(2);
                    SetParameter(3);
                    SetParameter(4);
                    filter = new Skeletonization();
                    image.Filters.Add(new Skeletonization());
                    break;
                case FilterModes.SkeletonizationZhangSuen:
                    SetParameter(2);
                    SetParameter(3);
                    SetParameter(4);
                    filter = new SkeletonizationZhangSuen();
                    image.Filters.Add(new SkeletonizationZhangSuen());
                    break;
                case FilterModes.HorizontalBands:
                    SetParameter(2, "B", "Borders", "Process gaps");
                    SetParameter(3, "G", "Gap", "The pixel gap size");
                    SetParameter(4);
                    filter = new BandsHorizontal(valueB, valueA);
                    image.Filters.Add(new BandsHorizontal(valueB, valueA));
                    break;
                case FilterModes.VerticalBands:
                    SetParameter(2, "B", "Borders", "Process gaps");
                    SetParameter(3, "G", "Gap", "The pixel gap size");
                    SetParameter(4);
                    filter = new BandsVertical(valueB,valueA);
                    image.Filters.Add(new BandsVertical(valueB, valueA));
                    break;
                case FilterModes.FillHoles:
                    SetParameter(2, "B", "Borders", "Process gaps");
                    SetParameter(3, "W", "Width", "The pixel threshold");
                    SetParameter(4, "H", "Height", "The pixel threshold");
                    filter = new FillHoles(valueC,valueB,valueA);
                    image.Filters.Add(new FillHoles(valueC, valueB, valueA));
                    break;
            }

            DA.SetData(0, image);
            DA.SetData(1, image.GetFilteredBitmap());
            DA.SetData(2, filter);
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