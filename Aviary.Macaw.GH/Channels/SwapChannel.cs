using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Aviary.Macaw.GH.Channels
{
    public class SwapChannel : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the SwapChannel class.
        /// </summary>
        public SwapChannel()
          : base("Swap Channel", "Swap", "Description", "Aviary 1", "Image")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        public enum LimitedChannels { Alpha, Red, Green, Blue}

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Image", "I", "The Layer Bitmap", GH_ParamAccess.item);
            
            pManager.AddIntegerParameter("Source", "S", "", GH_ParamAccess.item, 0);
            pManager[1].Optional = true;

            pManager.AddIntegerParameter("Target", "T", "", GH_ParamAccess.item, 0);
            pManager[2].Optional = true;

            Param_Integer paramA = (Param_Integer)pManager[1];
            foreach (Image.Channels value in Enum.GetValues(typeof(Image.Channels)))
            {
                paramA.AddNamedValue(value.ToString(), (int)value);
            }

            Param_Integer paramB = (Param_Integer)pManager[2];
            foreach (LimitedChannels value in Enum.GetValues(typeof(LimitedChannels)))
            {
                paramB.AddNamedValue(value.ToString(), (int)value);
            }

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Image", "I", "The resulting image", GH_ParamAccess.item);
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

            int source = 0;
            DA.GetData(1, ref source);

            int target = 0;
            DA.GetData(2, ref target);

            image.SwapChannel((Image.Channels)source, (Image.Channels)target);

            DA.SetData(0, image);
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
            get { return new Guid("9b8d8b65-1abc-4133-8bb0-bc58e7bc0946"); }
        }
    }
}