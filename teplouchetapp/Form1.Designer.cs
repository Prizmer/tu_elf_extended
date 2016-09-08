namespace elfextendedapp
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
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
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.buttonImport = new System.Windows.Forms.Button();
            this.dgv1 = new System.Windows.Forms.DataGridView();
            this.ofd1 = new System.Windows.Forms.OpenFileDialog();
            this.buttonPing = new System.Windows.Forms.Button();
            this.sfd1 = new System.Windows.Forms.SaveFileDialog();
            this.buttonPoll = new System.Windows.Forms.Button();
            this.buttonExport = new System.Windows.Forms.Button();
            this.comboBoxComPorts = new System.Windows.Forms.ComboBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.buttonStop = new System.Windows.Forms.Button();
            this.numericUpDownComReadTimeout = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxPollOffline = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.numericUpDownComWriteTimeout = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBoxTcp = new System.Windows.Forms.CheckBox();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.textBoxIp = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbModePreVals = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgv1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownComReadTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownComWriteTimeout)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonImport
            // 
            this.buttonImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonImport.Location = new System.Drawing.Point(187, 4);
            this.buttonImport.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(97, 25);
            this.buttonImport.TabIndex = 3;
            this.buttonImport.Text = "Импорт (*.xls)";
            this.toolTip1.SetToolTip(this.buttonImport, "Загрузить таблицу содержающую столбец с номерами квартир и столбец с заводскими н" +
        "омерами счетчиков");
            this.buttonImport.UseVisualStyleBackColor = true;
            this.buttonImport.Click += new System.EventHandler(this.buttonImport_Click);
            // 
            // dgv1
            // 
            this.dgv1.AllowUserToAddRows = false;
            this.dgv1.AllowUserToDeleteRows = false;
            this.dgv1.AllowUserToResizeRows = false;
            this.dgv1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv1.ColumnHeadersVisible = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgv1.Location = new System.Drawing.Point(5, 72);
            this.dgv1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dgv1.Name = "dgv1";
            this.dgv1.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgv1.RowTemplate.Height = 28;
            this.dgv1.Size = new System.Drawing.Size(611, 293);
            this.dgv1.TabIndex = 4;
            // 
            // ofd1
            // 
            this.ofd1.FileName = "openFileDialog1";
            // 
            // buttonPing
            // 
            this.buttonPing.Enabled = false;
            this.buttonPing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPing.Location = new System.Drawing.Point(8, 36);
            this.buttonPing.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonPing.Name = "buttonPing";
            this.buttonPing.Size = new System.Drawing.Size(83, 25);
            this.buttonPing.TabIndex = 5;
            this.buttonPing.Text = "Тест связи";
            this.toolTip1.SetToolTip(this.buttonPing, "Выполняется только проверка связи без получения каких-либо данных со счетчика");
            this.buttonPing.UseVisualStyleBackColor = true;
            this.buttonPing.Click += new System.EventHandler(this.buttonPing_Click);
            // 
            // buttonPoll
            // 
            this.buttonPoll.Enabled = false;
            this.buttonPoll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPoll.Location = new System.Drawing.Point(99, 36);
            this.buttonPoll.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonPoll.Name = "buttonPoll";
            this.buttonPoll.Size = new System.Drawing.Size(83, 25);
            this.buttonPoll.TabIndex = 6;
            this.buttonPoll.Text = "Опрос";
            this.toolTip1.SetToolTip(this.buttonPoll, "Выполняются проверка связи и опрос счетчика по текущим значениям");
            this.buttonPoll.UseVisualStyleBackColor = true;
            this.buttonPoll.Click += new System.EventHandler(this.buttonPoll_Click);
            // 
            // buttonExport
            // 
            this.buttonExport.Enabled = false;
            this.buttonExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExport.Location = new System.Drawing.Point(287, 4);
            this.buttonExport.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(97, 25);
            this.buttonExport.TabIndex = 41;
            this.buttonExport.Text = "Экспорт (*.xls)";
            this.toolTip1.SetToolTip(this.buttonExport, "Сохранить полученные в программе данные");
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // comboBoxComPorts
            // 
            this.comboBoxComPorts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxComPorts.FormattingEnabled = true;
            this.comboBoxComPorts.Location = new System.Drawing.Point(8, 8);
            this.comboBoxComPorts.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBoxComPorts.Name = "comboBoxComPorts";
            this.comboBoxComPorts.Size = new System.Drawing.Size(85, 21);
            this.comboBoxComPorts.TabIndex = 42;
            this.toolTip1.SetToolTip(this.comboBoxComPorts, "Системный последовательный порт");
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 9, 0);
            this.statusStrip1.Size = new System.Drawing.Size(629, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 43;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(133, 16);
            this.toolStripProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 17);
            // 
            // buttonStop
            // 
            this.buttonStop.Enabled = false;
            this.buttonStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonStop.Location = new System.Drawing.Point(308, 36);
            this.buttonStop.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(77, 25);
            this.buttonStop.TabIndex = 44;
            this.buttonStop.Text = "Стоп";
            this.toolTip1.SetToolTip(this.buttonStop, "Прекращает длительные процессы в программе и закрывает системный порт");
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // numericUpDownComReadTimeout
            // 
            this.numericUpDownComReadTimeout.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numericUpDownComReadTimeout.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownComReadTimeout.Location = new System.Drawing.Point(101, 10);
            this.numericUpDownComReadTimeout.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.numericUpDownComReadTimeout.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDownComReadTimeout.Minimum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.numericUpDownComReadTimeout.Name = "numericUpDownComReadTimeout";
            this.numericUpDownComReadTimeout.Size = new System.Drawing.Size(46, 16);
            this.numericUpDownComReadTimeout.TabIndex = 45;
            this.toolTip1.SetToolTip(this.numericUpDownComReadTimeout, "Время ожидания ответа одного счетчика");
            this.numericUpDownComReadTimeout.Value = new decimal(new int[] {
            800,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(151, 13);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 13);
            this.label1.TabIndex = 46;
            this.label1.Text = "мс";
            // 
            // checkBoxPollOffline
            // 
            this.checkBoxPollOffline.AutoSize = true;
            this.checkBoxPollOffline.Enabled = false;
            this.checkBoxPollOffline.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxPollOffline.Location = new System.Drawing.Point(191, 41);
            this.checkBoxPollOffline.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.checkBoxPollOffline.Name = "checkBoxPollOffline";
            this.checkBoxPollOffline.Size = new System.Drawing.Size(103, 17);
            this.checkBoxPollOffline.TabIndex = 47;
            this.checkBoxPollOffline.Text = "Не отвечающие";
            this.toolTip1.SetToolTip(this.checkBoxPollOffline, "Если установлен флаг, тест связи и опрос работают только для счетчиков, которые н" +
        "е ответили в прошлый раз");
            this.checkBoxPollOffline.UseVisualStyleBackColor = true;
            this.checkBoxPollOffline.CheckedChanged += new System.EventHandler(this.checkBoxPollOffline_CheckedChanged);
            // 
            // numericUpDownComWriteTimeout
            // 
            this.numericUpDownComWriteTimeout.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numericUpDownComWriteTimeout.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownComWriteTimeout.Location = new System.Drawing.Point(321, 34);
            this.numericUpDownComWriteTimeout.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.numericUpDownComWriteTimeout.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDownComWriteTimeout.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownComWriteTimeout.Name = "numericUpDownComWriteTimeout";
            this.numericUpDownComWriteTimeout.Size = new System.Drawing.Size(46, 16);
            this.numericUpDownComWriteTimeout.TabIndex = 56;
            this.toolTip1.SetToolTip(this.numericUpDownComWriteTimeout, "Время ожидания ответа одного счетчика");
            this.numericUpDownComWriteTimeout.Value = new decimal(new int[] {
            800,
            0,
            0,
            0});
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.numericUpDownComWriteTimeout);
            this.groupBox1.Controls.Add(this.checkBoxTcp);
            this.groupBox1.Controls.Add(this.textBoxPort);
            this.groupBox1.Controls.Add(this.textBoxIp);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.richTextBox1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Location = new System.Drawing.Point(5, 369);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(611, 57);
            this.groupBox1.TabIndex = 49;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Индивидуальный блок";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(318, 18);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 13);
            this.label4.TabIndex = 58;
            this.label4.Text = "Таймаут записи";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(371, 37);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 13);
            this.label3.TabIndex = 57;
            this.label3.Text = "мс";
            // 
            // checkBoxTcp
            // 
            this.checkBoxTcp.AutoSize = true;
            this.checkBoxTcp.Location = new System.Drawing.Point(273, 37);
            this.checkBoxTcp.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.checkBoxTcp.Name = "checkBoxTcp";
            this.checkBoxTcp.Size = new System.Drawing.Size(15, 14);
            this.checkBoxTcp.TabIndex = 55;
            this.checkBoxTcp.UseVisualStyleBackColor = true;
            this.checkBoxTcp.CheckedChanged += new System.EventHandler(this.checkBoxTcp_CheckedChanged);
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(213, 34);
            this.textBoxPort.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(43, 20);
            this.textBoxPort.TabIndex = 54;
            this.textBoxPort.Text = "4003";
            // 
            // textBoxIp
            // 
            this.textBoxIp.Location = new System.Drawing.Point(213, 14);
            this.textBoxIp.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBoxIp.Name = "textBoxIp";
            this.textBoxIp.Size = new System.Drawing.Size(75, 20);
            this.textBoxIp.TabIndex = 53;
            this.textBoxIp.Text = "192.168.4.1";
            // 
            // button4
            // 
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Location = new System.Drawing.Point(152, 14);
            this.button4.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(49, 38);
            this.button4.TabIndex = 52;
            this.button4.Text = "Опросить";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(414, 14);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBox1.Size = new System.Drawing.Size(187, 39);
            this.richTextBox1.TabIndex = 51;
            this.richTextBox1.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 18);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 50;
            this.label2.Text = "Серийный номер";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(8, 34);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(135, 20);
            this.textBox1.TabIndex = 49;
            this.textBox1.Text = "20133295";
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxLogo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxLogo.Image = global::elfextendedapp.Properties.Resources.pi_logo_2;
            this.pictureBoxLogo.InitialImage = null;
            this.pictureBoxLogo.Location = new System.Drawing.Point(556, 4);
            this.pictureBoxLogo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(61, 59);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxLogo.TabIndex = 50;
            this.pictureBoxLogo.TabStop = false;
            this.pictureBoxLogo.Click += new System.EventHandler(this.pictureBoxLogo_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(415, 19);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(132, 26);
            this.label5.TabIndex = 51;
            this.label5.Text = "ELF Apator";
            // 
            // cbModePreVals
            // 
            this.cbModePreVals.AutoSize = true;
            this.cbModePreVals.Location = new System.Drawing.Point(411, 44);
            this.cbModePreVals.Name = "cbModePreVals";
            this.cbModePreVals.Size = new System.Drawing.Size(140, 17);
            this.cbModePreVals.TabIndex = 52;
            this.cbModePreVals.Text = "Режим предустановки";
            this.cbModePreVals.UseVisualStyleBackColor = true;
            this.cbModePreVals.CheckedChanged += new System.EventHandler(this.cbModePreVals_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 450);
            this.Controls.Add(this.cbModePreVals);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pictureBoxLogo);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.checkBoxPollOffline);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDownComReadTimeout);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.comboBoxComPorts);
            this.Controls.Add(this.buttonExport);
            this.Controls.Add(this.buttonPoll);
            this.Controls.Add(this.buttonPing);
            this.Controls.Add(this.dgv1);
            this.Controls.Add(this.buttonImport);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Заголовок генерируется автоматически";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgv1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownComReadTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownComWriteTimeout)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonImport;
        private System.Windows.Forms.DataGridView dgv1;
        private System.Windows.Forms.OpenFileDialog ofd1;
        private System.Windows.Forms.Button buttonPing;
        private System.Windows.Forms.SaveFileDialog sfd1;
        private System.Windows.Forms.Button buttonPoll;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.ComboBox comboBoxComPorts;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.NumericUpDown numericUpDownComReadTimeout;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxPollOffline;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox checkBoxTcp;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.TextBox textBoxIp;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownComWriteTimeout;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox cbModePreVals;
    }
}

