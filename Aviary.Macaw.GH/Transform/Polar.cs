using System;
using System.Collections.Generic;
using System.Drawing;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

using Af = Aviary.Macaw.Filters.Transform;

namespace Aviary.Macaw.GH.Transform
{
    public class Polar : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Polar class.
        /// </summary>
        public Polar()
          : base("Polar Image", "Polar", "Apply a polar transformation to an image" + Environment.NewLine + "Built on the Accord Imaging Library" + Environment.NewLine + "http://accord-framework.net/", "Aviary 1", "Image")

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
            pManager.AddGenericParameter("Image", "I", "The Layer Bitmap", GH_ParamAccess.item);
            pManager.AddNumberParameter("Angle", "A", "[0-360] The rotation angle in degrees.", GH_ParamAccess.item, 0);
            pManager[1].Optional = true;
            pManager.AddNumberParameter("Pinch", "P", "[0-1] Unitized value where 0 pinches the distortion to the corners and 1 to smoothed circular distortion", GH_ParamAccess.item, 0);
            pManager[2].Optional = true;
            pManager.AddBooleanParameter("To", "T", "True maps from orthagonal to polar and false from polar to orthagonal", GH_ParamAccess.item, true);
            pManager[2].Optional = true;
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
            
            double angle = 0;
            DA.GetData(1, ref angle);

            double depth = 0;
            DA.GetData(2, ref depth);

            bool toPolar = true;
            DA.GetData(3, ref toPolar);

            Filter filter = new Af.Polar(toPolar,depth,angle,false,false);
            image.Filters.Add(new Af.Polar(toPolar, depth, angle, false, false));
            
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
                return Properties.Resources.Filter_Xform_Polar;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("62ff159c-395d-4609-960b-51f5fd342c85"); }
        }
    }
}