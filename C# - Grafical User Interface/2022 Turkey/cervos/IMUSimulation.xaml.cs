using HelixToolkit.Wpf;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace cervos
{
    /// <summary>
    /// Interaction logic for IMUSimulation.xaml
    /// </summary>
    public partial class IMUSimulation : UserControl
    {
        public IMUSimulation()
        {
            InitializeComponent();

            init3D();
            /*System.Windows.Forms.MessageBox.Show("Position:" + viewPort3d.Camera.Position.ToString());  // 2,16,20 
            System.Windows.Forms.MessageBox.Show("Look direction:" + viewPort3d.Camera.LookDirection.ToString()); //-2,-16,20
            System.Windows.Forms.MessageBox.Show("Up direction" + viewPort3d.Camera.UpDirection.ToString()); // 0,0,1*/
        }

        private const string path = "Part1.stl";
        ModelVisual3D device3D = new ModelVisual3D();
        Model3D device;
        double preRoll = 0, prePitch = 0, preYaw = 0;
        double Troll, Tpitch, Tyaw;
        Matrix3D matrix;




        public void init3D()
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    //viewPort3d.Children.Clear();

                    preRoll = 0;
                    prePitch = 0;
                    preYaw = 0;
                    device3D.Content = Display3d(path);
                    viewPort3d.Children.Add(device3D);
                    viewPort3d.RotateGesture = new MouseGesture(MouseAction.LeftClick);
                    viewPort3d.ShowCoordinateSystem = true;
                    viewPort3d.Children.Add(new DefaultLights());

                    var matrix = device3D.Transform.Value;
                    matrix.Rotate(new Quaternion(new Vector3D(1, 0, 0), 90));
                    device3D.Transform = new MatrixTransform3D(matrix);

                    matrix.Rotate(new Quaternion(new Vector3D(0, 1, 0), 0));
                    device3D.Transform = new MatrixTransform3D(matrix);

                    matrix.Rotate(new Quaternion(new Vector3D(0, 0, 1), 180));
                    device3D.Transform = new MatrixTransform3D(matrix);

                    PerspectiveCamera camera = new PerspectiveCamera(new Point3D(0, 0, 0), new Vector3D(-12, -16, -4), new Vector3D(0, 0, 1), 30);
                    viewPort3d.Camera = camera;
                    viewPort3d.FixedRotationPointEnabled = true;
                    viewPort3d.FixedRotationPoint = new Point3D(5, 5, 5);

                });
            }
            catch (Exception err)
            {
                System.Windows.Forms.MessageBox.Show(err.Message);
            }
        }

        /// <summary>
        /// Display 3D Model
        /// </summary>
        /// <param name="model">Path to the Model file</param>
        /// <returns>3D Model Content</returns>
        private Model3D Display3d(string model)
        {
            Model3D device = null;
            try
            {
                //Import 3D model file
                ModelImporter import = new ModelImporter();

                //Load the 3D model file
                device = import.Load(path);
            }

            catch (Exception e)
            {
                // Handle exception in case can not find the 3D model file
                MessageBox.Show("Exception Error : " + e.StackTrace);
            }
            return device;
        }

        public void Rotate(double roll, double pitch, double yaw)
        {
            try
            {
                var centerR = new Point3D(0, 5, 7.5);
                var centerP = new Point3D(-12, 0, 7.5);
                var centerY = new Point3D(-12, 5, 0);

                double rollAngle = roll - preRoll;
                double pitchAngle = pitch - prePitch;
                double yawAngle = yaw - preYaw;

                
                var matrix = device3D.Transform.Value;
                matrix.RotateAt(new Quaternion(new Vector3D(1, 0, 0), rollAngle), centerR);
                device3D.Transform = new MatrixTransform3D(matrix);

                matrix.RotateAt(new Quaternion(new Vector3D(0, 1, 0), pitchAngle), centerP);
                device3D.Transform = new MatrixTransform3D(matrix);

                matrix.RotateAt(new Quaternion(new Vector3D(0, 0, 1), yawAngle), centerY);
                device3D.Transform = new MatrixTransform3D(matrix);

                

                
                preRoll = roll;
                prePitch = pitch;
                preYaw = yaw;
            }
            catch (Exception err)
            {
                System.Windows.Forms.MessageBox.Show(err.Message);
            }
        }

        public string resetView()
        {
            try
            {
                var centerR = new Point3D(0, 5, 7.5);
                var centerP = new Point3D(-12, 0, 7.5);
                var centerY = new Point3D(-12, 5, 0);


                matrix = device.Transform.Value;
                matrix.RotateAt(new Quaternion(new Vector3D(1, 0, 0), 0), centerR);
                device.Transform = new MatrixTransform3D(matrix);

                matrix.RotateAt(new Quaternion(new Vector3D(0, 1, 0), 0), centerP);
                device.Transform = new MatrixTransform3D(matrix);

                matrix.RotateAt(new Quaternion(new Vector3D(0, 0, 1), 0), centerY);
                device.Transform = new MatrixTransform3D(matrix);

                return Troll.ToString() + " " + Tpitch.ToString() + " " + Tyaw.ToString();

            }
            catch (Exception err)
            {
                return "";
                System.Windows.Forms.MessageBox.Show(err.Message);
            }
        }

    }
}
