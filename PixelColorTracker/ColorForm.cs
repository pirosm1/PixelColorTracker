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
    public partial class ColorForm : Form
    {
        private Label[][] labels;

        private const int numberOfCols = 9;
        private const int numberOfRows = 9;

        public ColorForm()
        {
            InitializeComponent();


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
            labels[5][0] = label6;
            labels[6][0] = label7;
            labels[7][0] = label8;
            labels[8][0] = label9;

            labels[0][1] = label10;
            labels[1][1] = label11;
            labels[2][1] = label12;
            labels[3][1] = label13;
            labels[4][1] = label14;
            labels[5][1] = label15;
            labels[6][1] = label16;
            labels[7][1] = label17;
            labels[8][1] = label18;

            labels[0][2] = label19;
            labels[1][2] = label20;
            labels[2][2] = label21;
            labels[3][2] = label22;
            labels[4][2] = label23;
            labels[5][2] = label24;
            labels[6][2] = label25;
            labels[7][2] = label26;
            labels[8][2] = label27;

            labels[0][3] = label28;
            labels[1][3] = label29;
            labels[2][3] = label30;
            labels[3][3] = label31;
            labels[4][3] = label32;
            labels[5][3] = label33;
            labels[6][3] = label34;
            labels[7][3] = label35;
            labels[8][3] = label36;

            labels[0][4] = label37;
            labels[1][4] = label38;
            labels[2][4] = label39;
            labels[3][4] = label40;
            labels[4][4] = label41;
            labels[5][4] = label42;
            labels[6][4] = label43;
            labels[7][4] = label44;
            labels[8][4] = label45;

            labels[0][5] = label46;
            labels[1][5] = label47;
            labels[2][5] = label48;
            labels[3][5] = label49;
            labels[4][5] = label50;
            labels[5][5] = label51;
            labels[6][5] = label52;
            labels[7][5] = label53;
            labels[8][5] = label54;

            labels[0][6] = label55;
            labels[1][6] = label56;
            labels[2][6] = label57;
            labels[3][6] = label58;
            labels[4][6] = label59;
            labels[5][6] = label60;
            labels[6][6] = label61;
            labels[7][6] = label62;
            labels[8][6] = label63;

            labels[0][7] = label64;
            labels[1][7] = label65;
            labels[2][7] = label66;
            labels[3][7] = label67;
            labels[4][7] = label68;
            labels[5][7] = label69;
            labels[6][7] = label70;
            labels[7][7] = label71;
            labels[8][7] = label72;

            labels[0][8] = label73;
            labels[1][8] = label74;
            labels[2][8] = label75;
            labels[3][8] = label76;
            labels[4][8] = label77;
            labels[5][8] = label78;
            labels[6][8] = label79;
            labels[7][8] = label80;
            labels[8][8] = label81;
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

        public void Load_Colors(string color, Point middle, Bitmap loadedImage)
        {
            // for each label
            for (int x = 0; x < numberOfCols; x++)
            {
                int colOffset = middle.X + x - 4;
                // If the cursor is on the very right or left of the screen,
                if (colOffset < 0 || colOffset >= loadedImage.Width)
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
                    int rowOffset = middle.Y + y - 4;

                    // if the cursor is on the very top or very bottom of the screen
                    if (rowOffset < 0 || rowOffset >= loadedImage.Height)
                    {
                        // we can throw out this cell
                        labels[x][y].Text = "";
                        labels[x][y].BackColor = Color.Black;
                    }
                    else
                    {
                        Color pixelColor = loadedImage.GetPixel(colOffset, rowOffset);
                        byte chosenColor = 0;
                        if (color == "red")
                        {
                            chosenColor = pixelColor.R;
                        }
                        else if (color == "blue")
                        {
                            chosenColor = pixelColor.B;
                        }
                        else
                        {
                            chosenColor = pixelColor.G;
                        }
                        labels[x][y].Text = string.Format("({0}, {1}){2}{3}", colOffset, rowOffset, Environment.NewLine, chosenColor);
                        Color calcColor = Color.FromArgb(chosenColor, chosenColor, chosenColor);
                        labels[x][y].BackColor = calcColor;
                        labels[x][y].ForeColor = Determine_Font_Color(calcColor);
                    }
                }
            }
        }
    }
}
