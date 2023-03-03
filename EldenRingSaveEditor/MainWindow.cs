using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Collections.Generic;

/* ----------------------------------------------------
 * EldenRingSaveEditor
 * ----------------------------------------------------
 * Used to edit save files in the game Elden Ring
 * This software should allow editing different aspects
 * of the save files for different reasons. 
 * ----------------------------------------------------
 * - You can alter the SteamID to allow sharing saves
 * - Change the character name
 * - More to come
 * ----------------------------------------------------
 * Author: Adrian Borchardt
 * 
 * ----------------------------------------------------
 * SOME OF THE CONSTANTS AND CONCEPTS
 * IN THIS PROGRAM WERE BORROWED OR ADAPTED FROM THE
 * EldenRingSaveCopy PROJECT by Benjamin Green
 * see https://github.com/BenGrn/EldenRingSaveCopier
 * ---------------------------------------------------- */

namespace EldenRingSaveEditor
{
  public partial class MainWindow : Form
  {


    // CHARACTER LIST INDEXES
    private const int SLOT_IDX = 0;
    private const int NAME_IDX = 1;
    private const int LEVEL_IDX = 2;
    private const int VIGOR_IDX = 3;
    private const int MIND_IDX = 4;
    private const int ENDUR_IDX = 5;
    private const int STR_IDX = 6;
    private const int DEXT_IDX = 7;
    private const int INTEL_IDX = 8;
    private const int FAITH_IDX = 9;
    private const int ARCH_IDX = 10;

    private const byte CHAR_ACTIVE = 0x01;
    private const byte SLOT_CHANGED = 0x01;

    private const int MAX_SAVE_SLOTS = 10; // MAX NUMBER OF SAVE SLOTS
    private const int STEAM_ID_LENGTH = 8; // LENGTH OF THE STEAM ID

    private const int SLOT_LENGTH = 2621440;      // LENGTH OF EACH CHARACTER SLOT
    private const int CHAR_HEADER_LENGTH = 588;   // HEADER LENGTH FOR EACH CHAR SLOT
    private const int CHAR_NAME_LENGTH = 34;      // MAX CHAR NAME LENGTH
    private const int HEADER_LENGTH = 393216;     // LENGTH OF THE ENTIRE HEADER
    private const int CHECKSUM_LENGTH = 16;       // CHECKSUM LENGTH

    private const int SLOT_START_INDEX = 0x310;   // FIRST SAVE SLOT START INDEX

    private const int HEADER_START_INDEX = 0x19003B0;       // START OF THE ENTIRE HEADER
    private const int CHAR_HEADER_START_INDEX = 0x1901D0E;  // FIRST INDEX OF THE FIRST CHAR SLOT

    private const int CHAR_LEVEL_LOCATION = 0x22;   // INDEX TO CHARACTER LEVEL

    //private const int CHAR_PLAYED_START_INDEX = 0x26;

    private const int CHAR_ACTIVE_START_IDX = 0x1901D04;  // CHARACTER ENABLED START INDEX
    private const int CHAR_ACTIVE_STOP_IDX = 0x1901D0D;   // CHARACTER ENABLED STOP ENDEX

    private const int STEAM_ID_INDEX = 0x19003B4;         // INDEX TO THE KNOWN STEAM ID
    private const int SLOT_CHECKSUM_START = 0x300;        // DATA CHECKSUM START POINT
    private const int HEADER_CHECKSUM_START = 0x19003A0;  // HEADER CHECKSUM START POINT

    private bool resetRequired;       // USED TO MANAGE DATA FILE CHANGES
    private bool unsavedChanges;      // UNSAVED CHANGES
    private bool steamIdChanged;      // STEAM ID CHANGED
    private bool reloadChanges;       // RELOADING CHAGES TO THE FILE
    private byte[] saveFileData;      // FILE DATA STORAGE

    /* DATA STORAGE FOR PARSED DATA FROM THE SAVE FILE
 * THE SAVED DATA SLOT NUMBER IS EXPRESSED BY THE ARRAY POSITION 0 - MAX_SLOTS
 */
    private byte[][] CharacterData = new byte[MAX_SAVE_SLOTS][];
    private byte[][] CharacterHeader = new byte[MAX_SAVE_SLOTS][];
    private byte[] SteamIdPattern = new byte[STEAM_ID_LENGTH]; // THE BYTE ARRAY OF THE STEAM ID
    private int[] SteamIdPositions = new int[MAX_SAVE_SLOTS];       // THE INDEX POSITIONS OF EACH STEAM ID ENTRY
    private int[] CharacterLevel = new int[MAX_SAVE_SLOTS];
    private int[] CharacterStatsIndex = new int[MAX_SAVE_SLOTS];
    // STATS
    private short[] CharacterVigor = new short[MAX_SAVE_SLOTS];
    private short[] CharacterMind = new short[MAX_SAVE_SLOTS];
    private short[] CharacterEndurance = new short[MAX_SAVE_SLOTS];
    private short[] CharacterStrength = new short[MAX_SAVE_SLOTS];
    private short[] CharacterDexterity = new short[MAX_SAVE_SLOTS];
    private short[] CharacterIntelligence = new short[MAX_SAVE_SLOTS];
    private short[] CharacterFaith = new short[MAX_SAVE_SLOTS];
    private short[] CharacterArcane = new short[MAX_SAVE_SLOTS];

    private long SteamID = new long();
    //public bool[] CharacterActive = new bool[MAX_SLOTS];   // 0 INACTIVE - 1 ACTIVE
    private byte[] CharacterActive = new byte[MAX_SAVE_SLOTS];   // 0 INACTIVE - 1 ACTIVE
    private byte[] SlotChanged = new byte[MAX_SAVE_SLOTS];   // 0 NOCHANGE - 1 CHANGED
    private string[] CharacterName = new string[MAX_SAVE_SLOTS];

    private List<string>[] characters = new List<string>[MAX_SAVE_SLOTS];

    //SaveData _saveData;               // DATA STORAGE

    public MainWindow()
    {
      InitializeComponent();  // SEE MainWindow.Designer
      /* LOCAL INITS */
      saveFileData = new byte[0];
      StatusBarText.Text = "Select a save file to begin";
      resetRequired = false;
      unsavedChanges = false;
      //_saveData = new SaveData();
      SaveBtn.Enabled = false;
      SteamIdText.Text = "";
      ChangeSteamIdBtn.Enabled = false;
      
    }

    public void ResetData()
    { // CALLED WHEN A FILE IS CHANGED
      saveFileData = new byte[0];  // CLEAR OUT THE BUFFER
      FileSelectionTxt.Text = "No file selected";  // IN CASE THE DIALOG IS CANCLED THIS WILL SHOW
      FileSelectDialog.FileName = "";  // IN CASE THE DIALOG IS CANCLED THE OLD FILE WON'T BE STUCK
      //_saveData = new SaveData(); // CLEAR OLD SAVE DATA
      StatusBarText.Text = "Select a save file to begin";  // THE INFO WINDOW
      DataGrid.Rows.Clear();
      StatusBarProgress.Value = 0;
      SaveBtn.Enabled = false;
      SteamIdText.Text = "";
      ChangeSteamIdBtn.Enabled = false;
    }

    /* EVENT HANDLER FILE SELECTION BUTTON PRESSED */
    private void FileSelectBtn_Click(object sender, EventArgs e)
    {
      if(resetRequired) { // A FILE WAS PREVIOUSLY PARSED SO RESET UNLESS DATA WAS SAVED
        if(unsavedChanges || steamIdChanged) {
          /* MAKE SURE THE USER KNOWS ANYTHING UNSAVED WILL BE LOST */
          DialogResult dr = MessageBox.Show("Changing the file will result in unsaved changes.\r\nAre you sure you want to change files?", "Warning", MessageBoxButtons.YesNo);
          if(dr == DialogResult.No) { return; } // ANSWER WAS NO, DON'T RESET
          unsavedChanges = false; // RESET THE FLAG
          steamIdChanged = false;
        }
        ResetData();
        resetRequired = false;
      }
      FileSelectDialog.ShowDialog();
    }

    private void DisableControls()
    { // DISABLE CONTROLS DURING PROCESSING
      FileSelectBtn.Enabled = false;
      SaveBtn.Enabled = false;
      ChangeSteamIdBtn.Enabled = false;
    }

    private void EnableControls()
    { // ENABLE CONTROLS WHEN PROCESSING COMPLETES
      FileSelectBtn.Enabled = true;
      if(unsavedChanges || steamIdChanged) { SaveBtn.Enabled = true; }
    }

    private void ReloadChanges()
    {
      reloadChanges = true;
      DisableControls();  // DISABLE CONTROLS BEFORE PROCESSING
      DataGrid.Rows.Clear();
      SteamIdText.Text = "";
      StatusBarProgress.Control.Show();
      BeginDataParse.RunWorkerAsync();
    }

    /* EVENT HANDLER FOR THE FILE SELECTION DIALOG ONLY TRIGGERS ON OPEN NOT CANCEL */
    private void FileSelectDialog_FileOk(object sender, CancelEventArgs e)
    { // FILE SELECTION VALIDATED, FILE EXISTS AND IS OK
      FileSelectionTxt.Text = FileSelectDialog.FileName;  // COPY THE FILENAME INTO THE SELECTION TEXT BOX WHICH IS READ ONLY
      DisableControls();  // DISABLE CONTROLS BEFORE PROCESSING
      StatusBarProgress.Control.Show();
      BeginDataParse.RunWorkerAsync();
    }

    private void UpdateChecksums()
    {
      MD5 md5 = MD5.Create();
      byte[] headerChecksum = new byte[CHECKSUM_LENGTH];
      byte[] headerData = new byte[HEADER_LENGTH];
      Array.Copy(saveFileData, HEADER_START_INDEX, headerData, 0, HEADER_LENGTH);   // CURRENT HEADER
      Array.Copy(saveFileData, HEADER_CHECKSUM_START, headerChecksum, 0, CHECKSUM_LENGTH);  // CURRENT HEADER CHECKSUM
      md5.ComputeHash(headerData);  // COMPUTE CURRENT HEADER
      if(!md5.Hash.SequenceEqual(headerChecksum)) {
        // MISMATCH, UPDATE CHECKSUM
        Console.WriteLine("Header checksum mismatch updating");
        Array.Copy(md5.Hash, 0, saveFileData, HEADER_CHECKSUM_START, CHECKSUM_LENGTH);
      }
      /* CHECK THE CHAR SLOT CHECKSUMS AND UPDATE */
      for(int idx = 0; idx < MAX_SAVE_SLOTS; idx++) {
        md5.ComputeHash(CharacterData[idx]);    // COMPUTE THE CHAR SLOT CHECKSUM
        byte[] charChecksum = new byte[CHECKSUM_LENGTH];  // STORE CURRENT CHECKSUM
        int charChecksumIdx = (SLOT_CHECKSUM_START + (idx * CHECKSUM_LENGTH) + (idx * SLOT_LENGTH));  // CURRENT CHECKSUM INDEX
        Array.Copy(saveFileData, charChecksumIdx, charChecksum, 0, CHECKSUM_LENGTH);  // GRAB THE CURRENT CHECKSUM
        if(!md5.Hash.SequenceEqual(charChecksum)) {  // TEST AGAINST THE CURRENT CHECKSUM
          // MATCH FAILED UPDATE CHECKSUM
          Array.Copy(md5.Hash, 0, saveFileData, charChecksumIdx, CHECKSUM_LENGTH);  // OVERWRITE WITH NEW CHECKSUM
        }
      }

    }

    private void BeginDataParse_DoWork(object sender, DoWorkEventArgs e)
    {
      BackgroundWorker worker = sender as BackgroundWorker;
      int idx = 0;
      worker.ReportProgress(0);
      if(!reloadChanges) {
        if(File.Exists(FileSelectionTxt.Text)) {  // EXTRA GUARD, YOU NEVER KNOW
          FileInfo fi = new FileInfo(FileSelectionTxt.Text);  // GRAB THE FILE INFO
          FileSelectDialog.InitialDirectory = fi.DirectoryName;
          saveFileData = new byte[fi.Length];
          try {
            saveFileData = File.ReadAllBytes(FileSelectionTxt.Text);  // READ IN ALL THE DATA TO THE BYTE ARRAY
            resetRequired = true;  // RESET WILL BE REQUIRED AFTER THIS
          } catch(IOException ex) {
            Console.Write("ERROR: Failed to read the file data!\r\nPlease try another file.\r\nExtended Message: " + ex.GetType().Name + "\r\n");
            resetRequired = true; // RESET
            unsavedChanges = false; // NO UNSAVED CHANGES
            e.Cancel = true;
            return;
          }
          /* ONE LAST CHECK TO MAKE SURE EVERYTHING WAS READ */
          if(saveFileData.Length != fi.Length) {
            resetRequired = true; // RESET
            unsavedChanges = false; // NO UNSAVED CHANGES
            e.Cancel = true;
            return;
          }
        }
      }

      try {
        Array.Copy(saveFileData, STEAM_ID_INDEX, SteamIdPattern, 0, STEAM_ID_LENGTH);
        SteamID = BitConverter.ToInt64(SteamIdPattern, 0); // GET THE STEAM ID
        //SteamIdPattern = saveFileData.Skip(STEAM_ID_INDEX).Take(STEAM_ID_LENGTH).ToArray();
          
        int pos = 0;
        for(idx = 0; idx < (saveFileData.Length - STEAM_ID_LENGTH); idx++) {  // RUN 8 LESS TO LENGTH TO PREVENT OUT OF RANGE
          if(saveFileData[idx] == SteamIdPattern[0]
            && saveFileData[idx + 1] == SteamIdPattern[1]
            && saveFileData[idx + 2] == SteamIdPattern[2]) {
            //FIRST 3 BYTES MATCH
            if(BitConverter.ToInt64(saveFileData, idx) == SteamID) {  // CONVERT TO LONG TO TEST
              Console.WriteLine("Found Steam ID at Position: " + idx);
              SteamIdPositions[pos++] = idx;  // ADD IN THE POSITION
            }
          }
        }

        Array.Copy(saveFileData, CHAR_ACTIVE_START_IDX, CharacterActive, 0, MAX_SAVE_SLOTS);  // CHAR ACTIVE STATUS

        for(idx = 0; idx < MAX_SAVE_SLOTS; idx++) {
          int charLevelIdx = ((CHAR_HEADER_START_INDEX + (idx * CHAR_HEADER_LENGTH)) + CHAR_LEVEL_LOCATION);
          int charDataIdx = (SLOT_START_INDEX + (idx * CHECKSUM_LENGTH) + (idx * SLOT_LENGTH));
          int charHeaderIdx = (CHAR_HEADER_START_INDEX + (idx * CHAR_HEADER_LENGTH));
          CharacterName[idx] = Encoding.Unicode.GetString(saveFileData, charHeaderIdx, CHAR_NAME_LENGTH);
          CharacterLevel[idx] = BitConverter.ToInt32(saveFileData, charLevelIdx);
          CharacterData[idx] = new byte[SLOT_LENGTH];
          CharacterHeader[idx] = new byte[CHAR_HEADER_LENGTH];
          Array.Copy(saveFileData, charDataIdx, CharacterData[idx], 0, SLOT_LENGTH);
          Array.Copy(saveFileData, charHeaderIdx, CharacterHeader[idx], 0, CHAR_HEADER_LENGTH);
          if(CharacterActive[idx] == CHAR_ACTIVE) {
            /* CHARACTER IS ACTIVE */
            for(int x = 0; x < CharacterData[idx].Length - 47; x++) {
              /* LOOP THROUGH CHARACTER DATA LOOKING FOR STATS */
              byte[] vig = { CharacterData[idx][x], CharacterData[idx][x + 1] };
              byte[] mind = { CharacterData[idx][x + 4], CharacterData[idx][x + 5] };
              byte[] endur = { CharacterData[idx][x + 8], CharacterData[idx][x + 9] };
              byte[] str = { CharacterData[idx][x + 12], CharacterData[idx][x + 13] };
              byte[] dex = { CharacterData[idx][x + 16], CharacterData[idx][x + 17] };
              byte[] intel = { CharacterData[idx][x + 20], CharacterData[idx][x + 21] };
              byte[] faith = { CharacterData[idx][x + 24], CharacterData[idx][x + 25] };
              byte[] arc = { CharacterData[idx][x + 28], CharacterData[idx][x + 29] };
              int statsIdx = BitConverter.ToInt16(vig, 0) + BitConverter.ToInt16(mind, 0);
              statsIdx += BitConverter.ToInt16(endur, 0) + BitConverter.ToInt16(str, 0);
              statsIdx += BitConverter.ToInt16(dex, 0) + BitConverter.ToInt16(intel, 0);
              statsIdx += BitConverter.ToInt16(faith, 0) + BitConverter.ToInt16(arc, 0);
              byte[] lvl_chk = { CharacterData[idx][x + 44], CharacterData[idx][x + 45], CharacterData[idx][x + 46], 0 };
              if(statsIdx == (CharacterLevel[idx] + 79) && BitConverter.ToInt32(lvl_chk, 0) == CharacterLevel[idx]) {
                CharacterStatsIndex[idx] = x;  // MARK THE STAT INDEX START
                CharacterVigor[idx] = BitConverter.ToInt16(vig, 0);
                CharacterMind[idx] = BitConverter.ToInt16(mind, 0);
                CharacterEndurance[idx] = BitConverter.ToInt16(endur, 0);
                CharacterStrength[idx] = BitConverter.ToInt16(str, 0);
                CharacterDexterity[idx] = BitConverter.ToInt16(dex, 0);
                CharacterIntelligence[idx] = BitConverter.ToInt16(intel, 0);
                CharacterFaith[idx] = BitConverter.ToInt16(faith, 0);
                CharacterArcane[idx] = BitConverter.ToInt16(arc, 0);
                break;
              }
            }
          }
          worker.ReportProgress((idx + 1) * MAX_SAVE_SLOTS);
        }
          
      } catch(Exception ex) {
        Console.WriteLine("ERROR: " + ex.GetType().Name);
        resetRequired = true; // RESET
        unsavedChanges = false; // NO UNSAVED CHANGES
        e.Cancel = true;
        return;
      }
      UpdateChecksums();
      e.Result = true;
    } /** END - private void BeginDataParse_DoWork(object sender, DoWorkEventArgs e) **/

    private void BeginDataParse_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      StatusBarProgress.Value = e.ProgressPercentage;
      StatusBarText.Text = "Processing " + (e.ProgressPercentage.ToString() + "%");
    }

    private void BeginDataParse_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if(reloadChanges) { reloadChanges = false; }
      StatusBarText.Text = "Done Processing, Updating List";
      if(!e.Cancelled) {
        if(SteamID > 0) {
          ChangeSteamIdBtn.Enabled = true;    // ALLOW CHANGES TO STEAM ID
          SteamIdText.Text = SteamID.ToString();  // FILL IN THE TEXT BOX
        }
        StatusBarProgress.Value = 0;
        
        for(int i = 0; i < MAX_SAVE_SLOTS; i++) {
          if(CharacterActive[i] == CHAR_ACTIVE) {  // LOOK FOR ACTIVE CHARACTERS
            StatusBarProgress.Value = (i * 10);
            List<string> charListItems = new List<string>();
            charListItems.Add((i + 1).ToString());  // SLOT NUMBER
            charListItems.Add(CharacterName[i]);
            charListItems.Add(CharacterLevel[i].ToString());
            charListItems.Add(CharacterVigor[i].ToString());
            charListItems.Add(CharacterMind[i].ToString());
            charListItems.Add(CharacterEndurance[i].ToString());
            charListItems.Add(CharacterStrength[i].ToString());
            charListItems.Add(CharacterDexterity[i].ToString());
            charListItems.Add(CharacterIntelligence[i].ToString());
            charListItems.Add(CharacterFaith[i].ToString());
            charListItems.Add(CharacterArcane[i].ToString());
            characters[i] = new List<string>();
            characters[i].AddRange(charListItems);
            DataGrid.Rows.Add(characters[i].ToArray());
          }
        }
      }
      EnableControls();
      StatusBarProgress.Value = 0;
      StatusBarProgress.Control.Hide();
      StatusBarText.Text = "Done";
    }

    private void ChangeSteamId()
    {
      if(SteamIdText.ReadOnly) {
        // SET THE STEAM ID INPUT BOX TO READ/WRITE
        SteamIdText.ReadOnly = false;
        ChangeSteamIdBtn.Text = "Done";
        return;
      }

      if(SteamIdText.Text == "") {
        MessageBox.Show("SteamID can't be blank!");
        SteamIdText.Text = SteamID.ToString();  // RETURN IT TO IT'S ORIGINAL ID
        return;
      }

      foreach(char num in SteamIdText.Text) {
        // CHECK TO MAKE SURE THE STEAM ID IS DIGITS
        if(num > '9' || num < '0') {
          MessageBox.Show("Steam ID Invalid, Digits only!");
          SteamIdText.Text = SteamID.ToString();  // RETURN IT TO IT'S ORIGINAL ID
          return;
        }
      }
      
      long newSteamID = long.Parse(SteamIdText.Text);   // PARSE THE STEAM ID
      if(newSteamID == SteamID) { // TEST FOR CHANGES
        SteamIdText.ReadOnly = true;
        ChangeSteamIdBtn.Text = "Change";
        if(steamIdChanged) {  // DO RESETS IF USER CHANGES ID BACK
          StatusBarText.Text = "";
          steamIdChanged = false;
          if(!unsavedChanges) { SaveBtn.Enabled = false; }
        }
        return;
      }

      byte[] newSteamBytes = BitConverter.GetBytes(newSteamID); // GET THE STEAM ID TO THE BYTE ARRAY
      Array.Copy(newSteamBytes, 0, saveFileData, STEAM_ID_INDEX, STEAM_ID_LENGTH);
      for(int sidx = 0; sidx < MAX_SAVE_SLOTS; sidx++) {
        if(SteamIdPositions[sidx] != 0) { // IF THERE WAS A VALID INDEX
          // OVERWRITE THE CHANGES
          Array.Copy(newSteamBytes, 0, saveFileData, SteamIdPositions[sidx], STEAM_ID_LENGTH);
        }
      }

      SteamIdText.ReadOnly = true;
      StatusBarText.Text = "Steam ID Altered, don't forget to save";
      steamIdChanged = true;
      ChangeSteamIdBtn.Text = "Change";
      SaveBtn.Enabled = true;
      ReloadChanges();
      //UpdateChecksums();
      SaveBtn.Focus();
    }

    private void ChangeSteamIdBtn_Click(object sender, EventArgs e)
    {
      ChangeSteamId();
    }

    private void SteamIdText_Keypressed(object sender, KeyPressEventArgs e)
    {
      // LISTEN FOR ENTER PRESS
      if(e.KeyChar == (char)Keys.Enter) {
        ChangeSteamId();  // STEAM ID CHANGE
        e.Handled = true;
      }
    }

    private void SaveBtn_Click(object sender, EventArgs e)
    {
      if(steamIdChanged || unsavedChanges) {  // DOUBLE CHECK CHANGES ARE PENDING
        StatusBarText.Text = "Updating save file";
        string backupFile = FileSelectionTxt.Text + ".bak";   // BACKUP FILENAME
        for(int chIdx = 0; chIdx < MAX_SAVE_SLOTS; chIdx++) {
          if(SlotChanged[chIdx] == SLOT_CHANGED) {
            Console.WriteLine("Updating Character Data Index " + chIdx);
            int stat_start = CharacterData[chIdx][CharacterStatsIndex[chIdx]];
            Console.WriteLine("Stats Start Index is " + chIdx);
            //short vig_i = Int16.Parse(characters[chIdx][VIGOR_IDX]);
            //short mind_i = Int16.Parse(characters[chIdx][MIND_IDX]);
            //short endur_i = Int16.Parse(characters[chIdx][ENDUR_IDX]);
            //short str_i = Int16.Parse(characters[chIdx][STR_IDX]);
            //short dex_i = Int16.Parse(characters[chIdx][DEXT_IDX]);
            //short intel_i = Int16.Parse(characters[chIdx][INTEL_IDX]);
            //short faith_i = Int16.Parse(characters[chIdx][FAITH_IDX]);
            //short arc_i = Int16.Parse(characters[chIdx][ARCH_IDX]);
            CharacterName[chIdx] = characters[chIdx][NAME_IDX];
            CharacterLevel[chIdx] = Int32.Parse(characters[chIdx][LEVEL_IDX]);
            byte[] vig = BitConverter.GetBytes(Int16.Parse(characters[chIdx][VIGOR_IDX]));
            byte[] mind = BitConverter.GetBytes(Int16.Parse(characters[chIdx][MIND_IDX]));
            byte[] endur = BitConverter.GetBytes(Int16.Parse(characters[chIdx][ENDUR_IDX]));
            byte[] str = BitConverter.GetBytes(Int16.Parse(characters[chIdx][STR_IDX]));
            byte[] dex = BitConverter.GetBytes(Int16.Parse(characters[chIdx][DEXT_IDX]));
            byte[] intel = BitConverter.GetBytes(Int16.Parse(characters[chIdx][INTEL_IDX]));
            byte[] faith = BitConverter.GetBytes(Int16.Parse(characters[chIdx][FAITH_IDX]));
            byte[] arc = BitConverter.GetBytes(Int16.Parse(characters[chIdx][ARCH_IDX]));
            byte[] char_name = Encoding.Unicode.GetBytes(CharacterName[chIdx]);

            Array.Copy(vig, 0, CharacterData[chIdx], stat_start, 2);
            Array.Copy(mind, 0, CharacterData[chIdx], (stat_start + 4), 2);
            Array.Copy(endur, 0, CharacterData[chIdx], (stat_start + 8), 2);
            Array.Copy(str, 0, CharacterData[chIdx], (stat_start + 12), 2);
            Array.Copy(dex, 0, CharacterData[chIdx], (stat_start + 16), 2);
            Array.Copy(intel, 0, CharacterData[chIdx], (stat_start + 20), 2);
            Array.Copy(faith, 0, CharacterData[chIdx], (stat_start + 24), 2);
            Array.Copy(arc, 0, CharacterData[chIdx], (stat_start + 28), 2);
            
            // ADD UP ALL THE STATS
            int statsIdx = BitConverter.ToInt16(vig, 0) + BitConverter.ToInt16(mind, 0);
            statsIdx += BitConverter.ToInt16(endur, 0) + BitConverter.ToInt16(str, 0);
            statsIdx += BitConverter.ToInt16(dex, 0) + BitConverter.ToInt16(intel, 0);
            statsIdx += BitConverter.ToInt16(faith, 0) + BitConverter.ToInt16(arc, 0);

            // ADJUST THE STATS SO THE LEVELS MATCH
            if(statsIdx != (CharacterLevel[chIdx] + 79)) {
              int lvl_adjst = statsIdx - (CharacterLevel[chIdx] + 79);
              CharacterLevel[chIdx] += lvl_adjst;
            }

            byte[] lvl_chk = BitConverter.GetBytes(CharacterLevel[chIdx]);
            Array.Copy(lvl_chk, 0, CharacterData[chIdx], (stat_start + 44), 3);

            if(statsIdx == (CharacterLevel[chIdx] + 79) && BitConverter.ToInt32(lvl_chk, 0) == CharacterLevel[chIdx]) {
              Console.WriteLine("Level data matches");
              int charLevelIdx = ((CHAR_HEADER_START_INDEX + (chIdx * CHAR_HEADER_LENGTH)) + CHAR_LEVEL_LOCATION);
              int charHeaderIdx = (CHAR_HEADER_START_INDEX + (chIdx * CHAR_HEADER_LENGTH));
              int charDataIdx = (SLOT_START_INDEX + (chIdx * CHECKSUM_LENGTH) + (chIdx * SLOT_LENGTH));
              // COPY IN THE NAME AND LEVEL AND CHAR DATA
              Array.Copy(BitConverter.GetBytes(CharacterLevel[chIdx]), 0, saveFileData, charLevelIdx, 2);
              //Array.Copy(char_name, 0, saveFileData, charHeaderIdx, CHAR_NAME_LENGTH);
              Array.Copy(CharacterData[chIdx], 0, saveFileData, charDataIdx, SLOT_LENGTH);
              // UPDATE CHECKSUMS
              UpdateChecksums();
            } else {
              Console.WriteLine("Level Validation failed, Expected " + statsIdx + " got " + (CharacterLevel[chIdx] + 79));
              unsavedChanges = false;
              SaveBtn.Enabled = false;
              ReloadChanges();
              return;
            }

          }
        }


        try {
          if(File.Exists(backupFile)) { File.Delete(backupFile); }  // REMOVE OLD BACKUP FILE
          File.Move(FileSelectionTxt.Text, backupFile);   // BACKUP THE CURRENT FILE
          File.WriteAllBytes(FileSelectionTxt.Text, saveFileData);  // WRITE THE UPDATED FILE
          
        } catch(Exception ex) {
          Console.WriteLine("ERROR: " + ex.GetType().Name);
        }

        StatusBarText.Text = "Updated Save to: " + FileSelectionTxt.Text;
        SaveBtn.Enabled = false;
        steamIdChanged = false;
        unsavedChanges = false;
        ReloadChanges();
      } else {
        // BUTTON GOT ENABLED SOMEHOW?
        SaveBtn.Enabled = false;
        StatusBarText.Text = "No changes to save";
      }
    }

    private void DataGrid_CellContentChanged(object sender, DataGridViewCellEventArgs e)
    {
      if(e.RowIndex == -1) { return; }  // INIT BLOCK
      Console.WriteLine("Column: " + e.ColumnIndex + " Row: " + e.RowIndex + " Changed");
      if(e.ColumnIndex > 1) {
        string newValue = DataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
        foreach(char num in newValue) {
          // CHECK TO MAKE SURE THE STEAM ID IS DIGITS
          if(num > '9' || num < '0') {
            MessageBox.Show("Only Numbers allowed");
            DataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = characters[e.RowIndex].ElementAt(e.ColumnIndex);
            return;
          }
        }
      }
      if(characters[e.RowIndex][e.ColumnIndex] != DataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) {
        characters[e.RowIndex][e.ColumnIndex] = DataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
        SlotChanged[e.RowIndex] = SLOT_CHANGED;
        unsavedChanges = true;
        SaveBtn.Enabled = true;
        StatusBarText.Text = "You have unsaved changes...";
      }
    }
  } /** END - public partial class MainWindow : Form **/
}
