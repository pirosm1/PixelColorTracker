using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelColorTracker
{
    public partial class PixelColorTrackerForm : Form
    {
        private Label[][] labels;

        private const int numberOfCols = 5;
        private const int numberOfRows = 5;

        public PixelColorTrackerForm()
        {
            InitializeComponent();
        }

        private void PixelColorTrackerForm_Load(object sender, EventArgs e)
        {
            labels = new Label[numberOfCols][];

            for (int x = 0; x < numberOfCols; x++)
            {
                labels[x] = new Label[numberOfRows];
            }

            labels[0][0] = label1;
            labels[1][0] = label2;
            labels[2][0] = label3;
            labels[3][0] = label4;
            labels[4][0] = label5;

            labels[0][1] = label6;
            labels[1][1] = label7;
            labels[2][1] = label8;
            labels[3][1] = label9;
            labels[4][1] = label10;

            labels[0][2] = label11;
            labels[1][2] = label12;
            labels[2][2] = label13;
            labels[3][2] = label14;
            labels[4][2] = label15;

            labels[0][3] = label16;
            labels[1][3] = label17;
            labels[2][3] = label18;
            labels[3][3] = label19;
            labels[4][3] = label20;

            labels[0][4] = label21;
            labels[1][4] = label22;
            labels[2][4] = label23;
            labels[3][4] = label24;
            labels[4][4] = label25;
        }

        private void Load_Image_Button_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openImageDlg = new OpenFileDialog())
            {
                openImageDlg.Title = "Load Image";
                openImageDlg.Filter = "Image Files |*.bmp;*.jpg;*.jpeg;*.gif;*.png";

                if (openImageDlg.ShowDialog() == DialogResult.OK)
                {
                    Loaded_Image_Box.Image = new Bitmap(openImageDlg.FileName);
                }
            }
        }

        // Determine pixel color in accordance to W3C recommendations
        // Determines "relative luminance"
        private Color Determine_Font_Color(Color pixelColor)
        {
            // see https://www.w3.org/TR/WCAG20/relative-luminance.xml
            // for relative luminance definition
            double sR = pixelColor.R / 255.0, sG = pixelColor.G / 255.0, sB = pixelColor.B / 255.0;

            if (sR <= 0.03928)
            {
                sR /= 12.92;
            }
            else
            {
                sR = Math.Pow((sR + 0.055) / 1.055, 2.4);
            }
            if (sG <= 0.03928)
            {
                sG /= 12.92;
            }
            else
            {
                sG = Math.Pow((sG + 0.055) / 1.055, 2.4);
            }
            if (sB <= 0.03928)
            {
                sB /= 12.92;
            }
            else
            {
                sB = Math.Pow((sB + 0.055) / 1.055, 2.4);
            }

            double L = 0.2126 * sR + 0.7152 * sG + 0.0722 * sB;

            // Recommended contrast ratio is (L1 + 0.05) / (L2 + 0.05)
            // where L1 is the lighter color and L2 is the darker color
            // Luminance of black is 0.0, luminance of white is 1.0
            // so if the contrast between the relative luminance of our color L and black
            // is greater that of the relative luminance of our color L and white,
            // then we use black, otherwise we use white.
            // (L + 0.05) / 0.05 > 1.05 / (L + 0.05)
            // simpilfies to L > 0.179
            if (L > 0.179)
            {
                return Color.Black;
            }
            return Color.White;
        }
        
        private void Loaded_Image_Box_MouseMove(object sender, MouseEventArgs e)
        {
            // if the image is not loaded, do nothing
            if (Loaded_Image_Box.Image == null)
            {
                return;
            }
            Bitmap loadedImage = (Bitmap)Loaded_Image_Box.Image;
            Point mouseLocation = e.Location;
            for (int x = 0; x < numberOfCols; x++)
            {
                int colOffset = mouseLocation.X + x - 2;
                // If the cursor is on the very right or left of the screen,
                if (colOffset < 0 || colOffset >= Loaded_Image_Box.Image.Width)
                {
                    // we can safely throw out the whole column
                    for (int y = 0; y < numberOfRows; y++)
                    {
                        labels[x][y].Text = "";
                        labels[x][y].BackColor = Color.Black;
                    }
                    continue;
                }
                for (int y = 0; y < numberOfRows; y++)
                {
                    int rowOffset = mouseLocation.Y + y - 2;

                    // if the cursor is on the very top or very bottom of the screen
                    if (rowOffset < 0 || rowOffset >= Loaded_Image_Box.Image.Height)
                    {
                        // we can throw out this cell
                        labels[x][y].Text = "";
                        labels[x][y].BackColor = Color.Black;
                    }
                    else
                    {
                        Color pixelColor = loadedImage.GetPixel(colOffset, rowOffset);
                        labels[x][y].Text = string.Format("({0}, {1}){5}{2}{5}{3}{5}{4}", colOffset, rowOffset, pixelColor.R, pixelColor.G, pixelColor.G, Environment.NewLine);
                        labels[x][y].BackColor = pixelColor;
                        labels[x][y].ForeColor = Determine_Font_Color(pixelColor);
                    }
                }
            }
        }
    }
}
