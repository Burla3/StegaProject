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
            this.SizeOfHammingMatrix = new System.Windows.Forms.NumericUpDown();
            this.TextBoxLabel = new System.Windows.Forms.Label();
            this.PictureBoxLabel = new System.Windows.Forms.Label();
            this.HammingMatrixSizeLabel = new System.Windows.Forms.Label();
            this.CharactersLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.LoadedPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SizeOfHammingMatrix)).BeginInit();
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
            this.TextBox.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            // 
            // LengthOfText
            // 
            this.LengthOfText.Location = new System.Drawing.Point(568, 208);
            this.LengthOfText.Name = "LengthOfText";
            this.LengthOfText.ReadOnly = true;
            this.LengthOfText.Size = new System.Drawing.Size(69, 20);
            this.LengthOfText.TabIndex = 5;
            // 
            // SizeOfHammingMatrix
            // 
            this.SizeOfHammingMatrix.Location = new System.Drawing.Point(318, 289);
            this.SizeOfHammingMatrix.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.SizeOfHammingMatrix.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SizeOfHammingMatrix.Name = "SizeOfHammingMatrix";
            this.SizeOfHammingMatrix.Size = new System.Drawing.Size(30, 20);
            this.SizeOfHammingMatrix.TabIndex = 6;
            this.SizeOfHammingMatrix.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SizeOfHammingMatrix.ValueChanged += new System.EventHandler(this.SizeOfHammingMatrix_ValueChanged);
            // 
            // TextBoxLabel
            // 
            this.TextBoxLabel.AutoSize = true;
            this.TextBoxLabel.Location = new System.Drawing.Point(315, 34);
            this.TextBoxLabel.Name = "TextBoxLabel";
            this.TextBoxLabel.Size = new System.Drawing.Size(49, 13);
            this.TextBoxLabel.TabIndex = 7;
            this.TextBoxLabel.Text = "Text Box";
            // 
            // PictureBoxLabel
            // 
            this.PictureBoxLabel.AutoSize = true;
            this.PictureBoxLabel.Location = new System.Drawing.Point(49, 33);
            this.PictureBoxLabel.Name = "PictureBoxLabel";
            this.PictureBoxLabel.Size = new System.Drawing.Size(61, 13);
            this.PictureBoxLabel.TabIndex = 8;
            this.PictureBoxLabel.Text = "Picture Box";
            // 
            // HammingMatrixSizeLabel
            // 
            this.HammingMatrixSizeLabel.AutoSize = true;
            this.HammingMatrixSizeLabel.Location = new System.Drawing.Point(210, 291);
            this.HammingMatrixSizeLabel.Name = "HammingMatrixSizeLabel";
            this.HammingMatrixSizeLabel.Size = new System.Drawing.Size(105, 13);
            this.HammingMatrixSizeLabel.TabIndex = 9;
            this.HammingMatrixSizeLabel.Text = "Hamming Matrix Size";
            this.HammingMatrixSizeLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // CharactersLabel
            // 
            this.CharactersLabel.AutoSize = true;
            this.CharactersLabel.Location = new System.Drawing.Point(568, 189);
            this.CharactersLabel.Name = "CharactersLabel";
            this.CharactersLabel.Size = new System.Drawing.Size(58, 13);
            this.CharactersLabel.TabIndex = 10;
            this.CharactersLabel.Text = "Characters";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 350);
            this.Controls.Add(this.CharactersLabel);
            this.Controls.Add(this.HammingMatrixSizeLabel);
            this.Controls.Add(this.PictureBoxLabel);
            this.Controls.Add(this.TextBoxLabel);
            this.Controls.Add(this.SizeOfHammingMatrix);
            this.Controls.Add(this.LengthOfText);
            this.Controls.Add(this.TextBox);
            this.Controls.Add(this.ExtractMessage);
            this.Controls.Add(this.EncodeMsg);
            this.Controls.Add(this.LoadedPicture);
            this.Controls.Add(this.LoadImage);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.LoadedPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SizeOfHammingMatrix)).EndInit();
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
        private System.Windows.Forms.NumericUpDown SizeOfHammingMatrix;
        private System.Windows.Forms.Label TextBoxLabel;
        private System.Windows.Forms.Label PictureBoxLabel;
        private System.Windows.Forms.Label HammingMatrixSizeLabel;
        private System.Windows.Forms.Label CharactersLabel;
    }
}

