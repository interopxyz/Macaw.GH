using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Aviary.Macaw.GH
{
    public class TraceImage : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the TraceImage class.
        /// </summary>
        public TraceImage()
          : base("Trace Image", "Trace", "---", "Aviary 1", "Image")
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
            pManager.AddGenericParameter("Image", "I", "An Aviary Image or Bitmap", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Mode", "M", "Set the ", GH_ParamAccess.item, 0);
            pManager[1].Optional = true;
            pManager.AddIntegerParameter("Size", "S", "The pixel sample size", GH_ParamAccess.item, 2);
            pManager[2].Optional = true;
            pManager.AddNumberParameter("Error", "E", "The error tolerance [0-1]", GH_ParamAccess.item, 0.2);
            pManager[3].Optional = true;
            pManager.AddNumberParameter("Threshold", "T", "Unitized brightness threshold [0-1]", GH_ParamAccess.item, 0.9);
            pManager[4].Optional = true;
            pManager.AddNumberParameter("Alpha", "A", "Corner detection threshold [0-1]", GH_ParamAccess.item, 1.0);
            pManager[5].Optional = true;

            Param_Integer param = (Param_Integer)pManager[1];
            foreach (TraceBitmap.TurnModes value in Enum.GetValues(typeof(TraceBitmap.TurnModes)))
            {
                param.AddNamedValue(value.ToString(), (int)value);
            }
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Curves", "C", "---", GH_ParamAccess.list);
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

            Bitmap bmp = bitmap.ToAccordBitmap(Filter.ImageTypes.Rgb32bpp);

            int mode = 0;
            DA.GetData(1, ref mode);

            int size = 10;
            DA.GetData(2, ref size);

            double tolerance = 1.0;
            DA.GetData(3, ref tolerance);

            double threshold = 1.0;
            DA.GetData(4, ref threshold);

            double alpha = 1.0;
            DA.GetData(5, ref alpha);

            List<Polyline> polylines = bmp.TraceToRhino(threshold, alpha, tolerance, size, false, (TraceBitmap.TurnModes)mode);

            DA.SetDataList(0, polylines);
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
                return Properties.Resources.TraceBitmap;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("6be19558-404e-451b-8674-5e826a723815"); }
        }
    }
}