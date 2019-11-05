using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

using Aviary.Macaw.Filters;
using Aviary.Macaw.Filters.Difference;
using System.Drawing;

namespace Aviary.Macaw.GH.Filters
{
    public class FilterDifference : GH_Component
    {
        private enum FilterModes { Add, Subtract, Multiply, Divide, Euclidean, FlatField, Intersect, Merge, Morph, MoveTowards, Simple}
        /// <summary>
        /// Initializes a new instance of the Filter class.
        /// </summary>
        public FilterDifference()
          : base("Filter Difference", "Difference", "Compare the difference between two images" + Environment.NewLine + "Built on the Accord Imaging Library" + Environment.NewLine + "http://accord-framework.net/", "Aviary 1", "Image")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.quarternary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Image", "I", "The Layer Bitmap", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Mode", "M", "", GH_ParamAccess.item, 0);
            pManager[1].Optional = true;
            pManager.AddGenericParameter("Bitmap", "B", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("Value", "V", "", GH_ParamAccess.item, 1.0);
            pManager[3].Optional = true;

            Param_Integer param = (Param_Integer)pManager[1];
            foreach (FilterModes value in Enum.GetValues(typeof(FilterModes)))
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

            IGH_Goo gooA = null;
            Bitmap overlay = new Bitmap(100, 100);
            if (DA.GetData(2, ref gooA)) if (goo.TryGetBitmap(ref overlay)) ;
            
            double numVal = 1.0;
            DA.GetData(3, ref numVal);

            Filter filter = new Filter();

            switch ((FilterModes)mode)
            {
                case FilterModes.Add:
                    filter = new Add(overlay);
                    image.Filters.Add(new Add(overlay));
                    break;
                case FilterModes.Subtract:
                    filter = new Subtract(overlay);
                    image.Filters.Add(new Subtract(overlay));
                    break;
                case FilterModes.Multiply:
                    filter = new Multiply(overlay);
                    image.Filters.Add(new Multiply(overlay));
                    break;
                case FilterModes.Divide:
                    filter = new Divide(overlay);
                    image.Filters.Add(new Divide(overlay));
                    break;
                case FilterModes.Euclidean:
                    filter = new Euclidean(overlay,(int)numVal);
                    image.Filters.Add(new Euclidean(overlay, (int)numVal));
                    break;
                case FilterModes.FlatField:
                    filter = new FlatField(overlay);
                    image.Filters.Add(new FlatField(overlay));
                    break;
                case FilterModes.Intersect:
                    filter = new Intersect(overlay);
                    image.Filters.Add(new Intersect(overlay));
                    break;
                case FilterModes.Merge:
                    filter = new Merge(overlay);
                    image.Filters.Add(new Merge(overlay));
                    break;
                case FilterModes.Morph:
                    filter = new Morph(overlay, numVal);
                    image.Filters.Add(new Morph(overlay, numVal));
                    break;
                case FilterModes.MoveTowards:
                    filter = new MoveTowards(overlay, (int)numVal);
                    image.Filters.Add(new MoveTowards(overlay,(int)numVal));
                    break;
                case FilterModes.Simple:
                    filter = new Simple(overlay, (int)numVal);
                    image.Filters.Add(new Simple(overlay, (int)numVal));
                    break;
            }

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
                return Properties.Resources.Difference1;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("5127b391-ecb6-4268-b08d-a14f4edd00cd"); }
        }
    }
}