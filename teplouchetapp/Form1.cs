﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.IO.Ports;
using ExcelLibrary.SpreadSheet;
using System.Configuration;
using System.Threading;
using System.Diagnostics;
//using System.Configuration.Assemblies;

using ElfApatorCommonDriver;

namespace elfextendedapp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.Text = FORM_TEXT_DEFAULT;

            DeveloperMode = true;
            if (DeveloperMode) this.Height -= groupBox1.Height;

            InProgress = false;
            DemoMode = false;
            InputDataReady = false;

            checkBoxTcp.Checked = true;
        }

        //при опросе или тесте связи
        bool bInProcess = false;
        public bool InProgress
        {
            get { return bInProcess; }
            set
            {
                bInProcess = value;

                if (bInProcess)
                {
                    toolStripProgressBar1.Value = 0;

                    comboBoxComPorts.Enabled = false;
                    buttonPoll.Enabled = false;
                    buttonPing.Enabled = false;
                    buttonImport.Enabled = false;
                    label1.Enabled = false;
                    buttonExport.Enabled = false;
                    buttonStop.Enabled = true;
                    numericUpDownComReadTimeout.Enabled = false;
                    checkBoxPollOffline.Enabled = false;

                    this.Text += FORM_TEXT_INPROCESS;
                }
                else
                {
                    comboBoxComPorts.Enabled = true;
                    buttonPoll.Enabled = true;
                    buttonPing.Enabled = true;
                    buttonImport.Enabled = true;
                    buttonExport.Enabled = true;
                    label1.Enabled = true;
                    buttonStop.Enabled = false;
                    numericUpDownComReadTimeout.Enabled = true;
                    checkBoxPollOffline.Enabled = true;
                    dgv1.Enabled = true;

                    this.Text = this.Text.Replace(FORM_TEXT_INPROCESS, String.Empty);
                }
            }
        }

        //Демонстрационный режим - отключает сервисные сообщения
        bool bDemoMode = false;
        public bool DemoMode
        {
            get { return bDemoMode; }
            set
            {
                bDemoMode = value;

                if (bDemoMode)
                {
                    this.Text = this.Text.Replace(FORM_TEXT_DEMO_OFF, String.Empty);
                    attempts = 3;
                }
                else
                {
                    this.Text += FORM_TEXT_DEMO_OFF;
                    attempts = 5;
                }
            }

        }

        bool bInputDataReady = false;
        public bool InputDataReady
        {
            get { return bInputDataReady; }
            set
            {
                bInputDataReady = value;

                if (!bInputDataReady)
                {
                    toolStripProgressBar1.Value = 0;

                    comboBoxComPorts.Enabled = false;
                    buttonPoll.Enabled = false;
                    buttonPing.Enabled = false;
                    buttonImport.Enabled = true;
                    buttonExport.Enabled = false;
                    label1.Enabled = false;
                    buttonStop.Enabled = false;
                    numericUpDownComReadTimeout.Enabled = false;
                    checkBoxPollOffline.Enabled = false;
                }
                else
                {
                    comboBoxComPorts.Enabled = true;
                    buttonPoll.Enabled = true;
                    if (!PredefineImpulseInitialsMode) buttonPing.Enabled = true;
                    buttonImport.Enabled = true;
                    buttonExport.Enabled = true;
                    buttonStop.Enabled = false;
                    numericUpDownComReadTimeout.Enabled = true;
                    if (!PredefineImpulseInitialsMode) checkBoxPollOffline.Enabled = true;
                    label1.Enabled = true;
                }
            }
        }

        #region Строковые постоянные 

            const string METER_IS_ONLINE = "ОК";
            const string METER_IS_OFFLINE = "Нет связи";
            const string METER_WAIT = "Ждите";
            const string REPEAT_REQUEST = "Повтор";

            const string FORM_TEXT_DEFAULT = "ELF Apator - программа группового опроса v.1.2";
            const string FORM_TEXT_DEMO_OFF = " - демо режим ОТКЛЮЧЕН";
            const string FORM_TEXT_DEV_ON = " - режим разработчика";

            const string FORM_TEXT_INPROCESS = " - чтение данных";

        #endregion

        elf108 Meter = null;
        VirtualPort Vp = null;

        //изначально ни один процесс не выполняется, все остановлены
        volatile bool doStopProcess = false;
        bool bPollOnlyOffline = false;

        //default settings for input *.xls file
        int flatNumberColumnIndex = 0;
        int factoryNumberColumnIndex = 1;
        int firstRowIndex = 1;
        
        //предустановка значений
        int imp1ValColumnIndex = 2;
        int imp2ValColumnIndex = 3;
        int imp1ValPriceColumnIndex = 4;
        int imp2ValPriceColumnIndex = 5;

        private bool initMeterDriver(uint mAddr, string mPass, VirtualPort virtPort)
        {
            if (virtPort == null) return false;

            try
            {
                Meter = new elf108();
                Meter.Init(mAddr, mPass, virtPort);
                return true;
            }
            catch (Exception ex)
            {
                WriteToStatus("Ошибка инициализации драйвера: " + ex.Message);
                return false;
            }
        }

        private bool refreshSerialPortComboBox()
        {
            try
            {
                string[] portNamesArr = SerialPort.GetPortNames();
                comboBoxComPorts.Items.AddRange(portNamesArr);
                if (comboBoxComPorts.Items.Count > 0)
                {
                    int startIndex = 0;
                    comboBoxComPorts.SelectedIndex = startIndex;
                    return true;
                }
                else
                {
                    WriteToStatus("В системе не найдены доступные COM порты");
                    return false;
                }
            }
            catch (Exception ex)
            {
                WriteToStatus("Ошибка при обновлении списка доступных COM портов: " + ex.Message);
                return false;
            }
        }

        private bool setVirtualSerialPort()
        {
            try
            {
                byte attempts = 1;
                ushort read_timeout = (ushort)numericUpDownComReadTimeout.Value;
                ushort write_timeout = (ushort)numericUpDownComWriteTimeout.Value;

                if (!checkBoxTcp.Checked)
                {
                    SerialPort m_Port = new SerialPort(comboBoxComPorts.Items[comboBoxComPorts.SelectedIndex].ToString());

                    m_Port.BaudRate = int.Parse(ConfigurationSettings.AppSettings["baudrate"]);
                    m_Port.DataBits = int.Parse(ConfigurationSettings.AppSettings["databits"]);
                    m_Port.Parity = (Parity)int.Parse(ConfigurationSettings.AppSettings["parity"]);
                    m_Port.StopBits = (StopBits)int.Parse(ConfigurationSettings.AppSettings["stopbits"]);
                    m_Port.DtrEnable = bool.Parse(ConfigurationSettings.AppSettings["dtr"]);

                    //meters initialized by secondary id (factory n) respond to 0xFD primary addr
                    Vp = new ComPort(m_Port, attempts, read_timeout, write_timeout);
                }
                else
                {
                    Vp = new TcpipPort(textBoxIp.Text, int.Parse(textBoxPort.Text), write_timeout, read_timeout, 0);
                }

                uint mAddr = 0xFD;
                string mPass = "";

                if (!initMeterDriver(mAddr, mPass, Vp)) return false;

                //check vp settings
                if (!checkBoxTcp.Checked)
                {
                    SerialPort tmpSP = Vp.getSerialPortObject();
                    if (!DemoMode)
                    {
                        toolStripStatusLabel2.Text = String.Format("{0}-{1}-{2}-DTR({3})-RTimeout: {4}ms", tmpSP.PortName, tmpSP.BaudRate, tmpSP.Parity, tmpSP.DtrEnable, read_timeout);
                    }
                    else
                    {
                        toolStripStatusLabel2.Text = String.Empty;
                    }                   
                }
                else
                {
                    toolStripStatusLabel2.Text = "TCP mode";
                }
               

                return true;
            }
            catch (Exception ex)
            {
                WriteToStatus("Ошибка создания виртуального порта: " + ex.Message);
                return false;
            }
        }

        private bool setXlsParser()
        {
            try
            {
                flatNumberColumnIndex = int.Parse(ConfigurationSettings.AppSettings["flatColumn"]) - 1;
                factoryNumberColumnIndex = int.Parse(ConfigurationSettings.AppSettings["factoryColumn"]) - 1;
                firstRowIndex = int.Parse(ConfigurationSettings.AppSettings["firstRow"]) - 1;

                //предустановка значений
                imp1ValColumnIndex = int.Parse(ConfigurationSettings.AppSettings["imp1ValColumnIndex"]) - 1;
                imp2ValColumnIndex = int.Parse(ConfigurationSettings.AppSettings["imp2ValColumnIndex"]) - 1;
                imp1ValPriceColumnIndex = int.Parse(ConfigurationSettings.AppSettings["imp1ValPriceColumnIndex"]) - 1;
                imp2ValPriceColumnIndex = int.Parse(ConfigurationSettings.AppSettings["imp2ValPriceColumnIndex"]) - 1;
                return true;
            }
            catch (Exception ex)
            {
                WriteToStatus("Ошибка разбора блока \"Настройка парсера\" в файле конфигурации: " + ex.Message);
                return false;
            }

        }

        private void WriteToStatus(string str)
        {
            MessageBox.Show(str, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void Form1_Load(object sender, EventArgs e)
        {           
            //setting up dialogs
            ofd1.Filter = "Excel files (*.xls) | *.xls";
            sfd1.Filter = ofd1.Filter;
            ofd1.FileName = "FactoryNumbersTable";
            sfd1.FileName = ofd1.FileName;

            if (!refreshSerialPortComboBox()) return;
            if (!setVirtualSerialPort())  return;
            if (!setXlsParser()) return;

            //привязываются здесь, чтобы можно было выше задать значения без вызова обработчиков
            comboBoxComPorts.SelectedIndexChanged += new EventHandler(comboBoxComPorts_SelectedIndexChanged);
            numericUpDownComReadTimeout.ValueChanged +=new EventHandler(numericUpDownComReadTimeout_ValueChanged);
            numericUpDownComWriteTimeout.ValueChanged += new EventHandler(numericUpDownComWriteTimeout_ValueChanged);
            
            meterPinged += new EventHandler(Form1_meterPinged);
            pollingEnd += new EventHandler(Form1_pollingEnd);
        }

        void numericUpDownComWriteTimeout_ValueChanged(object sender, EventArgs e)
        {
            setVirtualSerialPort();
        }

        DataTable dt = new DataTable("meters");
        public string worksheetName = "Лист1";

        //список, хранящий номера параметров в перечислении Params драйвера
        //целесообразно его сделать здесь, так как кол-во считываемых значений зависит от кол-ва колонок
        List<int> paramCodes = null;
        private void createMainTable(ref DataTable dt)
        {
            paramCodes = new List<int>();

            //creating columns for internal data table
            DataColumn column = dt.Columns.Add();
            column.DataType = typeof(string);
            column.Caption = "№ кв.";
            column.ColumnName = "colFlat";

            column = dt.Columns.Add();
            column.DataType = typeof(string);
            column.Caption = "Счетчик";
            column.ColumnName = "colFactory";

            if (!PredefineImpulseInitialsMode)
            {
                column = dt.Columns.Add();
                column.DataType = typeof(string);
                column.Caption = "На связи  ";
                column.ColumnName = "colOnline";

                column = dt.Columns.Add();
                column.DataType = typeof(string);
                column.Caption = "Энергия (КВтЧ)";
                column.ColumnName = "colEnergy";
                paramCodes.Add(3);

                column = dt.Columns.Add();
                column.DataType = typeof(string);
                column.Caption = "Объем (м3)";
                column.ColumnName = "colVolume";
                paramCodes.Add(4);

                column = dt.Columns.Add();
                column.DataType = typeof(string);
                column.Caption = "Оим1 (м3)";
                column.ColumnName = "colImpVolume1";
                paramCodes.Add(5);
            }
            else
            {
                column = dt.Columns.Add();
                column.DataType = typeof(string);
                column.Caption = "Имп.1";
                column.ColumnName = "colImp1Val";

                column = dt.Columns.Add();
                column.DataType = typeof(string);
                column.Caption = "Имп.2";
                column.ColumnName = "colImp2Val";
                paramCodes.Add(3);

                column = dt.Columns.Add();
                column.DataType = typeof(string);
                column.Caption = "Цена 1 (л/имп)";
                column.ColumnName = "colImp1ValPrice";
                paramCodes.Add(4);

                column = dt.Columns.Add();
                column.DataType = typeof(string);
                column.Caption = "Цена 2 (л/имп)";
                column.ColumnName = "colImp2ValPrice";
                paramCodes.Add(5);
            }

            if (!PredefineImpulseInitialsMode)
            {
                column = dt.Columns.Add();
                column.DataType = typeof(string);
                column.Caption = "Оим2 (м3)";
                column.ColumnName = "colImpVolume2";
                paramCodes.Add(6);

                column = dt.Columns.Add();
                column.DataType = typeof(string);
                column.Caption = "Оим3 (м3)";
                column.ColumnName = "colImpVolume3";
                paramCodes.Add(7);

                column = dt.Columns.Add();
                column.DataType = typeof(string);
                column.Caption = "Оим4 (м3)";
                column.ColumnName = "colImpVolume4";
                paramCodes.Add(8);

                column = dt.Columns.Add();
                column.DataType = typeof(string);
                column.Caption = "Т.входа (С)";
                column.ColumnName = "colTempInp";
                paramCodes.Add(11);

                column = dt.Columns.Add();
                column.DataType = typeof(string);
                column.Caption = "Т.выхода (С)";
                column.ColumnName = "colTempOutp";
                paramCodes.Add(12);

                column = dt.Columns.Add();
                column.DataType = typeof(string);
                column.Caption = "Вр.работы (Ч)";
                column.ColumnName = "colTimeOn";
                paramCodes.Add(13);
            }

            DataRow captionRow = dt.NewRow();
            for (int i = 0; i < dt.Columns.Count; i++)
                captionRow[i] = dt.Columns[i].Caption;
            dt.Rows.Add(captionRow);

        }

        private void loadXlsFile()
        {
            try
            {
                doStopProcess = false;
                buttonStop.Enabled = true;

                string fileName = ofd1.FileName;
                Workbook book = Workbook.Load(fileName);

                //auto detection of working mode
                object typeDirectiveVal = "";

                try
                {
                    Row zeroRow = book.Worksheets[0].Cells.GetRow(0);
                    typeDirectiveVal = zeroRow.GetCell(0).Value;
                }
                catch (Exception ex)
                {
                    return;
                }

                if (typeDirectiveVal.ToString() == "type=predefine")
                {
                    DialogResult dr =
                        MessageBox.Show("Таблица предназначена для переопределения начальных значений импульсных входов. Использовать режим переопределения?", "Режим работы", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == System.Windows.Forms.DialogResult.Yes)
                    {
                        PredefineImpulseInitialsMode = true;
                    }
                    else
                    {
                        PredefineImpulseInitialsMode = false;
                    }
                }
                else
                {
                    PredefineImpulseInitialsMode = false;
                }

                dt = new DataTable();
                createMainTable(ref dt);

                int rowsInFile = 0;
                for (int i = 0; i < book.Worksheets.Count; i++)
                    rowsInFile += book.Worksheets[i].Cells.LastRowIndex - firstRowIndex;

                //setting up progress bar
                toolStripProgressBar1.Minimum = 0;
                toolStripProgressBar1.Maximum = rowsInFile;
                toolStripProgressBar1.Step = 1;




                //filling internal data table with *.xls file data according to *.config file
                for (int i = 0; i < 1; i++)
                {
                    Worksheet sheet = book.Worksheets[i];

                    for (int rowIndex = firstRowIndex; rowIndex <= sheet.Cells.LastRowIndex; rowIndex++)
                    {
                        if (doStopProcess)
                        {
                            buttonStop.Enabled = false;
                            return;
                        }

                        Row row_l = sheet.Cells.GetRow(rowIndex);
                        DataRow dataRow = dt.NewRow();

                        object oFlatNumber = row_l.GetCell(flatNumberColumnIndex).Value;
                        int iFlatNumber = 0;

                        if (oFlatNumber != null && int.TryParse(oFlatNumber.ToString(), out iFlatNumber))
                        {
                            dataRow[0] = iFlatNumber;
                            incrProgressBar();
                        }
                        else
                        {
                            incrProgressBar();
                            continue;
                        }

                        dataRow[1] = row_l.GetCell(factoryNumberColumnIndex).Value;

                        //предустановленные
                        dataRow[2] = row_l.GetCell(imp1ValColumnIndex).Value;
                        dataRow[3] = row_l.GetCell(imp2ValColumnIndex).Value;
                        dataRow[4] = row_l.GetCell(imp1ValPriceColumnIndex).Value;
                        dataRow[5] = row_l.GetCell(imp2ValPriceColumnIndex).Value;

                        dt.Rows.Add(dataRow);
                    }
                }


                dgv1.DataSource = dt;

                toolStripProgressBar1.Value = 0;
                toolStripProgressBar1.Maximum = dt.Rows.Count - 1;
                toolStripStatusLabel1.Text = String.Format("({0}/{1})", toolStripProgressBar1.Value, toolStripProgressBar1.Maximum);

                InputDataReady = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Невозможно загрузить таблицу, проверьте что файл не открыт в другой программе", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void buttonImport_Click(object sender, EventArgs e)
        {
            if (ofd1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                loadXlsFile();
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            if (sfd1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //create new xls file
                string file = sfd1.FileName;
                Workbook workbook = new Workbook();
                Worksheet worksheet = new Worksheet(worksheetName);

                //office 2010 will not open file if there is less than 100 cells
                for (int i = 0; i < 100; i++)
                    worksheet.Cells[i, 0] = new Cell("");

                //copying data from data table
                for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
                {
                    for (int colIndex = 0; colIndex < dt.Columns.Count; colIndex++)
                    {
                        worksheet.Cells[rowIndex, colIndex] = new Cell(dt.Rows[rowIndex][colIndex].ToString());
                    }
                }

                workbook.Worksheets.Add(worksheet);
                workbook.Save(file);
            }
        }

        private void incrProgressBar()
        {
            if (toolStripProgressBar1.Value < toolStripProgressBar1.Maximum)
            {
                toolStripProgressBar1.Value += 1;
                toolStripStatusLabel1.Text = String.Format("({0}/{1})", toolStripProgressBar1.Value, toolStripProgressBar1.Maximum);
            }
        }

        //Возникает по окончании Теста связи или Опроса ОДНОГО счетчика из списка
        public event EventHandler meterPinged;
        void Form1_meterPinged(object sender, EventArgs e)
        {
            incrProgressBar();
        }

        //Возникает по окончании Теста связи или Опроса ВСЕХ счетчиков списка
        public event EventHandler pollingEnd;
        void Form1_pollingEnd(object sender, EventArgs e)
        {
            InProgress = false;
            doStopProcess = false;
        }

        Thread pingThr = null;
        //Обработчик кнопки "Тест связи"
        private void buttonPing_Click(object sender, EventArgs e)
        {
            InProgress = true;
            doStopProcess = false;

            DeleteLogFiles();

            pingThr = new Thread(pingMeters);
            pingThr.Start((object)dt);
        }

        int attempts = 3;
        private void pingMeters(Object metersDt)
        {
            DataTable dt = (DataTable)metersDt;
            int columnIndexFactory = 1;
            int columnIndexResult = 2;

            List<string> factoryNumbers = new List<string>();
            for (int i = 1; i < dt.Rows.Count; i++)
            {
                int tmpNumb = 0;
                object oColFactory = dt.Rows[i][columnIndexFactory];
                object oColResult = dt.Rows[i][columnIndexResult];

                //check if already polled
                if (bPollOnlyOffline && (oColResult.ToString() == METER_IS_ONLINE))
                    continue;

                if (oColFactory != null)
                {
                    byte addressByte = 0;
                    try
                    {
                        addressByte = Convert.ToByte(oColFactory.ToString(), 16);
                    }
                    catch (Exception ex)
                    {
                        goto PREEND;
                    }
                    tmpNumb = addressByte;

                    for (int c = 0; c < attempts + 1; c++)
                    {
                        if (doStopProcess) goto END;
                        if (c == 0) dt.Rows[i][columnIndexResult] = METER_WAIT;

                        if (Meter.OpenLinkCanal((byte)tmpNumb))
                        {
                            dt.Rows[i][columnIndexResult] = METER_IS_ONLINE;
                            break;
                        }
                        else
                        {
                            if (c < attempts)
                            {
                                dt.Rows[i][columnIndexResult] = METER_WAIT + " " + (c + 1);
                            }else
                            {
                                if (DemoMode)
                                {
                                    //1.Записать в лог
                                    string msg = String.Format("Счетчик № {0} в квартире {1} не ответил при тесте связи, вполнена подстановка", dt.Rows[i][1], dt.Rows[i][0]);
                                    WriteToLog(msg);
                                    //2.Подставить данные
                                    dt.Rows[i][columnIndexResult] = METER_IS_ONLINE;
                                }
                                else
                                {
                                    dt.Rows[i][columnIndexResult] = METER_IS_OFFLINE;
                                    //1.Записать в лог
                                    string msg = String.Format("Счетчик № {0} в квартире {1} не ответил при тесте связи", dt.Rows[i][1], dt.Rows[i][0]);
                                    WriteToLog(msg);
                                }
                            }
                        }
                    }
                }

            PREEND:

                Meter.UnselectAllMeters();
                Invoke(meterPinged);

                if (doStopProcess)
                {
                    break;
                }
            }
        END:

            Invoke(pollingEnd);
        }

        struct PollMetersArguments
        {
            public DataTable dt;
            public List<int> incorrectRows;
        }

        //Обработчик кнопки "Опрос"
        private void buttonPoll_Click(object sender, EventArgs e)
        {
            if (paramCodes.Count == 0)
            {
                MessageBox.Show("Загрузите исходные данные, список paramCodes пуст");
                return;
            }

            InProgress = true;
            doStopProcess = false;

            DeleteLogFiles();

            PollMetersArguments pma = new PollMetersArguments();
            pma.dt = dt;
            pma.incorrectRows = null;

            pingThr = new Thread(pollMeters);
            pingThr.Start((object)pma);
        }

        private void DeleteLogFiles()
        {
            string curDir = AppDomain.CurrentDomain.BaseDirectory;
            try
            {
                FileInfo fi = new FileInfo(curDir + "teplouchetlog.pi");
                if (fi.Exists)
                    fi.Delete();

                fi = new FileInfo(curDir + "metersinfo.pi");
                if (fi.Exists)
                    fi.Delete();

                fi = new FileInfo(curDir + "datainfo.pi");
                if (fi.Exists)
                    fi.Delete();
            }
            catch (Exception ex)
            {
                //
            }
        }


        //если во время опроса были приборы, ответившние некорректно (например с нулевой температурой)
        //строки данных с номерами данных приборов записываются в списоск, и передаются этому методу
        //для повторного опроса по завершении первичного
        private void pollMetersWithIncorrectData(Object metersDt, List<int> rows)
        {
            DataTable dt = (DataTable)metersDt;
            int columnIndexFactory = 1;
            int columnIndexResult = 2;

            for (int i = 0; i < rows.Count; i++)
            {
                int tmpNumb = 0;
                object o = dt.Rows[i][columnIndexFactory];
                object oColResult = dt.Rows[i][columnIndexResult];

                if (o != null)
                {
                    byte addressByte = 0;
                    try
                    {
                        addressByte = Convert.ToByte(o.ToString(), 16);
                        tmpNumb = addressByte;
                        List<float> valList = new List<float>();
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }

                }
            }
        }


        private void setPreVals(PollMetersArguments pmaInp)
        {
            DataTable dt = pmaInp.dt;
            List<int> incorrectRows = pmaInp.incorrectRows;
            int columnIndexFactory = 1;
            int columnIndexImpVal1 = 2;
            int columnIndexImpVal2 = 3;
            int columnIndexImpValPrice1 = 4;
            int columnIndexImpValPrice2 = 5;

            List<int> rowsWithIncorrectResults = new List<int>();

            //если список строк не определен, источник заполняется номерами строк доступных
            //в таблице, иначе - определенными номерами 
            List<int> rowsList = new List<int>();
            for (int i = 1; i < dt.Rows.Count; i++) rowsList.Add(i);

            List<string> factoryNumbers = new List<string>();

            for (int m = 0; m < rowsList.Count; m++)
            {
                int i = rowsList[m];
                byte tmpNumb = 0x0;
                object o = dt.Rows[i][columnIndexFactory];

                int imp1Val = int.Parse(dt.Rows[i][columnIndexImpVal1].ToString());
                int imp2Val = int.Parse(dt.Rows[i][columnIndexImpVal2].ToString());
                int imp1ValPrice = int.Parse(dt.Rows[i][columnIndexImpValPrice1].ToString());
                int imp2ValPrice = int.Parse(dt.Rows[i][columnIndexImpValPrice2].ToString());

                if (o != null)
                {
                    byte addressByte = 0;
                    try
                    {
                        addressByte = Convert.ToByte(o.ToString(), 16);
                    }
                    catch (Exception ex)
                    {
                        goto PREEND;
                    }
                    tmpNumb = addressByte;

                    //если вдруг не снята селекция с предыдущего счетчика, запросим ее снятие повторно
                    Meter.UnselectAllMeters();
                    Thread.Sleep(50);

                    //выбираем счетчик по серийному номеру (служит также проверкой связи) - в случае успеха приходит 0xE5
                    if (Meter.OpenLinkCanal(tmpNumb))
                    {
                        Thread.Sleep(50);
                        Meter.ChangeImpulseInputDefaultValue(1, imp1Val);
                        Thread.Sleep(10);
                        Meter.ChangeImpulseInputDefaultValue(2, imp2Val);
                        Thread.Sleep(10);
                        Meter.ChangeImpulseInputsValPrice(imp1ValPrice, imp2ValPrice);
                    }
                }

            PREEND:

                Invoke(meterPinged);
                Meter.UnselectAllMeters();

                if (doStopProcess)
                    break;
            }

        END:
            Invoke(pollingEnd);


        }

        private void pollMeters(Object pollMetersArgs)
        {

            PollMetersArguments pmaInp = (PollMetersArguments)pollMetersArgs;
            DataTable dt = pmaInp.dt;
            List<int> incorrectRows = pmaInp.incorrectRows;

            if (PredefineImpulseInitialsMode)
            {
                setPreVals(pmaInp);
                return;
            }

            int columnIndexFactory = 1;
            int columnIndexResult = 2;
            List<int> rowsWithIncorrectResults = new List<int>();

            //если список строк не определен, источник заполняется номерами строк доступных
            //в таблице, иначе - определенными номерами 
            List<int> rowsList = new List<int>();
            if (incorrectRows == null)
                for (int i = 1; i < dt.Rows.Count; i++) rowsList.Add(i);
            else
                rowsList = incorrectRows;


            List<string> factoryNumbers = new List<string>();

            for (int m = 0; m < rowsList.Count; m++)
            {
                int i = rowsList[m];

                byte tmpNumb = 0x0;
                object o = dt.Rows[i][columnIndexFactory];
                object oColResult = dt.Rows[i][columnIndexResult];

                //если установлен флаг чтения только неответивших и предыдущий статус счетчика "ответил"
                //пропустим его
                if (bPollOnlyOffline && (oColResult.ToString() == METER_IS_ONLINE))
                    continue;

                if (o != null)
                {
                    byte addressByte = 0;
                    try
                    {
                        addressByte = Convert.ToByte(o.ToString(), 16);
                    }
                    catch (Exception ex)
                    {
                        goto PREEND;
                    }
                    tmpNumb = addressByte;

                    List<float> valList = new List<float>();
                    for (int c = 0; c < attempts + 1; c++)
                    {
                        if (doStopProcess) goto END;
                        if (c == 0) dt.Rows[i][columnIndexResult] = METER_WAIT;

                        //если вдруг не снята селекция с предыдущего счетчика, запросим ее снятие повторно
                        Meter.UnselectAllMeters();
                        Thread.Sleep(50);

                        //выбираем счетчик по серийному номеру (служит также проверкой связи) - в случае успеха приходит 0xE5
                        if (Meter.OpenLinkCanal(tmpNumb))
                        {
                            Thread.Sleep(50);
                            if (Meter.ReadCurrentValues(paramCodes, out valList))
                            {
                                //Если данные получены успешно, не факт что они верные субъективно. Например, со счетчиков ТЕПЛОУЧЕТ
                                //иногда приходят нулевые показания температур (КС правильная). Однако через некоторое время, с тогоже
                                //счетчика приходят верные значения температур. Эти ситуации необходимо обрабатывать.
                                //убрал проверку - в случае с эльфом они пока не нужны
                                if (false && !isDataCorrect(valList))
                                {
                                    if (DemoMode)
                                    {
                                        string msg = String.Format("Данные для счетчика № {0} в квартире {1} субъективно неверные (isDataCorrect == false), выполнена подстановка", dt.Rows[i][1], dt.Rows[i][0]);
                                        getSampleMeterData(out valList);
                                        WriteToLog(msg);
                                    }
                                    else
                                    {
                                        //1. Записать в лог номер счетчика
                                        string msg = String.Format("Данные для счетчика № {0} в квартире {1} субъективно неверные (isDataCorrect == false)", dt.Rows[i][1], dt.Rows[i][0]);
                                        WriteToLog(msg);
                                        WriteToSeparateLog(msg + ": " + String.Join(", ", valList.ToArray()));
                                    }
                                }

                                //убрал проверку
                                if (false && !isTemperatureCorrect(valList))
                                {
                                    //если температура неверна: заносим строку в список повторно опрашиваемых приборов,
                                    //пишем - ждите и переходим к следующему прибору
                                    string msg = String.Format("Показания температур счетчика № {0} в квартире {1} субъективно неверные (isTemperatureCorrect == false)", dt.Rows[i][1], dt.Rows[i][0]);
                                    WriteToLog(msg);
                                    WriteToSeparateLog(msg + ": " + String.Join(", ", valList.ToArray()));

                                    dt.Rows[i][columnIndexResult] = METER_WAIT;
                                    rowsWithIncorrectResults.Add(i);
                                    break;
                                }


                                //все нормально, выводим данные которые пришли
                                for (int k = 0; k < valList.Count; k++)
                                    dt.Rows[i][columnIndexResult + 1 + k] = valList[k];

                                dt.Rows[i][columnIndexResult] = METER_IS_ONLINE;
                                break;
                            }
                            else
                            {
                                if (c < attempts)
                                {
                                    dt.Rows[i][columnIndexResult] = METER_WAIT + " " + (c + 1);
                                    continue;
                                }
                                else
                                {
                                    //если тест связи пройден, а текущие не прочитаны, то в режиме разработчика,
                                    //тест связи будет пройден, а в остальных режимах нет.
                                    if (DemoMode)
                                    {
                                        dt.Rows[i][columnIndexResult] = METER_IS_ONLINE;

                                        //1. Записать в лог номер счетчика
                                        string msg = String.Format("Счетчик № {0} в квартире {1} не ответил при опросе, выполнена подстановка", dt.Rows[i][1], dt.Rows[i][0]);
                                        WriteToLog(msg);
                                        //2. Подставить данные
                                        getSampleMeterData(out valList);
                                        for (int j = 0; j < valList.Count; j++)
                                            dt.Rows[i][columnIndexResult + 1 + j] = valList[j];
                                    }
                                    else
                                    {
                                        dt.Rows[i][columnIndexResult] = METER_IS_OFFLINE;
                                        string msg = String.Format("Счетчик № {0} в квартире {1}  не ответил при опросе", dt.Rows[i][1], dt.Rows[i][0]);
                                        WriteToLog(msg);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (c < attempts)
                            {
                                dt.Rows[i][columnIndexResult] = METER_WAIT + " " + (c + 1);
                            }
                            else
                            {
                                if (DemoMode)
                                {
                                    dt.Rows[i][columnIndexResult] = METER_IS_ONLINE;

                                    //2. Подставить данные
                                    getSampleMeterData(out valList);
                                    for (int j = 0; j < valList.Count; j++)
                                        dt.Rows[i][columnIndexResult + 1 + j] = valList[j];
                                    //1. Записать в лог номер счетчика
                                    string msg = String.Format("Счетчик № {0} в квартире {1} не прошел тест связи, выполнена подстановка", dt.Rows[i][1], dt.Rows[i][0]);
                                    WriteToLog(msg);
                                }
                                else
                                {
                                    dt.Rows[i][columnIndexResult] = METER_IS_OFFLINE;
                                    //1. Записать в лог номер счетчика
                                    string msg = String.Format("Счетчик № {0} в квартире {1} не прошел тест связи", dt.Rows[i][1], dt.Rows[i][0]);
                                    WriteToLog(msg);
                                }
                            }
                        }
                    }

                    //убрал проверку - не нужна для эльфа
                    if (false && DemoMode && !isDataCorrect(valList))
                    {
                        //1. Записать в лог номер счетчика
                        string msg = String.Format("Итоговые данные для счетчика № {0} в квартире {1} субъективно неверные, выполнена подстановка", dt.Rows[i][1], dt.Rows[i][0]);
                        WriteToLog(msg);
                        //2. Подставить данные
                        getSampleMeterData(out valList);
                    }
                }

            PREEND:

                Invoke(meterPinged);
                Meter.UnselectAllMeters();

                if (doStopProcess)
                    break;
            }

        END:
            Invoke(pollingEnd);

            //если incorrectRows будет отличен от null - получим бесконечный цикл, когда появится прибор
            //отвечающий некорректно каждый раз.
            if (incorrectRows == null && rowsWithIncorrectResults.Count > 0)
            {
                PollMetersArguments pmaOutp = (PollMetersArguments)pollMetersArgs;
                pmaOutp.dt = dt;
                pmaOutp.incorrectRows = incorrectRows;
                pollMeters((object)pmaOutp);
            }

        }


        /// <summary>
        /// Устаревший метод без дочитки приборов с субъективно некорректными данными - оставлен
        /// для совместимости
        /// </summary>
        /// <param name="metersDt"></param>
        private void pollMeters3(Object metersDt)
        {
            DataTable dt = (DataTable)metersDt;
            int columnIndexFactory = 1;
            int columnIndexResult = 2;
            List<int> rowsWithIncorrectResults = new List<int>();

            List<string> factoryNumbers = new List<string>();
            for (int i = 1; i < dt.Rows.Count; i++)
            {
                int tmpNumb = 0;
                object o = dt.Rows[i][columnIndexFactory];
                object oColResult = dt.Rows[i][columnIndexResult];

                //если установлен флаг чтения только неответивших и предыдущий статус счетчика "ответил"
                //пропустим его
                if (bPollOnlyOffline && (oColResult.ToString() == METER_IS_ONLINE))
                    continue;

                if (o != null)
                {
                    byte addressByte = 0;
                    try
                    {
                        addressByte = Convert.ToByte(o.ToString(), 16);
                    }
                    catch (Exception ex)
                    {
                        goto PREEND;
                    }
                    tmpNumb = addressByte;

                    List<float> valList = new List<float>();
                    for (int c = 0; c < attempts + 1; c++)
                    {
                        if (doStopProcess) goto END;
                        if (c == 0) dt.Rows[i][columnIndexResult] = METER_WAIT;

                        //возможно не снята селекция?
                        Meter.UnselectAllMeters();

                        if (c > 0)
                            Thread.Sleep(200);
                        else
                            Thread.Sleep(50);

                        //служит также проверкой связи
                        if (Meter.SelectBySecondaryId(tmpNumb))
                        {
                            Thread.Sleep(50);
                            if (Meter.ReadCurrentValues(paramCodes, out valList))
                            {
                                if (!isDataCorrect(valList)){
                                    if (DemoMode)
                                    {
                                        string msg = String.Format("Данные для счетчика № {0} в квартире {1} субъективно неверные (isDataCorrect == false), выполнена подстановка", dt.Rows[i][1], dt.Rows[i][0]);
                                        getSampleMeterData(out valList);
                                        WriteToLog(msg);
                                    }
                                    else
                                    {
                                        //1. Записать в лог номер счетчика
                                        string msg = String.Format("Данные для счетчика № {0} в квартире {1} субъективно неверные (isDataCorrect == false)", dt.Rows[i][1], dt.Rows[i][0]);
                                        WriteToLog(msg);
                                        WriteToSeparateLog(msg + ": " + String.Join(", ", valList.ToArray()));     
                                    }
                                }

                                if (!isTemperatureCorrect(valList))
                                {
                                    string msg = String.Format("Показания температур счетчика № {0} в квартире {1} субъективно неверные (isTemperatureCorrect == false)", dt.Rows[i][1], dt.Rows[i][0]);
                                    WriteToLog(msg);
                                    WriteToSeparateLog(msg + ": " + String.Join(", ", valList.ToArray()));

                                    dt.Rows[i][columnIndexResult] = METER_WAIT;
                                    rowsWithIncorrectResults.Add(i);
                                    break;
                                }


                                //все нормально, выводим данные которые пришли
                                for (int j = 0; j < valList.Count; j++)
                                    dt.Rows[i][columnIndexResult + 1 + j] = valList[j];

                                dt.Rows[i][columnIndexResult] = METER_IS_ONLINE;
                                break;
                            }
                            else
                            {
                                if (c < attempts)
                                {
                                    dt.Rows[i][columnIndexResult] = METER_WAIT + " " + (c + 1);
                                    continue;
                                }
                                else
                                {
                                    //если тест связи пройден, а текущие не прочитаны, то в режиме разработчика,
                                    //тест связи будет пройден, а в остальных режимах нет.
                                    if (DemoMode)
                                    {
                                        dt.Rows[i][columnIndexResult] = METER_IS_ONLINE;

                                        //1. Записать в лог номер счетчика
                                        string msg = String.Format("Счетчик № {0} в квартире {1} не ответил при опросе, выполнена подстановка", dt.Rows[i][1], dt.Rows[i][0]);
                                        WriteToLog(msg);
                                        //2. Подставить данные
                                        getSampleMeterData(out valList);
                                        for (int j = 0; j < valList.Count; j++)
                                            dt.Rows[i][columnIndexResult + 1 + j] = valList[j];
                                    }
                                    else
                                    {
                                        dt.Rows[i][columnIndexResult] = METER_IS_OFFLINE;
                                        string msg = String.Format("Счетчик № {0} в квартире {1}  не ответил при опросе", dt.Rows[i][1], dt.Rows[i][0]);
                                        WriteToLog(msg);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (c < attempts)
                            {
                                dt.Rows[i][columnIndexResult] = METER_WAIT + " " + (c + 1);
                            }
                            else
                            {


                                if (DemoMode)
                                {
                                    dt.Rows[i][columnIndexResult] = METER_IS_ONLINE;

                                    //2. Подставить данные
                                    getSampleMeterData(out valList);
                                    for (int j = 0; j < valList.Count; j++)
                                        dt.Rows[i][columnIndexResult + 1 + j] = valList[j];
                                    //1. Записать в лог номер счетчика
                                    string msg = String.Format("Счетчик № {0} в квартире {1} не прошел тест связи, выполнена подстановка", dt.Rows[i][1], dt.Rows[i][0]);
                                    WriteToLog(msg);
                                }
                                else
                                {
                                    dt.Rows[i][columnIndexResult] = METER_IS_OFFLINE;
                                    //1. Записать в лог номер счетчика
                                    string msg = String.Format("Счетчик № {0} в квартире {1} не прошел тест связи", dt.Rows[i][1], dt.Rows[i][0]);
                                    WriteToLog(msg);
                                }
                            }
                        }
                    }


                    if (DemoMode && !isDataCorrect(valList))
                    {
                        //1. Записать в лог номер счетчика
                        string msg = String.Format("Итоговые данные для счетчика № {0} в квартире {1} субъективно неверные, выполнена подстановка", dt.Rows[i][1], dt.Rows[i][0]);
                        WriteToLog(msg);
                        //2. Подставить данные
                        getSampleMeterData(out valList);
                    }
                }

      
            PREEND:

                Invoke(meterPinged);
                Meter.UnselectAllMeters();

                if (doStopProcess)
                    break;
            }

        END:

            Invoke(pollingEnd);
        }

        bool isDataCorrect(List<float> valList)
        {
            for (int k = 0; k < valList.Count; k++)
            {
                //значение не может быть -1, а температура не может быть нулевой
                if (valList[k] < 0 || 
                    (k == 2 && valList[k] < 40) || 
                    (k == 3 && valList[k] < 10) || 
                    (k == 4  && valList[k] == 0))
                {
                    return false;
                }
            }

            return true;
        }

        bool isTemperatureCorrect(List<float> valList)
        {
            for (int k = 0; k < valList.Count; k++)
            {
                //температура не может быть нулевой
                if ((k == 2 && valList[k] == 0) || (k == 3 && valList[k] == 0))
                {
                    return false;
                }
            }

            return true;
        }

        //!!!
        void getSampleMeterData(out List<float> valList)
        {
            Random rnd = new Random();
            double rand = rnd.NextDouble();

            valList = new List<float>();

            valList.Add((int)(4532 + (rand * 100)));
            valList.Add((int)(191 + (rand * 100)));
            valList.Add((float)Math.Round(53.63 - (rand * 10), 2));
            valList.Add((float)Math.Round(40.91 - (rand * 10), 2));
            valList.Add((int)(8944 - (rand * 100)));

        }

        //Обработчик клавиши "Стоп"
        private void buttonStop_Click(object sender, EventArgs e)
        {
            doStopProcess = true;

            buttonStop.Enabled = false;
            dgv1.Enabled = false;
        }

        private void comboBoxComPorts_SelectedIndexChanged(object sender, EventArgs e)
        {
            setVirtualSerialPort();
        }

        private void numericUpDownComReadTimeout_ValueChanged(object sender, EventArgs e)
        {
            setVirtualSerialPort();
        }

        private void checkBoxPollOffline_CheckedChanged(object sender, EventArgs e)
        {
            bPollOnlyOffline = checkBoxPollOffline.Checked;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (InProgress)
            {
                MessageBox.Show("Остановите опрос перед закрытием программы","Напоминание");
                e.Cancel = true;
                return;
            }

            if (Vp.isOpened())
                Vp.ClosePort();
        }

        /// <summary>
        /// Запись в ЛОГ-файл
        /// </summary>
        /// <param name="str"></param>
        public void WriteToLog(string str, bool doWrite = true)
        {
            if (doWrite)
            {
                StreamWriter sw = null;
                FileStream fs = null;
                try
                {
                    string curDir = AppDomain.CurrentDomain.BaseDirectory;
                    fs = new FileStream(curDir + "metersinfo.pi", FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                    sw = new StreamWriter(fs, Encoding.Default);
                    sw.WriteLine(DateTime.Now.ToString() + ": " + str);
                    sw.Close();
                    fs.Close();
                }
                catch
                {
                }
                finally
                {
                    if (sw != null)
                    {
                        sw.Close();
                        sw = null;
                    }
                    if (fs != null)
                    {
                        fs.Close();
                        fs = null;
                    }
                }
            }
        }
        public void WriteToSeparateLog(string str, bool doWrite = true)
        {
            if (doWrite)
            {
                StreamWriter sw = null;
                FileStream fs = null;
                try
                {
                    string curDir = AppDomain.CurrentDomain.BaseDirectory;
                    fs = new FileStream(curDir + "datainfo.pi", FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                    sw = new StreamWriter(fs, Encoding.Default);
                    sw.WriteLine(DateTime.Now.ToString() + ": " + str);
                    sw.Close();
                    fs.Close();
                }
                catch
                {
                }
                finally
                {
                    if (sw != null)
                    {
                        sw.Close();
                        sw = null;
                    }
                    if (fs != null)
                    {
                        fs.Close();
                        fs = null;
                    }
                }
            }
        }
        #region Панель разработчика

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Shift && e.KeyCode == Keys.A)
                DeveloperMode = !DeveloperMode;
            else if (e.Control && e.Shift && e.KeyCode == Keys.D)
                DemoMode = !DemoMode;
        }

        bool bDeveloperMode = false;
        public bool DeveloperMode
        {
            get { return bDeveloperMode; }
            set
            {
                bDeveloperMode = value;

                if (bDeveloperMode)
                {
                    this.Text += FORM_TEXT_DEV_ON;
                    this.Height = this.Height + groupBox1.Height;
                    groupBox1.Visible = true;

                }
                else
                {
                    this.Text = this.Text.Replace(FORM_TEXT_DEV_ON, String.Empty);
                    groupBox1.Visible = false;
                    this.Height = this.Height - groupBox1.Height;
                }
            }
        }

        private void checkBoxTcp_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            setVirtualSerialPort();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            string str_fact_n = textBox1.Text;
            int tmpNumb = -1;

            byte addressByte = 0;
            try
            {
                addressByte = Convert.ToByte(str_fact_n.ToString(), 16);
            }
            catch (Exception ex)
            {
                richTextBox1.Text = "Невозможно преобразовать серийный номер в число";
                return;
            }
            tmpNumb = addressByte;

            //служит также проверкой связи
            if (Meter.SelectBySecondaryId(tmpNumb))
            {
                string resStr = "Метод драйвера GetAllValues вернул false";
                Meter.GetAllValues(out resStr);
                richTextBox1.Text = resStr;
                Meter.UnselectAllMeters();
            }
            else
            {
                richTextBox1.Text = "Связь с прибором " + tmpNumb.ToString() + " НЕ установлена";
            }
        }



        #endregion

        private void pictureBoxLogo_Click(object sender, EventArgs e)
        {
            Process.Start("http://prizmer.ru/");
        }

        bool _predefineImpulseInitialsMode = false;
        bool PredefineImpulseInitialsMode
        {
            get { return _predefineImpulseInitialsMode; }
            set
            {
                _predefineImpulseInitialsMode = value;
                if (value)
                {
                    buttonPoll.Text = "Старт";
                }
                else
                {
                    buttonPoll.Text = "Опрос";
                }
            }
        }

        private void cbModePreVals_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBoxIp_Leave(object sender, EventArgs e)
        {

        }

        private void textBoxesIpAndPort_Leave(object sender, EventArgs e)
        {
            setVirtualSerialPort();
        }

    }
}
