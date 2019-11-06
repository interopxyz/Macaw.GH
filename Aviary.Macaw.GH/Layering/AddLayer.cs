using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

using Ml = Aviary.Macaw.Layering;

namespace Aviary.Macaw.GH.Layering
{
    public class AddLayer : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the SetLayer class.
        /// </summary>
        public AddLayer()
          : base("Add Layer", "Layer", "Set layer image and properties" + Environment.NewLine + "Built on the Dynamic Image Library" + Environment.NewLine + "https://dynamicimage.apphb.com/", "Aviary 1", "Image")
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
            pManager.AddGenericParameter("Image", "I", "An Aviary Image or Bitmap", GH_ParamAccess.item);
            pManager.AddGenericParameter("Mask Image", "X", "An optional Aviary Image or Bitmap opacity mask", GH_ParamAccess.item);
            pManager[1].Optional = true;

            pManager.AddIntegerParameter("Blend Mode", "M", "The transparency blend mode.", GH_ParamAccess.item, 0);
            pManager[2].Optional = true;

            pManager.AddNumberParameter("Opacity", "O", "An opacity value from 0-1", GH_ParamAccess.item, 1.0);
            pManager[3].Optional = true;

            Param_Integer paramA = (Param_Integer)pManager[2];
            foreach (Ml.Layer.BlendModes value in Enum.GetValues(typeof(Ml.Layer.BlendModes)))
            {
                paramA.AddNamedValue(value.ToString(), (int)value);
            }
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Layer", "L", "The resulting layer", GH_ParamAccess.item);
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

            Ml.Layer layer = new Ml.Layer(bitmap);
            
            IGH_Goo gooM = null;
            Bitmap bitmapM = new Bitmap(100, 100);
            if (DA.GetData(0, ref gooM)) if (goo.TryGetBitmap(ref bitmapM)) layer.Mask = bitmapM;

            int blendMode = 0;
            DA.GetData(2, ref blendMode);

            double opacity = 1.0;
            DA.GetData(3, ref opacity);

            layer.BlendMode = (Ml.Layer.BlendModes)blendMode;
            layer.Opacity = 100 * opacity;

            DA.SetData(0, layer);
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
                return Properties.Resources.AddLayer;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("7b845d98-dd80-42a7-bae5-6b792f3e4ece"); }
        }
    }
}