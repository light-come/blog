using Newtonsoft.Json;
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
                                fileName.Add(worksheet.Cells[row, 1].Value?.ToString().Trim());

                                if (worksheet.Cells[row, 2].Value?.ToString().Trim() != null)
                                    title = worksheet.Cells[row, 2].Value?.ToString().Trim();
                                titleName.Add(title);
                                for (int col = 1; col <= colCount; col++)
                                {
                                    Console.WriteLine(" Row:" + row + " column:" + col + " Value:" + worksheet.Cells[row, col].Value?.ToString().Trim());
                                }
                            }

                            string titlePath = null;

                            foreach (var item in titleName.Distinct().ToArray())
                            {
                                titlePath = "\\" + Regex.Replace(item, @"[^a-zA-Z0-9\u4e00-\u9fa5\s]", "").Trim().Replace(" ", "") + "\\";
                                if (!Directory.Exists(path + titlePath))//去重
                                {
                                    Directory.CreateDirectory(path + titlePath);
                                    //if (!File.Exists(path + titlePath))
                                    //{
                                    //    File.Create(path + titlePath).Close();
                                    //}

                                }
                            }
                            var index = 0;
                            string dic = "# 目录\r\n\r\n";
                            foreach (var item in fileName)
                            {
                              
                                var timeName = DateTime.Now.ToString("yyyyMMdd");
                                index++;
                                string FILE_NAME = $"{path + titlePath}{timeName}{index}.md";

                                {
                                    dic += $"[{index}-{item}]({timeName}{index}.html)\r\n\r\n";
                                }

                                StreamWriter sr;
                                if (File.Exists(FILE_NAME))
                                    sr = File.AppendText(FILE_NAME);
                                else
                                    sr = File.CreateText(FILE_NAME);
                                sr.WriteLine("---");
                                sr.WriteLine("title: " + item);
                                sr.WriteLine("---"); sr.WriteLine(""); sr.WriteLine("");
                                sr.WriteLine("# " + titleName[index - 1]);
                                sr.WriteLine("");
                                sr.WriteLine("## " + item);
                                sr.WriteLine(""); sr.WriteLine("");

                                sr.WriteLine("[![CC0](http://mirrors.creativecommons.org/presskit/buttons/88x31/svg/cc-zero.svg)](https://creativecommons.org/publicdomain/zero/1.0/)");
                                sr.WriteLine("To the extent possible under law, [LIGHT-COME](https://github.com/light-come) has waived all copyright and related or neighboring rights to this work.");
                                sr.Close();

                            }

                            {
                                string FILE_NAME = path + titlePath + "README.md";
                                StreamWriter sr;
                                if (File.Exists(FILE_NAME))
                                    sr = File.AppendText(FILE_NAME);
                                else
                                    sr = File.CreateText(FILE_NAME);
                                dic += "<Valine/>";
                                sr.WriteLine(dic);
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

        private void button2_Click_1(object sender, EventArgs e)
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
    }
}
