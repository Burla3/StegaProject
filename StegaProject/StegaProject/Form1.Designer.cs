namespace StegaProject {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing ) {
            if ( disposing && ( components != null ) ) {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.LoadImage = new System.Windows.Forms.Button();
            this.LoadedPicture = new System.Windows.Forms.PictureBox();
            this.EncodeMsg = new System.Windows.Forms.Button();
            this.ExtractMessage = new System.Windows.Forms.Button();
            this.TextBox = new System.Windows.Forms.RichTextBox();
            this.LengthOfText = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.LoadedPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // LoadImage
            // 
            this.LoadImage.Location = new System.Drawing.Point(101, 260);
            this.LoadImage.Name = "LoadImage";
            this.LoadImage.Size = new System.Drawing.Size(75, 23);
            this.LoadImage.TabIndex = 0;
            this.LoadImage.Text = "Load Image";
            this.LoadImage.UseVisualStyleBackColor = true;
            this.LoadImage.Click += new System.EventHandler(this.LoadImage_Click);
            // 
            // LoadedPicture
            // 
            this.LoadedPicture.Location = new System.Drawing.Point(49, 53);
            this.LoadedPicture.Name = "LoadedPicture";
            this.LoadedPicture.Size = new System.Drawing.Size(197, 176);
            this.LoadedPicture.TabIndex = 1;
            this.LoadedPicture.TabStop = false;
            // 
            // EncodeMsg
            // 
            this.EncodeMsg.Location = new System.Drawing.Point(315, 260);
            this.EncodeMsg.Name = "EncodeMsg";
            this.EncodeMsg.Size = new System.Drawing.Size(75, 23);
            this.EncodeMsg.TabIndex = 2;
            this.EncodeMsg.Text = "Encode Message";
            this.EncodeMsg.UseVisualStyleBackColor = true;
            this.EncodeMsg.Click += new System.EventHandler(this.EncodeMsg_Click);
            // 
            // ExtractMessage
            // 
            this.ExtractMessage.Location = new System.Drawing.Point(486, 260);
            this.ExtractMessage.Name = "ExtractMessage";
            this.ExtractMessage.Size = new System.Drawing.Size(75, 23);
            this.ExtractMessage.TabIndex = 3;
            this.ExtractMessage.Text = "Extract";
            this.ExtractMessage.UseVisualStyleBackColor = true;
            this.ExtractMessage.Click += new System.EventHandler(this.ExtractMessage_Click);
            // 
            // TextBox
            // 
            this.TextBox.Location = new System.Drawing.Point(315, 53);
            this.TextBox.Name = "TextBox";
            this.TextBox.ReadOnly = true;
            this.TextBox.Size = new System.Drawing.Size(246, 176);
            this.TextBox.TabIndex = 4;
            this.TextBox.Text = "";
            // 
            // LengthOfText
            // 
            this.LengthOfText.Location = new System.Drawing.Point(568, 208);
            this.LengthOfText.Name = "LengthOfText";
            this.LengthOfText.Size = new System.Drawing.Size(47, 20);
            this.LengthOfText.TabIndex = 5;
            this.LengthOfText.ReadOnly = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 350);
            this.Controls.Add(this.LengthOfText);
            this.Controls.Add(this.TextBox);
            this.Controls.Add(this.ExtractMessage);
            this.Controls.Add(this.EncodeMsg);
            this.Controls.Add(this.LoadedPicture);
            this.Controls.Add(this.LoadImage);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.LoadedPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button LoadImage;
        private System.Windows.Forms.PictureBox LoadedPicture;
        private System.Windows.Forms.Button EncodeMsg;
        private System.Windows.Forms.Button ExtractMessage;
        private System.Windows.Forms.RichTextBox TextBox;
        private System.Windows.Forms.TextBox LengthOfText;
    }
}

