using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

using Mp = Aviary.Macaw.Layering;

namespace Aviary.Macaw.GH.Construct
{
    public class CompositeImages : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CompositeImages class.
        /// </summary>
        public CompositeImages()
          : base("Composite Images", "Composite", "Quick composite two images with a mask and blend mode" + Environment.NewLine + "Built on the Dynamic Image Library" + Environment.NewLine + "https://dynamicimage.apphb.com/", "Aviary 1", "Image")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Top Image", "T", "The top Aviary Image or Bitmap", GH_ParamAccess.item);
            pManager.AddGenericParameter("Bottom Image", "B", "The bottom Aviary Image or Bitmap", GH_ParamAccess.item);
            pManager.AddGenericParameter("Mask Image", "X", "An optional layer mask Aviary Image or Bitmap", GH_ParamAccess.item);
            pManager[2].Optional = true;

            pManager.AddIntegerParameter("Blend Mode", "M", "The transparency blend mode applied to the top image", GH_ParamAccess.item, 0);
            pManager[3].Optional = true;

            pManager.AddNumberParameter("Opacity", "O", "An opacity value from 0-1", GH_ParamAccess.item, 1.0);
            pManager[4].Optional = true;
            
            Param_Integer paramA = (Param_Integer)pManager[3];
            foreach (Mp.Layer.BlendModes value in Enum.GetValues(typeof(Mp.Layer.BlendModes)))
            {
                paramA.AddNamedValue(value.ToString(), (int)value);
            }
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Image", "I", "The resultant image", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IGH_Goo goo = null;
            Bitmap top = new Bitmap(100, 100);
            if (!DA.GetData(0, ref goo)) return;
            if (!goo.TryGetBitmap(ref top)) return;

            Mp.Layer topLayer = new Mp.Layer(top);

            IGH_Goo gooT = null;
            Bitmap bottom = new Bitmap(100, 100);
            if (!DA.GetData(1, ref gooT)) return;
            if (!gooT.TryGetBitmap(ref bottom)) return;

            Mp.Layer bottomLayer = new Mp.Layer(bottom);

            IGH_Goo gooM = null;
            Image img = new Image();
            bool hasMask = false;
            Bitmap mask = new Bitmap(100, 100);
            if (DA.GetData(2, ref gooM)) if (gooM.TryGetImage(ref img))
                {
                    hasMask = true;
                    img.SwapChannel(Image.Channels.Luminance, Image.Channels.Alpha);
                    mask = img.Bitmap;
                }

            int blendMode = 0;
            DA.GetData(3, ref blendMode);

            double opacity = 1.0;
            DA.GetData(4, ref opacity);

            topLayer.BlendMode = (Mp.Layer.BlendModes)blendMode;
            topLayer.Opacity = 100 * opacity;
            if (hasMask) topLayer.Mask = mask;

            Mp.Composition composition = new Mp.Composition();
            composition.Layers.Add(bottomLayer);
            composition.Layers.Add(topLayer);

            DA.SetData(0, new Image(composition.GetBitmap()));
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
                return Properties.Resources.Composite2Images;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("72d23fdd-3890-47b7-bf2f-bf336dd8a3a8"); }
        }
    }
}