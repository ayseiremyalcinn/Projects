using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GCS_pdr
{
    public partial class UserControl1 : UserControl
    {

        // Load images
        Bitmap mybitmap1 = new Bitmap(Properties.Resources.horizon);
        Bitmap mybitmap2 = new Bitmap(Properties.Resources.wings);
        Bitmap mybitmap3 = new Bitmap(Properties.Resources.heading);
        Bitmap mybitmap4 = new Bitmap(Properties.Resources.bezel);

        //Pen MyPen;

        public double PitchAngle = 0;
        public double RollAngle = 0;
        public double YawAngle = 0;




        Point ptBoule = new Point(-25, -410); //Ground-Sky initial location
        Point ptHeading = new Point(-592, 150); // Heading ticks          
        Point ptRotation = new Point(150, 150); // Point of rotation




        public UserControl1()
        {
            InitializeComponent();

            // This bit of code (using double buffer) reduces flicker from Refresh commands
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            //////////// END "reduce flicker" code ///////
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
            /*mybitmap2.MakeTransparent(Color.Yellow); // Sets image transparency
            mybitmap3.MakeTransparent(Color.Black); // Sets image transparency
            mybitmap4.MakeTransparent(Color.Yellow); // Sets image transparency*/
        }

        protected override void OnPaint(PaintEventArgs paintEvnt)
        {

            try
            {

                // Calling the base class OnPaint
                base.OnPaint(paintEvnt);

                // Clipping mask for Attitude Indicator
                paintEvnt.Graphics.Clip = new Region(new Rectangle(0, 0, 300, 300));
                paintEvnt.Graphics.FillRegion(Brushes.Black, paintEvnt.Graphics.Clip);

                // Make sure lines are drawn smoothly
                paintEvnt.Graphics.SmoothingMode = SmoothingMode.HighQuality;

                // Create the graphics object
                Graphics gfx = paintEvnt.Graphics;

                // Adjust and draw horizon image
                RotateAndTranslate(paintEvnt, mybitmap1, RollAngle, 0, ptBoule, (double)(4 * PitchAngle), ptRotation, 1);

                RotateAndTranslate2(paintEvnt, mybitmap3, YawAngle, RollAngle, 0, ptHeading, (double)(4 * PitchAngle), ptRotation, 1);
                //gfx.DrawImage(mybitmap3, 0, 0); // Draw wings image
               // gfx.DrawImage(mybitmap1, 400, 150); // Draw wings image
                gfx.DrawImage(mybitmap4, 0, 0); // Draw bezel image
                gfx.DrawImage(mybitmap2, 75, 125); // Draw wings image
                
                

            }
            catch (Exception)
            {

            }
        }


        protected void RotateAndTranslate(PaintEventArgs pe, Image img, Double alphaRot, Double alphaTrs, Point ptImg, double deltaPx, Point ptRot, float scaleFactor)
        {
            try
            {

                double beta = 0;
                double d = 0;
                float deltaXRot = 0;
                float deltaYRot = 0;
                float deltaXTrs = 0;
                float deltaYTrs = 0;

                // Rotation

                if (ptImg != ptRot)
                {
                    // Internals coeffs
                    if (ptRot.X != 0)
                    {
                        beta = Math.Atan((double)ptRot.Y / (double)ptRot.X);
                    }

                    d = Math.Sqrt((ptRot.X * ptRot.X) + (ptRot.Y * ptRot.Y));

                    // Computed offset
                    deltaXRot = (float)(d * (Math.Cos(alphaRot - beta) - Math.Cos(alphaRot) * Math.Cos(alphaRot + beta) - Math.Sin(alphaRot) * Math.Sin(alphaRot + beta)));
                    deltaYRot = (float)(d * (Math.Sin(beta - alphaRot) + Math.Sin(alphaRot) * Math.Cos(alphaRot + beta) - Math.Cos(alphaRot) * Math.Sin(alphaRot + beta)));
                }

                // Translation

                // Computed offset
                deltaXTrs = (float)(deltaPx * (Math.Sin(alphaTrs)));
                deltaYTrs = (float)(-deltaPx * (-Math.Cos(alphaTrs)));

                // Rotate image support
                pe.Graphics.RotateTransform((float)(alphaRot * 180 / Math.PI));

                // Dispay image
                pe.Graphics.DrawImage(img, (ptImg.X + deltaXRot + deltaXTrs) * scaleFactor, (ptImg.Y + deltaYRot + deltaYTrs) * scaleFactor, img.Width * scaleFactor, img.Height * scaleFactor);

                // Put image support as found
                pe.Graphics.RotateTransform((float)(-alphaRot * 180 / Math.PI));

            }
            catch (Exception)
            {

            }
        }

        protected void RotateAndTranslate2(PaintEventArgs pe, Image img, Double yawRot, Double alphaRot, Double alphaTrs, Point ptImg, double deltaPx, Point ptRot, float scaleFactor)
        {
            try
            {

                double beta = 0;
                double d = 0;
                float deltaXRot = 0;
                float deltaYRot = 0;
                float deltaXTrs = 0;
                float deltaYTrs = 0;

                // Rotation

                if (ptImg != ptRot)
                {
                    // Internals coeffs
                    if (ptRot.X != 0)
                    {
                        beta = Math.Atan((double)ptRot.Y / (double)ptRot.X);
                    }

                    d = Math.Sqrt((ptRot.X * ptRot.X) + (ptRot.Y * ptRot.Y));

                    // Computed offset
                    deltaXRot = (float)(d * (Math.Cos(alphaRot - beta) - Math.Cos(alphaRot) * Math.Cos(alphaRot + beta) - Math.Sin(alphaRot) * Math.Sin(alphaRot + beta) + yawRot));
                    deltaYRot = (float)(d * (Math.Sin(beta - alphaRot) + Math.Sin(alphaRot) * Math.Cos(alphaRot + beta) - Math.Cos(alphaRot) * Math.Sin(alphaRot + beta)));
                }

                // Translation

                // Computed offset
                deltaXTrs = (float)(deltaPx * (Math.Sin(alphaTrs)));
                deltaYTrs = (float)(-deltaPx * (-Math.Cos(alphaTrs)));

                // Rotate image support
                pe.Graphics.RotateTransform((float)(alphaRot * 180 / Math.PI));

                // Dispay image
                pe.Graphics.DrawImage(img, (ptImg.X + deltaXRot + deltaXTrs) * scaleFactor, (ptImg.Y + deltaYRot + deltaYTrs) * scaleFactor, img.Width * scaleFactor, img.Height * scaleFactor);

                // Put image support as found
                pe.Graphics.RotateTransform((float)(-alphaRot * 180 / Math.PI));

            }
            catch (Exception)
            {

            }
        }

        public void yenile()
        {

            Invalidate();
        }

    }
}
