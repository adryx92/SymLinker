﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace SymLinker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.LastSourcePath) && !string.IsNullOrWhiteSpace(Properties.Settings.Default.LastDestinationPath))
            {
                TextBoxSrc.Text = Properties.Settings.Default.LastSourcePath;
                TextBoxDest.Text = Properties.Settings.Default.LastDestinationPath;
            }
        }
        private static void CreateSymbolicLink(string sourcePath, string destinationPath, bool isDirectoryLink)
        {
            if (string.IsNullOrWhiteSpace(sourcePath) || string.IsNullOrWhiteSpace(destinationPath))
            {
                System.Windows.MessageBox.Show("Source and destination must be specified.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string destName = System.IO.Path.GetFileName(sourcePath);

            try
            {
                string mkLinkCommand = "mklink";
                if (isDirectoryLink)
                    mkLinkCommand += " /D";
                mkLinkCommand += $" \"{destinationPath}\\{destName}\" \"{sourcePath}\"";
                ProcessStartInfo psi = new ProcessStartInfo("cmd.exe")
                {
                    UseShellExecute = true,
                    CreateNoWindow = true,
                    RedirectStandardInput = false,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false,
                    Arguments = $"/C {mkLinkCommand}"
                };

                Process process = Process.Start(psi);
                process.WaitForExit();

                int exitCode = process.ExitCode;
                if (exitCode == 0)
                {
                    System.Windows.MessageBox.Show("Symbolic link successfully created.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    System.Windows.MessageBox.Show("Error creating the symbolic link.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error creating the symbolic link: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CreateLink_Click(object sender, EventArgs e)
        {
            string sourcePath = TextBoxSrc.Text;
            string destinationPath = TextBoxDest.Text;
            bool isDirectoryLink = DirectoryCheckBox.IsChecked == true;

            CreateSymbolicLink(sourcePath, destinationPath, isDirectoryLink);
        }

        private static void OpenFileDlg(System.Windows.Controls.TextBox txtBoxElem)
        {
            var openFileDialog = new System.Windows.Forms.OpenFileDialog();
            DialogResult result = openFileDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                txtBoxElem.Text = openFileDialog.FileName;
            }
        }

        private static void OpenDirectoryDlg(System.Windows.Controls.TextBox txtBoxElem)
        {
            var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            DialogResult result = folderBrowserDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                txtBoxElem.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void SourceSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            bool isDirectoryLink = DirectoryCheckBox.IsChecked == true;
            if (isDirectoryLink)
                OpenDirectoryDlg(TextBoxSrc);
            else
                OpenFileDlg(TextBoxSrc);
        }

        private void DestSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            OpenDirectoryDlg(TextBoxDest);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TextBoxSrc.Text) && !string.IsNullOrWhiteSpace(TextBoxDest.Text))
            {
                Properties.Settings.Default.LastSourcePath = TextBoxSrc.Text;
                Properties.Settings.Default.LastDestinationPath = TextBoxDest.Text;
                Properties.Settings.Default.Save();
            }
        }

        private void DirectoryCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            TextBoxSrc.Text = string.Empty;
        }
    }
}