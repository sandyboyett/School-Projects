using System;
using System.Drawing;
using System.Windows.Forms;

namespace Game_Of_Life___Boyett__Sandy
{
    public partial class Form1 : Form
    {

        // The universe array
        bool[,] universe = new bool[100, 100];
        //scratch pad
        bool[,] scratchpad = new bool[100, 100];

        //random int
        int randCellNum = new Random().Next();

        //bool[,] cells; - might not need
        //bool alive;
        //bool dead;

        //bools declared
        bool isAlive = false;
        bool numberOfNeighbors = true;

        // Drawing colors
        Color gridColor = Color.Black;
        Color cellColor = Color.SandyBrown;

        // The Timer class
        Timer timer = new Timer();

        // Generation count
        int generations = 0;
        int living = 0;

        public Form1()
        {
            InitializeComponent();

            // Setup the timer
            timer.Interval = 100; // milliseconds
            timer.Tick += Timer_Tick;
            timer.Enabled = false; // start timer running
        }

        //counts neighbors method
        private void CountNeighbor(int x, int y, out int cellLife)
        {
            if (isAlive == true)
            {
                cellLife = CountNeighborsFinite(x, y);
            }
            else
            {
                cellLife = CountNeighborsToroidal(x, y);
            }
        }

        //supposed to empty out the universe[,] - possibly turn to a button
        //private void EmptyUniverse()
        //{
        //    bool[,] emptyuniverse = new bool[10, 10];
        //    scratchpad = emptyuniverse;
        //}

        // Calculate the next generation of cells
        private void NextGeneration()
        {
            int living = 0;
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    if (universe[x, y] == true)
                    {
                        living++;
                    }

                    CountNeighbor(x, y, out int cellLife);

                    //rules 1-4
                    if (universe[x, y] == true)
                    {
                        if (cellLife < 2)
                        {
                            scratchpad[x, y] = false;
                        }
                        else if (cellLife > 3)
                        {
                            scratchpad[x, y] = false;
                        }
                        else if (cellLife == 2 || cellLife == 3)
                        {
                            scratchpad[x, y] = true;
                        }
                    }
                    else if (universe[x, y] == false)
                    {
                        if (cellLife == 3)
                        {
                            scratchpad[x, y] = true;
                        }
                        else if (cellLife != 3)
                        {
                            scratchpad[x, y] = false;
                        }
                    }
                }
            }
            //swapping the bool[,] for universe/scratchpad
            bool[,] swappingtemp = universe;
            universe = scratchpad;
            scratchpad = swappingtemp;

            // Increment generation count
            generations++;

            // Update status strip generations
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();

            //living cell display
            toolStripStatusLabel1.Text = "Living Cells = " + living.ToString();

            //invalidate the graphicsPanel1
            graphicsPanel1.Invalidate();
        }

        //countneighbors finite method
        private int CountNeighborsFinite(int x, int y)
        {
            int count = 0;
            int xLen = universe.GetLength(0);
            int yLen = universe.GetLength(1);
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    int xCheck = x + xOffset;
                    int yCheck = y + yOffset;

                    // if xOffset and yOffset are both equal to 0 then continue
                    if (xOffset == 0 && yOffset == 0)
                    {
                        continue;
                    }
                    // if xCheck is less than 0 then continue
                    if (xCheck < 0)
                    {
                        continue;
                    }
                    // if yCheck is less than 0 then continue
                    if (yCheck < 0)
                    {
                        continue;
                    }
                    // if xCheck is greater than or equal too xLen then continue
                    if (xCheck >= xLen)
                    {
                        continue;
                    }
                    // if yCheck is greater than or equal too yLen then continue
                    if (yCheck >= yLen)
                    {
                        continue;
                    }

                    if (universe[xCheck, yCheck] == true) count++;
                }
            }
            return count;
        }

        //countneighbors toroidal method
        private int CountNeighborsToroidal(int x, int y)
        {
            int count = 0;
            int xLen = universe.GetLength(0);
            int yLen = universe.GetLength(1);
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    int xCheck = x + xOffset;
                    int yCheck = y + yOffset;

                    // if xOffset and yOffset are both equal to 0 then continue
                    if (xOffset == 0 && yOffset == 0)
                    {
                        continue;
                    }
                    // if xCheck is less than 0 then set to xLen - 1
                    if (xCheck < 0)
                    {
                        xCheck = xLen - 1;
                    }
                    // if yCheck is less than 0 then set to yLen - 1
                    if (yCheck < 0)
                    {
                        yCheck = yLen - 1;
                    }
                    // if xCheck is greater than or equal too xLen then set to 0
                    if (xCheck >= xLen)
                    {
                        xCheck = 0;
                    }
                    // if yCheck is greater than or equal too yLen then set to 0
                    if (yCheck >= yLen)
                    {
                        yCheck = 0;
                    }

                    if (universe[xCheck, yCheck] == true) count++;
                }
            }
            return count;
        }

        // The event called by the timer every Interval milliseconds.
        private void Timer_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }

        //prints the window with items inside
        private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
        {
            // Calculate the width and height of each cell in pixels
            // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
            float cellWidth = (float)graphicsPanel1.ClientSize.Width / (float)universe.GetLength(0);
            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            float cellHeight = (float)graphicsPanel1.ClientSize.Height / (float)universe.GetLength(1);

            // A Pen for drawing the grid lines (color, width)
            Pen gridPen = new Pen(gridColor, 1);

            // A Brush for filling living cells interiors (color)
            Brush cellBrush = new SolidBrush(cellColor);

            //printing the font
            Font font = new Font("Arial", 7f);

            //declaring the strings and aligning inside of rectangle
            StringFormat newstring = new StringFormat();
            newstring.Alignment = StringAlignment.Center;
            newstring.LineAlignment = StringAlignment.Center;

            RectangleF rectangle = new RectangleF(0, 0, 7, 7);

            //declaring starting int - possibly not needed
            //int neighbors = 0;

            //prints the cells - possibly not needed
            //e.Graphics.DrawString(neighbors.ToString(), font, Brushes.Black, rectangle, newstring);


            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    // A rectangle to represent each cell in pixels
                    RectangleF cellRectangle = RectangleF.Empty;
                    cellRectangle.X = x * cellWidth;
                    cellRectangle.Y = y * cellHeight;
                    cellRectangle.Width = cellWidth;
                    cellRectangle.Height = cellHeight;

                    // Fill the cell with a brush if alive
                    if (universe[x, y] == true)
                    {
                        e.Graphics.FillRectangle(cellBrush, cellRectangle);
                    }

                    // Outline the cell with a pen
                    e.Graphics.DrawRectangle(gridPen, cellRectangle.X, cellRectangle.Y, cellRectangle.Width, cellRectangle.Height);

                    //print the numbers inside the rectangles
                    if (numberOfNeighbors == true)
                    {
                        CountNeighbor(x, y, out int neighborIndex);
                        if (neighborIndex != 0)
                        {
                            e.Graphics.DrawString(neighborIndex.ToString(), font, Brushes.Black, cellRectangle, newstring);
                        }
                    }
                }
            }
            // Cleaning up pens and brushes
            gridPen.Dispose();
            cellBrush.Dispose();
        }

        //mouse info
        private void graphicsPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            // If the left mouse button was clicked
            if (e.Button == MouseButtons.Left)
            {
                // Calculate the width and height of each cell in pixels
                float cellWidth = (float)graphicsPanel1.ClientSize.Width / (float)universe.GetLength(0);
                float cellHeight = (float)graphicsPanel1.ClientSize.Height / (float)universe.GetLength(1);

                // Calculate the cell that was clicked in
                // CELL X = MOUSE X / CELL WIDTH
                float x = e.X / cellWidth;
                // CELL Y = MOUSE Y / CELL HEIGHT
                float y = e.Y / cellHeight;

                // Toggle the cell's state
                universe[(int)x, (int)y] = !universe[(int)x, (int)y];

                // Tell Windows you need to repaint
                graphicsPanel1.Invalidate();
            }
        }

        //buttons/tools
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            //play button - starts the program
            timer.Enabled = true;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //pause button - pauses the program
            timer.Enabled = false;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //next button - goes to next generation
            NextGeneration();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            //box with x - empties out the universe
            timer.Enabled = false;
            for (int i = 0; i < universe.GetLength(1); i++)
            {
                for (int j = 0; j < universe.GetLength(0); j++)
                {

                    universe[j, i] = false;
                }
            }
            living = 0;
            generations = 0;

            // Update status strip generations
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();

            //living cell display
            toolStripStatusLabel1.Text = "Living Cells = " + living.ToString();

            //boundary display
            toolStripStatusLabel2.Text = "Boundary";

            //invalidate graphicsPanel1
            graphicsPanel1.Invalidate();
        }

        private void randomizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //randomize tool - randomizes the universe
            Random randUniNum = new Random();
            for (int x = 0; x < universe.GetLength(1); x++)
            {
                for (int y = 0; y < universe.GetLength(0); y++)
                {
                    randCellNum = randUniNum.Next(0, 2);
                    if (randCellNum == 0)
                    {
                        universe[x, y] = true;
                    }
                    else if (randCellNum == 1 || randCellNum == 2)
                    {
                        universe[x, y] = false;
                    }
                }
            }
            graphicsPanel1.Invalidate();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            //finite button - rotate box
            toolStripButton6.CheckState = CheckState.Checked;
            toolStripButton5.CheckState = CheckState.Unchecked;
            isAlive = true;
            if (isAlive == true)
            {
                toolStripStatusLabel2.Text = "Finite";
            }
            else if (isAlive == false)
            {
                toolStripStatusLabel2.Text = "Toroidal";
            }
            graphicsPanel1.Invalidate();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            //toroidal button - infinity sign
            toolStripButton5.CheckState = CheckState.Checked;
            toolStripButton6.CheckState = CheckState.Unchecked;
            isAlive = false;
            if (isAlive == true)
            {
                toolStripStatusLabel2.Text = "Finite";
            }
            else if (isAlive == false)
            {
                toolStripStatusLabel2.Text = "Toroidal";
            }
            graphicsPanel1.Invalidate();
        }

    }
}

