﻿using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using Aviary.Macaw.Filters.Dither;

namespace Aviary.Macaw.GH.Filters
{
    public class FilterDithering : GH_Component
    {
        private enum FilterModes { Bayer, Burkes, Carry, FloydSteinberg, JarvisJudiceNinke, Ordered, Sierra, Stucki }
        /// <summary>
        /// Initializes a new instance of the FiltersDithering class.
        /// </summary>
        public FilterDithering()
          : base("Filters Dithering", "Dithering", "Description", "Aviary 1", "Image")
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
            pManager.AddIntegerParameter("Mode", "M", "Select filter type", GH_ParamAccess.item, 0);
            pManager[1].Optional = true;
            pManager.AddIntegerParameter("Value", "V", "---", GH_ParamAccess.item, 1);
            pManager[2].Optional = true;

            Param_Integer param = (Param_Integer)pManager[1];
            foreach (FilterModes value in Enum.GetValues(typeof(FilterModes)))
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

            int numVal = 0;
            DA.GetData(2, ref numVal);

            Filter filter = new Filter();

            switch ((FilterModes)mode)
            {
                case FilterModes.Bayer:
                    filter = new Bayer();
                    image.Filters.Add(new Bayer());
                    break;
                case FilterModes.Burkes:
                    filter = new Burkes(numVal);
                    image.Filters.Add(new Burkes(numVal));
                    break;
                case FilterModes.Carry:
                    filter = new Carry(numVal);
                    image.Filters.Add(new Carry(numVal));
                    break;
                case FilterModes.FloydSteinberg:
                    filter = new FloydSteinberg(numVal);
                    image.Filters.Add(new FloydSteinberg(numVal));
                    break;
                case FilterModes.JarvisJudiceNinke:
                    filter = new JarvisJudiceNinke(numVal);
                    image.Filters.Add(new JarvisJudiceNinke(numVal));
                    break;
                case FilterModes.Ordered:
                    filter = new Ordered();
                    image.Filters.Add(new Ordered());
                    break;
                case FilterModes.Sierra:
                    filter = new Sierra(numVal);
                    image.Filters.Add(new Sierra(numVal));
                    break;
                case FilterModes.Stucki:
                    filter = new Stucki(numVal);
                    image.Filters.Add(new Stucki(numVal));
                    break;
            }

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
            get { return new Guid("806a05e5-b39e-452c-abe6-439f9ce6016e"); }
        }
    }
}