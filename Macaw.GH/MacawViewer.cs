using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using Rhino.Geometry;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace Aviary.Macaw.GH
{
    public class MacawViewer : GH_Component
    {
        public Image img = null;

        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public MacawViewer()
          : base("View Image", "Viewer", "Preview a bitmap in canvas", "Aviary 1", "Bitmap Out")
        {
        }
        public override void CreateAttributes()
        {
            img = Properties.Resources.Macaw_sm;
            m_attributes = new Attributes_Custom(this);
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Bitmap", "B", "A bitmap to preview", GH_ParamAccess.item);
            pManager[0].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Bitmap bmp = Properties.Resources.Macaw_sm;
            if (!DA.GetData<Bitmap>(0, ref bmp));

            img = bmp;
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("eba2de7a-71c8-485e-bae1-78660b645776"); }
        }
    }

    public class Attributes_Custom : GH_ComponentAttributes
    {
        public Attributes_Custom(GH_Component owner) : base(owner) { }

        private Rectangle ButtonBounds { get; set; }
        protected override void Layout()
        {
            base.Layout();
            MacawViewer comp = Owner as MacawViewer;

            int width = comp.img.Width;
            int height = comp.img.Height;
            Rectangle rec0 = GH_Convert.ToRectangle(Bounds);

            int cWidth = rec0.Width;
            int cHeight = rec0.Height;

            rec0.Width = width;
            rec0.Height += height;

            Rectangle rec1 = rec0;
            rec1.Y = rec1.Bottom - height;
            rec1.Height = height;
            rec1.Width = width;

            Bounds = rec0;
            ButtonBounds = rec1;

        }

        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            base.Render(canvas, graphics, channel);
            MacawViewer comp = Owner as MacawViewer;

            if (channel == GH_CanvasChannel.Objects)
            {
                GH_Capsule capsule = GH_Capsule.CreateCapsule(ButtonBounds, GH_Palette.Normal,0, 0);
                capsule.Render(graphics, Selected, Owner.Locked, true);
                capsule.AddOutputGrip(this.OutputGrip.Y);
                capsule.Dispose();
                capsule = null;

                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;

                RectangleF textRectangle = ButtonBounds;
                
                graphics.DrawImage(comp.img,Bounds.X+2 , m_innerBounds.Y-(ButtonBounds.Height-Bounds.Height), comp.img.Width-4, comp.img.Height-2);

                format.Dispose();
            }
        }
    }
}
