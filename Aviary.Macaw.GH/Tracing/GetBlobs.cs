using System;
using System.Collections.Generic;
using System.Drawing;
using Aviary.Macaw.Tracing;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

using Aviary.Wind;

namespace Aviary.Macaw.GH.Tracing
{
    public class GetBlobs : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GetBlobs class.
        /// </summary>
        public GetBlobs()
          : base("Blob Boundaries", "Boundaries", "Get blob boundaries from a bitmap" + Environment.NewLine + "Built on the Accord Imaging Library" + Environment.NewLine + "http://accord-framework.net/", "Aviary 1", "Image")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.senary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Image", "I", "---", GH_ParamAccess.item);
            pManager.AddIntervalParameter("Width Domain", "W", "---", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddIntervalParameter("Height Domain", "H", "---", GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager.AddColourParameter("Background Color", "C", "---", GH_ParamAccess.item);
            pManager[3].Optional = true;
            pManager.AddBooleanParameter("Limit", "L", "---", GH_ParamAccess.item);
            pManager[4].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddRectangleParameter("Boundaries", "R", "---", GH_ParamAccess.list);
            pManager.AddGenericParameter("Bitmap", "B", "---", GH_ParamAccess.list);
            pManager.AddColourParameter("Colors", "C", "---", GH_ParamAccess.list);
            pManager.AddPointParameter("Points", "P", "---", GH_ParamAccess.list);
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

            Blobs blobs = new Blobs();

            Interval width = new Interval();
            if(DA.GetData(1,ref width))
            {
                blobs.MinWidth = (int)width.T0;
                blobs.MaxWidth = (int)width.T1;
            }

            Interval height = new Interval();
            if (DA.GetData(2, ref height))
            {
                blobs.MinHeight = (int)height.T0;
                blobs.MaxHeight = (int)height.T1;
            }

            Color color = new Color();
            if (DA.GetData(3, ref color)) blobs.BackgroundColor = color;

            bool limit = false;
            if (DA.GetData(4, ref limit)) blobs.Coupled = limit;

            blobs.CalculateBlobs(bitmap);
            
            DA.SetDataList(0, blobs.GetBoundaries());
            DA.SetDataList(1, blobs.GetImages());
            DA.SetDataList(2, blobs.GetColors());
            DA.SetDataList(3, blobs.GetPoints());
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
                return Properties.Resources.TraceBlobsA;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("f1ee54fe-85b0-46bf-ae35-98244258e085"); }
        }
    }
}