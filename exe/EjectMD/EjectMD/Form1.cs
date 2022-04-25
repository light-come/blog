﻿using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EjectMD
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        }
        private string SelectPath()
        {
            string path = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Files (*.xlsx)|*.xlsx"//如果需要筛选txt文件（"Files (*.txt)|*.txt"）
            };

            //var result = openFileDialog.ShowDialog();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path = openFileDialog.FileName;
            }

            return path;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                FileInfo existingFile = new FileInfo(SelectPath());
                string path = null;
                FolderBrowserDialog dilog = new FolderBrowserDialog
                {
                    Description = "请选择导出文件夹"
                };
                if (dilog.ShowDialog() == DialogResult.OK || dilog.ShowDialog() == DialogResult.Yes)
                {
                    path = dilog.SelectedPath;
                }
                if (!(path is null))
                    using (ExcelPackage package = new ExcelPackage(existingFile))
                    {

                        foreach (var worksheet in package.Workbook.Worksheets)
                        {

                            if (worksheet.Dimension is null) continue;
                            int colCount = worksheet.Dimension.End.Column;  //get Column Count
                            int rowCount = worksheet.Dimension.End.Row;     //get row count
                            List<string> fileName = new List<string>();
                            List<string> titleName = new List<string>();
                            string title = null;
                            for (int row = 1; row <= rowCount; row++)
                            {
                                fileName.Add(worksheet.Cells[row, 1].Value?.ToString().Trim().Replace(" ", "").ToUpper());

                                if (worksheet.Cells[row, 2].Value?.ToString().Trim() != null)
                                    title = worksheet.Cells[row, 2].Value?.ToString().Trim().Replace(" ", "");
                                titleName.Add(title);
                                for (int col = 1; col <= colCount; col++)
                                {
                                    Console.WriteLine(" Row:" + row + " column:" + col + " Value:" + worksheet.Cells[row, col].Value?.ToString().Trim());
                                }
                            }

                            var index = 1;
                            string titlePath = null;

                            foreach (var item in titleName.Distinct().ToArray())
                            {
                                titlePath = "\\" + Regex.Replace(item, @"[^a-zA-Z0-9\u4e00-\u9fa5\s]", "").Trim().Replace(" ","") + "\\";
                                if (!Directory.Exists(path + titlePath))
                                {
                                    Directory.CreateDirectory(path + titlePath);
                                    //if (!File.Exists(path + titlePath))
                                    //{
                                    //    File.Create(path + titlePath).Close();
                                    //}
                                }
                            } //去重
                            foreach (var item in fileName)
                            {

                                string FILE_NAME = $"{path + titlePath}{DateTime.Now.ToString("yyyyMMdd")}{index++}.{Regex.Replace(item.Replace(" ", ""), @"[^a-zA-Z0-9\u4e00-\u9fa5\s]", "")}.md";
                                StreamWriter sr;
                                if (File.Exists(FILE_NAME))
                                    sr = File.AppendText(FILE_NAME);
                                else
                                    sr = File.CreateText(FILE_NAME);
                                sr.WriteLine("---");
                                sr.WriteLine("title: " + item);
                                sr.WriteLine("---"); sr.WriteLine(""); sr.WriteLine("");
                                sr.WriteLine("# " + titleName[index - 2]);
                                sr.WriteLine("");
                                sr.WriteLine("## " + item);
                                sr.Close();

                            }
                            System.Diagnostics.Process.Start("explorer.exe", path + titlePath);
                        }

                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog dilog = new FolderBrowserDialog
            {
                Description = "请选择导出文件夹"
            };
            if (dilog.ShowDialog() == DialogResult.OK || dilog.ShowDialog() == DialogResult.Yes)
            {
                string path = dilog.SelectedPath;
                List<string> list = new List<string>();
                DirectoryInfo root = new DirectoryInfo(path);
                FileInfo[] files = root.GetFiles();
                foreach (var item in files)
                {
                    string extension = Path.GetExtension(item.FullName);
                    if (extension is ".md" && Path.GetFileNameWithoutExtension(item.FullName) != "README")
                    {

                        list.Add(Path.GetFileNameWithoutExtension(item.FullName));
                    }
                }

                string arr = JsonConvert.SerializeObject(list);//JObject.Parse(JsonConvert.SerializeObject(list))
                Console.WriteLine();
                Console.WriteLine(arr);
                System.Windows.Forms.Clipboard.SetText(arr);
                MessageBox.Show("已复制到粘贴板");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dilog = new FolderBrowserDialog
            {
                Description = "请选择导出文件夹"
            };
            if (dilog.ShowDialog() == DialogResult.OK || dilog.ShowDialog() == DialogResult.Yes)
            {
                string path = dilog.SelectedPath;
                DirectoryInfo root = new DirectoryInfo(path);
                FileInfo[] files = root.GetFiles();

                string dic = "# 目录\r\n\r\n"; int index = 0;
                foreach (var item in files)
                {
                    string extension = Path.GetExtension(item.FullName);
                    if (extension is ".md" && Path.GetFileNameWithoutExtension(item.FullName) != "README")
                    {
                        string name = Path.GetFileNameWithoutExtension(item.FullName).Split('.')[1];
                        index++;
                        dic += $"[{index}-{name}]({Path.GetFileNameWithoutExtension(item.FullName)}.html)\r\n\r\n";
                    }
                }
                dic += "<Valine/>";
                Console.WriteLine(dic);
                System.Windows.Forms.Clipboard.SetText(dic);
                MessageBox.Show("已复制到粘贴板");
            }
        }

    }
}