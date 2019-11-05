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
    public class Rotate : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Rotate class.
        /// </summary>
        public Rotate()
          : base("Rotate Image", "Rotate", "Rotate an image about its center" + Environment.NewLine + "Built on the Accord Imaging Library" + Environment.NewLine + "http://accord-framework.net/", "Aviary 1", "Image")
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
            pManager.AddNumberParameter("Angle", "A", "", GH_ParamAccess.item, 0);
            pManager[2].Optional = true;
            pManager.AddBooleanParameter("Keep Original", "K", "", GH_ParamAccess.item, true);
            pManager[3].Optional = true;
            pManager.AddColourParameter("Color", "C", "", GH_ParamAccess.item, Color.Black);
            pManager[4].Optional = true;
            
            Param_Integer param = (Param_Integer)pManager[1];
            foreach (Af.Rotate.Modes value in Enum.GetValues(typeof(Af.Rotate.Modes)))
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

            double angle = 0 ;
            DA.GetData(2, ref angle);

            bool original = false;
            DA.GetData(3, ref original);

            Color color = Color.Black;
            DA.GetData(4, ref color);

            Filter filter = new Af.Rotate(angle,  original,color, (Af.Rotate.Modes)mode);
            image.Filters.Add(new Af.Rotate(angle, original, color, (Af.Rotate.Modes)mode));
            
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
                return Properties.Resources.Filter_Xform_Rotate;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("6d4b6638-be96-4d07-839d-ac0269a01c8a"); }
        }
    }
}