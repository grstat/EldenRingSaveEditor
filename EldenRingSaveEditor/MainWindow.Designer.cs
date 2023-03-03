namespace EldenRingSaveEditor
{
  partial class MainWindow
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if(disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.FileSelectionTxt = new System.Windows.Forms.TextBox();
      this.FileSelectBtn = new System.Windows.Forms.Button();
      this.FileSelectDialog = new System.Windows.Forms.OpenFileDialog();
      this.BeginDataParse = new System.ComponentModel.BackgroundWorker();
      this.StatusBar = new System.Windows.Forms.StatusStrip();
      this.StatusBarProgress = new System.Windows.Forms.ToolStripProgressBar();
      this.StatusBarText = new System.Windows.Forms.ToolStripStatusLabel();
      this.SteamIdLabel = new System.Windows.Forms.Label();
      this.SteamIdText = new System.Windows.Forms.TextBox();
      this.SaveBtn = new System.Windows.Forms.Button();
      this.ChangeSteamIdBtn = new System.Windows.Forms.Button();
      this.DataGrid = new System.Windows.Forms.DataGridView();
      this.dgv_SlotNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.dgv_CharacterName = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.dgv_Level = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.dgv_Vig = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.dgv_Mnd = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.dgv_End = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.dgv_Str = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.dgv_Dex = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.dgv_Int = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.dgv_Fth = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.dgv_Arc = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.StatusBar.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.DataGrid)).BeginInit();
      this.SuspendLayout();
      // 
      // FileSelectionTxt
      // 
      this.FileSelectionTxt.BackColor = System.Drawing.SystemColors.MenuBar;
      this.FileSelectionTxt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.FileSelectionTxt.Location = new System.Drawing.Point(88, 12);
      this.FileSelectionTxt.MinimumSize = new System.Drawing.Size(336, 20);
      this.FileSelectionTxt.Name = "FileSelectionTxt";
      this.FileSelectionTxt.ReadOnly = true;
      this.FileSelectionTxt.Size = new System.Drawing.Size(336, 20);
      this.FileSelectionTxt.TabIndex = 0;
      this.FileSelectionTxt.TabStop = false;
      this.FileSelectionTxt.Text = "No file selected";
      // 
      // FileSelectBtn
      // 
      this.FileSelectBtn.Location = new System.Drawing.Point(7, 11);
      this.FileSelectBtn.Name = "FileSelectBtn";
      this.FileSelectBtn.Size = new System.Drawing.Size(75, 23);
      this.FileSelectBtn.TabIndex = 0;
      this.FileSelectBtn.Text = "Browse";
      this.FileSelectBtn.UseVisualStyleBackColor = true;
      this.FileSelectBtn.Click += new System.EventHandler(this.FileSelectBtn_Click);
      // 
      // FileSelectDialog
      // 
      this.FileSelectDialog.Filter = "Elden Ring Save|*.sl2;*.co2|Elden Ring|*.sl2|Seemless Coop|*.co2";
      this.FileSelectDialog.InitialDirectory = "C:\\Users\\Vertigo\\AppData\\Roaming\\EldenRing";
      this.FileSelectDialog.Title = "Select your Elden Ring Save";
      this.FileSelectDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.FileSelectDialog_FileOk);
      // 
      // BeginDataParse
      // 
      this.BeginDataParse.WorkerReportsProgress = true;
      this.BeginDataParse.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BeginDataParse_DoWork);
      this.BeginDataParse.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BeginDataParse_ProgressChanged);
      this.BeginDataParse.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BeginDataParse_RunWorkerCompleted);
      // 
      // StatusBar
      // 
      this.StatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusBarProgress,
            this.StatusBarText});
      this.StatusBar.Location = new System.Drawing.Point(0, 382);
      this.StatusBar.Name = "StatusBar";
      this.StatusBar.Size = new System.Drawing.Size(701, 22);
      this.StatusBar.TabIndex = 2;
      // 
      // StatusBarProgress
      // 
      this.StatusBarProgress.Name = "StatusBarProgress";
      this.StatusBarProgress.Size = new System.Drawing.Size(100, 16);
      this.StatusBarProgress.Visible = false;
      // 
      // StatusBarText
      // 
      this.StatusBarText.BackColor = System.Drawing.SystemColors.MenuBar;
      this.StatusBarText.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
      this.StatusBarText.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.StatusBarText.Name = "StatusBarText";
      this.StatusBarText.RightToLeft = System.Windows.Forms.RightToLeft.No;
      this.StatusBarText.Size = new System.Drawing.Size(113, 17);
      this.StatusBarText.Text = "Select a file to begin";
      // 
      // SteamIdLabel
      // 
      this.SteamIdLabel.AutoSize = true;
      this.SteamIdLabel.Location = new System.Drawing.Point(25, 42);
      this.SteamIdLabel.Name = "SteamIdLabel";
      this.SteamIdLabel.Size = new System.Drawing.Size(57, 13);
      this.SteamIdLabel.TabIndex = 4;
      this.SteamIdLabel.Text = "Steam ID: ";
      this.SteamIdLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // SteamIdText
      // 
      this.SteamIdText.Location = new System.Drawing.Point(88, 38);
      this.SteamIdText.Name = "SteamIdText";
      this.SteamIdText.ReadOnly = true;
      this.SteamIdText.Size = new System.Drawing.Size(255, 20);
      this.SteamIdText.TabIndex = 5;
      this.SteamIdText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SteamIdText_Keypressed);
      // 
      // SaveBtn
      // 
      this.SaveBtn.Location = new System.Drawing.Point(614, 353);
      this.SaveBtn.Name = "SaveBtn";
      this.SaveBtn.Size = new System.Drawing.Size(75, 23);
      this.SaveBtn.TabIndex = 3;
      this.SaveBtn.Text = "Save";
      this.SaveBtn.UseVisualStyleBackColor = true;
      this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
      // 
      // ChangeSteamIdBtn
      // 
      this.ChangeSteamIdBtn.Location = new System.Drawing.Point(349, 37);
      this.ChangeSteamIdBtn.Name = "ChangeSteamIdBtn";
      this.ChangeSteamIdBtn.Size = new System.Drawing.Size(75, 23);
      this.ChangeSteamIdBtn.TabIndex = 1;
      this.ChangeSteamIdBtn.Text = "Change";
      this.ChangeSteamIdBtn.UseVisualStyleBackColor = true;
      this.ChangeSteamIdBtn.Click += new System.EventHandler(this.ChangeSteamIdBtn_Click);
      // 
      // DataGrid
      // 
      this.DataGrid.AllowUserToAddRows = false;
      this.DataGrid.AllowUserToDeleteRows = false;
      this.DataGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
      this.DataGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
      this.DataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.DataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgv_SlotNumber,
            this.dgv_CharacterName,
            this.dgv_Level,
            this.dgv_Vig,
            this.dgv_Mnd,
            this.dgv_End,
            this.dgv_Str,
            this.dgv_Dex,
            this.dgv_Int,
            this.dgv_Fth,
            this.dgv_Arc});
      this.DataGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
      this.DataGrid.Location = new System.Drawing.Point(12, 64);
      this.DataGrid.MultiSelect = false;
      this.DataGrid.Name = "DataGrid";
      this.DataGrid.Size = new System.Drawing.Size(677, 283);
      this.DataGrid.TabIndex = 6;
      this.DataGrid.TabStop = false;
      this.DataGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGrid_CellContentChanged);
      // 
      // dgv_SlotNumber
      // 
      this.dgv_SlotNumber.HeaderText = "Slot #";
      this.dgv_SlotNumber.MaxInputLength = 3;
      this.dgv_SlotNumber.MinimumWidth = 45;
      this.dgv_SlotNumber.Name = "dgv_SlotNumber";
      this.dgv_SlotNumber.ReadOnly = true;
      this.dgv_SlotNumber.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      this.dgv_SlotNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      this.dgv_SlotNumber.ToolTipText = "Character slot number";
      this.dgv_SlotNumber.Width = 45;
      // 
      // dgv_CharacterName
      // 
      this.dgv_CharacterName.HeaderText = "Character Name";
      this.dgv_CharacterName.MaxInputLength = 16;
      this.dgv_CharacterName.MinimumWidth = 152;
      this.dgv_CharacterName.Name = "dgv_CharacterName";
      this.dgv_CharacterName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      this.dgv_CharacterName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      this.dgv_CharacterName.Width = 152;
      // 
      // dgv_Level
      // 
      this.dgv_Level.HeaderText = "Level";
      this.dgv_Level.MaxInputLength = 3;
      this.dgv_Level.MinimumWidth = 52;
      this.dgv_Level.Name = "dgv_Level";
      this.dgv_Level.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      this.dgv_Level.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      this.dgv_Level.ToolTipText = "Character Level";
      this.dgv_Level.Width = 52;
      // 
      // dgv_Vig
      // 
      this.dgv_Vig.HeaderText = "Vig";
      this.dgv_Vig.MaxInputLength = 3;
      this.dgv_Vig.MinimumWidth = 45;
      this.dgv_Vig.Name = "dgv_Vig";
      this.dgv_Vig.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      this.dgv_Vig.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      this.dgv_Vig.ToolTipText = "Vigor";
      this.dgv_Vig.Width = 45;
      // 
      // dgv_Mnd
      // 
      this.dgv_Mnd.HeaderText = "Mnd";
      this.dgv_Mnd.MaxInputLength = 3;
      this.dgv_Mnd.MinimumWidth = 45;
      this.dgv_Mnd.Name = "dgv_Mnd";
      this.dgv_Mnd.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      this.dgv_Mnd.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      this.dgv_Mnd.ToolTipText = "Mind";
      this.dgv_Mnd.Width = 45;
      // 
      // dgv_End
      // 
      this.dgv_End.HeaderText = "End";
      this.dgv_End.MaxInputLength = 3;
      this.dgv_End.MinimumWidth = 45;
      this.dgv_End.Name = "dgv_End";
      this.dgv_End.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      this.dgv_End.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      this.dgv_End.ToolTipText = "Endurance";
      this.dgv_End.Width = 45;
      // 
      // dgv_Str
      // 
      this.dgv_Str.HeaderText = "Str";
      this.dgv_Str.MaxInputLength = 3;
      this.dgv_Str.MinimumWidth = 50;
      this.dgv_Str.Name = "dgv_Str";
      this.dgv_Str.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      this.dgv_Str.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      this.dgv_Str.ToolTipText = "Strength";
      this.dgv_Str.Width = 50;
      // 
      // dgv_Dex
      // 
      this.dgv_Dex.HeaderText = "Dex";
      this.dgv_Dex.MaxInputLength = 3;
      this.dgv_Dex.MinimumWidth = 50;
      this.dgv_Dex.Name = "dgv_Dex";
      this.dgv_Dex.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      this.dgv_Dex.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      this.dgv_Dex.ToolTipText = "Dexterity";
      this.dgv_Dex.Width = 50;
      // 
      // dgv_Int
      // 
      this.dgv_Int.HeaderText = "Int";
      this.dgv_Int.MaxInputLength = 3;
      this.dgv_Int.MinimumWidth = 50;
      this.dgv_Int.Name = "dgv_Int";
      this.dgv_Int.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      this.dgv_Int.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      this.dgv_Int.ToolTipText = "Intelligence";
      this.dgv_Int.Width = 50;
      // 
      // dgv_Fth
      // 
      this.dgv_Fth.HeaderText = "Fth";
      this.dgv_Fth.MaxInputLength = 3;
      this.dgv_Fth.MinimumWidth = 50;
      this.dgv_Fth.Name = "dgv_Fth";
      this.dgv_Fth.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      this.dgv_Fth.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      this.dgv_Fth.ToolTipText = "Faith";
      this.dgv_Fth.Width = 50;
      // 
      // dgv_Arc
      // 
      this.dgv_Arc.HeaderText = "Arc";
      this.dgv_Arc.MaxInputLength = 3;
      this.dgv_Arc.MinimumWidth = 50;
      this.dgv_Arc.Name = "dgv_Arc";
      this.dgv_Arc.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      this.dgv_Arc.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      this.dgv_Arc.ToolTipText = "Arcane";
      this.dgv_Arc.Width = 50;
      // 
      // MainWindow
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(701, 404);
      this.Controls.Add(this.DataGrid);
      this.Controls.Add(this.ChangeSteamIdBtn);
      this.Controls.Add(this.SaveBtn);
      this.Controls.Add(this.SteamIdText);
      this.Controls.Add(this.SteamIdLabel);
      this.Controls.Add(this.StatusBar);
      this.Controls.Add(this.FileSelectBtn);
      this.Controls.Add(this.FileSelectionTxt);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Name = "MainWindow";
      this.Text = "EldenRingSaveEditor";
      this.StatusBar.ResumeLayout(false);
      this.StatusBar.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.DataGrid)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox FileSelectionTxt;
    private System.Windows.Forms.Button FileSelectBtn;
    private System.Windows.Forms.OpenFileDialog FileSelectDialog;
    private System.ComponentModel.BackgroundWorker BeginDataParse;
    private System.Windows.Forms.StatusStrip StatusBar;
    private System.Windows.Forms.ToolStripProgressBar StatusBarProgress;
    private System.Windows.Forms.ToolStripStatusLabel StatusBarText;
    private System.Windows.Forms.Label SteamIdLabel;
    private System.Windows.Forms.TextBox SteamIdText;
    private System.Windows.Forms.Button SaveBtn;
    private System.Windows.Forms.Button ChangeSteamIdBtn;
    private System.Windows.Forms.DataGridView DataGrid;
    private System.Windows.Forms.DataGridViewTextBoxColumn dgv_SlotNumber;
    private System.Windows.Forms.DataGridViewTextBoxColumn dgv_CharacterName;
    private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Level;
    private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Vig;
    private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Mnd;
    private System.Windows.Forms.DataGridViewTextBoxColumn dgv_End;
    private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Str;
    private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Dex;
    private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Int;
    private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Fth;
    private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Arc;
  }
}

