
namespace Bluetooth_LE_Forms {
    partial class BluetoothSelector_Form {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.BluetoothServiceList = new System.Windows.Forms.ListBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BluetoothServiceList
            // 
            this.BluetoothServiceList.FormattingEnabled = true;
            this.BluetoothServiceList.HorizontalScrollbar = true;
            this.BluetoothServiceList.Location = new System.Drawing.Point(15, 25);
            this.BluetoothServiceList.Name = "BluetoothServiceList";
            this.BluetoothServiceList.ScrollAlwaysVisible = true;
            this.BluetoothServiceList.Size = new System.Drawing.Size(260, 251);
            this.BluetoothServiceList.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(263, 13);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Select the bluetooth serivce you would like to pair with";
            // 
            // BluetoothSelector_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(290, 289);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.BluetoothServiceList);
            this.Name = "BluetoothSelector_Form";
            this.Text = "BluetoothSelector_Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox BluetoothServiceList;
        private System.Windows.Forms.Label lblTitle;
    }
}