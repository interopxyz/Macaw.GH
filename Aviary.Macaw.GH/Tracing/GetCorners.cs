using System;
using System.Collections.Generic;
using System.Drawing;
using Aviary.Macaw.Tracing;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Aviary.Macaw.GH.Tracing
{
    public class GetCorners : GH_Component
    {
        public enum CornerModes { Fast, Susan, Morvec, Harris}

        /// <summary>
        /// Initializes a new instance of the GetCorners class.
        /// </summary>
        public GetCorners()
          : base("Bitmap Corners", "Corners", "Get blob corner points from a bitmap" + Environment.NewLine + "Built on the Accord Imaging Library" + Environment.NewLine + "http://accord-framework.net/", "Aviary 1", "Image")
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
            pManager.AddIntegerParameter("Mode", "M", "---", GH_ParamAccess.item,0);
            pManager[1].Optional = true;
            pManager.AddIntegerParameter("Threshold", "T", "---", GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager.AddNumberParameter("Value", "V", "---", GH_ParamAccess.item);
            pManager[3].Optional = true;

            Param_Integer param = (Param_Integer)pManager[1];
            foreach (CornerModes value in Enum.GetValues(typeof(CornerModes)))
            {
                param.AddNamedValue(value.ToString(), (int)value);
            }
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "P", "Corner Points", GH_ParamAccess.list);
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

            Corners corners = new Corners(bitmap);

            int threshold = 10;
            if (DA.GetData(2, ref threshold)) corners.Threshold = threshold;

            double valueModifier = 10;
            if (DA.GetData(3, ref valueModifier)) corners.Value = valueModifier;

            int mode = 0;
            DA.GetData(1, ref mode) ;

            List<Point3d> points = new List<Point3d>();

            switch((CornerModes)mode)
            {
                default:
                    points = corners.GetSusanCorners();
                    break;
                case CornerModes.Fast:
                    points = corners.GetFastCorners();
                    break;
                case CornerModes.Harris:
                    points = corners.GetHarrisCorners();
                    break;
                case CornerModes.Morvec:
                    points = corners.GetMorvacCorners();
                    break;
            }

            DA.SetDataList(0, points);
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
                return Properties.Resources.TraceCornersA;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("2c867b86-1908-43a7-aa83-9c866394ab9a"); }
        }
    }
}