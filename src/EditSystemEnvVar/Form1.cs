using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EditSystemEnvVar
{
    public partial class Form1 : Form
    {
        private ErrorProvider __errorProvider = null;

        public Form1()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.MainIcon;
            __errorProvider = new ErrorProvider(this);

            this.listView.HideSelection = false;
            this.listView.FullRowSelect = true;

            this.listView.Columns[0].Width = 80;
            this.txtKey.ReadOnly = true;

            this.showConsoleToolStripMenuItem.Checked = Properties.Settings.Default.ShowConsoleWindow;
            this.splitContainer1.Panel2Collapsed = !this.showConsoleToolStripMenuItem.Checked;
            this.SizeChanged += (s, e) =>
            {
                try
                {
                    AdjustListViewColumnSize();
                }
                catch (Exception ex)
                {
                    WriteConsole(ex);
                }
            };

            this.showConsoleToolStripMenuItem.Click += (s, e) =>
            {
                try
                {
                    ToolStripMenuItem item = (ToolStripMenuItem)s;
                    item.Checked = !item.Checked;

                    this.splitContainer1.Panel2Collapsed = !item.Checked;

                    Properties.Settings.Default.ShowConsoleWindow = item.Checked;
                    Properties.Settings.Default.Save();
                }
                catch (Exception ex)
                {
                    WriteConsole(ex);
                    throw;
                }
            };

            this.Load += (s, e) =>
            {
                try
                {
                    AdjustListViewColumnSize();
                    LoadSystemEnvironmentVariables();

                    this.ShowStatusLabelText("Status: Ready");
                }
                catch (Exception ex)
                {
                    WriteConsole(ex);
                }
            };

            this.listView.ItemSelectionChanged += (s, e) =>
            {
                try
                {
                    if (e.Item != null)
                    {
                        this.txtKey.ReadOnly = true;
                        this.txtKey.Text = e.Item.Text;
                        this.txtValue.Text = e.Item.SubItems[1].Text;
                    }
                }
                catch (Exception ex)
                {
                    WriteConsole(ex);
                }
            };

            this.reloadToolStripMenuItem.Click += (s, e) => { this.btnReload.PerformClick(); };

            this.newVariableToolStripMenuItem.Click += (s, e) => { this.btnNew.PerformClick(); };

            this.saveToolStripMenuItem.Click += (s, e) => { this.btnSave.PerformClick(); };

            this.deleteToolStripMenuItem.Click += (s, e) => { this.btnDelete.PerformClick(); };

            this.exitToolStripMenuItem.Click += (s, e) => { this.Close(); };

            this.btnReload.Click += (s, e) =>
            {
                try
                {
                    LoadSystemEnvironmentVariables();
                    this.txtKey.ReadOnly = true;
                }
                catch (Exception ex)
                {
                    WriteConsole(ex);
                }
            };

            this.btnNew.Click += (s, e) =>
            {
                try
                {
                    this.listView.SelectedIndices.Clear();
                    this.txtKey.ResetText();
                    this.txtValue.ResetText();
                    this.txtKey.ReadOnly = false;
                    this.txtKey.Focus();
                    this.txtKey.Select();

                    this.WriteConsole("Status: {0}", "New variable");
                    this.ShowStatusLabelText("Status: Add new item");
                }
                catch (Exception ex)
                {
                    WriteConsole(ex);
                }
            };

            this.txtKey.Validating += (s, e) =>
            {
                try
                {
                    TextBox thisControl = (TextBox)s;
                    if (!thisControl.ReadOnly)
                    {
                        this.ClearErrorProvider();

                        string strKey = thisControl.Text.Trim();
                        //if (String.IsNullOrEmpty(strKey))
                        //{
                        //    this.SetError(this.txtKey, "Could not add empty key.");
                        //    this.ShowStatusLabelText("Status: Error [Could not add empty key.]");
                        //    e.Cancel = true;
                        //}
                        //else
                        //{
                        //}

                        foreach (ListViewItem item in this.listView.Items)
                        {
                            if (item.Name.ToUpper().Equals(strKey.ToUpper()))
                            {
                                this.SetError(thisControl, "Could not add duplicate key.");
                                this.ShowStatusLabelText("Status: Error [Could not add duplicate key.]");
                                e.Cancel = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    WriteConsole(ex);
                }
            };

            this.btnSave.Click += (s, e) =>
            {
                this.ClearErrorProvider();
                string strKey = this.txtKey.Text.Trim();
                string strValue = this.txtValue.Text.Trim();
                try
                {
                    if (String.IsNullOrEmpty(strKey))
                    {
                        this.SetError(this.txtKey, "Could not add empty key.");
                    }
                    else if (String.IsNullOrEmpty(strValue))
                    {
                        this.SetError(this.txtKey, "Could not add empty value.");
                    }
                    else
                    {
                        //Environment.GetEnvironmentVariable(strKey, EnvironmentVariableTarget.Machine);
                        Environment.SetEnvironmentVariable(strKey, strValue, EnvironmentVariableTarget.Machine);

                        this.WriteConsole("Status: '{0}' saved.", strKey);

                        this.LoadSystemEnvironmentVariables();

                        this.listView.Items[strKey].Selected = true;
                        this.listView.EnsureVisible(this.listView.Items[strKey].Index);
                        this.ShowStatusLabelText("Status: Saved.");
                    }
                }
                catch (Exception ex)
                {
                    WriteConsole(ex);
                    this.ShowStatusLabelText("Status: Error");
                }
                finally
                {
                }
            };

            this.btnDelete.Click += (s, e) =>
            {
                string strKey = this.txtKey.Text.Trim();
                string strValue = null;

                string message = $"Do you want to delete '{strKey}'?";
                DialogResult result = MessageBox.Show(message, "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (DialogResult.Yes != result) { return; }

                try
                {
                    Environment.SetEnvironmentVariable(strKey, strValue, EnvironmentVariableTarget.Machine);
                    this.WriteConsole("Status: '{0}' deleted.", strKey);
                }
                catch (Exception ex)
                {
                    WriteConsole(ex);
                }
                finally
                {
                    this.LoadSystemEnvironmentVariables();
                    this.listView.SelectedIndices.Clear();
                }
            };
        }

        private void ClearErrorProvider()
        {
            __errorProvider.Clear();
        }

        private void SetError(Control ctr, string message)
        {
            __errorProvider.SetError(ctr, message);
        }

        private void LoadSystemEnvironmentVariables()
        {
            this.txtKey.ResetText();
            this.txtValue.ResetText();

            this.listView.Items.Clear();
            try
            {
                System.Collections.IDictionary variables = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Machine);

                foreach (System.Collections.DictionaryEntry entry in variables)
                {
                    //WriteConsole("{0}={1}", entry.Key, entry.Value);
                    ListViewItem item = new ListViewItem();
                    item.Name = $"{entry.Key}";
                    item.Text = $"{entry.Key}";
                    item.SubItems.Add($"{entry.Value}");

                    this.listView.Items.Add(item);
                }

                this.WriteConsole("Status: Load {0:n0} variable(s).", variables.Count);
                this.ShowStatusLabelText("Status: Ready");
            }
            catch (Exception ex)
            {
                WriteConsole(ex);
            }
        }

        private void AdjustListViewColumnSize()
        {
            try
            {
                int scrollbarWidth = 21;

                Size listViewSize = this.listView.Size;
                int keyWidth = this.listView.Columns[0].Width;
                int valueWidth = listViewSize.Width - keyWidth - scrollbarWidth;
                if (valueWidth < 100) { valueWidth = 100; }
                this.listView.Columns[1].Width = valueWidth;
            }
            catch (Exception ex)
            {
                WriteConsole(ex);
            }
        }

        private void WriteConsole(string format, params object[] args)
        {
            if (this.consoleWindow.TextLength > 0) { this.consoleWindow.AppendText(Environment.NewLine); }
            this.consoleWindow.AppendText(String.Format(format, args));
        }

        private void WriteConsole(Exception ex)
        {
            string message = ex.Message;
            string stackTrace = ex.StackTrace;

            WriteConsole("Message: {0}\r\nStack trace: {1}", message, stackTrace);
        }

        private void ShowStatusLabelText(string message)
        {
            this.lblStatus.Text = message;
        }
    }
}