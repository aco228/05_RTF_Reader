using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace _05_RTF_Reader
{
    public partial class Form1 : Form
    {
        // Var
        bool ucitano = false;
        string FileName;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "RTF fajlovi (*.rtf)|*rtf| TXT fajlovi(*.txt)|*.txt";
            if (op.ShowDialog() == DialogResult.OK)
            {
                lblDisplay.Text = "Ucitavanje...";
                FileName = op.FileName;
                string text = File.ReadAllText(FileName);
                string aba = obrada(text);
                lblDisplay.Text = aba;
                ucitano = true;
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ucitano)
            {
                SaveFileDialog sf = new SaveFileDialog();
                sf.Filter = "RTF fajlovi (*.rtf)|*rtf TXT fajlovi(*.txt)|*.txt";
                sf.Title = "Sacuvaj";
                sf.FileName = FileName;
                if (sf.ShowDialog() == DialogResult.OK)
                {
                    TextWriter tw = File.CreateText(sf.FileName);
                    tw.Write(lblDisplay.Text); tw.Close();
                }
            }
            else MessageBox.Show("Nista nije ucitano!");
        }

        //
        // OBRADA TEKSTA
        //
        private string obrada(string a)
        {
            string back = "{";
            a = a.Replace("\r", String.Empty); a = a.Replace("\n", String.Empty); a = a.Replace("\t", String.Empty);
            int br = -1, mulLine = -1; bool izasao = false;
            for (int i = 1; i < a.Length; i++)
            {
                if (a[i] == '{') 
                {
                    br++; mulLine++;
                    if(br == 0) back += "\r\n\r\n" + getSpace(br);
                    else back += "\r\n" + getSpace(br);
                }
                if (a[i] == '}') 
                { 
                    br--;
                    if (mulLine == 0 && br == -1) { back += "}"; mulLine = -1; }
                    if (br == -1 && mulLine > 0) { back += "\r\n}"; mulLine = -1;  }
                    if (br >= 0) { back += "}\r\n" + getSpace(br); }

                    izasao = true;
                    continue;
                }
                if (a[i] == '\\' && izasao && br == -1) { back += "\r\n\r\n"; izasao = false; }
                back += a[i];
            }
            return back;
        }
        private string getSpace(int a)
        {
            string back= "", space = "    ";
            for (int i = 0; i < a; i++) back += space;
            return back;
        }
        private string getFileName(string a)
        {
            string back = "";
            int len = a.Length;
            for (int i = a.Length-1; i >= 0; i--)
            {
                //if (a[i] == '\\') MessageBox.Show(a.Substring(i, a.Length - 2)); 
            }
            //MessageBox.Show(back);
            return back;
        }

        private void lblRow_CheckedChanged(object sender, EventArgs e)
        {
            if (lblRow.Checked) { lblDisplay.WordWrap = true; }
            if (!lblRow.Checked) lblDisplay.WordWrap = false;
        }

        private void btnS_Click(object sender, EventArgs e)
        {
            if (txtS.Text != "")
            {
                search(txtS.Text);
            }
        }
        private void search(string a)
        {
            int curPos = lblDisplay.SelectionStart;
            int selectLen = lblDisplay.SelectionLength;
            int len = a.Length;
            int i = lblDisplay.Text.IndexOf(a);

            string temp = lblDisplay.Text; lblDisplay.Text = ""; lblDisplay.Text = temp;

            while (i >= 0)
            {
                lblDisplay.Select(i, len);
                //lblDisplay.SelectionFont = new Font(me.SelectionFont, style);
                lblDisplay.SelectionBackColor = Color.FromArgb(196, 255, 252);
                i = lblDisplay.Text.IndexOf(a, i + len);
            }

            lblDisplay.SelectionStart = curPos;
            lblDisplay.SelectionLength = selectLen;
        }


    }
}
