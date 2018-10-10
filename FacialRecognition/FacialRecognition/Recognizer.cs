using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.IO;
using System.Data.Sql;
using System.Data.SqlClient;

namespace FacialRecognition
{
    public partial class Recognizer : Form
    {
       //initialize
        Image<Bgr, Byte> currentFrame;
        Capture grabber;
        HaarCascade face;
        Image<Gray, byte> result, TrainedFace = null;
        Image<Gray, byte> gray = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.5d, 0.5d);
      //now initialize a list to save recognized names
        List<string> NamePersons = new List<string>();
        string name = null;
        int t, ContTrain, NumLabels;

        Conexion c = new Conexion();
        public Recognizer()
        {
            InitializeComponent();
               face = new HaarCascade("haarcascade_frontalface_default.xml");
               try
               {
                   //Load of previus trainned faces and labels for each image
                   string Labelsinfo = File.ReadAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt");
                //MessageBox.Show(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt");
         
                   string[] Labels = Labelsinfo.Split('%');
                   NumLabels = Convert.ToInt16(Labels[0]);
                   ContTrain = NumLabels;
                   string LoadFaces;

                   for (int tf = 1; tf < NumLabels + 1; tf++)
                   {
                       LoadFaces = "face" + tf + ".bmp";
                       trainingImages.Add(new Image<Gray, byte>(Application.StartupPath + "/TrainedFaces/" + LoadFaces));
                       labels.Add(Labels[tf]);//make list string 
                   }
                   //initialize all variable same as previous

               }
               catch (Exception e)
               {
                   MessageBox.Show("no image trained");
               }
        }

        private void Recognizer_Load(object sender, EventArgs e)
        {
            if (label2.Text.Equals('1') )
            {
                this.BackColor = System.Drawing.Color.Lime;
            }
            else
            {
                this.BackColor = System.Drawing.Color.Red;

            }

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //link it to star form
            //Hi lets start face recognition 
            //import opencv libraries
            //now same start camera and capture face as describe in face detection then go to recognition so
            //Initialize the capture device
            grabber = new Capture();
            grabber.QueryFrame();
            //Initialize the FrameGraber event
            Application.Idle += new EventHandler(FrameGrabber);
            //add frame grabber same in detection lecture
            //copy code
        }
        void FrameGrabber(object sender, EventArgs e)
        {

            NamePersons.Add("");
            //now detect no. of faces in scene
            label2.Text = "0";

            //Get the current frame form capture device

            currentFrame = grabber.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

            //Convert it to Grayscale
            gray = currentFrame.Convert<Gray, Byte>();

            //Face Detector
            MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
          face,
          1.3,
          10,
          Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
          new Size(20, 20));

            //Action for each element detected
            foreach (MCvAvgComp f in facesDetected[0])
            {
                DateTime horadetectada = DateTime.Now;
                t = t + 1;
                result = currentFrame.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                //draw the face detected in the 0th (gray) channel with blue color
                currentFrame.Draw(f.rect, new Bgr(Color.Red), 2);
                //initialize result,t and gray if (trainingImages.ToArray().Length != 0)
                {
                    //termcriteria against each image to find a match with it perform different iterations
                    MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain, 0.001);
                    //call class by creating object and pass parameters
                    EigenObjectRecognizer recognizer = new EigenObjectRecognizer(
                         trainingImages.ToArray(),
                         labels.ToArray(),
                         1000,
                         ref termCrit);
                    //next step is to name find for recognize face
                    name = recognizer.Recognize(result);
                    //now show recognized person name so
                  
                    currentFrame.Draw(name, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.LightGreen));//initalize font for the name captured
                    c.InsertarHoradetect(name, horadetectada);
                }
                NamePersons[t - 1] = name;
                NamePersons.Add("");
                //now we will check detected faces multiple or just one in next lecture uptill now we are done with recognition
                label2.Text = facesDetected[0].Length.ToString();


                if (label2.Text == "1")
                {
                    c.InsertarHoradetect(name, horadetectada);
                    MessageBox.Show(" Te la inserto toda");

                    this.BackColor = System.Drawing.Color.Lime;
                }

                else
                {
                    MessageBox.Show(" No se logro Insertar nada ctm");
                    this.BackColor = System.Drawing.Color.Red;

                }


                
            }
            imageBox1.Image = currentFrame;
            //load haarclassifier and previous save image in directory to find match
          
            //hi now perform face recognitione
            //first of all add eigen class to project
            //i will upload in resource section so you can have it
            
            //Check that trained faces are present to recognize face
           //Done now run and test your program
            //Done with this now i will upload complete face recognition sdk
            //hope you learn program and enjoyed it

        }

        private void button4_Click(object sender, EventArgs e)
        {
            StartScreen s = new StartScreen();
            s.Show();
           
           
            this.Hide();
        }
       

    }
}
