using System.Windows.Forms;

// Change the namespace to your project's namespace.
namespace Game_Of_Life___Boyett__Sandy
{
    internal class GraphicsPanel : Panel
    {
        // Default constructor
        public GraphicsPanel()
        {
            // Turn on double buffering.
            this.DoubleBuffered = true;

            // Allow repainting when the window is resized.
            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }
    }
}
