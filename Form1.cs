using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace PIN_MFA_0._2
{
    public partial class Form1 : Form
    {
        Button[] keypad = new Button[10]; //hold all buttons in array 
        int[] digitOrder = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }; //handles new shuffled keypad digit order
        int[] lieSequence = { 1, 2, 3, 4, 0, 0, 0 }; //randomized once per session, communicates blinkpass scheme
        string initialPin;
        string transformedPin;
        int failedAttempts = 0;
        Label[] matrixLabels = new Label[10]; //hold all transformation matrix labels in array

        // Import necessary Win32 APIs
        [DllImport("gdi32.dll")]
        private static extern bool SetDeviceGammaRamp(IntPtr hDC, ref RAMP ramp);

        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

        // Define the RAMP structure for gamma ramp
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct RAMP
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public UInt16[] Red;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public UInt16[] Green;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public UInt16[] Blue;
        }

        public Form1()
        {
            int[,] transMatrix = new int[2, 5];

            InitializeComponent();

            keypad[0] = button0; //add buttons to arr
            keypad[1] = button1;
            keypad[2] = button2;
            keypad[3] = button3;
            keypad[4] = button4;
            keypad[5] = button5;
            keypad[6] = button6;
            keypad[7] = button7;
            keypad[8] = button8;
            keypad[9] = button9;

            matrixLabels[0] = matrixLabel1; //add matrix labels to arr
            matrixLabels[1] = matrixLabel2;
            matrixLabels[2] = matrixLabel3;
            matrixLabels[3] = matrixLabel4;
            matrixLabels[4] = matrixLabel5;
            matrixLabels[5] = matrixLabel6;
            matrixLabels[6] = matrixLabel7;
            matrixLabels[7] = matrixLabel8;
            matrixLabels[8] = matrixLabel9;
            matrixLabels[9] = matrixLabel10;

            shuffleNumbers(new Random(), lieSequence); //generate new lie-sequence

            foreach(int i in lieSequence)
            {
                System.Console.WriteLine(i);
            }

            
            Random rng = new Random(); //generate 4 random 0-9 digits for initial PIN
            for(int i = 0; i<4; i++)
            {
               initialPin += rng.Next(0, 10).ToString();
            }
            labelInitialPin.Text += initialPin;
            
           
            populate(transMatrix);

            getTransformedPin();
            System.Console.WriteLine("Transformed PIN: " + transformedPin);


            //could add a timeout/inactivity feature that closes or restarts authentication process

            //TODO: brainstorm new feature to communicate user PIN
            //TODO: screen blinks occur BEFORE application is visible
            //TODO: alert user of auth lockout & forced cool down via email

            //getNextLieSequence();
            //https://learn.microsoft.com/en-us/dotnet/desktop/winforms/order-of-events-in-windows-forms?view=netframeworkdesktop-4.8
            //https://stackoverflow.com/questions/31512673/wont-load-label-text-at-start-of-windows-form
        }


        protected override void OnShown(EventArgs e) //TODO: need to find correct event to bind this method to
        {
            System.Console.WriteLine("OnShown event triggered");
            getNextLieSequence();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Restore the original gamma ramp values when the form is closing
            SetGamma(140);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBoxDisplay.Text.Length < 7)
            {
                textBoxDisplay.AppendText(button1.Text);
                shuffleNumbers(new Random(), digitOrder);
                getNextLieSequence();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBoxDisplay.Text.Length < 7)
            {
                textBoxDisplay.AppendText(button2.Text);
                shuffleNumbers(new Random(), digitOrder);
                getNextLieSequence();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBoxDisplay.Text.Length < 7)
            {
                textBoxDisplay.AppendText(button3.Text);
                shuffleNumbers(new Random(), digitOrder);
                getNextLieSequence();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBoxDisplay.Text.Length < 7)
            {
                textBoxDisplay.AppendText(button4.Text);
                shuffleNumbers(new Random(), digitOrder);
                getNextLieSequence();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBoxDisplay.Text.Length < 7)
            {
                textBoxDisplay.AppendText(button5.Text);
                shuffleNumbers(new Random(), digitOrder);
                getNextLieSequence();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBoxDisplay.Text.Length < 7)
            {
                textBoxDisplay.AppendText(button6.Text);
                shuffleNumbers(new Random(), digitOrder);
                getNextLieSequence();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (textBoxDisplay.Text.Length < 7)
            {
                textBoxDisplay.AppendText(button7.Text);
                shuffleNumbers(new Random(), digitOrder);
                getNextLieSequence();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (textBoxDisplay.Text.Length < 7)
            {
                textBoxDisplay.AppendText(button8.Text);
                shuffleNumbers(new Random(), digitOrder);
                getNextLieSequence();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (textBoxDisplay.Text.Length < 7)
            {
                textBoxDisplay.AppendText(button9.Text);
                shuffleNumbers(new Random(), digitOrder);
                getNextLieSequence();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (textBoxDisplay.Text.Length < 7)
            {
                textBoxDisplay.AppendText(button0.Text);
                shuffleNumbers(new Random(), digitOrder);
                getNextLieSequence();
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            textBoxDisplay.Clear();
            getNextLieSequence();
        }

        //delete last number in textbox
        private void button11_Click(object sender, EventArgs e)
        {
            
            if(textBoxDisplay.Text.Length > 0) //ensure textbox has >=1 character
            {
                textBoxDisplay.Text = textBoxDisplay.Text.Substring(0, textBoxDisplay.Text.Length -1);
                getNextLieSequence();
            }
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            if (textBoxDisplay.Text.Length < 7) //user tries to submit before finishing lie-sequence
            {
                MessageBox.Show("Insufficient PIN digits entered");
                return;
            }

            if (authenticate())
            {
                SetGamma(140);
                MessageBox.Show("Authentication Success!");
                Application.Restart();
                Environment.Exit(0);
            }
            else if(!authenticate() && failedAttempts==2) //3 failed attempts, lock out user & close app
            {
                DialogResult dialog = MessageBox.Show("Exceeded Max Failed Attempts\nLocking user out...");
                if(dialog == DialogResult.OK)
                {
                    //*************RESET MONITOR GAMMA HERE!!**********************
                    SetGamma(140);
                    Application.Exit();
                }
            }
            else
            {
                MessageBox.Show("Authentication Failure!");
                failedAttempts++;
                labelFailedAttempts.Text = "Failed Attempts: "+failedAttempts;
                textBoxDisplay.Clear();
                getNextLieSequence();
            }
            
            shuffleNumbers(new Random(), digitOrder); //ramdomize placement of digits on numberpad
            //reflect randomized order into buttons List
            /*
            for (int i = 0; i < keypad.Length; i++)
            {
                keypad[i].Text = digitOrder[i].ToString();
            }*/

            textBoxDisplay.Clear();
            getNextLieSequence();
        }

        private void shuffleNumbers(Random rng, int[] array)
        {
            int n = array.Length; 

            while (n > 1)   //randomize int array via fisher-yates algorithm
            {
                int k = rng.Next(n--);
                int temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }

            //reflect randomized order into buttons List
            for (int i = 0; i < keypad.Length; i++)
            {
                keypad[i].Text = digitOrder[i].ToString();
            }
        }
        private void getNextLieSequence() //after each keypad button click read next digit in lie sequence; update label to reflect # of blinks
        {
            int lieIndex = textBoxDisplay.Text.Length; //determine next lie digit on # of digits alreadty entered
            int digit;

            if( lieIndex == 7 ) //dont need new lie digit on last input digit
            {
                return;
            }
            else
            {
                digit = lieSequence[lieIndex];
            }

            switch (digit)
            {
                case 0:
                    //blinkLabel.Text = "Screen is Dark";
                    screenBlink(0);
                    break;
                case 1:
                    //blinkLabel.Text = "Screen Blinks: 1";
                    screenBlink(1);
                    break;
                case 2:
                    //blinkLabel.Text = "Screen Blinks: 2";
                    screenBlink(2);
                    break;
                case 3:
                    //blinkLabel.Text = "Screen Blinks: 3";
                    screenBlink(3);
                    break;
                case 4:
                    //blinkLabel.Text = "Screen Blinks 4";
                    screenBlink(4);
                    break;
            }

            //return digit;
        }

        private void screenBlink(int numBlinks)
        {
            // set monitor's screen gamma to change brightness
            // repeat blinks per numBlinks value

            //StatusLabel.Text = "Status: Please Wait"; //doing this seems to break the app??

            if (numBlinks == 0)  //dark screen
            {
                SetGamma(50);
            }
            else if(numBlinks > 0)
            {
                for (int i = 0; i < numBlinks; i++) //blinking occurs BEFORE application is visible on startup
                {
                    SetGamma(140);
                    System.Threading.Thread.Sleep(333);
                    //wait(333);
                    SetGamma(50);
                    System.Threading.Thread.Sleep(333); //Thread.Sleep will lock UI; wait() will not
                    //wait(333);
                    SetGamma(140);
                    System.Threading.Thread.Sleep(333);
                    //wait(333);
                }
            }
            //StatusLabel.Text = "Status: Ready!";
        }



        private void populate(int[,] matrix) //fill all elements of matrix with random digits 0-9
        {
            Random rng = new Random();
            int labelIndex = 0;
            
            for(int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = rng.Next(0, 10); //generate random trans matrix value
                    matrixLabels[labelIndex].Text = matrix[i, j].ToString(); //update corresponding label with new value
                    labelIndex++;
                }
            }
        }

        private void getTransformedPin() //calculate transformed PIN from initial using transformation matrix
        {
            foreach (char digit in initialPin)
            {
                switch (digit)
                {
                    case '1':
                        transformedPin += matrixLabel1.Text;
                        break;
                    case '2':
                        transformedPin += matrixLabel2.Text;
                        break;
                    case '3':
                        transformedPin += matrixLabel3.Text;
                        break;
                    case '4':
                        transformedPin += matrixLabel4.Text;
                        break;
                    case '5':
                        transformedPin += matrixLabel5.Text;
                        break;
                    case '6':
                        transformedPin += matrixLabel6.Text;
                        break;
                    case '7':
                        transformedPin += matrixLabel7.Text;
                        break;
                    case '8':
                        transformedPin += matrixLabel8.Text;
                        break;
                    case '9':
                        transformedPin += matrixLabel9.Text;
                        break;
                    case '0':
                        transformedPin += matrixLabel10.Text;
                        break;
                }
            }
        }

        //acceptable gamma range is 1-256
        public static void SetGamma(int gamma)
        {
            if (gamma <= 256 && gamma >= 1)
            {
                RAMP ramp = new RAMP();
                ramp.Red = new ushort[256];
                ramp.Green = new ushort[256];
                ramp.Blue = new ushort[256];
                for (int i = 1; i < 256; i++)
                {
                    int iArrayValue = i * (gamma + 128);

                    if (iArrayValue > 65535)
                        iArrayValue = 65535;
                    ramp.Red[i] = ramp.Blue[i] = ramp.Green[i] = (ushort)iArrayValue;
                }
                SetDeviceGammaRamp(GetDC(IntPtr.Zero), ref ramp);
            }
        }

        private void wait(int milliseconds)
        {
            var timer1 = new System.Windows.Forms.Timer();
            if (milliseconds == 0 || milliseconds < 0) return;

            // Console.WriteLine("start wait timer");
            timer1.Interval = milliseconds;
            timer1.Enabled = true;
            timer1.Start();

            timer1.Tick += (s, e) =>
            {
                timer1.Enabled = false;
                timer1.Stop();
                // Console.WriteLine("stop wait timer");
            };

            while (timer1.Enabled)
            {
                Application.DoEvents();
            }
        }

        private Boolean authenticate()
        {
            //parse input, matching expected PIN with lie sequence
            string input = textBoxDisplay.Text;
            for(int i=0; i<lieSequence.Length; i++)
            {
                if (lieSequence[i] == 0) //false digit, ignore
                {
                    continue;
                }
                else if (lieSequence[i] == 1) //verify 1st digit of transformed PIN is found at input index
                {
                    if (!(input[i].Equals(transformedPin[0])))
                    {
                        return false;
                    }
                }
                else if (lieSequence[i] == 2) //verify 2nd digit of transformed PIN is found at input index
                {
                    if (!(input[i].Equals(transformedPin[1])))
                    {
                        return false;
                    }
                }
                else if (lieSequence[i] == 3) //verify 3rd digit of transformed PIN is found at input index
                {
                    if (!(input[i].Equals(transformedPin[2])))
                    {
                        return false;
                    }
                }
                else if (lieSequence[i] == 4) //verify 4th digit of transformed PIN is found at input index
                {
                    if (!(input[i].Equals(transformedPin[3])))
                    {
                        return false;
                    }
                }
            }

            //if all expected digits present in correct order
            return true;
        }
    }
}
