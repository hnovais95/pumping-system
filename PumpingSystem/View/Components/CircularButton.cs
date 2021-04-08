using System.Windows.Forms;

namespace PumpingSystem.View
{
    class CircularButton : Button
    {
        protected override void OnPaint(PaintEventArgs pevent)
        {
            System.Drawing.Drawing2D.GraphicsPath grPath = new System.Drawing.Drawing2D.GraphicsPath();
            grPath.AddEllipse(0, 0, ClientSize.Width, ClientSize.Height);
            this.Region = new System.Drawing.Region(grPath);
            base.OnPaint(pevent);
        }
    }
}
