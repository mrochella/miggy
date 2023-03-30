﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace THA_W5_MEGAN_R
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        DataTable dtProdukSimpan = new DataTable();
        DataTable dtProdukTampil = new DataTable();
        DataTable dtCategory = new DataTable(); 
        List<Product> products = new List<Product>();
        List<string> categories = new List<string>();
        int categoriesCount = 0;
        int productid = 0;

        private void Form1_Load(object sender, EventArgs e)
        {
            dtProdukSimpan.Columns.Add("ID Product");
            dtProdukSimpan.Columns.Add("Nama Product");
            dtProdukSimpan.Columns.Add("Harga");
            dtProdukSimpan.Columns.Add("Stock");
            dtProdukSimpan.Columns.Add("ID Category");
            dgvProdukSimpan.DataSource = dtProdukSimpan;

            dtCategory.Columns.Add("ID Category");
            dtCategory.Columns.Add("Nama Category");
            dgvCategory.DataSource = dtCategory;

            dtProdukTampil.Columns.Add("ID Product");
            dtProdukTampil.Columns.Add("Nama Product");
            dtProdukTampil.Columns.Add("Harga");
            dtProdukTampil.Columns.Add("Stock");
            dtProdukTampil.Columns.Add("ID Category");

            products.Add(new Product("Jas Hitam", "100000", "10", "Jas"));
            products.Add(new Product("T-Shirt Black Pink", "70000", "20", "T-Shirt"));
            products.Add(new Product("T-Shirt Obsessive", "75000", "16", "T-Shirt"));
            products.Add(new Product("Rok Mini", "82000", "26", "Rok"));
            products.Add(new Product("Jeans Biru", "90000", "5", "Celana"));
            products.Add(new Product("Celana Pendek Coklat", "60000", "14", "Celana"));
            products.Add(new Product("Cawat Blink-Blink", "100000", "1", "Cawat"));
            products.Add(new Product("Rocca Shirt", "50000", "8", "T-Shirt"));

            foreach (Product product in products)
            {
                if (!categories.Contains(product.Category))
                {
                    categories.Add(product.Category);
                    categoriesCount++;
                    dtCategory.Rows.Add("C" + categoriesCount, product.Category);
                }
            }

            for (int i = 65; i <= 90; i++)
            {
                foreach (Product product in products)
                {
                    if (product.Name[0] == Convert.ToChar(i))
                    {
                        productid++;
                        product.IDProduct = Convert.ToChar(i) + productid.ToString("000");
                    }
                }
                productid = 0;
            }

            foreach (Product product in products)
            {
                product.CategoryID = generateCategoryID(product);
                dtProdukSimpan.Rows.Add(product.IDProduct, product.Name, product.Harga, product.Stock, product.CategoryID);
            }
            comboBox_data.Enabled = false;

            for (int i = 0; i < dtCategory.Rows.Count; i++)
            {
                comboBox_cat.Items.Add(dtCategory.Rows[i][1].ToString());
            }
        }
        private string generateProductID(Product product)
        {
            string IDProduct = "";
            for (int i = 65; i <= 90; i++)
            {
                if (product.Name[0] == Convert.ToChar(i))
                {
                    foreach (Product produk in products)
                    {
                        if (produk.Name[0] == product.Name[0])
                        {
                            productid++;
                        }
                    }
                    IDProduct = Convert.ToChar(i) + productid.ToString("000");
                }
                productid = 0;
            }
            return IDProduct;

        }
        private string generateCategoryID(Product product)
        {
            string categoryID = "";
            for (int i = 0; i < dtCategory.Rows.Count; i++)
            {
                if (dtCategory.Rows[i][1].ToString() == product.Category)
                {
                    categoryID = dtCategory.Rows[i][0].ToString();
                }
            }
            return categoryID;
        }

        private void butt_addCategory_Click(object sender, EventArgs e)
        {
            categoriesCount++;
            if (textBox_nameCat.Text == "")
            {
                MessageBox.Show("Pls add a category name", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                dtCategory.Rows.Add("C" + categoriesCount, textBox_nameCat.Text);
            }
            comboBox_cat.Items.Clear();
            for (int i = 0; i < dtCategory.Rows.Count; i++)
            {
                comboBox_cat.Items.Add(dtCategory.Rows[i][1].ToString());
            }
        }

        private void butt_all_Click(object sender, EventArgs e)
        {
            comboBox_data.Items.Clear();
            comboBox_data.Enabled = false;
            dgvProdukSimpan.DataSource = dtProdukSimpan;
        }

        private void butt_filter_Click(object sender, EventArgs e)
        {
            comboBox_data.Enabled = true;
            comboBox_data.Items.Clear();
            for (int i = 0; i < dtCategory.Rows.Count; i++)
            {
                comboBox_data.Items.Add(dtCategory.Rows[i][1].ToString());
            }
        }

        private void comboBox_data_SelectionChangeCommitted(object sender, EventArgs e)
        {
            dgvProdukSimpan.DataSource = dtProdukTampil;
            dtProdukTampil.Rows.Clear();
            foreach (Product product in products)
            {
                for (int i = 0; i < dtProdukSimpan.Rows.Count; i++)
                {
                    if (product.Name == dtProdukSimpan.Rows[i][1].ToString() && product.Category == comboBox_data.Text)
                    {
                        dtProdukTampil.Rows.Add(product.IDProduct, product.Name, product.Harga, product.Stock, product.CategoryID);
                    }
                }
            }
        }

        private void butt_addProduct_Click(object sender, EventArgs e)
        {
            if (textBox_nameDet.Text == "" || textBox_hargaDet.Text == "" || textBox_stockDet.Text == "" || comboBox_cat.Text == "")
            {
                MessageBox.Show("Please fill the text boxes.", "OOPS!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Product product = new Product(textBox_nameDet.Text, textBox_hargaDet.Text, textBox_stockDet.Text, comboBox_cat.Text);
                products.Add(product);
                product.IDProduct = generateProductID(product);
                product.CategoryID = generateCategoryID(product);
                dtProdukSimpan.Rows.Add(product.IDProduct, product.Name, product.Harga, product.Stock, product.CategoryID);
            }
        }

        private void butt_removeProduct_Click(object sender, EventArgs e)
        {
            dtProdukSimpan.Rows.RemoveAt(dgvProdukSimpan.CurrentCell.RowIndex);
        }

        private void butt_editProduct_Click(object sender, EventArgs e)
        {
            if (textBox_nameDet.Text == "" || textBox_hargaDet.Text == "" || textBox_stockDet.Text == "" || comboBox_cat.Text == "")
            {
                MessageBox.Show("Please fill the text boxes.", "OOPS!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int index = dgvProdukSimpan.CurrentCell.RowIndex;
                string namaProduk = dtProdukSimpan.Rows[index][1].ToString();
                foreach (Product product in products)
                {
                    if (namaProduk == product.Name)
                    {
                        product.Name = textBox_nameDet.Text;
                        product.Category = comboBox_cat.Text;
                        product.Harga = textBox_hargaDet.Text;
                        product.Stock = textBox_stockDet.Text;
                        product.IDProduct = generateProductID(product);
                        product.CategoryID = generateCategoryID(product);
                        dtProdukSimpan.Rows.Clear();
                        foreach (Product produk in products)
                        {
                            dtProdukSimpan.Rows.Add(produk.IDProduct, produk.Name, produk.Harga, produk.Stock, produk.CategoryID);
                        }

                    }
                }
            }
            textBox_nameDet.Text = "";
            comboBox_cat.SelectedIndex = -1;
            textBox_hargaDet.Text = "";
            textBox_stockDet.Text = "";
        }

        private void dgvProdukSimpan_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvProdukSimpan.CurrentCell.RowIndex;
            foreach (Product product in products)
            {
                if (dgvProdukSimpan.CurrentRow.Cells["Nama Product"].Value.ToString() == product.Name)
                {
                    textBox_nameDet.Text = product.Name;
                    comboBox_cat.Text = product.Category;
                    textBox_hargaDet.Text = product.Harga;
                    textBox_stockDet.Text = product.Stock;
                }
            }
        }

        private void butt_removeCategory_Click(object sender, EventArgs e)
        {
            int index = dgvProdukSimpan.CurrentCell.RowIndex;
            string removedCategoryID = dtCategory.Rows[index][0].ToString();
            for (int i = 0; i < dtProdukSimpan.Rows.Count; i++)
            {
                if (dtProdukSimpan.Rows[i][4].ToString() == removedCategoryID)
                {
                    dtProdukSimpan.Rows.RemoveAt(i);
                }
            }
            dtCategory.Rows.RemoveAt(index);
        }
    }


}

