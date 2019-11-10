using System;
using System.Collections.Generic;
using System.Drawing;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

using Af = Aviary.Macaw.Filters.Transform;

namespace Aviary.Macaw.GH.Transform
{
    public class Crop : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Crop class.
        /// </summary>
        public Crop()
          : base("Crop Image", "Crop", "Crop a bitmap with a rectangular region" + Environment.NewLine + "Built on the Accord Imaging Library" + Environment.NewLine + "http://accord-framework.net/", "Aviary 1", "Image")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.quinary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Image", "I", "An Aviary Image or Bitmap", GH_ParamAccess.item);
            pManager.AddRectangleParameter("Region", "R", "", GH_ParamAccess.item, new Rectangle3d(Plane.WorldXY, 100, 100));
            pManager[1].Optional = true;
            pManager.AddBooleanParameter("Keep Original", "K", "", GH_ParamAccess.item, true);
            pManager[2].Optional = true;
            pManager.AddColourParameter("Color", "C", "", GH_ParamAccess.item, Color.Black);
            pManager[3].Optional = true;
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

            Rectangle3d region= new Rectangle3d();
            DA.GetData(1, ref region);

            bool original = false;
            DA.GetData(2, ref original);

            Color color = Color.Black;
            DA.GetData(3, ref color);

            Filter filter = new Af.Crop(original,color,region.ToDrawingRect(image.Bitmap.Height));
            image.Filters.Add(new Af.Crop(original, color, region.ToDrawingRect(image.Bitmap.Height)));


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
                return Properties.Resources.Filter_Xform_Crop;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("c8a4cbd0-806e-4124-9e20-a674a0200365"); }
        }
    }
}