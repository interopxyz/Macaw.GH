using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

using Aviary.Macaw.Filters;
using Aviary.Macaw.Filters.Figures;

namespace Aviary.Macaw.GH.Filters
{
    public class FilterBlob : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the FilterBlob class.
        /// </summary>
        public FilterBlob()
          : base("Filter Blob", "Blob", "Filter out unique shape blob from an image" + Environment.NewLine + "Built on the Accord Imaging Library" + Environment.NewLine + "http://accord-framework.net/", "Aviary 1", "Image")
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
            pManager.AddGenericParameter("Image", "I", "An Aviary Image or Bitmap", GH_ParamAccess.item);
            pManager.AddIntervalParameter("Width", "W", "The horizontal threshold domain for the filtered blobs", GH_ParamAccess.item, new Interval(1,100));
            pManager[1].Optional = true;
            pManager.AddIntervalParameter("Height", "H", "The vertical threshold domain for the filtered blobs", GH_ParamAccess.item, new Interval(1, 100));
            pManager[2].Optional = true;
            pManager.AddBooleanParameter("Unique", "U", "If true, each discrete blob is color individually", GH_ParamAccess.item, false);
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
            pManager.AddGenericParameter("Image", "I", "An Aviary Image with the filter added to it", GH_ParamAccess.item);
            pManager.AddGenericParameter("Filter", "F", "The specified Filter", GH_ParamAccess.item);
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

            Interval numValA = new Interval(1,100);
            DA.GetData(1, ref numValA);

            Interval numValB = new Interval(1, 100);
            DA.GetData(2, ref numValB);

            bool unique = false;
            DA.GetData(3, ref unique);

            bool blobs = false;
            DA.GetData(4, ref blobs);

            bool coupled = false;
            DA.GetData(5, ref coupled);

            Filter filter = new Filter();

            if (unique)
            {
                filter = new BlobsUnique(numValA.ToDomain(),numValB.ToDomain(),blobs,coupled);
                image.Filters.Add(new BlobsUnique(numValA.ToDomain(), numValB.ToDomain(), blobs, coupled));
            }
            else
            {
                filter = new BlobsFilter(numValA.ToDomain(), numValB.ToDomain(), coupled);
                image.Filters.Add(new BlobsFilter(numValA.ToDomain(), numValB.ToDomain(), coupled));
            }


            DA.SetData(0, image);
            DA.SetData(1, filter);
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
                return Properties.Resources.Blob0;
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