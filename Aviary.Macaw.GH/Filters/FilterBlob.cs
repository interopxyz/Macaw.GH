﻿using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Aviary.Macaw.GH.Filters
{
    public class FilterBlob : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the FilterBlob class.
        /// </summary>
        public FilterBlob()
          : base("Filter Blob", "Blob", "Description", "Aviary 1", "Image")
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
            pManager.AddIntervalParameter("Width", "W", "", GH_ParamAccess.item, new Interval(1,100));
            pManager[1].Optional = true;
            pManager.AddIntervalParameter("Height", "H", "", GH_ParamAccess.item, new Interval(1, 100));
            pManager[2].Optional = true;
            pManager.AddBooleanParameter("Unique", "U", "", GH_ParamAccess.item, false);
            pManager[3].Optional = true;
            pManager.AddBooleanParameter("Blobs", "B", "", GH_ParamAccess.item, false);
            pManager[4].Optional = true;
            pManager.AddBooleanParameter("Coupled", "C", "", GH_ParamAccess.item, false);
            pManager[5].Optional = true;

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

            IGH_Goo gooA = null;
            Image imageA = new Image();
            if (!DA.GetData(2, ref gooA)) return;
            if (!goo.TryGetImage(ref imageA)) return;

            Bitmap overlay = imageA.GetFilteredBitmap();

            double numVal = 1.0;
            DA.GetData(3, ref numVal);

            Filter filter = new Filter();

            switch ((FilterModes)mode)
            {
                case FilterModes.Add:
                    filter = new Add(overlay);
                    image.Filters.Add(new Add(overlay));
                    break;
                case FilterModes.Subtract:
                    filter = new Subtract(overlay);
                    image.Filters.Add(new Subtract(overlay));
                    break;
                case FilterModes.Multiply:
                    filter = new Multiply(overlay);
                    image.Filters.Add(new Multiply(overlay));
                    break;
                case FilterModes.Divide:
                    filter = new Divide(overlay);
                    image.Filters.Add(new Divide(overlay));
                    break;
                case FilterModes.Euclidean:
                    filter = new Euclidean(overlay, (int)numVal);
                    image.Filters.Add(new Euclidean(overlay, (int)numVal));
                    break;
                case FilterModes.FlatField:
                    filter = new FlatField(overlay);
                    image.Filters.Add(new FlatField(overlay));
                    break;
                case FilterModes.Intersect:
                    filter = new Intersect(overlay);
                    image.Filters.Add(new Intersect(overlay));
                    break;
                case FilterModes.Merge:
                    filter = new Merge(overlay);
                    image.Filters.Add(new Merge(overlay));
                    break;
                case FilterModes.Morph:
                    filter = new Morph(overlay, numVal);
                    image.Filters.Add(new Morph(overlay, numVal));
                    break;
                case FilterModes.MoveTowards:
                    filter = new MoveTowards(overlay, (int)numVal);
                    image.Filters.Add(new MoveTowards(overlay, (int)numVal));
                    break;
                case FilterModes.Simple:
                    filter = new Simple(overlay, (int)numVal);
                    image.Filters.Add(new Simple(overlay, (int)numVal));
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
            get { return new Guid("26a9e635-954d-44e3-a369-89b1ef9cc0d8"); }
        }
    }
}