using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Aviary.Macaw.GH.Filters
{
    public class ApplyFilters : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ApplyFilters class.
        /// </summary>
        public ApplyFilters()
          : base("Apply Filters", "Apply", "Apply filters to the image's bitmap" + Environment.NewLine + "Built on the Accord Imaging Library" + Environment.NewLine + "http://accord-framework.net/", "Aviary 1", "Image")
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
            pManager.AddGenericParameter("Image", "I", "The Image or Bitmap for the layer", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Clear Filters", "C", "", GH_ParamAccess.item, false);
            pManager[1].Optional = true;
            pManager.AddIntegerParameter("Loops", "L", "", GH_ParamAccess.item, 0);
            pManager[2].Optional = true;
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
            Image image = new Image();
            if (!DA.GetData(0, ref image)) return;
            Image output = new Image(image);

            int loops = 0;
            DA.GetData(2, ref loops);

            Bitmap bitmap = output.GetFilteredBitmap(loops);
            

            DA.SetData(0, new Image(bitmap));
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
                return Properties.Resources.Base;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("b390afd0-9776-4a57-8c7a-0561e7955234"); }
        }
    }
}