using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using Aviary.Macaw;
using Aviary.Wind;

namespace Aviary.Macaw.GH
{
    public class PerturbNoise : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PerturbNoise class.
        /// </summary>
        public PerturbNoise()
          : base("Perturb Noise", "Perturb", "Description", "Aviary 1", "Bitmap ")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("Width", "W", "---", GH_ParamAccess.item, 100);
            pManager[0].Optional = true;
            pManager.AddIntegerParameter("Height", "H", "---", GH_ParamAccess.item, 100);
            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Bitmap", "B", "---", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

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
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("996fb7ce-44e9-49df-827b-4e4d6d09c57a"); }
        }
    }
}