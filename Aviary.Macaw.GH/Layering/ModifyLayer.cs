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
    public class ModifyLayer : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ModifyLayer class.
        /// </summary>
        public ModifyLayer()
          : base("Modify Layer", "Modify", "Modify Layer filters" + Environment.NewLine + "Built on the Dynamic Image Library" + Environment.NewLine + "https://dynamicimage.apphb.com/", "Aviary 1", "Image")
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
            pManager.AddGenericParameter("Layer", "L", "An Aviary Image Layer to modify", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Mode", "M", "The layer modifier mode ", GH_ParamAccess.item, 0);
            pManager[1].Optional = true;
            pManager.AddNumberParameter("Value", "V", "The value of the modifier's parameter, when available", GH_ParamAccess.item, 0.0);
            pManager[2].Optional = true;
            pManager.AddColourParameter("Color", "C", "Color parameter for select modifiers", GH_ParamAccess.item, Color.Black);
            pManager[3].Optional = true;
            
            Param_Integer param = (Param_Integer)pManager[1];
            foreach (Ml.Modifier.ModifierModes value in Enum.GetValues(typeof(Ml.Modifier.ModifierModes)))
            {
                param.AddNamedValue(value.ToString(), (int)value);
            }
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Layer", "L", "The modifier Aviary Image Layer", GH_ParamAccess.item);
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

            int mode = 0;
            DA.GetData(1, ref mode);
            Ml.Modifier modifier = new Ml.Modifier( (Ml.Modifier.ModifierModes)mode);

            double value = 0.0;
            if (DA.GetData(2, ref value)) modifier.Value = value;

            Color color = Color.Black;
            if (DA.GetData(3, ref color)) modifier.Color = color.ToWindColor();

            layer.Modifiers.Add(modifier);

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
                return Properties.Resources.ModifyLayerProperties;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("3d11f8f9-e1c1-401c-b336-cbbd80871833"); }
        }
    }
}