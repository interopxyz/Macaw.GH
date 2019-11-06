using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

using Aviary.Macaw.Filters;
using Grasshopper.Kernel.Types;
using Aviary.Macaw.Filters.Effects;

namespace Aviary.Macaw.GH.Filters
{
    public class FilterEffects : GH_Component
    {

        private enum FilterModes { Additive, SaltPepper, Daube, Jitter, Kuwahara, Posterize, Blur, GaussianBlur, Pixellate }
        /// <summary>
        /// Initializes a new instance of the AdjustFilters class.
        /// </summary>
        public FilterEffects()
          : base("Filter Effects", "Effects", "Apply various effect filters to an image" + Environment.NewLine + "Note: Not all filter modes use the additional parameter inputs." + Environment.NewLine + "Built on the Accord Imaging Library" + Environment.NewLine + "http://accord-framework.net/", "Aviary 1", "Image")
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
            pManager.AddNumberParameter("Not Used", "-", "Parameter not used by this filter", GH_ParamAccess.item, 1.0);
            pManager[2].Optional = true;
            pManager.AddNumberParameter("Not Used", "-", "Parameter not used by this filter", GH_ParamAccess.item, 1.0);
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

            double numValA = 0;
            DA.GetData(2, ref numValA);

            double numValB = 0;
            DA.GetData(3, ref numValB);

            Filter filter = new Filter();

            switch ((FilterModes)mode)
            {
                case FilterModes.Additive:
                    SetParameter(2);
                    SetParameter(3);
                    filter = new Additive();
                    image.Filters.Add(new Additive());
                    break;
                case FilterModes.Daube:
                    SetParameter(2,"S","Size", "[0-1] Unitized adjustment value");
                    SetParameter(3);
                    filter = new Daube(numValA);
                    image.Filters.Add(new Daube(numValA));
                    break;
                case FilterModes.SaltPepper:
                    SetParameter(2, "N", "Noise", "[0-1] Unitized adjustment value");
                    SetParameter(3);
                    filter = new SaltPepper(numValA);
                    image.Filters.Add(new SaltPepper(numValA));
                    break;
                case FilterModes.Jitter:
                    SetParameter(2, "R", "Radius", "[0-1] Unitized adjustment value");
                    SetParameter(3);
                    filter = new Jitter(numValA);
                    image.Filters.Add(new Jitter(numValA));
                    break;
                case FilterModes.Kuwahara:
                    SetParameter(2, "S", "Size", "[0-1] Unitized adjustment value");
                    SetParameter(3);
                    filter = new Kuwahara(numValA);
                    image.Filters.Add(new Kuwahara(numValA));
                    break;
                case FilterModes.Posterize:
                    SetParameter(2, "I", "Interval", "[0-1] Unitized adjustment value");
                    SetParameter(3);
                    filter = new Posterize(numValA);
                    image.Filters.Add(new Posterize(numValA));
                    break;
                case FilterModes.GaussianBlur:
                    SetParameter(2, "X", "Sigma", "[0-1] Unitized adjustment value");
                    SetParameter(3, "S", "Size", "[0-1] Unitized adjustment value");
                    filter = new GaussianBlur(numValA, numValB);
                    image.Filters.Add(new GaussianBlur(numValA, numValB));
                    break;
                case FilterModes.Pixellate:
                    SetParameter(2, "W", "Width", "[0-1] Unitized adjustment value");
                    SetParameter(3, "H", "Height", "[0-1] Unitized adjustment value");
                    filter = new Pixellate(numValA, numValB);
                    image.Filters.Add(new Pixellate(numValA, numValB));
                    break;
                case FilterModes.Blur:
                    SetParameter(2, "D", "Divisor", "[0-1] Unitized adjustment value");
                    SetParameter(3, "T", "Threshold", "[0-1] Unitized adjustment value");
                    filter = new Blur(numValA, numValB);
                    image.Filters.Add(new Blur(numValA, numValB));
                    break;
            }

            DA.SetData(0, image);
            DA.SetData(1, filter);
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
                return Properties.Resources.Effects1;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("97575abe-3ac9-4c67-9541-63041a127d4e"); }
        }
    }
}