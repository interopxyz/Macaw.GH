using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Aviary.Macaw.GH.Channels
{
    public class SwapChannels : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the SwapChannels class.
        /// </summary>
        public SwapChannels()
          : base("Swap Channels", "Swap*", "Description", "Aviary 1", "Image")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Image", "I", "The Layer Bitmap", GH_ParamAccess.item);

            pManager.AddIntegerParameter("Alpha", "A", "", GH_ParamAccess.item, 0);
            pManager[1].Optional = true;

            pManager.AddIntegerParameter("Red", "R", "", GH_ParamAccess.item, 1);
            pManager[2].Optional = true;

            pManager.AddIntegerParameter("Green", "G", "", GH_ParamAccess.item, 2);
            pManager[3].Optional = true;

            pManager.AddIntegerParameter("Blue", "B", "", GH_ParamAccess.item, 3);
            pManager[4].Optional = true;

            Param_Integer paramA = (Param_Integer)pManager[1];
            foreach (Image.Channels value in Enum.GetValues(typeof(Image.Channels)))
            {
                paramA.AddNamedValue(value.ToString(), (int)value);
            }

            Param_Integer paramB = (Param_Integer)pManager[2];
            foreach (Image.Channels value in Enum.GetValues(typeof(Image.Channels)))
            {
                paramB.AddNamedValue(value.ToString(), (int)value);
            }

            Param_Integer paramC = (Param_Integer)pManager[3];
            foreach (Image.Channels value in Enum.GetValues(typeof(Image.Channels)))
            {
                paramC.AddNamedValue(value.ToString(), (int)value);
            }

            Param_Integer paramD = (Param_Integer)pManager[4];
            foreach (Image.Channels value in Enum.GetValues(typeof(Image.Channels)))
            {
                paramD.AddNamedValue(value.ToString(), (int)value);
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

            int alpha = 0;
            DA.GetData(1, ref alpha);

            int red = 1;
            DA.GetData(2, ref red);

            int green = 2;
            DA.GetData(3, ref green);

            int blue = 3;
            DA.GetData(4, ref blue);

            image.SwapChannels((Image.Channels)alpha, (Image.Channels)red, (Image.Channels)green, (Image.Channels)blue);

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
            get { return new Guid("10f4b41c-bf44-4a8d-aee2-2782e6adc169"); }
        }
    }
}