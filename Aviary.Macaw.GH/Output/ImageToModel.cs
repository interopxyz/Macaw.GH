using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Aviary.Macaw.GH.Output
{
    public class ImageToModel : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ImageToModel class.
        /// </summary>
        public ImageToModel()
          : base("Image To Model", "Img Mesh", "Description", "Aviary 1", "Image")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("FilePath", "F", "", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Plane", "P", "---", GH_ParamAccess.item, Plane.WorldXY);
            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddMeshParameter("Mesh", "M", "---", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string P = String.Empty;
            if (!DA.GetData(0, ref P)) return;

            Plane plane = Plane.WorldXY;
            if (!DA.GetData(1, ref plane)) return;

            Bitmap bitmap = GetBitmap.GetBitmapFromFile(P);

            int width = bitmap.Width;
            int height = bitmap.Height;

            Mesh mesh = Mesh.CreateFromPlane(plane, new Interval(0, width), new Interval(0, height), 1, 1);
            
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
            get { return new Guid("89c4132b-c117-4e02-8cb1-3bb94d00a375"); }
        }
    }
}