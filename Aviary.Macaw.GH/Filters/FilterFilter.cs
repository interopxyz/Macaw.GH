using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

using Aviary.Macaw.Filters;
using Aviary.Macaw.Filters.Filtering;

namespace Aviary.Macaw.GH.Filters
{
    public class FilterFilter : GH_Component
    {
        public enum FilterModes { Channel,RGB,HSL,YCbCr};


        /// <summary>
        /// Initializes a new instance of the FiltersFilter class.
        /// </summary>
        public FilterFilter()
          : base("Filter Filters", "Filters", "Filter out channels on an image" + Environment.NewLine + "Built on the Accord Imaging Library" + Environment.NewLine + "http://accord-framework.net/", "Aviary 1", "Image")
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
            pManager.AddIntegerParameter("Mode", "M", "", GH_ParamAccess.item, 0);
            pManager[1].Optional = true;

            pManager.AddIntervalParameter("Red", "R", "---", GH_ParamAccess.item, new Interval(0, 1));
            pManager[2].Optional = true;
            pManager.AddIntervalParameter("Green", "G", "---", GH_ParamAccess.item, new Interval(0, 1));
            pManager[3].Optional = true;
            pManager.AddIntervalParameter("Blue", "B", "---", GH_ParamAccess.item, new Interval(0, 1));
            pManager[4].Optional = true;

            pManager.AddBooleanParameter("Flip", "F", "---", GH_ParamAccess.item, true);
            pManager[5].Optional = true;

            pManager.AddColourParameter("Color", "C", "---", GH_ParamAccess.item, Color.Black);
            pManager[6].Optional = true;

            Param_Integer paramA = (Param_Integer)pManager[1];
            foreach (FilterModes value in Enum.GetValues(typeof(FilterModes)))
            {
                paramA.AddNamedValue(value.ToString(), (int)value);
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

            Interval valA = new Interval(0,1);
            DA.GetData(2, ref valA);
            Interval valB = new Interval(0, 1);
            DA.GetData(3, ref valB);
            Interval valC = new Interval(0, 1);
            DA.GetData(4, ref valC);

            bool flip = true;
            DA.GetData(5, ref flip);

            Color color = Color.Black;
            DA.GetData(6, ref color);

            Filter filter = new Filter();

            switch ((FilterModes)mode)
            {
                case FilterModes.Channel:
                    SetParameter(2, "R", "Red", "[0-1] Unitized adjustment value");
                    SetParameter(3, "G", "Green", "[0-1] Unitized adjustment value");
                    SetParameter(4, "B", "Blue", "[0-1] Unitized adjustment value");
                    SetParameter(5, "F", "Outside", "Flip between inside and outside range");
                    filter = new Channel(valA.ToDomain(),valB.ToDomain(), valC.ToDomain(), flip);
                    image.Filters.Add(new Channel(valA.ToDomain(), valB.ToDomain(), valC.ToDomain(), flip));
                    break;
                case FilterModes.HSL:
                    SetParameter(2, "H", "Hue", "[0-1] Unitized adjustment value");
                    SetParameter(3, "S", "Saturation", "[0-1] Unitized adjustment value");
                    SetParameter(4, "L", "Luminance", "[0-1] Unitized adjustment value");
                    SetParameter(5, "F", "Outside", "Flip between inside and outside range");
                    SetParameter(6, "C", "Color", "Replacement Color");
                    filter = new ColorFilter(valA.ToDomain(), valB.ToDomain(), valC.ToDomain(), flip,color);
                    image.Filters.Add(new ColorFilter(valA.ToDomain(), valB.ToDomain(), valC.ToDomain(), flip,color));
                    break;
                case FilterModes.RGB:
                    SetParameter(2, "R", "Red", "[0-1] Unitized adjustment value");
                    SetParameter(3, "G", "Green", "[0-1] Unitized adjustment value");
                    SetParameter(4, "B", "Blue", "[0-1] Unitized adjustment value");
                    SetParameter(5, "F", "Outside", "Flip between inside and outside range");
                    SetParameter(6, "C", "Color", "Replacement Color");
                    filter = new HSL(valA.ToDomain(), valB.ToDomain(), valC.ToDomain(), flip,color);
                    image.Filters.Add(new HSL(valA.ToDomain(), valB.ToDomain(), valC.ToDomain(), flip,color));
                    break;
                case FilterModes.YCbCr:
                    SetParameter(2, "Y", "Y", "[0-1] Unitized adjustment value");
                    SetParameter(3, "Cb", "Cb", "[0-1] Unitized adjustment value");
                    SetParameter(4, "Cr", "Cr", "[0-1] Unitized adjustment value");
                    SetParameter(5, "F", "Outside", "Flip between inside and outside range");
                    SetParameter(6, "C", "Color", "Replacement Color");
                    filter = new YCbCr(valA.ToDomain(), valB.ToDomain(), valC.ToDomain(), flip,color);
                    image.Filters.Add(new YCbCr(valA.ToDomain(), valB.ToDomain(), valC.ToDomain(), flip, color));
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
                return Properties.Resources.Filters1;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("8aec8998-8929-497e-bf2c-5ec96272b208"); }
        }
    }
}