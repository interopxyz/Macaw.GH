using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Aviary.Macaw.GH.Output
{
    public class ImageProperties : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ImageProperties class.
        /// </summary>
        public ImageProperties()
          : base("Image Properties", "Properties", "Get the overall bitmap dimensions", "Aviary 1", "Image")
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
            pManager.AddGenericParameter("Bitmap", "B", "A bitmap object", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("Width", "W", "The width of the image in pixels", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Height", "H", "The height of the image in pixels", GH_ParamAccess.item);
            pManager.AddIntegerParameter("DPI X", "X", "The horizontal dpi", GH_ParamAccess.item);
            pManager.AddIntegerParameter("DPI Y", "Y", "The vertical dpi", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IGH_Goo goo = null;
            Bitmap bitmap = new Bitmap(100, 100);
            if (!DA.GetData(0, ref goo)) return;
            if (!goo.TryGetBitmap(ref bitmap)) return;

            DA.SetData(0, bitmap.Width);
            DA.SetData(1, bitmap.Height);
            DA.SetData(2, bitmap.HorizontalResolution);
            DA.SetData(3, bitmap.VerticalResolution);

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
                return Properties.Resources.BitmapProperties;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("a2066f0b-26f7-40fe-a75e-26c4e4c302fc"); }
        }
    }
}