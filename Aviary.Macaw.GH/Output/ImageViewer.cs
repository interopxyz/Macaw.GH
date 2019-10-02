using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Aviary.Macaw.GH
{
    public class ImageViewer : GH_Component
    {
        public Image img = null;
        string message = "Nothing here";

        /// <summary>
        /// Initializes a new instance of the BitmapViewer class.
        /// </summary>
        public ImageViewer()
          : base("Image Viewer", "Image", "Description", "Aviary 1", "Image")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.septenary; }
        }

        public override void CreateAttributes()
        {
            img = Properties.Resources.ImageViewer_Background;
            m_attributes = new Attributes_Custom(this);
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Image", "I", "---", GH_ParamAccess.item);
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
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IGH_Goo goo = null;
            Bitmap bitmap = new Bitmap(100, 100);

            if (DA.GetData(0, ref goo))
            {
                if(!goo.CastTo<Bitmap>(out bitmap)) bitmap = Properties.Resources.ImageViewer_Background;
            }
            else
            {
                bitmap = Properties.Resources.ImageViewer_Background;
            }

            img = (Bitmap)bitmap.Clone();
            message = bitmap.PixelFormat.ToString();
            UpdateMessage();
        }

        private void UpdateMessage()
        {
            Message = message;
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
                return Properties.Resources.ExportBitmap24;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("8b1a4d1c-08d2-492e-b84c-b085c3aa4b0d"); }
        }
    }

    public class Attributes_Custom : GH_ComponentAttributes
    {
        public Attributes_Custom(GH_Component owner) : base(owner) { }

        private Rectangle ButtonBounds { get; set; }
        protected override void Layout()
        {
            base.Layout();
            ImageViewer comp = Owner as ImageViewer;

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
            ImageViewer comp = Owner as ImageViewer;

            if (channel == GH_CanvasChannel.Objects)
            {
                GH_Capsule capsule = GH_Capsule.CreateCapsule(ButtonBounds, GH_Palette.Normal, 0, 0);
                capsule.Render(graphics, Selected, Owner.Locked, true);
                capsule.AddOutputGrip(this.OutputGrip.Y);
                capsule.Dispose();
                capsule = null;

                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;

                RectangleF textRectangle = ButtonBounds;

                graphics.DrawImage(comp.img, Bounds.X + 2, m_innerBounds.Y - (ButtonBounds.Height - Bounds.Height), comp.img.Width - 4, comp.img.Height - 2);

                format.Dispose();
            }
        }
    }
}