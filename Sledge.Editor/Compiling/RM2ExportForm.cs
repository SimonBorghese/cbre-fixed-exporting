﻿using Sledge.Editor.Documents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Sledge.Editor.Compiling
{
    public partial class RM2ExportForm : Form
    {
        public Document Document;

        public RM2ExportForm()
        {
            InitializeComponent();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textureDims_LostFocus(object sender, EventArgs e)
        {
            int dims = -1; int.TryParse(((TextBox)sender).Text,out dims);
            if (dims >= 512 && dims <= 4096)
            {
                Lightmapper.TextureDims = dims;
            }
            else
            {
                ((TextBox)sender).Text = Lightmapper.TextureDims.ToString();
            }
        }

        private void downscaleFactor_LostFocus(object sender, EventArgs e)
        {
            int factor = -1; int.TryParse(((TextBox)sender).Text, out factor);
            if (factor >= 12 && factor <= 32)
            {
                Lightmapper.DownscaleFactor = factor;
            }
            else
            {
                ((TextBox)sender).Text = Lightmapper.DownscaleFactor.ToString();
            }
        }

        private void blurRadius_LostFocus(object sender, EventArgs e)
        {
            int radius = -1; int.TryParse(((TextBox)sender).Text, out radius);
            if (radius >= 0 && radius <= 10)
            {
                Lightmapper.BlurRadius = radius;
            }
            else
            {
                ((TextBox)sender).Text = Lightmapper.BlurRadius.ToString();
            }
        }

        private string SaveFileName = "";
        private void export_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog save = new SaveFileDialog())
            {
                var filter = "SCP - Containment Breach RoomMesh 2 (*.rm2)|*.rm2";
                save.Filter = filter;
                if (save.ShowDialog() == DialogResult.OK)
                {
                    SaveFileName = save.FileName;
                    textureDims.Enabled = false;
                    downscaleFactor.Enabled = false;
                    blurRadius.Enabled = false;

                    ambientRed.Enabled = false;
                    ambientGreen.Enabled = false;
                    ambientBlue.Enabled = false;

                    export.Enabled = false;
                    cancel.Enabled = true;

                    ProgressLabel.Text = "Starting...";
                    ProgressBar.Enabled = true;

                    exportThread = new Thread(PerformExport);
                    exportThread.Start();
                }
            }
        }

        private void ambientRed_LostFocus(object sender, EventArgs e)
        {
            int r = -1; int.TryParse(((TextBox)sender).Text, out r);
            if (r >=0 && r<=255)
            {
                Lightmapper.AmbientColor = Color.FromArgb(r, Lightmapper.AmbientColor.G, Lightmapper.AmbientColor.B);
                ambientColorBox.BackColor = Lightmapper.AmbientColor;
            }
            else
            {
                ((TextBox)sender).Text = Lightmapper.AmbientColor.R.ToString();
            }
        }

        private void ambientGreen_LostFocus(object sender, EventArgs e)
        {
            int g = -1; int.TryParse(((TextBox)sender).Text, out g);
            if (g >= 0 && g <= 255)
            {
                Lightmapper.AmbientColor = Color.FromArgb(Lightmapper.AmbientColor.R, g, Lightmapper.AmbientColor.B);
                ambientColorBox.BackColor = Lightmapper.AmbientColor;
            }
            else
            {
                ((TextBox)sender).Text = Lightmapper.AmbientColor.G.ToString();
            }
        }

        private void ambientBlue_LostFocus(object sender, EventArgs e)
        {
            int b = -1; int.TryParse(((TextBox)sender).Text, out b);
            if (b >= 0 && b <= 255)
            {
                Lightmapper.AmbientColor = Color.FromArgb(Lightmapper.AmbientColor.R, Lightmapper.AmbientColor.G, b);
                ambientColorBox.BackColor = Lightmapper.AmbientColor;
            }
            else
            {
                ((TextBox)sender).Text = Lightmapper.AmbientColor.B.ToString();
            }
        }

        private void ambientRed_TextChanged(object sender, EventArgs e)
        {
            int r = -1; int.TryParse(((TextBox)sender).Text, out r);
            if (r >= 0 && r <= 255)
            {
                Lightmapper.AmbientColor = Color.FromArgb(r, Lightmapper.AmbientColor.G, Lightmapper.AmbientColor.B);
                ambientColorBox.BackColor = Lightmapper.AmbientColor;
            }
        }

        private void ambientGreen_TextChanged(object sender, EventArgs e)
        {
            int g = -1; int.TryParse(((TextBox)sender).Text, out g);
            if (g >= 0 && g <= 255)
            {
                Lightmapper.AmbientColor = Color.FromArgb(Lightmapper.AmbientColor.R, g, Lightmapper.AmbientColor.B);
                ambientColorBox.BackColor = Lightmapper.AmbientColor;
            }
        }

        private void ambientBlue_TextChanged(object sender, EventArgs e)
        {
            int b = -1; int.TryParse(((TextBox)sender).Text, out b);
            if (b >= 0 && b <= 255)
            {
                Lightmapper.AmbientColor = Color.FromArgb(Lightmapper.AmbientColor.R, Lightmapper.AmbientColor.G, b);
                ambientColorBox.BackColor = Lightmapper.AmbientColor;
            }
        }

        private void ambientColorBox_Click(object sender, EventArgs e)
        {
            if (ambientRed.Enabled)
            {
                using (var cb = new System.Windows.Forms.ColorDialog())
                {
                    if (cb.ShowDialog() == DialogResult.OK)
                    {
                        Lightmapper.AmbientColor = cb.Color;
                        ambientColorBox.BackColor = Lightmapper.AmbientColor;
                        ambientRed.Text = Lightmapper.AmbientColor.R.ToString();
                        ambientGreen.Text = Lightmapper.AmbientColor.G.ToString();
                        ambientBlue.Text = Lightmapper.AmbientColor.B.ToString();
                    }
                }
            }
        }

        Thread exportThread = null;
        private void PerformExport()
        {
            try
            {
                RM2Export.SaveToFile(SaveFileName, Document.Map, this);
            }
            catch (ThreadAbortException e)
            {
                if (Lightmapper.FaceRenderThreads != null)
                {
                    foreach (Thread thread in Lightmapper.FaceRenderThreads)
                    {
                        if (thread.IsAlive)
                        {
                            thread.Abort();
                        }
                    }
                }
                
                ProgressLabel.Invoke((MethodInvoker)(() => ProgressLabel.Text = "Cancelled by the user"));
                ProgressBar.Invoke((MethodInvoker)(() => ProgressBar.Value = 0));
            }
            catch (Exception e)
            {
                ProgressLabel.Invoke((MethodInvoker)(() => ProgressLabel.Text = "Error: " + e.Message));
                ProgressBar.Invoke((MethodInvoker)(() => ProgressBar.Value = 0));
            }
            finally
            {
                Invoke((MethodInvoker)(() =>
                {
                    textureDims.Enabled = true;
                    downscaleFactor.Enabled = true;
                    blurRadius.Enabled = true;

                    ambientRed.Enabled = true;
                    ambientGreen.Enabled = true;
                    ambientBlue.Enabled = true;

                    export.Enabled = true;
                    cancel.Enabled = false;

                    ProgressBar.Enabled = false;
                }));
            }
        }

        private void formClosing(object sender,FormClosingEventArgs args)
        {
            if (exportThread!=null && exportThread.IsAlive)
            {
                args.Cancel = true;
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            if (exportThread != null && exportThread.IsAlive)
            {
                exportThread.Abort();
            }
        }
    }
}