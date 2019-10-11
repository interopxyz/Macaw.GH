using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

using Aviary.Wind;

using Ml = Aviary.Macaw.Layering;
using Grasshopper.Kernel.Parameters;

namespace Aviary.Macaw.GH.Layering
{
    public class XformLayer : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the XformLayer class.
        /// </summary>
        public XformLayer()
          : base("Xform Layer", "Xform", "Description", "Aviary 1", "Image")
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
            pManager.AddGenericParameter("Layer", "L", "---", GH_ParamAccess.item);
            pManager.AddVectorParameter("Translation Vector", "V", "---", GH_ParamAccess.item, new Vector3d());
            pManager[1].Optional = true;
            pManager.AddIntegerParameter("Angle", "A", "---", GH_ParamAccess.item, 0);
            pManager[2].Optional = true;
            pManager.AddIntegerParameter("Width", "W", "---", GH_ParamAccess.item, 0);
            pManager[3].Optional = true;
            pManager.AddIntegerParameter("Height", "H", "---", GH_ParamAccess.item, 0);
            pManager[4].Optional = true;
            pManager.AddIntegerParameter("Mode", "M", "---", GH_ParamAccess.item, 0);
            pManager[5].Optional = true;

            Param_Integer param = (Param_Integer)pManager[5];
            foreach (Ml.Layer.FittingModes value in Enum.GetValues(typeof(Ml.Layer.FittingModes)))
            {
                param.AddNamedValue(value.ToString(), (int)value);
            }
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Layer", "L", "The modifier layer", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Ml.Layer input = new Ml.Layer();
            if (!DA.GetData(0, ref input)) return;
            Ml.Layer layer = new Ml.Layer(input);

            Vector3d vector = new Vector3d();
            if(DA.GetData(1, ref vector)) {
                layer.X = (int)vector.X;
                layer.Y = (int)vector.Y;
            };

            int angle = 0;
            if(DA.GetData(2, ref angle))layer.Angle = angle;

            int width = 0;
            if (DA.GetData(3, ref width))layer.Width = width;

            int height = 0;
            if (DA.GetData(4, ref height))layer.Height = height;

            int mode = 0;
            if (DA.GetData(5, ref mode)) layer.FittingMode = (Ml.Layer.FittingModes)mode;
            
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
                return Properties.Resources.XformLayer;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("9e77b8b4-ec21-4b7a-a3aa-2624e36238f6"); }
        }
    }
}