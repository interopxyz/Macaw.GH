using System;
using System.Collections.Generic;
using System.Drawing;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

using Af = Aviary.Macaw.Filters.Transform;

namespace Aviary.Macaw.GH.Transform
{
    public class Resize : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Resize class.
        /// </summary>
        public Resize()
          : base("Resize Image", "Resize", "Resize an image to a specific width and height" + Environment.NewLine + "Built on the Accord Imaging Library" + Environment.NewLine + "http://accord-framework.net/", "Aviary 1", "Image")

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
            pManager.AddIntegerParameter("Mode", "M", "", GH_ParamAccess.item, 0);
            pManager[1].Optional = true;
            pManager.AddIntegerParameter("Width", "W", "", GH_ParamAccess.item, 100);
            pManager[2].Optional = true;
            pManager.AddIntegerParameter("Height", "H", "", GH_ParamAccess.item, 100);
            pManager[3].Optional = true;
            
            Param_Integer param = (Param_Integer)pManager[1];
            foreach (Af.Resize.Modes value in Enum.GetValues(typeof(Af.Resize.Modes)))
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

            int width = 100;
            DA.GetData(2, ref width);

            int height = 100;
            DA.GetData(3, ref height);

            Filter filter = new Af.Resize(width, height, (Af.Resize.Modes) mode);
            image.Filters.Add(new Af.Resize(width, height, (Af.Resize.Modes)mode));
            
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
                return Properties.Resources.Filter_Xform_Resize_B;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("1437b50b-1776-4586-bb0a-2a9588e98ead"); }
        }
    }
}