using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using Aviary.Macaw.Filters.Channels;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;

namespace Aviary.Macaw.GH.Filters
{
    public class FilterReplaceChannels : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the FilterReplaceChannels class.
        /// </summary>
        public FilterReplaceChannels()
          : base("Filter Replace Channels", "Replace", "Description", "Aviary 1", "Image")
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
            pManager.AddGenericParameter("Bitmap", "B", "", GH_ParamAccess.item);

            Param_Integer param = (Param_Integer)pManager[1];
            foreach (Replace.Modes value in Enum.GetValues(typeof(Replace.Modes)))
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

            IGH_Goo gooA = null;
            Image imageA = new Image();
            if (!DA.GetData(2, ref gooA)) return;
            if (!goo.TryGetImage(ref imageA)) return;

            Filter filter = new Replace((Replace.Modes)mode,imageA.GetFilteredBitmap());
            image.Filters.Add(new Replace((Replace.Modes)mode, imageA.GetFilteredBitmap()));

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
            get { return new Guid("af6e94c4-2519-45eb-9b6d-915bc17da469"); }
        }
    }
}