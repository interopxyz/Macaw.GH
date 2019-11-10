using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

using Ml = Aviary.Macaw.Layering;

namespace Aviary.Macaw.GH.Layering
{
    public class MergeLayers : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CompositeLayers class.
        /// </summary>
        public MergeLayers()
          : base("Merge Layers", "Merge", "Build multiple layers into an Aviary Image" + Environment.NewLine + "Built on the Dynamic Image Library" + Environment.NewLine + "https://dynamicimage.apphb.com/", "Aviary 1", "Image")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.tertiary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Layers", "L", "A list of Aviary Image Layers to composite", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Image", "I", "The resulting Aviary Image", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<IGH_Goo> goos = new List<IGH_Goo>();
            if(!DA.GetDataList(0, goos))return;

            Ml.Composition composition = new Ml.Composition();

            foreach(IGH_Goo goo in goos)
            {
                Ml.Layer layer = new Ml.Layer();
                if (goo.CastTo<Ml.Layer>(out layer)) composition.Layers.Add(new Ml.Layer(layer));
            }

            Bitmap bitmap = composition.GetBitmap();

            DA.SetData(0, new Image(bitmap));
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
                return Properties.Resources.MergeLayers;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("c199cd33-e248-4db4-937b-7ab60f4124ec"); }
        }
    }
}