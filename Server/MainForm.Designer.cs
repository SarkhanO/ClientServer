namespace Server
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonStartStreaming = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonStartStreaming
            // 
            this.buttonStartStreaming.Location = new System.Drawing.Point(12, 369);
            this.buttonStartStreaming.Name = "buttonStartStreaming";
            this.buttonStartStreaming.Size = new System.Drawing.Size(75, 23);
            this.buttonStartStreaming.TabIndex = 0;
            this.buttonStartStreaming.Text = "Start";
            this.buttonStartStreaming.UseVisualStyleBackColor = true;
            this.buttonStartStreaming.Click += new System.EventHandler(this.buttonStartStreaming_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 404);
            this.Controls.Add(this.buttonStartStreaming);
            this.Name = "MainForm";
            this.Text = "Server";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonStartStreaming;
    }
}

