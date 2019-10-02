﻿using System;
using System.Collections.Generic;
using System.Drawing;
using Aviary.Macaw.Tracing;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Aviary.Macaw.GH.Tracing
{
    public class GetShapes : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GetShapes class.
        /// </summary>
        public GetShapes()
          : base("Bitmap Shapes", "Shapes", "Get shapes from bitmap", "Aviary 1", "Image")
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
            pManager.AddNumberParameter("Angles", "A", "---", GH_ParamAccess.item);
            pManager[5].Optional = true;
            pManager.AddNumberParameter("Distances", "D", "---", GH_ParamAccess.item);
            pManager[6].Optional = true;
            pManager.AddNumberParameter("Deviation", "X", "---", GH_ParamAccess.item);
            pManager[7].Optional = true;

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Circles", "C", "---", GH_ParamAccess.list);
            pManager.AddCurveParameter("Triangles", "T", "---", GH_ParamAccess.list);
            pManager.AddCurveParameter("Quadrilaterals", "Q", "---", GH_ParamAccess.list);
            pManager.AddCurveParameter("Polylines", "P", "---", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Image image = new Image();
            Bitmap bitmap = new Bitmap(100, 100);
            if (!DA.GetData(0, ref bitmap)) if (DA.GetData(0, ref image)) { bitmap = image.Bitmap; } else { return; }
            Bitmap bmp = (Bitmap)bitmap.Clone();

            Shapes shapes = new Shapes(bmp);

            Interval width = new Interval();
            if (DA.GetData(1, ref width))
            {
                shapes.MinWidth = (int)width.T0;
                shapes.MaxWidth = (int)width.T1;
            }

            Interval height = new Interval();
            if (DA.GetData(2, ref height))
            {
                shapes.MinHeight = (int)height.T0;
                shapes.MaxHeight = (int)height.T1;
            }

            Color color = new Color();
            if (DA.GetData(3, ref color)) shapes.BackgroundColor = color;

            bool limit = false;
            if (DA.GetData(4, ref limit)) shapes.Coupled = limit;

            double angle = -1;
            if (DA.GetData(5, ref angle)) shapes.Angle = angle;
            double distance = -1;
            if (DA.GetData(6, ref distance)) shapes.Length = distance;
            double deviation = -1;
            if (DA.GetData(7, ref deviation)) shapes.Distortion = deviation;

            shapes.CalculateShapes();

            DA.SetDataList(0, shapes.GetCircles);
            DA.SetDataList(1, shapes.GetTriangles);
            DA.SetDataList(2, shapes.GetQuadrilaterals);
            DA.SetDataList(3, shapes.GetPolylines);
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
                return Properties.Resources.TraceShapesA;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("d80f263a-bb8b-433f-82f5-8792c410d362"); }
        }
    }
}