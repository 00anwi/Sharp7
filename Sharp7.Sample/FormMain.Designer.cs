namespace Sharp7.Sample;

partial class FormMain
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        BWV_Main = new Microsoft.AspNetCore.Components.WebView.WindowsForms.BlazorWebView();
        SuspendLayout();
        // 
        // BWV_Main
        // 
        BWV_Main.Dock = DockStyle.Fill;
        BWV_Main.Location = new Point(0, 0);
        BWV_Main.Name = "BWV_Main";
        BWV_Main.Size = new Size(800, 450);
        BWV_Main.StartPath = "/";
        BWV_Main.TabIndex = 0;
        BWV_Main.Text = "blazorWebView1";
        // 
        // FormMain
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(BWV_Main);
        Name = "FormMain";
        Text = "Form1";
        ResumeLayout(false);
    }

    #endregion

    private Microsoft.AspNetCore.Components.WebView.WindowsForms.BlazorWebView BWV_Main;
}
